using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterStatusData
{
    Dictionary<string, StatusData> DicStatus = new Dictionary<string, StatusData>();

    bool bRefresh = false;
    StatusData TotalStatus = new StatusData();

    public void AddStatusData(string strKey, StatusData statusData)
    {
        this.DicStatus.Remove(strKey);
        this.DicStatus.Add(strKey, statusData);
        this.bRefresh = true;
    }

    public void RemoveStatusData(string strkey)
    {
        this.DicStatus.Remove(strkey);
        this.bRefresh = true;
    }

    public double GetStatusData(eStatusData statusData)
    {
        RefreshtotalStatus();
        return TotalStatus.GetStatusData(statusData);
    }

    void RefreshtotalStatus()
    {
        if (this.bRefresh == false)
        {
            return;
        }

        this.TotalStatus.InitData();

        foreach (KeyValuePair<string, StatusData> pair in DicStatus)
        {
            StatusData data = pair.Value;
            this.TotalStatus.AddCopy(data);
        }

        this.bRefresh = false;
    }
}
