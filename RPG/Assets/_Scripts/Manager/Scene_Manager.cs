using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoSingleton<Scene_Manager>
{
	bool IsAsyc = true;
	AsyncOperation Operation = null;

	eSceneType CurrentState = eSceneType.Scene_Logo;
	eSceneType NextState = eSceneType.Scene_None;

	float StackTime = 0.0f;
	public eSceneType CURRENT_SCENE
	{ get { return CurrentState; } }

	public void LoadScene(eSceneType type, bool isAsyc = true)
	{
		if (CurrentState == type)
			return;

		NextState = type;
		IsAsyc = isAsyc;
	}

	private void Update()
	{
		if (Operation != null)
		{
			StackTime += Time.deltaTime;

			UI_Tools.Instance.SubRootCreate();

			UI_Tools.Instance.ShowLoadingUI(Operation.progress);
			if (Operation.isDone == true)
			
			//UI_Tools.Instance.ShowLoadingUI(StackTime / 2f);
			//if (Operation.isDone == true && StackTime >= 2.0f)
			{
				CurrentState = NextState;
				ComplateLoad(CurrentState);

				Operation = null;
				NextState = eSceneType.Scene_None;
				UI_Tools.Instance.HideUI(eUIType.Pf_UI_Loading, true);
			}
			else
				return;
		}

		if (CurrentState == eSceneType.Scene_None)
			return;

		if(NextState != eSceneType.Scene_None
			&& CurrentState != NextState)
		{
			DisableScene(CurrentState);

			// 신 전환
			if(IsAsyc)
			{
				// 비동기
				Operation =
					SceneManager.LoadSceneAsync(NextState.ToString());
				StackTime = 0.0f;
				// 로딩바 생성
				UI_Tools.Instance.ShowLoadingUI(0.0f);				
			}
			else
			{
				// 동기
				SceneManager.LoadScene(NextState.ToString());
				CurrentState = NextState;
				NextState = eSceneType.Scene_None;
				ComplateLoad(CurrentState);
			}
		}
	}

	void ComplateLoad(eSceneType type)
	{
		UI_Tools.Instance.SubRootCreate();

		switch (type)
		{
			case eSceneType.Scene_None:
				break;
			case eSceneType.Scene_Logo:
				break;
			case eSceneType.Scene_Lobby:
				LobbyManager.Instance.LoadLobby();
				GameManager.Instance.GameInit();
				break;
			case eSceneType.Scene_Game:
				GameManager.Instance.LoadGame();
				break;
			default:
				break;
		}
	}

	void DisableScene(eSceneType type)
	{

		switch (type)
		{
			case eSceneType.Scene_None:
				break;
			case eSceneType.Scene_Logo:
				break;
			case eSceneType.Scene_Lobby:
				LobbyManager.Instance.DisableLobby();
				break;
			case eSceneType.Scene_Game:
				SkillManager.Instance.ClearSkill();
				break;
			default:
				break;
		}

		UI_Tools.Instance.Clear();

	}



}
