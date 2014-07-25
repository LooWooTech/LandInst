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
        //public void UpdateListItem<T>(int infoId, int userId, InfoType infoType, T item)
        //{
        //    var model = GetModel(infoId, infoType);

        //    if (model == null)
        //    {
        //        Core.InfoDataManager.Save(infoId, userId, infoType, new List<T> { item });
        //    }
        //    else
        //    {
        //        var list = model.Convert<List<T>>() ?? new List<T>();
        //        list.Add(item);
        //        Core.InfoDataManager.Save(infoId, userId, infoType, list);
        //    }
        //}

        //public void DeleteListItem<TItem, TKey>(int infoId, int userId, InfoType infoType, TKey keyValue, Func<TItem, TKey> getKey) where TKey : class
        //{
        //    var model = GetModel(infoId, infoType);
        //    if (model == null || model.Data == null) return;
        //    var list = model.Convert<List<TItem>>();
        //    var index = list.FindIndex(e => getKey(e) == keyValue);
        //    list.RemoveAt(index);
        //    Save(infoId, userId, infoType, list);
        //}

        public InfoData GetModel(int infoId, InfoType infoType)
        {
            if (infoId == 0)
            {
                return null;
            }
            using (var db = GetDataContext())
            {
                return db.InfoDatas.FirstOrDefault(e => e.InfoID == infoId && e.InfoType == infoType);
            }
        }

        public T GetModel<T>(int infoId, InfoType infoType)
        {
            var model = GetModel(infoId, infoType);
            return model == null ? default(T) : model.Convert<T>();
        }

        public void Save<T>(int infoId, int userId, InfoType type, T data)
        {
            using (var db = GetDataContext())
            {
                var entity = db.InfoDatas.FirstOrDefault(e => e.InfoID == infoId && e.InfoType == type);
                if (entity != null)
                {
                    entity.Data = data.ToBytes();
                }
                else
                {
                    entity = new InfoData
                    {
                        Data = data.ToBytes(),
                        InfoID = infoId,
                        InfoType = type,
                        UserID = userId
                    };

                    db.InfoDatas.Add(entity);
                }
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
