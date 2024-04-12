using System;
using System.Collections.Generic;

namespace IoBeans.Models
{
    public partial class Login
    {
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int RoleId { get; set; }
    }
}
