using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Model
{
    public class AnnualCheckProfile
    {
        public AnnualCheckProfile()
        {
            Results = new List<BusinessResult>();
        }

        /// <summary>
        /// 从业人员操守
        /// </summary>
        public string EmployeeConduct { get; set; }

        /// <summary>
        /// 中介机构收费标准执行情况
        /// </summary>
        public string StandardsImplementation { get; set; }

        /// <summary>
        /// 经营情况及主要业绩
        /// </summary>
        public string BusinessSituation { get; set; }

        /// <summary>
        /// 获奖与投诉情况
        /// </summary>
        public string AwardsAndComplaints { get; set; }

        public List<BusinessResult> Results { get; set; }

        /// <summary>
        /// 年度工作总结
        /// </summary>
        public string Summary { get; set; }

    }

    public class BusinessResult
    {
        /// <summary>
        /// 经营业务类别
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// 件数
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// 收入
        /// </summary>
        public string Income { get; set; }
    }
}
