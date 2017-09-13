using UnityEngine;
using System.Collections;

public class GameManager : MonoSingleton<GameManager>
{
    public Actor PlayerActor;

    bool IsInit = false;

    private void Start()
    {
        GameInit();
        LoadGame();
    }

    public void GameInit()
    {
        if (IsInit == true)
        {
            return;
        }

        IsInit = true;
    }

    public void LoadGame()
    {
        PlayerActor = ActorManager.Instance.PlayerLoad();

        // Camera Setting
        CameraManager.Instance.CameraInit(PlayerActor);
    }
}
