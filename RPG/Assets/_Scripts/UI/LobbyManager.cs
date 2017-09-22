using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoSingleton<LobbyManager> 
{
	public void LoadLobby()
	{
		UI_Tools.Instance.ShowUI(eUIType.Pf_UI_Lobby);
	}

	public void DisableLobby()
	{
		UI_Tools.Instance.HideUI(eUIType.Pf_UI_Lobby);
	}
}
