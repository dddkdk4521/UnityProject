using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Stage : BaseObject 
{
	bool IsInit = false;
	GameObject IconPrefab;
	UIGrid Grid;

    UIButton CloseBtn;

	private void Awake()
	{
		IconPrefab = 
			Resources.Load("Prefabs/UI/Pf_UI_StageIcon") as GameObject;
		Grid = GetComponentInChildren<UIGrid>();
		AddIcon();

        this.CloseBtn = GetChild("CloseBtn").GetComponent<UIButton>();
        EventDelegate.Add(CloseBtn.onClick,
            () => {
                UI_Tools.Instance.HideUI(eUIType.Pf_UI_Stage);
            });
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
