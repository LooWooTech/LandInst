using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Model
{
    public class Equipment
    {
        public string Name { get; set; }

        public int? Number { get; set; }

        /// <summary>
        /// 型号
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// 生产厂商
        /// </summary>
        public string Manufacturer { get; set; }

        public string Performance { get; set; }

        public string Note { get; set; }

        public static List<Equipment> GetList(NameValueCollection requestForm)
        {
            var list = new List<Equipment>();
            try
            {
                var equipmentNames = requestForm["equipment.Name"].Split(',');
                var equipmentNumbers = requestForm["equipment.Number"].Split(',');
                var equipmentModels = requestForm["equipment.Model"].Split(',');
                var equipmentManufacturers = requestForm["equipment.Manufacturer"].Split(',');
                var equipmentPerformances = requestForm["equipment.Performance"].Split(',');
                var equipmentNotes = requestForm["equipment.Note"].Split(',');
                for (var i = 0; i < equipmentNames.Length; i++)
                {
                    var number = 0;
                    int.TryParse(equipmentNames[i], out number);
                    list.Add(new Equipment
                    {
                        Name = equipmentNames[i],
                        Number = number,
                        Model = equipmentModels[i],
                        Manufacturer = equipmentManufacturers[i],
                        Performance = equipmentPerformances[i],
                        Note = equipmentNotes[i]
                    });
                }
            }
            catch { }

            return list;
        }
    }
}