using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
	bool IsInit = false;
	public Actor PlayerActor;

	bool IsGameOver = true;
	public bool GAME_OVER { get { return IsGameOver; } }

	public int SelectStage = 0;
	StageInfo SelectStageInfo = null;

	float StackTime = 0.0f;
	int KillCount = 0;

	//void Start ()
	//{
	//	GameInit();
	//	LoadGame();
	//}

	public void GameInit()
	{
		if (IsInit == true)
			return;

		StageManager.Instance.StageInit();


		IsInit = true;
	}

	public void LoadGame()
	{
		// Init
		StackTime = 0.0f;
		KillCount = 0;
		IsGameOver = false;

		// Stage
		SelectStageInfo = StageManager.Instance.LoadStage(SelectStage);

		// Player 
		// Get actor
		PlayerActor = ActorManager.Instance.PlayerLoad();

		// Camera Setting
		CameraManager.Instance.CameraInit(PlayerActor);
	}

	private void Update()
	{
		if (IsGameOver == true)
			return;

		if (Scene_Manager.Instance.CURRENT_SCENE != eSceneType.Scene_Game)
			return;

		if(SelectStageInfo.CLEAR_TYPE == eClearType.CLEAR_TIME)
		{
			StackTime += Time.deltaTime;

			if( SelectStageInfo.CLEAR_FINISH < StackTime)
			{
				SetGameOver();
			}
		}
	}

	public void KillCheck(Actor dieActor)
	{
		if (IsGameOver == true)
			return;

		if (Scene_Manager.Instance.CURRENT_SCENE != eSceneType.Scene_Game)
			return;

		if (SelectStageInfo.CLEAR_TYPE != eClearType.CLEAR_KILLCOUNT)
			return;

		if (PlayerActor.TeamType == dieActor.TeamType)
			return;

		KillCount++;

		if (SelectStageInfo.CLEAR_FINISH <= KillCount)
			SetGameOver();
	}


	public void SetGameOver()
	{
		IsGameOver = true;
		Time.timeScale = 0.1f;
		Debug.Log("GameOver");
		Invoke("GoLobby", 0.5f);
	}

	void GoLobby()
	{
		Time.timeScale = 1f;
		Scene_Manager.Instance.LoadScene(eSceneType.Scene_Lobby);
	}


}
