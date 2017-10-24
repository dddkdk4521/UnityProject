using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Logo : BaseObject 
{
	private UIButton StartBtn = null;

	void Start () 
	{
		Transform temp = this.GetChild("StartBtn");
		if(temp == null)
		{
			Debug.Log("Logo 에 StartBtn이 없습니다.");

            return;
		}

		this.StartBtn = temp.GetComponent<UIButton>();

		//StartBtn.onClick.Add(new EventDelegate(this, "GoLobby"));
		//EventDelegate.Add(StartBtn.onClick, new EventDelegate(this, "GoLobby"));

		EventDelegate.Add(this.StartBtn.onClick,
			// Lamda
			() =>
			{
				Scene_Manager.Instance.LoadScene(eSceneType.Scene_Lobby);
			});
	}

	//void GoLobby()
	//{
		// 신 전환
	//}
}
