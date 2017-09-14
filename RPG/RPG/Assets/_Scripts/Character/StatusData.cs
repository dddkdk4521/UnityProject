using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class StatusData
{
    Dictionary<eStatusData, double> DicData = new Dictionary<eStatusData, double>();

    public void InitData()
    {
        DicData.Clear();
    }

    public void AddCopy(StatusData data)
    {
        foreach (KeyValuePair<eStatusData, double> pair in data.DicData)
        {
            IncreaseData(pair.Key, pair.Value);
        }
    }

    public void IncreaseData(eStatusData key, double value)
    {
        double preValue = 0.0;

        DicData.TryGetValue(key, out preValue);

        DicData[key] = preValue + value;
    }

    public void DecreaseData(eStatusData key, double value)
    {
        double preValue = 0.0;

        DicData.TryGetValue(key, out preValue);

        DicData[key] = preValue - value;
    }

    public void SetData(eStatusData key, double value)
    {
        DicData[key] = value;
    }

    public void RemoveData(eStatusData key, double value)
    {
        if(DicData.ContainsKey(key) == true)
        {
            DicData.Remove(key);
        }
    }

    public double GetStatusData(eStatusData statusData)
    {
        double preValue = 0.0;

        DicData.TryGetValue(statusData, out preValue);

        return preValue;
    }

    public string StatusString()
    {
        string returnStr = string.Empty;

        foreach(var pair in DicData)
        {
            returnStr += pair.Key.ToString();
            returnStr += " " + pair.Value.ToString();
            returnStr += "\n";
        }

        return returnStr;
    }
}
