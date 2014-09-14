using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            LastLoginTime = RegisterTime;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Question { get; set; }

        public string Answer { get; set; }

        public DateTime RegisterTime { get; set; }

        public DateTime LastLoginTime { get; set; }

        [Column(TypeName = "int")]
        public UserRole Role { get; set; }

        public bool Deleted { get; set; }

        public string LastLoginIP { get; set; }

        public int LoginTimes { get; set; }
    }

    public enum UserRole
    {
        [Description("游客")]
        Everyone,
        [Description("会员")]
        Member,
        [Description("机构")]
        Institution,
        [Description("管理员")]
        Admin
    }
}
