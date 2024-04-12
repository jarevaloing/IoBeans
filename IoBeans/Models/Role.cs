using System;
using System.Collections.Generic;

namespace IoBeans.Models
{
    public partial class Role
    {
        public Role()
        {
            Logins = new HashSet<Login>();
        }

        public int RoleId { get; set; }
        public string RoleName { get; set; } = null!;

        public virtual ICollection<Login> Logins { get; set; }
    }
}
