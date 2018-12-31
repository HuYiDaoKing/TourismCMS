using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TourismCMS.Models
{
    public class AdminUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IDCard { get; set; }
        public string Email { get; set; }
        public string AccountId { get; set; }
        public string Phone { get; set; }
        public string Position { get; set; }
        public string PasswordSalt { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}