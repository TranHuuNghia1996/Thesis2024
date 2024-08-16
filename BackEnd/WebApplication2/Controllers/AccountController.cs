using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Profile;
using System.Web.Security;
using WebApplication2.Models;
using WebApplication2.utilities;

namespace WebApplication2.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            DeleteAndRecreateUsers();
            return View(new LoginViewModel());
        }

        public void DeleteAndRecreateUsers()
        {
            string filePath = @"D:\Sau Dai Hoc\Thesis\Project\Jmeter\Datatemps\login_users.csv";
            try
            {
                // Đọc danh sách người dùng từ tệp CSV
                var lines = System.IO.File.ReadAllLines(filePath);

                foreach (var line in lines)
                {
                    var data = line.Split(',');

                    if (data.Length == 2)
                    {
                        string username = data[0];
                        string password = data[1];

                        // Xóa người dùng nếu tồn tại
                        MembershipUser user = Membership.GetUser(username);
                        if (user != null)
                        {
                            bool deleteResult = Membership.DeleteUser(username, true);
                            if (!deleteResult)
                            {
                                Console.WriteLine($"Failed to delete user '{username}'. Skipping creation.");
                                continue; // Bỏ qua việc tạo lại nếu không thể xóa người dùng
                            }
                        }

                        var random = new Random();
                        var email = $"{username}@example.com";
                        MembershipCreateStatus createStatus;
                        MembershipUser newUser = Membership.CreateUser(
                            username, password, email,
                            "favorite pet?", "fluffy",
                            isApproved: true, null, out createStatus);



                        if (createStatus == MembershipCreateStatus.Success)
                        {
                            try
                            {
                                var roles = Roles.GetAllRoles();
                                var randomRole = roles[new Random().Next(0, roles.Length)];
                                Roles.AddUserToRole(username, randomRole);
                            }
                            catch (Exception e)
                            {
                                throw e;
                            }

                            try
                            {
                                SaveUserProfile(username, $"FirstName{username}", $"LastName{username}");
                            }
                            catch (Exception profileEx)
                            {
                                Console.WriteLine($"Error saving profile for user {username}: {profileEx}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Error creating user {username}: {createStatus}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        [HttpPost]
        public ActionResult Login(string userName, string password)
        {

            var user = Membership.GetUser(userName);
            if (user != null && user.IsLockedOut)
            {
                ModelState.AddModelError("", "Your account has been blocked. Please contact support.");
                return View();
            }

            if (Membership.ValidateUser(userName, password))
            {
                FormsAuthentication.SetAuthCookie(userName, false);
                TempData["Username"] = userName;
                return RedirectToAction("LoginSuccess", "Account");
            }

            ModelState.AddModelError("", "Invalid username or password.");
            return View();
        }

        //public ActionResult Login(string userName, string password)
        //{

        //    if (MembershipUtil.ValidateUser(userName, password))
        //    {
        //        FormsAuthentication.SetAuthCookie(userName, false);
        //        TempData["Username"] = userName;
        //        return RedirectToAction("LoginSuccess", "Account");
        //    }

        //    ModelState.AddModelError("", "Invalid username or password.");
        //    return View();
        //}
      

        public ActionResult LoginSuccess()
        {
            string userName = TempData["Username"] as string;
            ViewBag.UserName = userName;
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                MembershipCreateStatus createStatus;
                MembershipUser newUser = Membership.CreateUser(
                    model.UserName,
                    model.Password,
                    model.Email,
                    model.SecurityQuestion,
                    model.SecurityAnswer,
                    isApproved: true,
                    providerUserKey: null,
                    status: out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    dynamic profile = ProfileBase.Create(model.UserName, true);
                    profile.SetPropertyValue("FirstName", model.FirstName);
                    profile.SetPropertyValue("LastName", model.LastName);
                    profile.SetPropertyValue("DateOfBirth", model.DateOfBirth);
                    profile.Save();

                    Roles.AddUserToRole(model.UserName, "Admin");

                    FormsAuthentication.SetAuthCookie(model.UserName, false);


                    TempData["Username"] = model.UserName;
                    return RedirectToAction("RegisterSuccess");
                }
                else
                {
                    string errorMessage = ErrorCodeToString(createStatus);
                    ModelState.AddModelError("", errorMessage);
                }
            }

            return View(model);
        }


        public ActionResult RegisterSuccess()
        {
            return View();
        }






        void ExportUsers()
        {
            // Define the directory and file paths
            string directoryPath = @"D:\Sau Dai Hoc\Thesis\Project\Jmeter\Datatemps";
            string loginFilePath = Path.Combine(directoryPath, "login_users.csv");

            // Maximum number of users to export
            int maxUsers = 10000;

            // Ensure the directory exists
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Load existing users from the CSV file if it exists
            var existingUsers = System.IO.File.Exists(loginFilePath)
                ? System.IO.File.ReadAllLines(loginFilePath).Select(line => line.Split(',')[0]).ToHashSet()
                : new HashSet<string>();

            // Initialize the StringBuilder to hold the new CSV content
            StringBuilder loginCsvContent = new StringBuilder();

            // Get all users from the ASP.NET Membership system
            int totalUsers;
            MembershipUserCollection users = Membership.GetAllUsers(0, int.MaxValue, out totalUsers);

            int userCount = 0;

            foreach (MembershipUser user in users)
            {
                if (userCount >= maxUsers)
                    break;

                // Check if the user is not locked out (not blocked)
                if (!user.IsLockedOut)
                {
                    string userName = user.UserName;
                    string password = "123"; // Retrieve the password (placeholder here)

                    // Only add users who are not already in the CSV file
                    if (!existingUsers.Contains(userName))
                    {
                        loginCsvContent.AppendLine($"{userName},{password}");
                        userCount++;
                    }
                }
            }

            // Write the CSV content to the file
            System.IO.File.WriteAllText(loginFilePath, loginCsvContent.ToString());

            Console.WriteLine($"Exported {userCount} users to {loginFilePath}");
        }


        private string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "The username already exists.";
                case MembershipCreateStatus.DuplicateEmail:
                    return "The email address already exists.";
                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid.";
                case MembershipCreateStatus.InvalidEmail:
                    return "The email address provided is invalid.";
                case MembershipCreateStatus.InvalidAnswer:
                    return "The security answer provided is invalid.";
                case MembershipCreateStatus.InvalidQuestion:
                    return "The security question provided is invalid.";
                case MembershipCreateStatus.InvalidUserName:
                    return "The username provided is invalid.";
                case MembershipCreateStatus.ProviderError:
                    return "The provider returned an error.";
                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled.";
                default:
                    return "An unknown error occurred.";
            }
        }

        [HttpGet]
        public ActionResult List()
        {
            int totalUsers;
            int pageSize = 50;
            int pageIndex = 0;

            var users = Membership.GetAllUsers(pageIndex, pageSize, out totalUsers);

            var usersList = users.Cast<MembershipUser>().Select(user =>
            {
                var profile = ProfileBase.Create(user.UserName, true);
                return new UsersViewModel
                {
                    UserName = user.UserName ?? "Unknown",
                    Email = user.Email ?? "Unknown",
                    FirstName = profile.GetPropertyValue("FirstName") as string ?? "Unknown",
                    LastName = profile.GetPropertyValue("LastName") as string ?? "Unknown",
                    DateOfBirth = profile.GetPropertyValue("DateOfBirth") as DateTime?
                };
            }).ToList();

            ViewBag.TotalUsers = totalUsers;
            return View(usersList);
        }




        [HttpGet]
        public ActionResult SeedData()
        {
            SeedUsers();
            var allUsers = Membership.GetAllUsers().Cast<MembershipUser>();
            int newestUsers = allUsers.OrderByDescending(u => u.CreationDate).Count();
            return View(newestUsers);
        }

        public void SeedUsers()
        {
            string strGen = GetRandomString(9);

            for (int i = 0; i < 200000; i++)
            {
                var random = new Random();
                bool isLocked = random.Next(2) == 1;
                var userName = $"user{i}{strGen}";
                var password = "123";
                var email = $"user{i}{strGen}@example.com";

                MembershipCreateStatus createStatus;
                MembershipUser newUser = Membership.CreateUser(
                    userName, password, email,
                    "favorite pet?", "fluffy",
                    isApproved: true, null, out createStatus);

                if (!isLocked)
                {
                    SimulateFailedLogins(userName, 1);
                }

                if (createStatus == MembershipCreateStatus.Success)
                {
                    try
                    {
                        var roles = Roles.GetAllRoles();
                        var randomRole = roles[new Random().Next(0, roles.Length)];
                        Roles.AddUserToRole(userName, randomRole);
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }

                    try
                    {
                        SaveUserProfile(userName, $"FirstName{i}", $"LastName{i}");
                    }
                    catch (Exception profileEx)
                    {
                        Console.WriteLine($"Error saving profile for user {userName}: {profileEx}");
                    }
                }
                else
                {
                    Console.WriteLine($"Error creating user {userName}: {createStatus}");
                }
            }
        }

        private static readonly ThreadLocal<Random> randomWrapper = new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref seed)));
        private static int seed = Environment.TickCount;

        [HttpGet]
        public ActionResult Search(string query)
        {
            var results = new List<UsersViewModel>();

            if (!string.IsNullOrEmpty(query))
            {
                MembershipUserCollection usersByEmail = Membership.FindUsersByEmail($"%{query}%");

                results = usersByEmail.Cast<MembershipUser>()
                    .Select(user =>
                    {
                        var profile = ProfileBase.Create(user.UserName, true);
                        return new UsersViewModel
                        {
                            UserName = user.UserName ?? "Unknown",
                            Email = user.Email ?? "Unknown",
                            FirstName = profile.GetPropertyValue("FirstName") as string ?? "Unknown",
                            LastName = profile.GetPropertyValue("LastName") as string ?? "Unknown",
                            DateOfBirth = profile.GetPropertyValue("DateOfBirth") as DateTime?
                        };
                    }).ToList();
            }

            var viewModel = new SearchResultsViewModel
            {
                Users = results,
                Query = query
            };

            return View(viewModel);
        }


        private void SimulateFailedLogins(string userName, int attempts)
        {
            for (int attempt = 0; attempt < attempts; attempt++)
            {
                Membership.ValidateUser(userName, "wrongPassword");
            }
        }

        private void SaveUserProfile(string userName, string firstName, string lastName)
        {
            var profile = ProfileBase.Create(userName, true);
            profile.SetPropertyValue("FirstName", firstName);
            profile.SetPropertyValue("LastName", lastName);
            profile.SetPropertyValue("DateOfBirth", RandomDay());
            profile.Save();
        }

        public string GetRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[randomWrapper.Value.Next(0, s.Length)]).ToArray());
        }

        private DateTime RandomDay()
        {
            DateTime start = new DateTime(1970, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(randomWrapper.Value.Next(0, range));
        }
    }
}
