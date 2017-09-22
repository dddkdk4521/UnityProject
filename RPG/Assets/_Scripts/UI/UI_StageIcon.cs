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

    // NGUI 
	public void OnClick()
	{
        BaseObject bo = UI_Tools.Instance.ShowUI(eUIType.Pf_UI_Popup);
        UI_Popup popup = bo.GetComponent<UI_Popup>();

        popup.Set(
            () =>
            {
                Debug.Log(INFO.NAME + "입장");
                GameManager.Instance.SelectStage = int.Parse(INFO.KEY);
                Scene_Manager.Instance.LoadScene(eSceneType.Scene_Game);
            },
            () =>
            {
                UI_Tools.Instance.HideUI(eUIType.Pf_UI_Popup);
            },
            "스테이지 선택",
            "스테이지 " + INFO.NAME + " 을 입장시키겠습니까?"
            );
	}
}
