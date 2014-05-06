using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Loowoo.LandInst.Model;
using Loowoo.LandInst.Common;

namespace Loowoo.LandInst.Manager
{
    public class InfoDataManager : ManagerBase
    {
        public void UpdateListItem<T>(int infoId, InfoType infoType, InfoStatus status, T item)
        {
            var model = GetModel(infoId, infoType, status);

            if (model == null)
            {
                Core.InfoDataManager.Add(new InfoData
                {
                    InfoID = infoId,
                    InfoType = infoType,
                    Status = status,
                    Data = new List<T> { item }.ToBytes()
                });
            }
            else
            {
                var list = model.Convert<List<T>>() ?? new List<T>();
                list.Add(item);
                Core.InfoDataManager.Update(model);
            }
        }

        public void DeleteListItem<TItem, TKey>(int infoId, InfoType infoType, InfoStatus status,TKey keyValue, Func<TItem, TKey> getKey) where TKey : class
        {
            var model = GetModel(infoId, infoType, status);
            if (model == null || model.Data == null) return;
            var list = model.Convert<List<TItem>>();
            var index = list.FindIndex(e => getKey(e) == keyValue);
            list.RemoveAt(index);
            Update(model);
        }

        public InfoData GetModel(int infoId, InfoType infoType, InfoStatus status = InfoStatus.Draft)
        {
            if (infoId == 0)
            {
                return null;
            }
            using (var db = GetDataContext())
            {
                var list = db.InfoDatas.Where(e => e.InfoID == infoId && e.InfoType == infoType).ToList();
                var model = list.FirstOrDefault(e => e.Status == status);
                if (model == null)
                {
                    return list.FirstOrDefault();
                }
                return model;
            }
        }

        public T GetModel<T>(int infoId, InfoType infoType, InfoStatus status)
        {
            var model = GetModel(infoId, infoType, status);
            return model == null ? default(T) : model.Convert<T>();
        }

        public void Update(InfoData model)
        {
            if (model == null) return;
            if (model.Data == null) return;
            if (model.InfoID == 0) throw new ArgumentNullException("infoId");

            using (var db = GetDataContext())
            {
                var infoEntity = db.InfoDatas.FirstOrDefault(e => e.InfoID == model.InfoID && e.InfoType == model.InfoType && e.Status == model.Status);
                if (infoEntity == null)
                {
                    Add(model);
                    return;
                }
                infoEntity.Data = model.Data;
                db.SaveChanges();
            }
        }

        public void Add(InfoData model)
        {
            using (var db = GetDataContext())
            {
                var entity = new InfoData
                {
                    InfoID = model.InfoID,
                    InfoType = model.InfoType,
                    Status = model.Status,
                    Data = model.Data
                };
                db.InfoDatas.Add(entity);
                db.SaveChanges();
            }
        }

    }

    public static class InfoDataExtension
    {
        public static T Convert<T>(this InfoData model)
        {
            if (model != null && model.Data != null)
            {
                return Encoding.UTF8.GetString(model.Data).ToObject<T>();
            }
            return default(T);
        }

        public static byte[] ToBytes<T>(this T data)
        {
            return Encoding.UTF8.GetBytes(data.ToJson());
        }
    }
}
