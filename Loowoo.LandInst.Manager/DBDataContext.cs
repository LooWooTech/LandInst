using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loowoo.LandInst.Model;
using MySql.Data.Entity;

namespace Loowoo.LandInst.Manager
{
    public class DBDataContext : DbContext
    {
        public DBDataContext()
            : base("name=DBDataContext")
        {
        }

        public DBDataContext(string connectionString)
            : base(connectionString)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Member> Members { get; set; }

        public DbSet<Institution> Institutions { get; set; }

        public DbSet<Profile> Profiles { get; set; }

        public DbSet<Education> Educations { get; set; }

        public DbSet<AnnualCheck> AnnualChecks { get; set; }

        public DbSet<Exam> Exams { get; set; }

        public DbSet<Transfer> Transfers { get; set; }

        public DbSet<ExamResult> ExamResults { get; set; }

        public DbSet<VMemberExamResult> VMemberExamResults { get; set; }
        
        public DbSet<CheckLog> CheckLogs { get; set; }

        public DbSet<VCheckInst> VApprovalInsts { get; set; }

        public DbSet<VCheckMember> VApprovalMembers { get; set; }

        public DbSet<VCheckEducation> VApprovalEducations { get; set; }

        public DbSet<VCheckExam> VApprovalExams { get; set; }

        public DbSet<VCheckAnnual> VApprovalAnnualChecks { get; set; }
    }
}
