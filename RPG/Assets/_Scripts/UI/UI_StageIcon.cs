using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_StageIcon : BaseObject
{
	StageInfo Info = null;
	public StageInfo INFO
	{
		get { return Info; }
	}

	UILabel StageName = null;

	public void Init(StageInfo info)
	{
		Info = info;
		StageName = this.GetComponentInChildren<UILabel>();
		StageName.text = info.NAME;
	}

	public void OnClick()
	{
		Debug.Log(INFO.NAME + "입장");
		GameManager.Instance.SelectStage = int.Parse(INFO.KEY);
		Scene_Manager.Instance.LoadScene(eSceneType.Scene_Game);
	}
}
