using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Model
{
    public class Software
    {
        public string Name { get; set; }

        public int? Number { get; set; }

        public string Source { get; set; }

        public string Purpose { get; set; }

        public string Note { get; set; }

        public static List<Software> GetList(NameValueCollection requestForm)
        {
            var list = new List<Software>();
            try
            {
                var softwareNames = requestForm["software.Name"].Split(',');
                var softwareNumbers = requestForm["software.Number"].Split(',');
                var softwareSources = requestForm["software.Source"].Split(',');
                var softwarePurposes = requestForm["software.Purpose"].Split(',');
                var softwareNotes = requestForm["software.Note"].Split(',');
                for (var i = 0; i < softwareNames.Length; i++)
                {
                    var number = 0;
                    int.TryParse(softwareNames[i], out number);
                    list.Add(new Software
                    {
                        Name = softwareNames[i],
                        Number = number,
                        Source = softwareSources[i],
                        Purpose = softwarePurposes[i],
                        Note = softwareNotes[i]
                    });
                }
            }
            catch { }
            return list;
        }
    }
}
