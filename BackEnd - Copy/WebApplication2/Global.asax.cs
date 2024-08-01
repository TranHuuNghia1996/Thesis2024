using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Profile;
using System.Web.Routing;
using System.Web.Security;

namespace WebApplication2
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            SeedMembershipData();
         
        }

        private void SeedMembershipData()
        {
            // Add roles
            string[] roles = { "Admin", "User" };
            foreach (var role in roles)
            {
                if (!Roles.RoleExists(role))
                {
                    Roles.CreateRole(role);
                }
            }

            // Add users
            if (Membership.GetUser("admin") == null)
            {
                MembershipCreateStatus createStatus;
                MembershipUser newUser = Membership.CreateUser("admin", "password", "admin@example.com", passwordQuestion: null, passwordAnswer: null, isApproved: true, providerUserKey: null, status: out createStatus);
                if (createStatus == MembershipCreateStatus.Success)
                {
                    Roles.AddUserToRole(newUser.UserName, "Admin");
                }
            }

            // Add more users and roles as needed
        }

        

    }
}
