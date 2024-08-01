using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Profile;
using System.Web.Security;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MembershipsController : ApiController
    {
        
        public IHttpActionResult List()
        {
            var allUsers = Membership.GetAllUsers().Cast<MembershipUser>();

            // Lấy 10 người dùng mới nhất
            var newestUsers = allUsers.OrderByDescending(u => u.CreationDate).Take(10);

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


            return Ok(usersList);
        }

        [HttpGet]     
        public IHttpActionResult UserCount()
        {
            var allUsers = Membership.GetAllUsers().Cast<MembershipUser>();
            int userCount = allUsers.Count();
            return Ok(new { count = userCount });
        }

        [HttpGet]
        public IHttpActionResult BlockedUserCount()
        {
            var allUsers = Membership.GetAllUsers().Cast<MembershipUser>();
            int blockedUserCount = allUsers.Count(user => user.IsLockedOut);
            return Ok(new { count = blockedUserCount });
        }


        [HttpGet]
        public IHttpActionResult DailyCounts(int year, int month)
        {
            var dailyCounts = new List<DailyCount>();
            int daysInMonth = DateTime.DaysInMonth(year, month);

            for (int day = 1; day <= daysInMonth; day++)
            {
                var dayStart = new DateTime(year, month, day);
                var dayEnd = dayStart.AddDays(1);

                var count = Membership.GetAllUsers()
                    .Cast<MembershipUser>()
                    .Count(user => user.CreationDate >= dayStart && user.CreationDate < dayEnd);

                dailyCounts.Add(new DailyCount
                {
                    Date = dayStart.ToString("yyyy-MM-dd"),
                    Count = count
                });
            }

            return Ok(dailyCounts);
        }

        [HttpGet]
        public IHttpActionResult WeeklyCounts(int year, int month)
        {
            var weeklyCounts = new List<WeeklyCount>();
            var firstDayOfMonth = new DateTime(year, month, 1);
            var daysInMonth = DateTime.DaysInMonth(year, month);

            for (int week = 0; week < 5; week++)
            {
                var weekStart = firstDayOfMonth.AddDays(week * 7);
                var weekEnd = weekStart.AddDays(7);

                if (weekStart.Month != month)
                    break;

                var count = Membership.GetAllUsers()
                    .Cast<MembershipUser>()
                    .Count(user => user.CreationDate >= weekStart && user.CreationDate < weekEnd);

                weeklyCounts.Add(new WeeklyCount
                {
                    Week = $"Week {week + 1} ({weekStart:MM/dd} - {weekEnd.AddDays(-1):MM/dd})",
                    Count = count
                });
            }

            return Ok(weeklyCounts);
        }

        [HttpGet]
        public IHttpActionResult MonthlyCounts(int year)
        {
            var monthlyCounts = new List<MonthlyCount>();

            for (int month = 1; month <= 12; month++)
            {
                var monthStart = new DateTime(year, month, 1);
                var monthEnd = monthStart.AddMonths(1);

                var count = Membership.GetAllUsers()
                    .Cast<MembershipUser>()
                    .Count(user => user.CreationDate >= monthStart && user.CreationDate < monthEnd);

                monthlyCounts.Add(new MonthlyCount
                {
                    Month = monthStart.ToString("yyyy-MM"),
                    Count = count
                });
            }

            return Ok(monthlyCounts);
        }

       
        [HttpGet]
        public IHttpActionResult YearlyCounts()
        {
            var yearlyCounts = new List<YearlyCount>();
            int currentYear = DateTime.Now.Year;

            for (int year = currentYear - 4; year <= currentYear; year++)
            {
                var yearStart = new DateTime(year, 1, 1);
                var yearEnd = yearStart.AddYears(1);

                var count = Membership.GetAllUsers()
                    .Cast<MembershipUser>()
                    .Count(user => user.CreationDate >= yearStart && user.CreationDate < yearEnd);

                yearlyCounts.Add(new YearlyCount
                {
                    Year = year,
                    Count = count
                });
            }

            return Ok(yearlyCounts);
        }



        [HttpGet]
        public IHttpActionResult PagedUsers(int pageNumber = 1, int pageSize = 100, string filter = "recent", string searchQuery = "")
        {
            var allUsers = Membership.GetAllUsers().Cast<MembershipUser>();

            // Apply filters
            switch (filter.ToLower())
            {
                case "blocked":
                    allUsers = allUsers.Where(u => u.IsLockedOut);
                    break;
                case "notloggedinmonth":
                    var oneMonthAgo = DateTime.Now.AddMonths(-1);
                    allUsers = allUsers.Where(u => u.LastLoginDate < oneMonthAgo);
                    break;
                case "recent":
                default:
                    // No additional filtering
                    break;
            }

            // Apply search query
            if (!string.IsNullOrEmpty(searchQuery))
            {
                allUsers = allUsers.Where(u => u.UserName.Contains(searchQuery) || u.Email.Contains(searchQuery));
            }

            var totalCount = allUsers.Count();

            var users = allUsers
                .OrderByDescending(u => u.LastLoginDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new
                {
                    u.UserName,
                    u.Email,
                    FirstName = (string)ProfileBase.Create(u.UserName, true).GetPropertyValue("FirstName"),
                    LastName = (string)ProfileBase.Create(u.UserName, true).GetPropertyValue("LastName"),
                    u.LastLoginDate
                })
                .ToList();

            return Ok(new { users, totalCount });
        }



    }
}

public class DailyCount
{
    public string Date { get; set; }
    public int Count { get; set; }
}

public class WeeklyCount
{
    public string Week { get; set; }
    public int Count { get; set; }
}

public class MonthlyCount
{
    public string Month { get; set; }
    public int Count { get; set; }
}

public class YearlyCount
{
    public int Year { get; set; }
    public int Count { get; set; }
}
