using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.International.Converters.PinYinConverter;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string result_full = GetFullPinYin("格格y爱音乐");
            string result_first = GetFirstPinYin("格格y爱音乐");
            Console.WriteLine(result_full + "\n" + result_first);
        }

        /// <summary>
        /// 获取汉字拼音全拼
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string GetFullPinYin(string text)
        {
            string result = string.Empty;
            var chars = text.ToCharArray();
            //保存原始信息数组
            List<string[]> list = new List<string[]>();

            foreach (char c in chars)
            {
                if (ChineseChar.IsValidChar(c))//非汉子字符直接追加
                {
                    ChineseChar chineseChar = new ChineseChar(c);
                    var py = chineseChar.Pinyins;
                    ArrayList charList = new ArrayList();
                    foreach (var item in py)
                    {
                        if (item != null)
                        {
                            string addItem = item.Substring(0, item.Length - 1);
                            if (!charList.Contains(addItem))//不同音去重
                            {
                                charList.Add(addItem);
                            }
                        }
                    }
                    list.Add((string[])charList.ToArray(typeof(string)));//强制转换并添加
                }
                else
                {
                    list.Add(new string[] { c.ToString() });//强制转换并添加
                }

            }

            List<string> finalList = new List<string>();
            Descartes(list, 0, finalList, string.Empty);

            foreach (var item in finalList)
            {
                result += (item + ";");
            }

            return result.ToLower();
        }

        /// <summary>
        /// 获取汉字拼音首字母
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string GetFirstPinYin(string text)
        {
            string result = string.Empty;
            var chars = text.ToCharArray();
            //保存原始信息数组
            List<string[]> list = new List<string[]>();

            foreach (char c in chars)
            {
                if (ChineseChar.IsValidChar(c))//非汉子字符直接追加
                {
                    ChineseChar chineseChar = new ChineseChar(c);
                    var py = chineseChar.Pinyins;
                    ArrayList charList = new ArrayList();
                    foreach (var item in py)
                    {
                        if (item != null)
                        {
                            string addItem = item.Substring(0, 1);
                            if (!charList.Contains(addItem))//不同音去重
                            {
                                charList.Add(addItem);
                            }
                        }
                    }
                    list.Add((string[])charList.ToArray(typeof(string)));//强制转换并添加
                }
                else
                {
                    list.Add(new string[] { c.ToString() });//强制转换并添加
                }

            }

            List<string> finalList = new List<string>();
            Descartes(list, 0, finalList, string.Empty);

            foreach (var item in finalList)
            {
                result += (item + ";");
            }

            return result.ToLower();
        }
        /// <summary>
        /// 笛卡尔积算法
        /// </summary>
        /// <param name="list"></param>
        /// <param name="count"></param>
        /// <param name="result"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private static string Descartes(List<string[]> list, int count, List<string> result, string data)
        {
            string temp = data;
            //获取当前数组
            string[] astr = list[count];
            //循环当前数组
            foreach (var item in astr)
            {
                if (count + 1 < list.Count)
                {
                    temp += Descartes(list, count + 1, result, data + item);
                }
                else
                {
                    result.Add(data + item);
                }
            }
            return temp;
        }
    }
}
