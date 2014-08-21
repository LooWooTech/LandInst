using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Model
{
    public class Job
    {
        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string Institution { get; set; }

        public string Office { get; set; }

        public string Note { get; set; }

        public static List<Job> GetList(NameValueCollection requestForm)
        {
            var list = new List<Job>();
            try
            {

                var startDates = requestForm["job.StartDate"].Split(',');
                var endDates = requestForm["job.StartDate"].Split(',');
                var insts = requestForm["job.Institution"].Split(',');
                var offices = requestForm["job.Office"].Split(',');
                var notes = requestForm["job.Note"].Split(',');
                for (var i = 0; i < startDates.Length; i++)
                {
                    var startDate = DateTime.MinValue;
                    DateTime.TryParse(startDates[i], out startDate);
                    var endDate = DateTime.MinValue;
                    DateTime.TryParse(endDates[i], out endDate);

                    list.Add(new Job
                    {
                        StartDate = startDate == DateTime.MinValue ? null : startDate.ToShortDateString(),
                        EndDate = endDate == DateTime.MinValue ? null : endDate.ToShortDateString(),
                        Institution = insts[i],
                        Office = offices[i],
                        Note = notes[i]
                    });
                }

            }
            catch { }

            return list;
        }
    }
}
