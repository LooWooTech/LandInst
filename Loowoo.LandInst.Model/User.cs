using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loowoo.LandInst.Model
{
    [Table("User")]
    public class User
    {
        public User()
        {
            RegisterTime = DateTime.Now;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string PasswordQuestion { get; set; }

        public string PasswordAnswer { get; set; }

        public DateTime RegisterTime { get; set; }

        public DateTime? LastLoginTime { get; set; }

        public UserRole Role { get; set; }

        public bool Deleted { get; set; }
    }

    public enum UserRole
    {
        Everyone,
        Member,
        Institution,
        Admin
    }
}
