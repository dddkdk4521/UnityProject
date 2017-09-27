using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameScene : MonoBehaviour
{
    private void Awake()
    {
        Scene_Manager.Instance.LoadScene(eSceneType.Scene_Game);   
    }
}
