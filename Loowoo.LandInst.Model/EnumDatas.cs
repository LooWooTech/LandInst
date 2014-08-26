using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Model
{
    public enum City
    {
        浙江省, 杭州市, 宁波市, 温州市, 嘉兴市, 湖州市, 绍兴市, 金华市, 衢州市, 舟山市, 台州市, 丽水市
    }
    
    public enum Major
    {
        土地利用规划 = 1,
        土地管理,
        城市与区域规划,
        地理,
        农学,
        工程,
        环境,
        经济,
        法律,
        信息,
        其他
    }

    public enum ProfessionalLevel
    {
        高级 = 1,
        中级,
        初级,
    }

    public enum EduRecord
    {
        研究生及以上 = 1, 本科, 大专, 中专
    }
}
