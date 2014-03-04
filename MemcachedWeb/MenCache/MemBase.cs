using System;
using System.Collections;
using System.Configuration;
using Memcached.ClientLibrary;

namespace Auto.Utility
{
    /// <summary>
    /// MemCache管理基类
    /// </summary>
    public class MemBase
    {
        #region MemCache对象
        private static SockIOPool sockPool;
        private static MemcachedClient memClient;

        private static readonly string[] serverList; //= Function.Utils.GetConfigParam("MemClient",string.Empty).Split(',');
        private static readonly string poolName; //= Function.Utils.GetConfigParam("PoolName", "Auto");
        private static readonly string memLock; //= Function.Utils.GetConfigParam("MemLock", "0");
        #endregion

        #region 返回MemCache对象
        /// <summary>
        /// 返回MemCache对象
        /// </summary>
        private static MemcachedClient MClient
        {
            get
            {
                if (memLock == "0")
                {
                    if (memClient == null)
                    {
                        InitMemCached();
                    }

                    return memClient;
                }
                return new MemcachedClient();
            }
        }
        #endregion

        #region 初始化MemCache
        /// <summary>
        /// 初始化MemCache
        /// </summary>
        private static void InitMemCached()
        {
            memClient = new MemcachedClient();
            if (poolName.Trim() == "") return;
            if (sockPool == null)
            {
                //初始化池
                sockPool = SockIOPool.GetInstance(poolName);
                sockPool.SetServers(serverList);

                sockPool.InitConnections = 3;
                sockPool.MinConnections = 10;
                sockPool.MaxConnections = 10000;

                sockPool.SocketConnectTimeout = 1000;
                sockPool.SocketTimeout = 3000;

                sockPool.MaintenanceSleep = 30;
                sockPool.Failover = true;

                sockPool.Nagle = false;
                sockPool.Initialize();

                // 获得客户端实例
                MClient.PoolName = poolName;
                //是否数据压缩
                MClient.EnableCompression = false;
            }
        }
        #endregion

        #region 添加缓存
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool Add(string key, object obj)
        {
            return MClient.Set(key, obj, DateTime.Now.AddHours(1));
        }

        /// <summary>
        /// 添加缓存-设置自定义过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="validateTime"></param>
        /// <returns></returns>
        public static bool Add(string key, object obj, DateTime validateTime)
        {
            return MClient.Set(key, obj, validateTime);
        }

        /// <summary>
        /// 添加缓存-设置相对过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="hours"></param>
        /// <returns></returns>
        public static bool Add(string key, object obj, int hours)
        {
            return MClient.Set(key, obj, DateTime.Now.AddHours(hours));
        }
        #endregion

        #region 获取缓存
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object Get(string key)
        {
            return MClient.Get(key);
        }

        /// <summary>
        /// 获取缓存集合
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static Hashtable GetMultiple(string[] keys)
        {
            return MClient.GetMultiple(keys);
        }
        #endregion

        #region 更新缓存
        /// <summary>
        /// 更新缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool Replace(string key, object obj)
        {
            return MClient.Replace(key, obj);
        }

        /// <summary>
        /// 更新缓存-设置自定义过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="validateTime"></param>
        /// <returns></returns>
        public static bool Replace(string key, object obj, DateTime validateTime)
        {
            return MClient.Replace(key, obj, validateTime);
        }

        /// <summary>
        /// 更新缓存-设置相对过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="hours"></param>
        /// <returns></returns>
        public static bool Replace(string key, object obj, int hours)
        {
            return MClient.Replace(key, obj, DateTime.Now.AddHours(hours));
        }
        #endregion

        #region 移除缓存
        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Remove(string key)
        {
            return MClient.Delete(key);
        }
        #endregion

        #region 缓存是否存在
        /// <summary>
        /// 缓存是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsExists(string key)
        {
            return MClient.KeyExists(key);
        }
        #endregion

        #region MemCache状态
        /// <summary>
        /// 缓存状态
        /// </summary>
        /// <returns></returns>
        public static IDictionary Stats()
        {
            return MClient.Stats();
        }
        #endregion

        #region 清理缓存
        /// <summary>
        /// 清理所有缓存
        /// </summary>
        /// <returns></returns>
        public static bool Flush()
        {
            var alist = new ArrayList();
            for (int i = 0; i < serverList.Length; i++)
            {
                alist[i] = serverList[i];
            }
            return MClient.FlushAll(alist);
        }
        #endregion

    }

}