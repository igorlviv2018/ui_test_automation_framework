using Taf.UI.Core.Enums;
using System.Collections.Generic;

namespace Taf.UI.Core.Models
{
    public class User
    {
        public string Email { get; set; }

        public string Password { get; set; } = string.Empty;

        public List<UserRole> Roles { get; set; } = new List<UserRole>();

        public TestEnvironment TestEnvironment { get; set; }

        public string ClientId { get; set; } = string.Empty;
    }
}
