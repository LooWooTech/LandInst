using Loowoo.LandInst.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Loowoo.LandInst.Model
{
    [Table("Profile")]
    public class Profile
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int UserID { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

        public bool? CheckResult { get; set; }

        public string Json { get; set; }

        //public byte[] Data { get; set; }

        public T Convert<T>()
        {
            if (string.IsNullOrEmpty(Json))
            {
                return default(T);
            }
            return Json.ToObject<T>();
        }
    }
}