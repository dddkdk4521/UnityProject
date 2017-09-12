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
        // Player
        GameObject playerPrefab = Resources.Load("Prefabs/Actor/" + "Player") as GameObject;

        // Create Clone in Scene
        GameObject go = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        PlayerActor = go.GetComponent<Actor>();

        // Camera Setting
        CameraManager.Instance.CameraInit(PlayerActor);
    }
}
