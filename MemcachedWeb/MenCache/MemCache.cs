using System;
using System.Collections.Specialized;
using System.Collections;
using System.Linq;

namespace Auto.Utility.MemCache
{
    /// <summary>
    /// MemCache操作类
    /// </summary>
    public class MemCache
    {
        #region 获取缓存名称
        /// <summary>
        /// 获取缓存名称-并把子Cache名放入主Cache集合中
        /// </summary>
        /// <param name="mainname"></param>
        /// <param name="subname"></param>
        /// <returns></returns>
        public static string GetName(string mainname, string subname)
        {
            mainname = mainname.ToLower();
            subname = subname.ToLower();

            var keyname = new NameValueCollection();

            // 获取用户被缓存的访问记录
            object value = MemBase.Get(mainname);
            if (value != null)
            {
                keyname = (NameValueCollection)value;
            }

            // 根据参数值判断dt中是否有相应的数据			
            bool isNew = false;

            if (subname.Trim() == "") { subname = "&nbsp;"; }
            //判断缓存中是否存在 如果不存在则添加进去

            if (keyname != null && keyname.Count > 0)
            {
                if (keyname[subname] != null)
                {
                    if (keyname[subname].ToLower() != subname.ToLower())
                    {
                        isNew = true;
                        keyname.Add(subname, subname);
                    }
                }
                else
                {
                    isNew = true;
                    keyname.Add(subname, subname);
                }
            }
            else
            {
                isNew = true;
                keyname.Add(subname, subname);
            }

            if (isNew)
            {
                MemBase.Add(mainname, keyname, DateTime.Now.AddDays(30));
            }

            keyname.Clear();

            string getname = mainname + "_" + subname;
            if (getname.Trim() != "") { getname = getname.Replace("&nbsp;", ""); }

            return getname;
        }

        /// <summary>
        /// 获取主Cache中的所有子Cache名
        /// </summary>
        /// <param name="mainname"></param>
        /// <returns></returns>
        public static string[] GetName(string mainname)
        {
            mainname = mainname.ToLower();
            object value = MemBase.Get(mainname);

            if (value == null)
            {
                return new[] { "" };
            }

            var keyname = (NameValueCollection)value;

            var list = new String[keyname.Count];
            keyname.CopyTo(list, 0);
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i].Trim() != "") { list[i] = list[i].Replace("&nbsp;", ""); }
                list[i] = mainname + "_" + list[i];
            }
            return list;
        }

        /// <summary>
        /// 获取主Cache中的所有子Cache名
        /// </summary>
        /// <param name="mainname"></param>
        /// <param name="subname"></param>
        /// <param name="isall"></param>
        /// <returns></returns>
        public static string[] GetName(string mainname, string subname, bool isall)
        {
            mainname = mainname.ToLower();
            subname = subname.ToLower();
            if (isall)//全名称，无其他相关名称
            {
                return subname.Trim() != "" ? new[] { mainname + "_" + subname } : new[] { mainname };
            }

            object value = MemBase.Get(mainname);
            if (value == null)
            {
                return new[] { "" };
            }
            var keyname = (NameValueCollection)value;
            //直接拼keyname返回
            //根据subname获取所有subname的list
            var list = new String[keyname.Count];
            keyname.CopyTo(list, 0);
            var sublist = new ArrayList();
            foreach (string t in list.Where(t => t.IndexOf(subname) >= 0))
            {
                sublist.Add(mainname + "_" + t);
            }
            if (sublist.Count > 0)
            {
                list = new string[sublist.Count];
                sublist.CopyTo(list);
                return list;
            }
            return new[] { "" };
        }
        #endregion

        #region 判断MemCache
        /// <summary>
        /// 是否存在缓存对象
        /// </summary>
        /// <param name="cachename"></param>
        /// <returns></returns>
        public static bool IsExist(string cachename)
        {
            return MemBase.IsExists(cachename);
        }
        #endregion

        #region 添加缓存
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="cachename"></param>
        /// <param name="values"></param>
        /// <param name="vtime"></param>
        /// <returns></returns>
        public static bool Add(string cachename, object values)
        {
            return MemBase.Add(cachename, values);
        }

        /// <summary>
        /// 添加缓存-设置自定义过期时间
        /// </summary>
        /// <param name="cachename"></param>
        /// <param name="values"></param>
        /// <param name="vtime"></param>
        /// <returns></returns>
        public static bool Add(string cachename, object values, DateTime vtime)
        {
            return MemBase.Add(cachename, values, vtime);
        }

        /// <summary>
        /// 添加缓存-设置相对过期时间
        /// </summary>
        /// <param name="cachename"></param>
        /// <param name="values"></param>
        /// <param name="hours"></param>
        /// <returns></returns>
        public static bool Add(string cachename, object values, int hours)
        {
            return MemBase.Add(cachename, values, hours);
        }

        /// <summary>
        /// 添加缓存-同时添加到主Cache
        /// </summary>
        /// <param name="mainname"></param>
        /// <param name="subname"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool Add(string mainname, string subname, object values)
        {
            string cachename = GetName(mainname, subname);
            return MemBase.Add(cachename, values);
        }

        /// <summary>
        /// 添加缓存-设置自定义过期时间-同时添加到主Cache
        /// </summary>
        /// <param name="mainname"></param>
        /// <param name="subname"></param>
        /// <param name="values"></param>
        /// <param name="vtime"></param>
        /// <returns></returns>
        public static bool Add(string mainname, string subname, object values, DateTime vtime)
        {
            string cachename = GetName(mainname, subname);
            return MemBase.Add(cachename, values, vtime);
        }

        /// <summary>
        /// 添加缓存-设置相对过期时间-同时添加到主Cache
        /// </summary>
        /// <param name="mainname"></param>
        /// <param name="subname"></param>
        /// <param name="values"></param>
        /// <param name="hours"></param>
        /// <returns></returns>
        public static bool Add(string mainname, string subname, object values, int hours)
        {
            string cachename = GetName(mainname, subname);
            return MemBase.Add(cachename, values, hours);
        }

        #endregion

        #region 删除缓存
        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="mainname"></param>
        public static void Clear(string cachename)
        {
            MemBase.Remove(cachename);
        }

        /// <summary>
        /// 删除缓存-是否只删除主缓存
        /// </summary>
        /// <param name="mainname"></param>
        /// <param name="imain"></param>
        public static void Clear(string mainname, bool imain)
        {
            mainname = mainname.ToLower();
            if (mainname == "") return;
            if (!imain)
            {
                string[] cachename = GetName(mainname);
                foreach (string t in cachename.Where(t => t.Trim() != ""))
                {
                    MemBase.Remove(t);
                }
            }
            MemBase.Remove(mainname);//删除mainname的数据缓存
        }

        /// <summary>
        /// 删除MemCache-相关性
        /// </summary>
        /// <param name="mainname"></param>
        /// <param name="subname"></param>
        /// <param name="isall"></param>
        public static void Clear(string mainname, string subname, bool isall)
        {
            mainname = mainname.ToLower();
            subname = subname.ToLower();
            string[] cachename;
            if (mainname != "")
            {
                cachename = GetName(mainname, subname, isall);
                foreach (string t in cachename.Where(t => t.Trim() != ""))
                {
                    MemBase.Remove(t);

                }
            }
        }

        #endregion

        #region 获取缓存
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="cachename"></param>
        /// <returns></returns>
        public static object Get(string cachename)
        {
            return MemBase.Get(cachename);
        }

        /// <summary>
        /// 获取缓存-相关性
        /// </summary>
        /// <param name="mainname"></param>
        /// <param name="isall"></param>
        /// <returns></returns>
        public static object[] Get(string mainname, bool ismain)
        {
            if (ismain)
            {
                return new object[] { MemBase.Get(mainname) };
            }

            var value = MemBase.Get(mainname);
            if (value == null)
            {
                return new object[] { null };
            }

            var keyname = (NameValueCollection)value;
            var list = new object[keyname.Count];

            for (int iKint = 0; iKint < keyname.Count; iKint++)
            {
                var obj = MemBase.Get(mainname + "_" + keyname[iKint]);
                if (obj != null)
                { list[iKint] = obj; }
            }

            return list;
        }
        #endregion

        #region 更新缓存
        /// <summary>
        /// 更新缓存
        /// </summary>
        /// <param name="cachename"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool Replace(string cachename, object values)
        {
            return MemBase.Replace(cachename, values);
        }

        /// <summary>
        /// 更新缓存-设置自定义过期时间
        /// </summary>
        /// <param name="cachename"></param>
        /// <param name="values"></param>
        /// <param name="vtime"></param>
        /// <returns></returns>
        public static bool Replace(string cachename, object values, DateTime vtime)
        {
            return MemBase.Replace(cachename, values, vtime);
        }

        /// <summary>
        /// 更新缓存-设置相对过期时间
        /// </summary>
        /// <param name="cachename"></param>
        /// <param name="values"></param>
        /// <param name="hours"></param>
        /// <returns></returns>
        public static bool Replace(string cachename, object values, int hours)
        {
            return MemBase.Replace(cachename, values, hours);
        }

        /// <summary>
        /// 更新缓存-同步更新主缓存
        /// </summary>
        /// <param name="mainname"></param>
        /// <param name="subname"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool Replace(string mainname, string subname, object values)
        {
            string cachename = GetName(mainname, subname);
            return MemBase.Replace(cachename, values);
        }

        /// <summary>
        /// 更新缓存-设置自定义过期时间-同步更新主缓存
        /// </summary>
        /// <param name="mainname"></param>
        /// <param name="subname"></param>
        /// <param name="values"></param>
        /// <param name="vtime"></param>
        /// <returns></returns>
        public static bool Replace(string mainname, string subname, object values, DateTime vtime)
        {
            string cachename = GetName(mainname, subname);
            return MemBase.Replace(cachename, values, vtime);
        }

        /// <summary>
        /// 更新缓存-设置相对过期时间-同步更新主缓存
        /// </summary>
        /// <param name="mainname"></param>
        /// <param name="subname"></param>
        /// <param name="values"></param>
        /// <param name="hours"></param>
        /// <returns></returns>
        public static bool Replace(string mainname, string subname, object values, int hours)
        {
            string cachename = GetName(mainname, subname);
            return MemBase.Replace(cachename, values, hours);
        }

        /// <summary>
        /// 修改指定列的数据
        /// </summary>
        /// <param name="cachename"></param>
        /// <param name="cols"></param>
        /// <param name="values"></param>
        /// <param name="vtime"></param>
        public static void Replace(string cachename, ArrayList cols, ArrayList values, DateTime vtime)
        {
            if (cols.Count <= 0) { return; }
            if (cols.Count != values.Count) { return; }
            if (cachename.Trim() != "")
            {
                object value = MemBase.Get(cachename);
                if (value != null)
                {
                    var listinfo = (NameValueCollection)value;

                    for (int i = 0; i < cols.Count; i++)
                    {
                        listinfo[cols[i].ToString()] = values[i].ToString();
                    }

                    MemBase.Add(cachename, listinfo, vtime);
                }
            }
        }
        #endregion
    }
}
