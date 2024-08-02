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
        private List<MembershipUser> GetAllUsers()
        {
            return Membership.GetAllUsers().Cast<MembershipUser>().ToList();
        }

        public IHttpActionResult List()
        {
            var allUsers = GetAllUsers();

            // Lấy 10 người dùng mới nhất
            var newestUsers = allUsers.OrderByDescending(u => u.CreationDate).Take(10);

            var usersList = newestUsers.Select(user =>
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

            return Ok(usersList);
        }

        [HttpGet]
        public IHttpActionResult UserCount()
        {
            var allUsers = GetAllUsers();
            int userCount = allUsers.Count;
            return Ok(new { count = userCount });
        }

        [HttpGet]
        public IHttpActionResult BlockedUserCount()
        {
            var allUsers = GetAllUsers();
            int blockedUserCount = allUsers.Count(user => user.IsLockedOut);
            return Ok(new { count = blockedUserCount });
        }

        [HttpGet]
        public IHttpActionResult DailyCounts(int year, int month)
        {
            var dailyCounts = new List<DailyCount>();
            int daysInMonth = DateTime.DaysInMonth(year, month);
            var allUsers = GetAllUsers();

            for (int day = 1; day <= daysInMonth; day++)
            {
                var dayStart = new DateTime(year, month, day);
                var dayEnd = dayStart.AddDays(1);

                var count = allUsers.Count(user => user.CreationDate >= dayStart && user.CreationDate < dayEnd);

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
            var allUsers = GetAllUsers();

            for (int week = 0; week < 5; week++)
            {
                var weekStart = firstDayOfMonth.AddDays(week * 7);
                var weekEnd = weekStart.AddDays(7);

                if (weekStart.Month != month)
                    break;

                var count = allUsers.Count(user => user.CreationDate >= weekStart && user.CreationDate < weekEnd);

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
            var allUsers = GetAllUsers();

            for (int month = 1; month <= 12; month++)
            {
                var monthStart = new DateTime(year, month, 1);
                var monthEnd = monthStart.AddMonths(1);

                var count = allUsers.Count(user => user.CreationDate >= monthStart && user.CreationDate < monthEnd);

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
            var allUsers = GetAllUsers();

            for (int year = currentYear - 4; year <= currentYear; year++)
            {
                var yearStart = new DateTime(year, 1, 1);
                var yearEnd = yearStart.AddYears(1);

                var count = allUsers.Count(user => user.CreationDate >= yearStart && user.CreationDate < yearEnd);

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
            var allUsers = GetAllUsers();

            // Apply filters
            switch (filter.ToLower())
            {
                case "blocked":
                    allUsers = allUsers.Where(u => u.IsLockedOut).ToList();
                    break;
                case "notloggedinmonth":
                    var oneMonthAgo = DateTime.Now.AddMonths(-1);
                    allUsers = allUsers.Where(u => u.LastLoginDate < oneMonthAgo).ToList();
                    break;
                case "recent":
                default:
                    // No additional filtering
                    allUsers = allUsers.ToList();
                    break;
            }

            // Apply search query
            if (!string.IsNullOrEmpty(searchQuery))
            {
                allUsers = allUsers.Where(u => u.UserName.Contains(searchQuery) || u.Email.Contains(searchQuery)).ToList();
            }

            var totalCount = allUsers.Count;

            var users = allUsers
                .OrderByDescending(u => u.LastLoginDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(u =>
                {
                    var profile = ProfileBase.Create(u.UserName, true);
                    return new
                    {
                        u.UserName,
                        u.Email,
                        FirstName = (string)profile.GetPropertyValue("FirstName"),
                        LastName = (string)profile.GetPropertyValue("LastName"),
                        u.LastLoginDate
                    };
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

public class UserViewModel
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
}
