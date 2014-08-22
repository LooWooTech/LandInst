using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace Loowoo.LandInst.Model
{
    [Table("CheckLog")]
    public class CheckLog
    {
        public CheckLog()
        {
            CreateTime = DateTime.Now;
        }

        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int UserID { get; set; }

        public int InfoID { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

        public bool? Result { get; set; }

        [MaxLength(255)]
        public string Note { get; set; }

        [Column(TypeName = "int")]
        public CheckType CheckType { get; set; }

        [NotMapped]
        public bool Checked { get { return Result.HasValue; } }

        /// <summary>
        /// 提交申请会附加一些数据，在审核成功之后使用，比如年检的ProfileID、培训报名的科目、会员转移到机构的ID，目前只支持存放一个值，如果需要多个值，请自行解析值的含义
        /// </summary>
        [MaxLength(255)]
        public string Data { get; set; }

        public int DataAsInt()
        {
            var result = 0;
            int.TryParse(Data, out result);
            return result;
        }

        ///// <summary>
        ///// 不支持多层name，比如 xxx.yyy
        ///// </summary>
        //public void SetData<T>(string key, T value)
        //{
        //    key = key.ToLower();
        //    if (!string.IsNullOrEmpty(Data))
        //    {
        //        var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(Data);
        //        if (dict.ContainsKey(key))
        //        {
        //            dict[key] = value;
        //        }
        //        else
        //        {
        //            dict.Add(key, value);
        //        }
        //        Data = JsonConvert.SerializeObject(dict);
        //    }
        //    else
        //    {
        //        var dict = new Dictionary<string, object>();
        //        dict.Add(key, value);
        //        Data = JsonConvert.SerializeObject(dict);
        //    }
        //}

        //public T GetData<T>(string key)
        //{
        //    if (string.IsNullOrEmpty(Data)) return default(T);
        //    key = key.ToLower();
        //    var data = JsonConvert.DeserializeObject<Dictionary<string,object>>(Data);
        //    if (data.ContainsKey(key))
        //    {
        //        return (T)data[key];
        //    }
        //    return default(T);
        //}
    }

    public enum CheckType
    {
        [Description("变更登记")]
        Profile = 1,
        [Description("会员转移")]
        Transfer = 2,
        [Description("执业登记")]
        Practice = 3,
        [Description("年检审核")]
        Annual = 4,
        [Description("继续教育")]
        Education = 5,
        [Description("报名培训")]
        Exam = 6
    }
}
