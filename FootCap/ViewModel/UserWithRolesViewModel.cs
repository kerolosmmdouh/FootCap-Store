using FootCap.Models;
using System.Collections.Generic;

namespace FootCap.ViewModel
{
    public class UserWithRolesViewModel
    {
        public User User { get; set; }
        public IList<string> Roles { get; set; }
    }
}
