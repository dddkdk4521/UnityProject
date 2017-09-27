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
		ItemManager.Instance.ItemInit();

		IsInit = true;
	}

	public void LoadGame()
	{
		// Init
		this.StackTime = 0.0f;
		this.KillCount = 0;
		this.IsGameOver = false;

		// Stage
		SelectStageInfo = StageManager.Instance.LoadStage(SelectStage);

		// Player 
		// Get actor
		PlayerActor = ActorManager.Instance.PlayerLoad();

		// Item
		foreach(KeyValuePair<eSlotType, ItemInstance>pair in 
			ItemManager.Instance.DIC_EQUIP)
		{
			StatusData status = pair.Value.ITEM_INFO.STATUS;
			PlayerActor.SelfCharacter.GetCharacterStatus.AddStatusData
				(pair.Key.ToString(), status);
		}

		PlayerActor.SelfCharacter.IncreaseCurrentHP(9999999999999999999);

        // HP bar
		BaseBoard hpBoard = BoardManager.Instance.GetBoardData(PlayerActor, eBoardType.BOARD_HP);
		if(hpBoard != null)
		{
			hpBoard.SetData(ConstValue.SetData_HP,
				PlayerActor.GetStatusData(eStatusData.MAX_HP),
				PlayerActor.SelfCharacter.CURRENT_HP);
		}
		
		if(SelectStageInfo.CLEAR_TYPE == eClearType.CLEAR_TIME)
		{
			UIManager.Instance.SetText(false,
				(float)SelectStageInfo.CLEAR_FINISH - StackTime);
		}
		else
		{
			UIManager.Instance.SetText(true,
				(float)SelectStageInfo.CLEAR_FINISH - KillCount);
		}

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

			UIManager.Instance.SetText(false,
				(float)SelectStageInfo.CLEAR_FINISH - StackTime);

			if ( SelectStageInfo.CLEAR_FINISH < StackTime)
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

		UIManager.Instance.SetText(true,
			(float)SelectStageInfo.CLEAR_FINISH - KillCount);

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
