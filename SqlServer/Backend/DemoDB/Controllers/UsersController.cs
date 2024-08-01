using DemoDB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;


namespace DemoDB.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public UsersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        public JsonResult GetAspUsers()
        {
            string query = @"
                    SELECT * FROM aspnet_Users
                    ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection"); 
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }

        [HttpPost]
        public IActionResult Login([FromBody]UserLogin user)
        {
            string query = @"
            SELECT * FROM aspnet_Membership 
            WHERE Email = @Email AND Password = @Password
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Email", user.UserName);
                    myCommand.Parameters.AddWithValue("@Password", user.Password); // Should be hashed

                    myReader = myCommand.ExecuteReader();
                    if (myReader.HasRows)
                    {
                        table.Load(myReader);
                        myReader.Close();
                        myCon.Close();
                        // Authentication successful
                        return Ok(new { message = "Authentication successful" });
                    }
                    else
                    {
                        myReader.Close();
                        myCon.Close();
                        // Authentication failed
                        return Unauthorized(new { message = "Username or password is incorrect" });
                    }
                }
            }
        }

       


        [HttpGet("CountUsers")]
        public JsonResult GetAspCountUsers()
        {
            string query = @"
                    SELECT count(*) FROM aspnet_Users
                    ";

            int usersCount = 0;
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    // ExecuteScalar used for getting single value
                    usersCount = (int)myCommand.ExecuteScalar();
                }
                myCon.Close();
            }

            // Return the count as a JSON result
            return new JsonResult(new { UsersCount = usersCount });
        }

        [HttpGet("ApprovedAspUsersCount")]
        public JsonResult GetApprovedAspUsersCount()
        {
            string query = @"
            SELECT COUNT(*) FROM aspnet_Membership WHERE IsApproved = 1
            "; // Assuming IsApproved is a BIT column where 1 represents true

            int approvedUsersCount = 0;
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    // ExecuteScalar used for getting single value
                    approvedUsersCount = (int)myCommand.ExecuteScalar();
                }
                myCon.Close();
            }

            // Return the count as a JSON result
            return new JsonResult(new { ApprovedUsersCount = approvedUsersCount });
        }

        [HttpGet("UsersWithTwitterCommentsCount")]
        public JsonResult GetUsersWithTwitterCommentsCount()
        {
                    string query = @"
            SELECT COUNT(DISTINCT u.UserId) 
            FROM aspnet_Membership m
            JOIN aspnet_Users u ON m.UserId = u.UserId
            JOIN aspnet_Applications a ON u.ApplicationId = a.ApplicationId
            WHERE a.ApplicationId = 'c639a648-81ef-0290-6147-b33cbb161556'
            AND m.Comment IS NOT NULL 
            AND CAST(m.Comment AS NVARCHAR(MAX)) != ''           
            ";

            int usersWithTwitterCommentsCount = 0;
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    // ExecuteScalar used for getting single value
                    usersWithTwitterCommentsCount = Convert.ToInt32(myCommand.ExecuteScalar());
                }
                myCon.Close();
            }

            // Return the count as a JSON result
            return new JsonResult(new { UsersWithTwitterCommentsCount = usersWithTwitterCommentsCount });
        }


        [HttpGet("UsersWithInstagramCommentsCount")]
        public JsonResult GetUsersWithInstagramCommentsCount()
        {
            string query = @"
            SELECT COUNT(DISTINCT u.UserId) 
            FROM aspnet_Membership m
            JOIN aspnet_Users u ON m.UserId = u.UserId
            JOIN aspnet_Applications a ON u.ApplicationId = a.ApplicationId
            WHERE a.ApplicationId = '20267b2d-3b37-655d-d0a7-ed19b1ecfa19'
            AND m.Comment IS NOT NULL 
            AND CAST(m.Comment AS NVARCHAR(MAX)) != ''           
            ";

            int usersWithInstagramCommentsCount = 0;
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    // ExecuteScalar used for getting single value
                    usersWithInstagramCommentsCount = Convert.ToInt32(myCommand.ExecuteScalar());
                }
                myCon.Close();
            }

            // Return the count as a JSON result
            return new JsonResult(new { UsersWithInstagramCommentsCount = usersWithInstagramCommentsCount });
        }


        [HttpGet("UsersWithFaceBookCommentsCount")]
        public JsonResult GetUsersWithFaceBookCommentsCount()
        {
            string query = @"
            SELECT COUNT(DISTINCT u.UserId) 
            FROM aspnet_Membership m
            JOIN aspnet_Users u ON m.UserId = u.UserId
            JOIN aspnet_Applications a ON u.ApplicationId = a.ApplicationId
            WHERE a.ApplicationId = '5dcc8bc7-e777-aa7e-fc1b-4ecd723902c3'
            AND m.Comment IS NOT NULL           
            ";

            int usersWithFaceBookCommentsCount = 0;
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    // ExecuteScalar used for getting single value
                    usersWithFaceBookCommentsCount = Convert.ToInt32(myCommand.ExecuteScalar());
                }
                myCon.Close();
            }

            // Return the count as a JSON result
            return new JsonResult(new { UsersWithFaceBookCommentsCount = usersWithFaceBookCommentsCount });
        }


        [HttpGet("UsersWithTikTokCommentsCount")]
        public JsonResult GetUsersWithTikTokCommentsCount()
        {
            string query = @"
            SELECT COUNT(DISTINCT u.UserId) 
            FROM aspnet_Membership m
            JOIN aspnet_Users u ON m.UserId = u.UserId
            JOIN aspnet_Applications a ON u.ApplicationId = a.ApplicationId
            WHERE a.ApplicationId = '8ac84650-d74f-5981-afbe-af170a45437f'
            AND m.Comment IS NOT NULL           
            ";

            int usersWithTikTokCommentsCount = 0;
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    // ExecuteScalar used for getting single value
                    usersWithTikTokCommentsCount = Convert.ToInt32(myCommand.ExecuteScalar());
                }
                myCon.Close();
            }

            // Return the count as a JSON result
            return new JsonResult(new { UsersWithTikTokCommentsCount = usersWithTikTokCommentsCount });
        }

        [HttpGet("UsersWithInstagramCommentsAndMobileAliasCount")]
        public JsonResult GetusersWithInstagramCommentsAndMobileAliasCount()
        {
            string query = @"
            SELECT COUNT(DISTINCT u.UserId) 
            FROM aspnet_Membership m
            JOIN aspnet_Users u ON m.UserId = u.UserId
            JOIN aspnet_Applications a ON u.ApplicationId = a.ApplicationId
            WHERE a.ApplicationId = '20267b2d-3b37-655d-d0a7-ed19b1ecfa19'
            AND m.Comment IS NOT NULL 
            AND CAST(m.Comment AS NVARCHAR(MAX)) != '' 
            AND u.MobileAlias IS NOT NULL AND u.MobileAlias != ''
            ";

            int usersWithInstagramCommentsAndMobileAliasCount = 0;
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    // ExecuteScalar used for getting single value
                    usersWithInstagramCommentsAndMobileAliasCount = Convert.ToInt32(myCommand.ExecuteScalar());
                }
                myCon.Close();
            }

            // Return the count as a JSON result
            return new JsonResult(new { UsersWithInstagramCommentsAndMobileAliasCount = usersWithInstagramCommentsAndMobileAliasCount });
        }


        [HttpGet("UsersWithFaceBookCommentsAndMobileAliasCount")]
        public JsonResult GetUsersWithFaceBookCommentsAndMobileAliasCount()
        {
            string query = @"
            SELECT COUNT(DISTINCT u.UserId) 
            FROM aspnet_Membership m
            JOIN aspnet_Users u ON m.UserId = u.UserId
            JOIN aspnet_Applications a ON u.ApplicationId = a.ApplicationId
            WHERE a.ApplicationId = '5dcc8bc7-e777-aa7e-fc1b-4ecd723902c3'
            AND m.Comment IS NOT NULL 
            AND CAST(m.Comment AS NVARCHAR(MAX)) != '' 
            AND u.MobileAlias IS NOT NULL AND u.MobileAlias != ''
            ";

            int usersWithFaceBookCommentsAndMobileAliasCount = 0;
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    // ExecuteScalar used for getting single value
                    usersWithFaceBookCommentsAndMobileAliasCount = Convert.ToInt32(myCommand.ExecuteScalar());
                }
                myCon.Close();
            }

            // Return the count as a JSON result
            return new JsonResult(new { UsersWithFaceBookCommentsAndMobileAliasCount = usersWithFaceBookCommentsAndMobileAliasCount });
        }


        [HttpGet("UsersWithTwitterCommentsAndMobileAliasCount")]
        public JsonResult GetUsersWithTwitterCommentsAndMobileAliasCount()
        {
            string query = @"
            SELECT COUNT(DISTINCT u.UserId) 
            FROM aspnet_Membership m
            JOIN aspnet_Users u ON m.UserId = u.UserId
            JOIN aspnet_Applications a ON u.ApplicationId = a.ApplicationId
            WHERE a.ApplicationId = 'c639a648-81ef-0290-6147-b33cbb161556'
            AND m.Comment IS NOT NULL 
            AND CAST(m.Comment AS NVARCHAR(MAX)) != '' 
            AND u.MobileAlias IS NOT NULL AND u.MobileAlias != ''
            ";

            int usersWithTwitterCommentsAndMobileAliasCount = 0;
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    // ExecuteScalar used for getting single value
                    usersWithTwitterCommentsAndMobileAliasCount = Convert.ToInt32(myCommand.ExecuteScalar());
                }
                myCon.Close();
            }

            // Return the count as a JSON result
            return new JsonResult(new { UsersWithTwitterCommentsAndMobileAliasCount = usersWithTwitterCommentsAndMobileAliasCount });
        }


        [HttpGet("UsersWithTikTokCommentsAndMobileAliasCount")]
        public JsonResult GetUsersWithTikTokCommentsAndMobileAliasCount()
        {
            string query = @"
            SELECT COUNT(DISTINCT u.UserId) 
            FROM aspnet_Membership m
            JOIN aspnet_Users u ON m.UserId = u.UserId
            JOIN aspnet_Applications a ON u.ApplicationId = a.ApplicationId
            WHERE a.ApplicationId = '8ac84650-d74f-5981-afbe-af170a45437f'
            AND m.Comment IS NOT NULL 
            AND CAST(m.Comment AS NVARCHAR(MAX)) != '' 
            AND u.MobileAlias IS NOT NULL AND u.MobileAlias != ''
            ";

            int usersWithTikTokCommentsAndMobileAliasCount = 0;
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    // ExecuteScalar used for getting single value
                    usersWithTikTokCommentsAndMobileAliasCount = Convert.ToInt32(myCommand.ExecuteScalar());
                }
                myCon.Close();
            }

            // Return the count as a JSON result
            return new JsonResult(new { UsersWithTikTokCommentsAndMobileAliasCount = usersWithTikTokCommentsAndMobileAliasCount });
        }



        [HttpGet("LockedOutAspUsersCount")]
        public JsonResult GetLockedOutAspUsersCount()
        {
            string query = @"
            SELECT COUNT(*) FROM aspnet_Membership WHERE IsLockedOut = 1
            "; // Assuming IsApproved is a BIT column where 1 represents true

            int lockedOutAspUsersCount = 0;
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    // ExecuteScalar used for getting single value
                    lockedOutAspUsersCount = (int)myCommand.ExecuteScalar();
                }
                myCon.Close();
            }

            // Return the count as a JSON result
            return new JsonResult(new { LockedOutAspUsersCount = lockedOutAspUsersCount });
        }

        [HttpGet("FacebookUsersCount")]
        public JsonResult GetFacebookUsersCount()
        {
            string facebookAppId = "5dcbc8c7-e77f-a7ae-fc1b-4ecd723902c3"; // ID of Facebook application from your data
                        string query = @"
                SELECT COUNT(DISTINCT u.UserId) 
                FROM aspnet_Users u
                INNER JOIN aspnet_Applications a ON u.ApplicationId = a.ApplicationId
                WHERE a.ApplicationId = @facebookAppId
                "; // Query to count users associated with Facebook application

            int facebookUsersCount = 0;
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    // Add the parameter to avoid SQL injection
                    myCommand.Parameters.AddWithValue("@facebookAppId", facebookAppId);

                    // ExecuteScalar used for getting single value
                    facebookUsersCount = (int)myCommand.ExecuteScalar();
                }
                myCon.Close();
            }

            // Return the count as a JSON result
            return new JsonResult(new { FacebookUsersCount = facebookUsersCount });
        }



        [HttpGet("HighFailedPasswordAttemptCount")]
        public JsonResult GetHighFailedPasswordAttemptCount()
        {
            // Define the threshold as a constant or get it from configuration if it may change
            const int failedPasswordAttemptThreshold = 5;

            // SQL query to count users with FailedPasswordAttemptCount greater than the threshold
            string query = @"
        SELECT COUNT(*) FROM aspnet_Membership
        WHERE FailedPasswordAttemptCount > @FailedPasswordAttemptThreshold
    ";

            int count = 0;
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    // Add the threshold value as a parameter to the command to prevent SQL injection
                    myCommand.Parameters.AddWithValue("@FailedPasswordAttemptThreshold", failedPasswordAttemptThreshold);

                    // Execute the command and get the count of users
                    count = Convert.ToInt32(myCommand.ExecuteScalar());
                }
                myCon.Close();
            }

            // Return the count as part of a JSON result
            return new JsonResult(new { HighFailedPasswordAttemptCount = count });
        }




        [HttpGet("UsersWithHighFailedPasswordAttempts")]
        public JsonResult GetUsersWithHighFailedPasswordAttempts()
        {
            string query = @"
            SELECT * FROM aspnet_Membership
            WHERE FailedPasswordAttemptCount > @FailedPasswordAttemptThreshold
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    // Define the parameter used in the SQL query
                    myCommand.Parameters.AddWithValue("@FailedPasswordAttemptThreshold", 205231199);

                    using (SqlDataReader myReader = myCommand.ExecuteReader())
                    {
                        table.Load(myReader);
                    }
                }
                myCon.Close();
            }

            // Convert the DataTable to a list of dynamic objects to serialize to JSON
            var list = new List<dynamic>();
            foreach (DataRow row in table.Rows)
            {
                // This will convert each DataRow into a dynamic object
                var dict = row.Table.Columns.Cast<DataColumn>()
                                 .ToDictionary(col => col.ColumnName, col => row[col]);
                var expandoDict = new ExpandoObject() as IDictionary<string, Object>;
                foreach (var keyValuePair in dict)
                {
                    expandoDict.Add(keyValuePair);
                }
                list.Add(expandoDict);
            }

            return new JsonResult(list);
        }




        [HttpGet("GetAspUsersPage")]
        public ActionResult GetAspUsersPage([FromQuery] int currentPage, [FromQuery] int itemsPerPage)
        {
            if (currentPage < 1 || itemsPerPage < 1)
            {
                return BadRequest("currentPage and itemsPerPage must be greater than 0.");
            }

            try
            {
                string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();

                    // Calculate the number of rows to skip
                    int skip = (currentPage - 1) * itemsPerPage;

                    // Using a parameterized query to prevent SQL injection
                    string query = @"
                SELECT * FROM aspnet_Users
                ORDER BY UserName
                OFFSET @Skip ROWS FETCH NEXT @ItemsPerPage ROWS ONLY";

                    DataTable table = new DataTable();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.Parameters.AddWithValue("@Skip", skip);
                        myCommand.Parameters.AddWithValue("@ItemsPerPage", itemsPerPage);

                        using (SqlDataReader myReader = myCommand.ExecuteReader())
                        {
                            table.Load(myReader);
                        }
                    }

                    // Calculate total items for pagination metadata
                    string countQuery = "SELECT COUNT(*) FROM aspnet_Users";
                    int totalItems = 0;
                    using (SqlCommand countCommand = new SqlCommand(countQuery, myCon))
                    {
                        totalItems = Convert.ToInt32(countCommand.ExecuteScalar());
                    }

                    var result = new
                    {
                        TotalItems = totalItems,
                        TotalPages = (int)Math.Ceiling(totalItems / (double)itemsPerPage),
                        CurrentPage = currentPage,
                        ItemsPerPage = itemsPerPage,
                        Users = table
                    };

                    return new JsonResult(result);
                }
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, "An internal error occurred.");
            }
        }



        [HttpGet("count")]
        public JsonResult CountAspnetUsers()
        {
            string query = "SELECT COUNT(*) FROM aspnet_Users"; 

            int userCount = 0; 
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    // Sử dụng ExecuteScalar() để lấy số lượng người dùng vì câu truy vấn trả về một giá trị duy nhất
                    userCount = Convert.ToInt32(myCommand.ExecuteScalar());
                }
                myCon.Close();
            }

            // Tạo một đối tượng để trả về kết quả, bao gồm số lượng người dùng
            var result = new { UserCount = userCount };
            return new JsonResult(result);
        }

        [HttpPost("Adduser")]
        public JsonResult AddUser([FromBody] Aspnet_Users newUser)
        {
            // SQL query to insert a new row into the aspnet_Users table
            string query = @"
            INSERT INTO aspnet_Users (UserId, UserName, LoweredUserName, MobileAlias, IsAnonymous, LastActivityDate, ApplicationId)
            VALUES (NEWID(), @UserName, LOWER(@UserName), @MobileAlias, @IsAnonymous, @LastActivityDate,@ApplicationId)
            ";

            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            int rowsAffected = 0;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    // Add parameters to prevent SQL injection
                    myCommand.Parameters.AddWithValue("@UserName", newUser.UserName);
                    myCommand.Parameters.AddWithValue("@MobileAlias", newUser.MobileAlias ?? (object)DBNull.Value); // Use DBNull for null values
                    myCommand.Parameters.AddWithValue("@IsAnonymous", newUser.IsAnonymous);
                    myCommand.Parameters.AddWithValue("@LastActivityDate", newUser.LastActivityDate ?? DateTime.Now);
                    myCommand.Parameters.AddWithValue("@ApplicationId", "5DCC8BC7-E777-AA7E-FC1B-4ECD723902C3");

                    // Execute the query
                    rowsAffected = myCommand.ExecuteNonQuery();
                }
                myCon.Close();
            }

            // Check if the insert was successful
            if (rowsAffected > 0)
            {
                return new JsonResult("User Added Successfully");
            }
            else
            {
                return new JsonResult("Failed to Add User");
            }
        }

        [HttpGet("membershipCountByMonth")]
        public JsonResult GetMembershipCountByMonthAndYear()
        {
                    // SQL query to create a series of months and left join with the count of membership records
                    string query = @"
         WITH Months(Month) AS (
             SELECT 1 UNION ALL SELECT 2 UNION ALL SELECT 3 UNION ALL SELECT 4 UNION ALL 
             SELECT 5 UNION ALL SELECT 6 UNION ALL SELECT 7 UNION ALL SELECT 8 UNION ALL 
             SELECT 9 UNION ALL SELECT 10 UNION ALL SELECT 11 UNION ALL SELECT 12
         )
         SELECT  
             @year AS Year, 
             Months.Month, 
             ISNULL(CountData.TotalCount, 0) AS TotalCount
         FROM 
             Months
         LEFT JOIN (
             SELECT  
                 MONTH(CreateDate) as Month, 
                 COUNT(*) as TotalCount
             FROM 
                 aspnet_Membership
             WHERE
                 YEAR(CreateDate) = @year 
             GROUP BY 
                 MONTH(CreateDate)
         ) CountData ON Months.Month = CountData.Month
         ORDER BY 
             Months.Month";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    // Add the year parameter
                    //myCommand.Parameters.AddWithValue("@year", DateTime.Now);
                    myCommand.Parameters.AddWithValue("@year", "2023");
                    SqlDataReader myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                }
                myCon.Close();
            }

            var list = new List<Dictionary<string, object>>();
            foreach (DataRow row in table.Rows)
            {
                var dict = new Dictionary<string, object>
                {
                    ["Year"] = row["Year"],
                    ["Month"] = row["Month"],
                    ["TotalCount"] = row["TotalCount"]
                };
                list.Add(dict);
            }

            return new JsonResult(list);
        }

        [HttpGet("membershipCountByCurrentWeek")]
        public JsonResult GetMembershipCountByCurrentWeek()
        {
            // 1. Calculate Current Week's Start and End Dates
            var today = DateTime.Parse("2023-01-2");
            int currentDayOfWeek = (int)today.DayOfWeek;
            DateTime startOfWeek = today.AddDays(-currentDayOfWeek);
            DateTime endOfWeek = startOfWeek.AddDays(6);

            // 2. Construct SQL Query
            string query = @"
        WITH DaysOfWeek(DayNum) AS (
            SELECT 1 AS DayNum UNION ALL SELECT 2 UNION ALL SELECT 3 UNION ALL SELECT 4
            UNION ALL SELECT 5 UNION ALL SELECT 6 UNION ALL SELECT 7
        ),
        DaysOfWeekNames(DayNum, DayName) AS (
            SELECT DayNum, 
                CASE DayNum
                    WHEN 1 THEN 'Monday'
                    WHEN 2 THEN 'Tuesday'
                    WHEN 3 THEN 'Wednesday'
                    WHEN 4 THEN 'Thursday'
                    WHEN 5 THEN 'Friday'
                    WHEN 6 THEN 'Saturday'
                    WHEN 7 THEN 'Sunday'
                END AS DayName
            FROM DaysOfWeek
        )
        SELECT  
            DaysOfWeekNames.DayName AS Date,
            ISNULL(CountData.TotalCount, 0) AS TotalCount
        FROM 
            DaysOfWeekNames
        LEFT JOIN (
            SELECT  
                DATEPART(DAY, CreateDate) as Day, 
                COUNT(*) as TotalCount
            FROM 
                aspnet_Membership
            WHERE
                CreateDate >= @startDate AND CreateDate <= @endDate 
            GROUP BY 
                DATEPART(DAY, CreateDate)
        ) CountData ON DaysOfWeekNames.DayNum = CountData.Day
        ORDER BY 
            DaysOfWeekNames.DayNum";

            // 3. Database Connection and Command Setup
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    // 4. Add Parameters for Safe Execution
                    myCommand.Parameters.AddWithValue("@startDate", startOfWeek);
                    myCommand.Parameters.AddWithValue("@endDate", endOfWeek);

                    // 5. Execute Query and Load Results
                    SqlDataReader myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                }
                myCon.Close();
            }

            // 6. Transform Data for JSON
            var list = new List<Dictionary<string, object>>();
            foreach (DataRow row in table.Rows)
            {
                var dict = new Dictionary<string, object>
                {
                    ["Date"] = row["Date"],
                    ["TotalCount"] = row["TotalCount"]
                };
                list.Add(dict);
            }

            // 7. Return JSON Result
            return new JsonResult(list);
        }

        [HttpGet("membershipCountByYearForRecentYears")]
        public JsonResult GetMembershipCountByYearForRecentYears()
        {
            string query = @"
        SELECT
            YEAR(CreateDate) AS Year,
            COUNT(*) AS TotalCount
        FROM
            aspnet_Membership
        WHERE
            CreateDate >= DATEADD(YEAR, -5, GETDATE())
        GROUP BY
            YEAR(CreateDate)
        ORDER BY
            Year ASC";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    SqlDataReader myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                }
                myCon.Close();
            }

            var list = new List<Dictionary<string, object>>();
            foreach (DataRow row in table.Rows)
            {
                var dict = new Dictionary<string, object>
                {
                    ["Year"] = row["Year"],
                    ["TotalCount"] = row["TotalCount"]
                };
                list.Add(dict);
            }

            return new JsonResult(list);
        }


        [HttpGet("membershipCountByYear")]
        public JsonResult GetMembershipCountByCurrentYear()
        {
            int currentYear = DateTime.Now.Year-1; // Get the current year

            string query = @"
        SELECT
            YEAR(CreateDate) AS Year,
            COUNT(*) AS TotalCount
        FROM
            aspnet_Membership
        WHERE
            YEAR(CreateDate) = @year
        GROUP BY
            YEAR(CreateDate)";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@year", currentYear);

                    SqlDataReader myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                }
                myCon.Close();
            }

            var list = new List<Dictionary<string, object>>();
            if (table.Rows.Count > 0)
            {
                var row = table.Rows[0];
                var dict = new Dictionary<string, object>
                {
                    ["Year"] = row["Year"],
                    ["TotalCount"] = row["TotalCount"]
                };
                list.Add(dict);
            }
            else
            {
                // If no data is found for the current year, add an entry with a count of 0
                list.Add(new Dictionary<string, object>
                {
                    ["Year"] = currentYear,
                    ["TotalCount"] = 0
                });
            }

            return new JsonResult(list);
        }





        [HttpGet("membershipCountByPastYear")]
        public JsonResult GetMembershipCountByPastYear()
        {
            int Year = DateTime.Now.Year - 2; // Get the current year

            string query = @"
        SELECT
            YEAR(CreateDate) AS Year,
            COUNT(*) AS TotalCount
        FROM
            aspnet_Membership
        WHERE
            YEAR(CreateDate) = @year
        GROUP BY
            YEAR(CreateDate)";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@year", Year);

                    SqlDataReader myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                }
                myCon.Close();
            }

            var list = new List<Dictionary<string, object>>();
            if (table.Rows.Count > 0)
            {
                var row = table.Rows[0];
                var dict = new Dictionary<string, object>
                {
                    ["Year"] = row["Year"],
                    ["TotalCount"] = row["TotalCount"]
                };
                list.Add(dict);
            }
            else
            {
                // If no data is found for the current year, add an entry with a count of 0
                list.Add(new Dictionary<string, object>
                {
                    ["Year"] = Year,
                    ["TotalCount"] = 0
                });
            }

            return new JsonResult(list);
        }










    }
}
