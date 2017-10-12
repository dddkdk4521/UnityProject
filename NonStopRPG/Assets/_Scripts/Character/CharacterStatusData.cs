using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatusData
{
	private Dictionary<string, StatusData> DicStatus= new Dictionary<string, StatusData>();

	private bool bRefresh = false;
	private StatusData TotalStatus = new StatusData();

	public void AddStatusData(string strKey, StatusData statusData)
	{
		this.DicStatus.Remove(strKey);
		this.DicStatus.Add(strKey, statusData);

        this.bRefresh = true;
	}

	public void RemoveStatusData(string strKey)
	{
		this.DicStatus.Remove(strKey);

        bRefresh = true;
	}

	public double GetStatusData(eStatusData statusData)
	{
		this.RefreshTotalStatus();

        return this.TotalStatus.GetStatusData(statusData);
	}

	void RefreshTotalStatus()
	{
		if (this.bRefresh == false)
			return;

		this.TotalStatus.InitData();

		foreach (KeyValuePair<string,StatusData> pair in DicStatus)
		{
			StatusData data = pair.Value;
			this.TotalStatus.AddCopy(data);
		}

		this.bRefresh = false;
	}
}
