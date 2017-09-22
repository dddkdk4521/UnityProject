using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Stage : BaseObject 
{
	bool IsInit = false;
	GameObject IconPrefab;
	UIGrid Grid;

	private void Awake()
	{
		IconPrefab = 
			Resources.Load("Prefabs/UI/Pf_UI_StageIcon") as GameObject;
		Grid = GetComponentInChildren<UIGrid>();
		AddIcon();
	}

	void AddIcon()
	{
		foreach(KeyValuePair<int, StageInfo> pair in
			StageManager.Instance.DIC_STAGEINFO)
		{
			GameObject go =
				NGUITools.AddChild(Grid.gameObject, IconPrefab);

			go.GetComponent<UI_StageIcon>().Init(pair.Value);
		}

		Grid.repositionNow = true;
		
	}


}
