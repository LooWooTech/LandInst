﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Loowoo.LandInst.Model;
using Loowoo.LandInst.Model.Filters;

namespace Loowoo.LandInst.Manager
{
    public class MemberManager : ManagerBase
    {
        public void AddMember(Member member)
        {
            using (var db = GetDataContext())
            {
                db.Members.Add(member);
                db.SaveChanges();
            }

            //AddProfile(member);
        }

        private void AddProfile(Member member)
        {
            var profile = new MemberProfile(member);
            Core.ProfileManager.AddProfile(member.ID, profile);
        }

        public void UpdateMemberStatus(int memberId, MemberStatus status)
        {
            using (var db = GetDataContext())
            {
                var entity = db.Members.FirstOrDefault(e => e.ID == memberId);
                entity.Status = status;
                db.SaveChanges();
            }
        }

        public void UpdateMember(Member member)
        {
            using (var db = GetDataContext())
            {
                var entity = db.Members.FirstOrDefault(e => e.ID == member.ID);
                if (entity == null)
                {
                    throw new ArgumentException("Member.ID");
                }
                db.Entry(entity).CurrentValues.SetValues(member);
                db.SaveChanges();
            }
        }

        public Member GetMember(int memberId)
        {
            if (memberId == 0) return null;
            using (var db = GetDataContext())
            {
                return db.Members.FirstOrDefault(e => e.ID == memberId);
            }
        }

        public Member GetMember(string realName, string IDCardNo)
        {
            using (var db = GetDataContext())
            {
                var query = db.Members.Where(e => e.RealName == realName);
                if (query.Count() > 1)
                    query = query.Where(e => e.IDNo == IDCardNo);
                return query.FirstOrDefault();
            }
        }


        public MemberProfile GetProfile(int memberId)
        {
            return Core.ProfileManager.GetLastProfile<MemberProfile>(memberId);
            //if (memberId == 0) return null;
            //var checkLog = Core.CheckLogManager.GetLastLog(memberId, CheckType.Profile, checkResult);
            //if (checkLog == null)
            //    return null;
            //return Core.ProfileManager.GetProfile<MemberProfile>(checkLog.InfoID);
        }

        public void SaveProfile(Member member, MemberProfile profile)
        {
            profile.SetMemberField(member);
            Core.MemberManager.UpdateMember(profile);
            var entity = Core.ProfileManager.GetLastProfile(member.ID);
            if (entity == null)
            {
                Core.ProfileManager.AddProfile(member.ID, profile);
            }
            else
            {
                Core.ProfileManager.UpdateProfile(entity.ID, profile);
            }

        }

        public List<Member> GetMembers(MemberFilter filter)
        {
            using (var db = GetDataContext())
            {
                var query = db.Members.AsQueryable();

                if (filter.InstID.HasValue && filter.InstID.Value > 0)
                {
                    if (filter.InInst)
                    {
                        if (filter.IncludeNoHaveInstMember)
                        {
                            query = query.Where(e => e.InstitutionID == filter.InstID || e.InstitutionID == 0);
                        }
                        else
                        {
                            query = query.Where(e => e.InstitutionID == filter.InstID);
                        }
                    }
                    else
                    {
                        if (filter.IncludeNoHaveInstMember)
                        {
                            query = query.Where(e => e.InstitutionID != filter.InstID);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(filter.Keyword))
                {
                    query = query.Where(e => e.RealName.Contains(filter.Keyword));
                }

                if (filter.MinStatus.HasValue)
                {
                    query = query.Where(e => e.Status >= filter.MinStatus.Value);
                }

                if (filter.Status.HasValue)
                {
                    query = query.Where(e => e.Status == filter.Status.Value);
                }

                var list = query.OrderByDescending(e => e.ID).SetPage(filter.Page).ToList();
                if (filter.GetInstName)
                {
                    foreach (var item in list)
                    {
                        if (item.InstitutionID > 0)
                            item.InstitutionName = Core.InstitutionManager.GetInstName(item.InstitutionID);
                    }
                }
                return list;
            }
        }

        public List<VCheckMember> GetVCheckMembers(MemberFilter filter)
        {
            using (var db = GetDataContext())
            {
                var query = GetVCheckMembers(db.VCheckMembers, filter);
                if (filter.Status.HasValue)
                {
                    query = query.Where(e => e.Status == filter.Status.Value);
                }
                if (filter.InstID.HasValue && filter.InstID.Value > 0)
                {
                    query = query.Where(e => e.InstitutionID == filter.InstID.Value);
                }
                return query.OrderByDescending(e => e.ID).SetPage(filter.Page).ToList();
            }
        }

        internal IQueryable<VCheckMember> GetVCheckMembers(IQueryable<VCheckMember> query, CheckLogFilter filter)
        {
            query = query.Where(e => e.CheckType == filter.Type);
            if (filter.Result.HasValue)
            {
                query = query.Where(e => e.Result == filter.Result.Value);
            }
            if (filter.InfoID.HasValue && filter.InfoID.Value > 0)
            {
                query = query.Where(e => e.InfoID == filter.InfoID.Value);
            }
            if (!string.IsNullOrEmpty(filter.Keyword))
            {
                query = query.Where(e => e.RealName.Contains(filter.Keyword.Trim()));
            }
            if (filter.UserID.HasValue && filter.UserID.Value > 0)
            {
                query = query.Where(e => e.UserID == filter.UserID.Value);
            }

            return query;
        }

        public List<int> GetMemberIds(string[] realNames, int instId)
        {
            using (var db = GetDataContext())
            {
                var query = db.Members.Where(e => e.InstitutionID == instId && realNames.Contains(e.RealName));
                return query.Select(e => e.ID).ToList();
            }
        }

        public void PracticeMemeber(CheckLog checkLog)
        {
            using (var db = GetDataContext())
            {
                var entity = db.Practices.FirstOrDefault(e => e.ID == checkLog.ID);
                if (entity != null)
                {
                    var member = db.Members.FirstOrDefault(e => e.ID == checkLog.UserID);
                    if (member != null)
                    {
                        member.InstitutionID = entity.InstID;
                        db.SaveChanges();
                    }
                }
            }
        }
    }
}
