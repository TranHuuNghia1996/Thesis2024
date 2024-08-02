using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using System.Web.Profile;
using System.Web.Security;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            return View(new LoginViewModel());
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
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }
            }

            return View(model);
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
            var allUsers = Membership.GetAllUsers().Cast<MembershipUser>();
            var totalUsers = allUsers.Count();

            var usersList = allUsers.Take(20).Select(user =>
            {
                var profile = ProfileBase.Create(user.UserName, true);
                return new WebApplication2.Models.UserViewModel
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    FirstName = (string)profile.GetPropertyValue("FirstName"),
                    LastName = (string)profile.GetPropertyValue("LastName"),
                    DateOfBirth = profile.GetPropertyValue("DateOfBirth") as DateTime?
                };
            }).ToList(); // Convert to List here

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

            for (int i = 0; i < 300000; i++)
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
            var allUsers = Membership.GetAllUsers().Cast<MembershipUser>();
            var results = allUsers.Where(u => u.UserName.Contains(query) || u.Email.Contains(query)).Select(user =>
            {
                var profile = ProfileBase.Create(user.UserName, true);
                return new UserViewModel
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    FirstName = (string)profile.GetPropertyValue("FirstName"),
                    LastName = (string)profile.GetPropertyValue("LastName"),
                    DateOfBirth = profile.GetPropertyValue("DateOfBirth") as DateTime?
                };
            }).ToList();

            return View(results);
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
