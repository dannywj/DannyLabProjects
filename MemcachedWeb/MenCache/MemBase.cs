using System;
using System.Collections;
using System.Configuration;
using Memcached.ClientLibrary;

namespace Auto.Utility
{
    /// <summary>
    /// MemCache�������
    /// </summary>
    public class MemBase
    {
        #region MemCache����
        private static SockIOPool sockPool;
        private static MemcachedClient memClient;

        private static readonly string[] serverList; //= Function.Utils.GetConfigParam("MemClient",string.Empty).Split(',');
        private static readonly string poolName; //= Function.Utils.GetConfigParam("PoolName", "Auto");
        private static readonly string memLock; //= Function.Utils.GetConfigParam("MemLock", "0");
        #endregion

        #region ����MemCache����
        /// <summary>
        /// ����MemCache����
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

        #region ��ʼ��MemCache
        /// <summary>
        /// ��ʼ��MemCache
        /// </summary>
        private static void InitMemCached()
        {
            memClient = new MemcachedClient();
            if (poolName.Trim() == "") return;
            if (sockPool == null)
            {
                //��ʼ����
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

                // ��ÿͻ���ʵ��
                MClient.PoolName = poolName;
                //�Ƿ�����ѹ��
                MClient.EnableCompression = false;
            }
        }
        #endregion

        #region ��ӻ���
        /// <summary>
        /// ��ӻ���
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool Add(string key, object obj)
        {
            return MClient.Set(key, obj, DateTime.Now.AddHours(1));
        }

        /// <summary>
        /// ��ӻ���-�����Զ������ʱ��
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
        /// ��ӻ���-������Թ���ʱ��
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

        #region ��ȡ����
        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object Get(string key)
        {
            return MClient.Get(key);
        }

        /// <summary>
        /// ��ȡ���漯��
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static Hashtable GetMultiple(string[] keys)
        {
            return MClient.GetMultiple(keys);
        }
        #endregion

        #region ���»���
        /// <summary>
        /// ���»���
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool Replace(string key, object obj)
        {
            return MClient.Replace(key, obj);
        }

        /// <summary>
        /// ���»���-�����Զ������ʱ��
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
        /// ���»���-������Թ���ʱ��
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

        #region �Ƴ�����
        /// <summary>
        /// �Ƴ�����
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Remove(string key)
        {
            return MClient.Delete(key);
        }
        #endregion

        #region �����Ƿ����
        /// <summary>
        /// �����Ƿ����
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsExists(string key)
        {
            return MClient.KeyExists(key);
        }
        #endregion

        #region MemCache״̬
        /// <summary>
        /// ����״̬
        /// </summary>
        /// <returns></returns>
        public static IDictionary Stats()
        {
            return MClient.Stats();
        }
        #endregion

        #region ������
        /// <summary>
        /// �������л���
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