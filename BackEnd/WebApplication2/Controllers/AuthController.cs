using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Profile;
using System.Web.Security;


namespace WebApplication2.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AuthController : ApiController
    {

        [HttpPost]
        public IHttpActionResult Login(LoginRequest request)
        {
            if (Membership.ValidateUser(request.Username, request.Password))
            {
                var user = Membership.GetUser(request.Username);
                if (user != null)
                {
                    var roles = Roles.GetRolesForUser(request.Username);
                    if (roles.Contains("Admin"))
                    {
                        var userResponse = new UserResponse
                        {
                            Username = user.UserName,
                            Email = user.Email,
                            Role = "Admin"
                        };

                        // Generate a token if needed or return user info directly
                        return Ok(new { user = userResponse });
                    }
                    else
                    {
                        return Content(HttpStatusCode.Forbidden, "Access denied");
                    }
                }
            }

            return Content(HttpStatusCode.Unauthorized, "Incorrect username or password");
        }


    }


    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }


    public class UserResponse
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }




}
