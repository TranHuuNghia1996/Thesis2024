using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
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

            return View(new LoginViewModel());
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Login(string userName, string password)
        //{
        //    if (MembershipUtil.ValidateUser(userName, password))
        //    {
        //        FormsAuthentication.SetAuthCookie(userName, false);
        //        return RedirectToAction("Index", "Home");
        //    }

        //    ModelState.AddModelError("", "Invalid username or password.");
        //    return View();
        //}
        public ActionResult Login(string userName, string password)
        {
            if (Membership.ValidateUser(userName, password))
            {
                FormsAuthentication.SetAuthCookie(userName, false);
                TempData["Username"] = userName;
                return RedirectToAction("LoginSuccess", "Account");
            }

            ModelState.AddModelError("", "Invalid username or password.");
            return View();
        }

        public ActionResult LoginSuccess(string userName)
        {
            ViewBag.UserName = userName;
            return View();
        }


        [HttpGet]
        public ActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Register(RegisterViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        MembershipCreateStatus createStatus;
        //        MembershipUtil.CreateUser(
        //            model.UserName,
        //            model.Password,
        //            model.Email,
        //            model.SecurityQuestion,
        //            model.SecurityAnswer,
        //            isApproved: true,
        //            providerUserKey: null,
        //            out createStatus);

        //        if (createStatus == MembershipCreateStatus.Success)
        //        {
        //            // Lưu thông tin profile
        //            dynamic profile = ProfileBase.Create(model.UserName, true);
        //            profile.SetPropertyValue("FirstName", model.FirstName);
        //            profile.SetPropertyValue("LastName", model.LastName);
        //            profile.SetPropertyValue("DateOfBirth", model.DateOfBirth);
        //            profile.Save();

        //            // Đăng nhập tự động cho người dùng mới
        //            FormsAuthentication.SetAuthCookie(model.UserName, false);
        //            return RedirectToAction("Index", "Home");
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", ErrorCodeToString(createStatus));
        //        }
        //    }

        //    return View(model);
        //}


        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Tạo người dùng
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
                    // Lưu thông tin profile
                    dynamic profile = ProfileBase.Create(model.UserName, true);
                    profile.SetPropertyValue("FirstName", model.FirstName);
                    profile.SetPropertyValue("LastName", model.LastName);
                    profile.SetPropertyValue("DateOfBirth", model.DateOfBirth);
                    profile.Save();

                    // Thêm người dùng vào vai trò "User"
                    Roles.AddUserToRole(model.UserName, "Admin");

                    // Đăng nhập tự động cho người dùng mới
                    FormsAuthentication.SetAuthCookie(model.UserName, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }
            }

            // Nếu có lỗi, hiển thị lại form đăng ký
            return View(model);
        }

        private string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // Chuyển đổi mã lỗi thành chuỗi thông báo tương ứng
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Tên người dùng đã tồn tại.";
                case MembershipCreateStatus.DuplicateEmail:
                    return "Địa chỉ email đã tồn tại.";
                case MembershipCreateStatus.InvalidPassword:
                    return "Mật khẩu không hợp lệ.";
                case MembershipCreateStatus.InvalidEmail:
                    return "Địa chỉ email không hợp lệ.";
                case MembershipCreateStatus.InvalidAnswer:
                    return "Câu trả lời không hợp lệ.";
                case MembershipCreateStatus.InvalidQuestion:
                    return "Câu hỏi không hợp lệ.";
                case MembershipCreateStatus.InvalidUserName:
                    return "Tên người dùng không hợp lệ.";
                case MembershipCreateStatus.ProviderError:
                    return "Lỗi nhà cung cấp.";
                case MembershipCreateStatus.UserRejected:
                    return "Người dùng bị từ chối.";
                default:
                    return "Lỗi không xác định.";
            }
        }

        [HttpGet]
        public ActionResult List()
        {
            var allUsers = Membership.GetAllUsers().Cast<MembershipUser>();


            var newestUsersCount = allUsers.ToList().Count;

            // Lấy 10 người dùng mới nhất
            var newestUsers = allUsers.OrderByDescending(u => u.CreationDate).Take(20);

            var usersList = new List<UserViewModel>();
            foreach (var user in newestUsers)
            {
                var profile = ProfileBase.Create(user.UserName, true); 
                var model = new UserViewModel
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    FirstName = (string)profile.GetPropertyValue("FirstName"),
                    LastName = (string)profile.GetPropertyValue("LastName"),

                    DateOfBirth = profile.GetPropertyValue("DateOfBirth") as DateTime?
                };

                usersList.Add(model);
            }


            return View(usersList);
        }


        [HttpGet]
        public ActionResult SeedData()
        {

            SeedUsers();

            //SeedUsersParallel();

            //SeedUsersAsync();
            //SeedUsersAsyncBatchSize();
            var allUsers = Membership.GetAllUsers().Cast<MembershipUser>();
            int newestUsers = allUsers.OrderByDescending(u => u.CreationDate).Count();
            return View(newestUsers);
        }





        public static string GetRandomString1(int length)
        {
           
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();


            var randomString = "";


            for (var i = 0; i < length; i++)
            {

                var randomIndex = random.Next(chars.Length);

                randomString += chars[randomIndex];
            }


            return randomString;
        }


        public void SeedUsers()
        {

            string strGen = GetRandomString1(9);

            for (int i = 0; i < 300000; i++)
            {
                var random = new Random();
                bool isLocked = random.Next(2) == 1;
                var userName = $"user{i}{strGen}";
                var password = $"123";
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
                    //AssignUserToRandomRole(userName);

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

                    // MembershipUtil.UpdateMobilePINForUser(userName, new Random().Next(2) == 0 ? GetRandomString(9) : null);
                }
                else
                {
                    Console.WriteLine($"Error creating user {userName}: {createStatus}");
                }
            }
        }


        public class ThreadSafeRandom
        {
            private static readonly Random globalRandom = new Random();
            [ThreadStatic] private static Random localRandom;

            public static int Next(int minValue, int maxValue)
            {
                Random inst = localRandom;
                if (inst == null)
                {
                    int seed;
                    lock (globalRandom)
                    {
                        seed = globalRandom.Next();
                    }
                    localRandom = inst = new Random(seed);
                }
                return inst.Next(minValue, maxValue);
            }
        }


        public async Task SeedUsersAsyncBatchSize()
        {
            const int batchSize = 100000;
            const int totalUsers = 100000000;
            const int totalBatches = totalUsers / batchSize;

            for (int batch = 0; batch < totalBatches; batch++)
            {
                List<Task> tasks = new List<Task>();

                for (int i = 0; i < batchSize; i++)
                {
                    int userId = batch * batchSize + i;
                    tasks.Add(CreateUserAsync(userId));
                }

                await Task.WhenAll(tasks);

            }
        }


        public async Task SeedUsersAsync()
        {
            List<Task> tasks = new List<Task>();

            for (int i = 0; i < 1000000; i++)
            {
                tasks.Add(CreateUserAsync(i));
            }

            await Task.WhenAll(tasks);
        }

        private async Task CreateUserAsync(int i)
        {

            string strGen = GetRandomString(9);
            bool isApproved = randomWrapper.Value.Next(2) == 1;
            var userName = $"user{i}{strGen}";
            var password = $"P@ssw0rd!{strGen}";
            var email = $"user{i}{strGen}@example.com";

            MembershipCreateStatus createStatus;
            MembershipUser newUser = Membership.CreateUser(
                userName, password, email,
                "favorite pet?", "fluffy",
                isApproved: isApproved, null, out createStatus);

            if (isApproved)
            {
                int randFailed = randomWrapper.Value.Next(1);
                await SimulateFailedLogins(userName, randFailed);
            }

            if (createStatus == MembershipCreateStatus.Success)
            {
                AssignUserToRandomRole(userName);

                try
                {
                    SaveUserProfile(userName, $"FirstName{i}", $"LastName{i}");
                }
                catch (Exception profileEx)
                {
                    Console.WriteLine($"Error saving profile for user {userName}: {profileEx}");
                }

                //MembershipUtil.UpdateMobilePINForUser(userName, new Random().Next(2) == 0 ? GetRandomString(9) : null);
            }
            else
            {
                Console.WriteLine($"Error creating user {userName}: {createStatus}");
            }
        }

        public void SeedUsersParallel()
        {
            Parallel.For(0, 1000000, i =>
            {
                string strGen = GetRandomString(9);
                bool isApproved = randomWrapper.Value.Next(2) == 1;
                var userName = $"user{i}{strGen}";
                var password = $"P@ssw0rd!{strGen}";
                var email = $"user{i}{strGen}@example.com";

                MembershipCreateStatus createStatus;
                MembershipUser newUser = Membership.CreateUser(
                    userName, password, email,
                    "favorite pet?", "fluffy",
                    isApproved: isApproved, null, out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    AssignUserToRandomRole(userName);
                    if (!isApproved)
                    {
                        int rand = randomWrapper.Value.Next(1);
                        FailedLogins(userName, rand);
                    }

                    try
                    {
                        SaveUserProfile(userName, $"FirstName{i}", $"LastName{i}");

                    }
                    catch (Exception profileEx)
                    {
                        Console.WriteLine($"Error saving profile for user {userName}: {profileEx}");
                    }

                    // MembershipUtil.UpdateMobilePINForUser(userName, randomWrapper.Value.Next(2) == 0 ? GetRandomString(9) : null);
                }
                else
                {
                    Console.WriteLine($"Error creating user {userName}: {createStatus}");
                }
            });
        }

        private static readonly ThreadLocal<Random> randomWrapper = new ThreadLocal<Random>(() =>
        new Random(Interlocked.Increment(ref seed)));

        private static int seed = Environment.TickCount;

        private void FailedLogins(string userName, int attempts)
        {
            for (int attempt = 0; attempt < attempts; attempt++)
            {
                Membership.ValidateUser(userName, "wrongPassword");
            }
        }

        private void AssignUserToRandomRole(string userName)
        {
            var roles = Roles.GetAllRoles();
            var randomRole = roles[ThreadSafeRandom.Next(0, roles.Length)];
            if (!Roles.RoleExists(randomRole))
            {
                Roles.CreateRole(randomRole);
            }

            Roles.AddUserToRole(userName, randomRole);
        }

        private async Task SimulateFailedLogins(string userName, int attempts)
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

        private void SetMobilePIN(string userName, string mobilePin)
        {
            // Logic to set the MobilePIN
        }

        public string GetRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[ThreadSafeRandom.Next(0, s.Length)]).ToArray());
        }

        private DateTime RandomDay()
        {
            DateTime start = new DateTime(1970, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(ThreadSafeRandom.Next(0, range));
        }



        private DateTime RandomDay1()
        {
            Random random = new Random();
            DateTime start = new DateTime(1970, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(random.Next(range));
        }



    }
}

