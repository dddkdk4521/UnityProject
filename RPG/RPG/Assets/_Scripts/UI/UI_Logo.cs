using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Logo : BaseObject
{
    UIButton StartBtn = null;

	// Use this for initialization
	void Start ()
    {
        Transform temp = this.GetChild("StartBtn");
        if (temp == null)
        {
            Debug.Log("Logo에 StartBtn이 없음");
            return;
        }

        this.StartBtn = temp.GetComponent<UIButton>();
        //this.StartBtn.onClick.Add(new EventDelegate(this, "GoLobby"));

        //EventDelegate.Add(StartBtn.onClick, new EventDelegate(this, "GoLobbuy"));

        EventDelegate.Add(StartBtn.onClick, () => {
            Debug.Log("StartBtn OnClick");

            Scene_Manager.Instance.LoadScene(eSceneType.Scene_Game);
        });
	}
}
