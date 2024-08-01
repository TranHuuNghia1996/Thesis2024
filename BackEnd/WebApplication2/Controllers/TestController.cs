using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Profile;
using System.Web.Security;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class TestController : ApiController
    {
        
        public IHttpActionResult List()
        {
            var allUsers = Membership.GetAllUsers().Cast<MembershipUser>();

            // Lấy 10 người dùng mới nhất
            var newestUsers = allUsers.OrderByDescending(u => u.CreationDate).Take(10);

            var usersList = new List<UserViewModel>();
            foreach (var user in newestUsers)
            {
                var profile = ProfileBase.Create(user.UserName, true); // true để chỉ tạo ra profile nếu nó đã tồn tại
                var model = new UserViewModel
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    FirstName = (string)profile.GetPropertyValue("FirstName"),
                    LastName = (string)profile.GetPropertyValue("LastName"),
                    // Chú ý: GetPropertyValue có thể trả về null, hãy chắc chắn rằng bạn xử lý trường hợp này
                    DateOfBirth = profile.GetPropertyValue("DateOfBirth") as DateTime?
                };

                usersList.Add(model);
            }


            return Ok(usersList);
        }
    }
}
