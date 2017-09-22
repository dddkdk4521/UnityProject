using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatusData
{
	Dictionary<string, StatusData> DicStatus
		 = new Dictionary<string, StatusData>();

	bool bRefresh = false;
	StatusData TotalStatus = new StatusData();

	public void AddStatusData(
		string strKey, StatusData statusData)
	{
		DicStatus.Remove(strKey);
		DicStatus.Add(strKey, statusData);
		bRefresh = true;
	}

	public void RemoveStatusData(string strKey)
	{
		DicStatus.Remove(strKey);
		bRefresh = true;
	}

	public double GetStatusData(eStatusData statusData)
	{
		RefreshTotalStatus();
		return TotalStatus.GetStatusData(statusData);
	}

	void RefreshTotalStatus()
	{
		if (bRefresh == false)
			return;

		TotalStatus.InitData();

		foreach (KeyValuePair<string,StatusData> pair 
			in DicStatus)
		{
			StatusData data = pair.Value;
			TotalStatus.AddCopy(data);
		}

		bRefresh = false;
	}


}
