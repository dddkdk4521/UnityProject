using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Lobby : BaseObject 
{
	UIButton StageBtn = null;

	private void Awake()
	{
		Transform trans = GetChild("StageBtn");
		if(trans == null)
		{
			Debug.LogError("StageBtn 을 찾을수 없습니다.");
			return;
		}
		StageBtn = trans.GetComponent<UIButton>();
		EventDelegate.Add(StageBtn.onClick,
			new EventDelegate(this, "ShowStage"));

	}

	void ShowStage()
	{
		UI_Tools.Instance.ShowUI(eUIType.Pf_UI_Stage);
	}
}
