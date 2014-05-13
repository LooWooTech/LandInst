﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loowoo.LandInst.Model;
using Loowoo.LandInst.Model.Filters;

namespace Loowoo.LandInst.Manager
{
    public class ApprovalManager : ManagerBase
    {
        public Approval GetApproval(int approvalId)
        {
            using (var db = GetDataContext())
            {
                return db.Approvals.FirstOrDefault(e => e.ID == approvalId);
            }
        }

        public Approval GetApproval(int infoId, ApprovalType type)
        {
            using (var db = GetDataContext())
            {
                return db.Approvals.OrderByDescending(e => e.CreateTime).FirstOrDefault(e => e.InfoID == infoId && e.ApprovalType == type);
            }
        }

        public void UpdateApproval(int approvalId, bool result)
        {
            using (var db = GetDataContext())
            {
                var entity = db.Approvals.FirstOrDefault(e => e.ID == approvalId);
                if (entity == null) return;
                entity.ApprovalTime = DateTime.Now;
                entity.Result = result;
                db.SaveChanges();
            }
        }

        public void AddApproval(int infoId, int userId, ApprovalType type)
        {
            var model = new Approval
            {
                InfoID = infoId,
                UserID = userId,
                ApprovalType = type,
                CreateTime = DateTime.Now
            };
            using (var db = GetDataContext())
            {
                var entity = db.Approvals.FirstOrDefault(e => e.InfoID == infoId && e.UserID == userId && e.ApprovalType == type);
                if (entity != null)
                {
                    if (entity.ApprovalTime.HasValue)
                    {
                        return;
                    }
                }
                db.Approvals.Add(model);
                db.SaveChanges();
            }
        }
    }
}