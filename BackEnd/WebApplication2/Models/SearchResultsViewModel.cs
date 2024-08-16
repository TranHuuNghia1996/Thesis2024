using System.Collections.Generic;

namespace WebApplication2.Models
{
    public class SearchResultsViewModel
    {
        public List<UsersViewModel> Users { get; set; }
        public string Query { get; set; }
    }
}
