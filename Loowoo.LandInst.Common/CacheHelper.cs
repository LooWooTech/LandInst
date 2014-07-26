using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Common
{
    public static class CacheHelper
    {
        private static ConcurrentDictionary<string, DateTime> ExpireTime = new ConcurrentDictionary<string, DateTime>();

        private static ConcurrentDictionary<string, object> Data = new ConcurrentDictionary<string, object>();

        public  static T Get<T>(string key) where T : class
        {
            if (Data.ContainsKey(key))
            {
                var time = ExpireTime[key];
                if (DateTime.Now > time)
                {
                    return default(T);
                }
                return (T)Data[key];
            }
            return default(T);
        }
        public static void Set<T>(string key, T data, int minutes = 5) where T : class
        {
            if (Data.ContainsKey(key))
            {
                Data[key] = data;
                ExpireTime[key] = DateTime.Now.AddMinutes(minutes);
            }
            else
            {
                Data.TryAdd(key, data);
                ExpireTime.TryAdd(key, DateTime.Now.AddMinutes(minutes));
            }
        }
 
        public static T GetOrSet<T>(string key, Func<T> getDataFunc, int minutes = 5) where T : class
        {
            var data = CacheHelper.Get<T>(key);
            if (data == default(T))
            {
                data = getDataFunc();
                Set(key, data, minutes);
            }
            return data;
        }

        public static void Remove(string key)
        {
            object data = null;
            if (Data.TryRemove(key, out data))
            { 
                DateTime time = DateTime.Now;
                ExpireTime.TryRemove(key, out time);
            }
        }
   }
}
