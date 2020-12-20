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
            string secondaryKey, Dictionary<string, int> additionalDictionary, int amount)
        {
            foreach(var item in additionalDictionary)
            {
                if (!currentDictionary.ContainsKey(item.Key))
                    currentDictionary.Add(item.Key, new Dictionary<string, int> { { secondaryKey, item.Value } });
                else
                {
                    if (!currentDictionary[item.Key].ContainsKey(secondaryKey))
                        currentDictionary[item.Key].Add(secondaryKey, amount * 2);
                    else
                        currentDictionary[item.Key][secondaryKey] += item.Value;
                }
            }
        }

        public static void Unite(this Dictionary<string, Dictionary<string, Dictionary<string, int>>> currentDictionary,
            string mainKey, Dictionary<string, Dictionary<string, int>> additionalDictionary)
        {
            if (!currentDictionary.ContainsKey(mainKey))
                currentDictionary.Add(mainKey, additionalDictionary.CopyDictionary());
            else
            {
                foreach (var item in additionalDictionary)
                {
                    if (!currentDictionary[mainKey].ContainsKey(item.Key))
                        currentDictionary[mainKey].Add(item.Key, item.Value.CopyDictionary());
                    else
                    {
                        foreach(var innerItem in item.Value)
                        {
                            if (!currentDictionary[mainKey][item.Key].ContainsKey(innerItem.Key))
                                currentDictionary[mainKey][item.Key].Add(innerItem.Key, innerItem.Value);
                            else
                                currentDictionary[mainKey][item.Key][innerItem.Key] += innerItem.Value;
                        }
                    }
                }         
            }
        }

        public static void Unite(this Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>> currentDictionary,
            string mainKey, string secondaryKey, Dictionary<string, Dictionary<string, int>> additionalDictionary)
        {
            if (!currentDictionary.ContainsKey(mainKey))
                currentDictionary.Add(mainKey, new Dictionary<string, Dictionary<string, Dictionary<string, int>>> { { secondaryKey, additionalDictionary.CopyDictionary() } });
            else
            {
                if (!currentDictionary[mainKey].ContainsKey(secondaryKey))
                    currentDictionary[mainKey].Add(secondaryKey, additionalDictionary.CopyDictionary());
                else
                {
                    foreach(var item in additionalDictionary)
                    {
                        if (!currentDictionary[mainKey][secondaryKey].ContainsKey(item.Key))
                            currentDictionary[mainKey][secondaryKey].Add(item.Key, item.Value.CopyDictionary());
                        else
                        {
                            foreach (var innerItem in item.Value)
                            {
                                if (!currentDictionary[mainKey][secondaryKey][item.Key].ContainsKey(innerItem.Key))
                                    currentDictionary[mainKey][secondaryKey][item.Key].Add(innerItem.Key, innerItem.Value);
                                else
                                    currentDictionary[mainKey][secondaryKey][item.Key][innerItem.Key] += innerItem.Value;
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
        public static Dictionary<string, int> GetDictionary(this string compositionStr, string sectorNameStr, int number)
        {
            Dictionary<string, int> tempDictionary = new Dictionary<string, int>();

            using (PgContext context = new PgContext())
            {
                if (compositionStr != null)
                {
                    string tempStr = null;
                    foreach (char i in compositionStr.ToArray())
                    {
                        if (i.ToString() != "/" && i.ToString() != ".")
                            tempStr += i;
                        else
                        {
                            if (context.Materials.Find(tempStr) != null && context.Materials.Find(tempStr).sectorName.Equals(sectorNameStr))
                            {
                                if (!tempDictionary.ContainsKey(tempStr))
                                    tempDictionary.Add(tempStr, number);
                                else
                                    tempDictionary[tempStr] += number;
                            }
                            tempStr = null;
                        }
                    }
                }
            }
            return tempDictionary;
        }

        //Cuts Dictionary special.
        public static void Insert(this Dictionary<string, Dictionary<string, int>> currentDictionary, string topCompositionStr, string botCompositionStr,
            string mainCompositionStr, string size, int number)
        {
            if (topCompositionStr != null & botCompositionStr != null)
            {
                if (topCompositionStr == botCompositionStr)
                    currentDictionary.Add(topCompositionStr, new Dictionary<string, int> { { size, number * 2 } });
                else
                {
                    currentDictionary.Add(topCompositionStr, new Dictionary<string, int> { { size, number } });
                    currentDictionary.Add(botCompositionStr, new Dictionary<string, int> { { size, number } });
                }
            }  

            Dictionary<string, int> tempDict = mainCompositionStr.GetDictionary("Крой", number);
            if (tempDict.Count != 0)
                foreach (var item in tempDict)
                    currentDictionary.Add(item.Key, new Dictionary<string, int> { { size, number } });
        }
    }
}
