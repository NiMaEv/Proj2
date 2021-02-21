using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityModels;

namespace DictionaryExtensions
{
    public static partial class DictionaryExtensions
    {
        #region Methods Of Copy Dictionaries.
        public static Dictionary<string, Dictionary<string, int>> CopyDictionary(this Dictionary<string, Dictionary<string, int>> currentDictionary)
        {
            Dictionary<string, Dictionary<string, int>> newDictionary = new Dictionary<string, Dictionary<string, int>>();
            if (currentDictionary != null)
                foreach (var item in currentDictionary)
                    newDictionary.Add(item.Key, item.Value.CopyDictionary());
            return newDictionary;
        }

        public static Dictionary<string, int> CopyDictionary(this Dictionary<string, int> currentDictionary)
        {
            Dictionary<string, int> newDictionary = new Dictionary<string, int>();
            if (currentDictionary != null)
                foreach (var item in currentDictionary)
                    newDictionary.Add(item.Key, item.Value);
            return newDictionary;
        }
        #endregion

        #region Methods Of Unite Dictionaries.
        public static void Unite(this Dictionary<string, Dictionary<string, int>> currentDictionary,
            string secondaryKey, Dictionary<string, int> additionalDictionary)
        {
            foreach(var item in additionalDictionary)
            {
                if (!currentDictionary.ContainsKey(item.Key))
                    currentDictionary.Add(item.Key, new Dictionary<string, int> { { secondaryKey, item.Value } });
                else
                {
                    if (!currentDictionary[item.Key].ContainsKey(secondaryKey))
                        currentDictionary[item.Key].Add(secondaryKey, item.Value);
                    else
                        currentDictionary[item.Key][secondaryKey] += item.Value;
                }
            }
        }

        public static void Unite(this Dictionary<string, Dictionary<string, Dictionary<string, int>>> currentDictionary,
            string primaryKey, Dictionary<string, Dictionary<string, int>> additionalDictionary)
        {
            if (!currentDictionary.ContainsKey(primaryKey))
                currentDictionary.Add(primaryKey, additionalDictionary.CopyDictionary());
            else
            {
                foreach (var item in additionalDictionary)
                {
                    if (!currentDictionary[primaryKey].ContainsKey(item.Key))
                        currentDictionary[primaryKey].Add(item.Key, item.Value.CopyDictionary());
                    else
                    {
                        foreach(var innerItem in item.Value)
                        {
                            if (!currentDictionary[primaryKey][item.Key].ContainsKey(innerItem.Key))
                                currentDictionary[primaryKey][item.Key].Add(innerItem.Key, innerItem.Value);
                            else
                                currentDictionary[primaryKey][item.Key][innerItem.Key] += innerItem.Value;
                        }
                    }
                }         
            }
        }

        public static void Unite(this Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>> currentDictionary,
            string primaryKey, string secondaryKey, Dictionary<string, Dictionary<string, int>> additionalDictionary)
        {
            if (!currentDictionary.ContainsKey(primaryKey))
                currentDictionary.Add(primaryKey, new Dictionary<string, Dictionary<string, Dictionary<string, int>>> { { secondaryKey, additionalDictionary.CopyDictionary() } });
            else
            {
                if (!currentDictionary[primaryKey].ContainsKey(secondaryKey))
                    currentDictionary[primaryKey].Add(secondaryKey, additionalDictionary.CopyDictionary());
                else
                {
                    foreach(var item in additionalDictionary)
                    {
                        if (!currentDictionary[primaryKey][secondaryKey].ContainsKey(item.Key))
                            currentDictionary[primaryKey][secondaryKey].Add(item.Key, item.Value.CopyDictionary());
                        else
                        {
                            foreach (var innerItem in item.Value)
                            {
                                if (!currentDictionary[primaryKey][secondaryKey][item.Key].ContainsKey(innerItem.Key))
                                    currentDictionary[primaryKey][secondaryKey][item.Key].Add(innerItem.Key, innerItem.Value);
                                else
                                    currentDictionary[primaryKey][secondaryKey][item.Key][innerItem.Key] += innerItem.Value;
                            }  
                        }
                    }
                }
            }
        }
        #endregion

        public static void RemoveMatches(this Dictionary<string, Dictionary<string, int>> currentDictionary, 
            Dictionary<string, Dictionary<string, int>> targetDictionary, string secondaryKey)
        {
            Dictionary<string, Dictionary<string, int>> tempDictionary = currentDictionary.CopyDictionary();
            currentDictionary.Clear();

            foreach(var itemCD in tempDictionary)
            {
                StringBuilder sb = new StringBuilder(itemCD.Key);
                foreach (var itemTD in targetDictionary)
                    sb.Replace(itemTD.Key, "");
                while (sb[0] == '/')
                    sb.Remove(0, 1);
                while (sb.ToString().Contains("//") | sb.ToString().Contains("/."))
                {
                    sb.Replace("//", "/");
                    sb.Replace("/.", ".");
                }
                if (sb.Length > 2)
                {
                    if (!currentDictionary.ContainsKey(sb.ToString()))
                        currentDictionary.Add(sb.ToString(), itemCD.Value);
                    else
                        currentDictionary[sb.ToString()][secondaryKey] += itemCD.Value[secondaryKey];
                }
                sb.Clear();
            } 
        }
    }
}

namespace DictionaryExtensions.Special
{
    public static partial class DictionaryExtensions
    {
        //public static Dictionary<string, int> GetDictionary(this string compositionStr, string sectorNameStr, int number)
        //{
        //    Dictionary<string, int> tempDictionary = new Dictionary<string, int>();

        //    using (PgContext context = new PgContext())
        //    {
        //        if (compositionStr != null)
        //        {
        //            string tempStr = null;
        //            foreach (char i in compositionStr.ToArray())
        //            {
        //                if (i.ToString() != "/" && i.ToString() != ".")
        //                    tempStr += i;
        //                else
        //                {
        //                    if (context.Materials.Find(tempStr) != null && context.Materials.Find(tempStr).sectorName.Equals(sectorNameStr))
        //                    {
        //                        if (!tempDictionary.ContainsKey(tempStr))
        //                            tempDictionary.Add(tempStr, number);
        //                        else
        //                            tempDictionary[tempStr] += number;
        //                    }
        //                    tempStr = null;
        //                }
        //            }
        //        }
        //    }
        //    return tempDictionary;
        //}

        public static Dictionary<string, int> GetDictionary(this string compositionStr, string sectorNameStr, int number) //???
        {
            Dictionary<string, int> tempDictionary = new Dictionary<string, int>();

            using(PgContext context = new PgContext())
            {
                if (compositionStr != null)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var i in compositionStr.ToArray())
                    {
                        if (i != '/' & i != '.')
                            sb.Append(i);
                        else
                        {
                            if (context.Materials.Find(sb.ToString()) != null & context.Materials.Find(sb.ToString()).sectorName.Equals(sectorNameStr))
                            {
                                if (!tempDictionary.ContainsKey(sb.ToString()))
                                    tempDictionary.Add(sb.ToString(), number);
                                else
                                    tempDictionary[sb.ToString()] += number;
                            }
                            sb.Clear();
                        }
                    }
                }
            }
            return tempDictionary;
        }

        public static void Update(this Dictionary<string, Dictionary<string, int>> currentDictionary, int exValue, int newValue)
        {
            Dictionary<string, Dictionary<string, int>> tempDict = currentDictionary.CopyDictionary();
            currentDictionary.Clear();
            foreach(var item in tempDict)
            {
                if (!currentDictionary.ContainsKey(item.Key))
                    currentDictionary.Add(item.Key, new Dictionary<string, int>());
                foreach (var innerItem in item.Value)
                {
                    if (!currentDictionary[item.Key].ContainsKey(innerItem.Key))
                        currentDictionary[item.Key].Add(innerItem.Key, innerItem.Value / exValue * newValue);
                    else
                        currentDictionary[item.Key][innerItem.Key] += innerItem.Value / exValue * newValue;
                }
            }
        }

        public static void InsertCut(this Dictionary<string, Dictionary<string, int>> currentDictionary, string compositionStr, string size, int number)
        {
            if (!currentDictionary.ContainsKey(compositionStr))
                currentDictionary.Add(compositionStr, new Dictionary<string, int> { { size, number } });
            else
            {
                if (!currentDictionary[compositionStr].ContainsKey(size))
                    currentDictionary[compositionStr].Add(size, number);
                else
                    currentDictionary[compositionStr][size] += number;
            } 
        }

        public static void InsertComponents(this Dictionary<string, Dictionary<string, int>> currentDictionary, string compositionStr, string size, int number)
        {
            Dictionary<string, int> tempDict = compositionStr.GetDictionary("Крой", number);
            if (tempDict.Count > 0)
                foreach (var item in tempDict)
                {
                    if (!currentDictionary.ContainsKey(item.Key))
                        currentDictionary.Add(item.Key, new Dictionary<string, int> { { size, item.Value } });
                    else
                    {
                        if (!currentDictionary[compositionStr].ContainsKey(size))
                            currentDictionary[compositionStr].Add(size, item.Value);
                        else
                            currentDictionary[compositionStr][size] += item.Value;
                    }
                }
        }
    }
}
