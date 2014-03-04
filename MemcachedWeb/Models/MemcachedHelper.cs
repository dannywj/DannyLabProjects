using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Memcached;
using Memcached.ClientLibrary;

namespace MemcachedWeb.Models
{
    public class MemcachedHelper
    {
        private static MemcachedClient mclient;
        static MemcachedHelper()
        {
            string[] serverList = new string[] { "127.0.0.1:11211" };
            //需要添加log4net.dll和相关的dll引用！
            SockIOPool pool = SockIOPool.GetInstance("test");//需要填写一个实例名称，否则下次Get数据的时候是找不到的。
            pool.SetServers(serverList);
            pool.Initialize();
            mclient = new MemcachedClient();
            mclient.PoolName = "test";
            mclient.EnableCompression = false;
        }

        public static bool SetCache(string key, object value, DateTime expiry)
        {
            return mclient.Set(key, value, expiry);
        }

        public static object GetCache(string key)
        {
            return mclient.Get(key);
        }

    }
}