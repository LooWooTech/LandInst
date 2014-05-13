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

        public DbSet<InfoData> InfoDatas { get; set; }

        public DbSet<Education> Educations { get; set; }

        public DbSet<AnnualCheck> AnnualChecks { get; set; }

        public DbSet<Exam> Exams { get; set; }

        public DbSet<ExamResult> ExamResults { get; set; }

        public DbSet<VMemberExam> VMemberExams { get; set; }

        public DbSet<VMemberExamResult> VMemberExamResults { get; set; }

        public DbSet<VMemberEducation> VMemberEducations { get; set; }


        public DbSet<VInstAnnualCheck> VInstAnnualChecks { get; set; }


        
        public DbSet<Approval> Approvals { get; set; }


        public DbSet<VApprovalInst> VApprovalInsts { get; set; }

        public DbSet<VApprovalMember> VApprovalMembers { get; set; }

        public DbSet<VApprovalEducation> VApprovalEducations { get; set; }

        public DbSet<VApprovalExam> VApprovalExams { get; set; }

        public DbSet<VApprovalAnnualCheck> VApprovalAnnualChecks { get; set; }
    }
}
