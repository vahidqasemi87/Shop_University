using System;
using System.Collections.Generic;

namespace SimpleShop.Models
{
    [Serializable]
    public partial class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Family { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public bool IsAdmin { get; set; }
    }
}
