using Loowoo.LandInst.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new DBDataContext();
            var list = db.Profiles.AsQueryable();
            foreach (var item in list)
            {
                item.Json = Encoding.UTF8.GetString(item.Data).Replace("�", "").Replace("?,", "\",");
            }
            db.SaveChanges();
        }
    }
}
