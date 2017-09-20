using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public Actor PlayerActor;

    bool IsInit = false;
    bool IsGameOver = true;
    public bool GAME_OVER
    {
        get
        {
            return IsGameOver;
        }
    }

    //void Start()
    //{
    //    GameInit();
    //    LoadGame();
    //}

    public void GameInit()
    {
        if (this.IsInit == true)
        {
            return;
        }

        

        this.IsInit = true;
    }

    public void LoadGame()
    {
        // Get actor
        PlayerActor = ActorManager.Instance.PlayerLoad();
        
        // Camera Setting
        CameraManager.Instance.CameraInit(PlayerActor);

        this.IsGameOver = false;
    }

    public void SetGameOver()
    {
        this.IsGameOver = true;
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