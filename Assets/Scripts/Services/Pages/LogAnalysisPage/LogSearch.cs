using UnityEngine;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using TMPro;

public class LogSearch : MonoBehaviour
{
    List<Visitor> visitorList = CSVParser.visitorList;
    string keyword;

    public void OnClick()
    {
        GameObject keyobj = GameObject.Find("InputText-LogAnalysis");
        keyword = Regex.Escape(keyobj.GetComponent<TextMeshProUGUI>().text);

        GameObject obj = GameObject.Find("LogContent");
        foreach (Transform log in obj.transform)
        {
            Destroy(log.gameObject);
        }

        Search();
    }

    void Search()
    {
        if (string.IsNullOrEmpty(keyword))
        {
            LogCreater.instance.LogCreate(visitorList);
            return;
        }

        List<Visitor> foundVisitors = new List<Visitor>();

        if (visitorList != null)
        {
            foreach (Visitor visitor in visitorList)
            {
                if (visitor == null) continue;

                Debug.Log($"{visitor.name} = {keyword} ?");

                if (
                    (visitor.name != null && Regex.IsMatch(visitor.name, keyword, RegexOptions.IgnoreCase)) ||
                    (visitor.src != null && Regex.IsMatch(visitor.src, keyword, RegexOptions.IgnoreCase)) ||
                    (visitor.dst != null && Regex.IsMatch(visitor.dst, keyword, RegexOptions.IgnoreCase)) ||
                    (visitor.job != null && Regex.IsMatch(visitor.job, keyword, RegexOptions.IgnoreCase)) ||
                    (visitor.size != null && Regex.IsMatch(visitor.size, keyword, RegexOptions.IgnoreCase)) ||
                    (visitor.item != null && Regex.IsMatch(visitor.item, keyword, RegexOptions.IgnoreCase))
                )
                {
                    foundVisitors.Add(visitor);
                }
            }
        }

        foreach (Visitor visitor in foundVisitors)
        {
            Debug.Log($"{visitor.name}, {visitor.src}");
        }

        //LogCreater.instance.LogCreate(foundVisitors);
    }

    //string Normalize(string s)
    //{
    //    return s?.Trim().Replace("\r", "").Replace("\n", "").ToLower();
    //}

    //void Search()
    //{
    //    if (string.IsNullOrEmpty(keyword))
    //    {
    //        LogCreater.instance.LogCreate(visitorList);
    //        return;
    //    }

    //    foreach (var v in visitorList)
    //    {
    //        Debug.Log($"name='{v.name}' (len={v.name?.Length})");
    //        Debug.Log($"src='{v.src}' (len={v.src?.Length})");
    //        Debug.Log($"dst='{v.dst}' (len={v.dst?.Length})");
    //        Debug.Log($"job='{v.job}' (len={v.job?.Length})");
    //        Debug.Log($"size='{v.size}' (len={v.size?.Length})");
    //        Debug.Log($"item='{v.item}' (len={v.item?.Length})");
    //    }
    //    Debug.Log($"keyword='{keyword}' (len={keyword.Length})");

    //    var key = keyword.Trim().ToLower();
    //    Debug.Log($"[DEBUG] keyword='{keyword}' → key='{key}'");

    //    List<Visitor> foundVisitors = visitorList
    //        .Where(v => v != null)
    //        .Where(v =>
    //            Normalize(v.name)?.Contains(key) == true ||
    //            Normalize(v.src)?.Contains(key) == true ||
    //            Normalize(v.dst)?.Contains(key) == true ||
    //            Normalize(v.job)?.Contains(key) == true ||
    //            Normalize(v.size)?.Contains(key) == true ||
    //            Normalize(v.item)?.Contains(key) == true
    //        )
    //        .ToList();

    //    Debug.Log($"[DEBUG] foundVisitors.Count={foundVisitors.Count}");

    //    if (foundVisitors.Count == 0)
    //    {
    //        Debug.Log("検索結果なし");
    //        return;
    //    }

    //    LogCreater.instance.LogCreate(foundVisitors);
    //}
}
