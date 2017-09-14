using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    bool IsInit = false;
    public Actor PlayerActor;

    void Start()
    {
        GameInit();
        LoadGame();
    }

    public void GameInit()
    {
        if (IsInit == true)
            return;

        IsInit = true;
    }

    public void LoadGame()
    {
        // Get actor
        PlayerActor = ActorManager.Instance.PlayerLoad();
        
        // Camera Setting
        CameraManager.Instance.CameraInit(PlayerActor);
    }
}