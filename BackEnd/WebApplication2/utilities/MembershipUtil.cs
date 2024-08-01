using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;
using Npgsql;

namespace WebApplication2.utilities
{
    public static class MembershipUtil
    {
        private static string _connectionString = ConfigurationManager.ConnectionStrings["PgSqlServices"].ConnectionString;

        public static void CreateUser(string username, string password, string email, string securityQuestion, string securityAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            string salt = GenerateSalt();
            string hashedPassword = HashPassword(password, Convert.FromBase64String(salt));
            Guid userId = Guid.NewGuid();

            using (NpgsqlConnection conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Kiểm tra xem người dùng đã tồn tại chưa
                        using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT COUNT(*) FROM aspnet_users WHERE username = @username AND applicationname = @applicationname", conn))
                        {
                            cmd.Parameters.AddWithValue("username", username);
                            cmd.Parameters.AddWithValue("applicationname", "FaceBook");
                            int userCount = Convert.ToInt32(cmd.ExecuteScalar());
                            if (userCount > 0)
                            {
                                status = MembershipCreateStatus.DuplicateUserName;
                                return;
                            }
                        }

                        // Insert into aspnet_users
                        using (NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO aspnet_users (userid, username, applicationname) VALUES (@userid, @username, @applicationname)", conn))
                        {
                            cmd.Parameters.AddWithValue("userid", userId);
                            cmd.Parameters.AddWithValue("username", username);
                            cmd.Parameters.AddWithValue("applicationname", "FaceBook");
                            cmd.ExecuteNonQuery();
                        }

                        // Insert into aspnet_membership
                        using (NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO aspnet_membership (userid, password, passwordsalt, passwordformat, email, passwordquestion, passwordanswer, isapproved, islockedout, creationdate, lastlogindate, lastpasswordchangeddate) VALUES (@userid, @password, @passwordsalt, @passwordformat, @email, @passwordquestion, @passwordanswer, @isapproved, @islockedout, @creationdate, @lastlogindate, @lastpasswordchangeddate)", conn))
                        {
                            cmd.Parameters.AddWithValue("userid", userId);
                            cmd.Parameters.AddWithValue("password", hashedPassword);
                            cmd.Parameters.AddWithValue("passwordsalt", salt);
                            cmd.Parameters.AddWithValue("passwordformat", 1);
                            cmd.Parameters.AddWithValue("email", email);
                            cmd.Parameters.AddWithValue("passwordquestion", securityQuestion);
                            cmd.Parameters.AddWithValue("passwordanswer", securityAnswer);
                            cmd.Parameters.AddWithValue("isapproved", isApproved ? 1 : 0);
                            cmd.Parameters.AddWithValue("islockedout", false ? 1 : 0);
                            cmd.Parameters.AddWithValue("creationdate", DateTime.UtcNow);
                            cmd.Parameters.AddWithValue("lastlogindate", DateTime.UtcNow);
                            cmd.Parameters.AddWithValue("lastpasswordchangeddate", DateTime.UtcNow);
                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        status = MembershipCreateStatus.Success;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        status = MembershipCreateStatus.ProviderError;
                        throw ex;
                    }
                }
            }
        }

        public static bool ValidateUser(string username, string password)
        {
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(_connectionString))
                {
                    conn.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT m.password, m.passwordsalt FROM aspnet_membership m JOIN aspnet_users u ON m.userid = u.userid WHERE u.username = @username AND u.applicationname = @applicationname", conn))
                    {
                        cmd.Parameters.AddWithValue("username", username);
                        cmd.Parameters.AddWithValue("applicationname", "FaceBook");

                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string storedPassword = reader.GetString(0);
                                string storedSalt = reader.GetString(1);
                                string hashedPassword = HashPassword(password, Convert.FromBase64String(storedSalt));

                                return storedPassword == hashedPassword;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return false;
        }

        private static string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        private static string HashPassword(string password, byte[] salt)
        {
            byte[] passwordBytes = Encoding.Unicode.GetBytes(password);
            byte[] saltedPasswordBytes = new byte[passwordBytes.Length + salt.Length];

            Buffer.BlockCopy(salt, 0, saltedPasswordBytes, 0, salt.Length);
            Buffer.BlockCopy(passwordBytes, 0, saltedPasswordBytes, salt.Length, passwordBytes.Length);

            using (SHA1 sha1 = SHA1.Create())
            {
                byte[] hashBytes = sha1.ComputeHash(saltedPasswordBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}
