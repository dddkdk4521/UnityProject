using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoSingleton<Scene_Manager>
{
    bool IsAsyc = true;
    AsyncOperation Operation = null;

    eSceneType CurrentState = eSceneType.Scene_Logo;
    eSceneType NextState = eSceneType.Scene_None;

    float StackTime = 0.0f;
    public eSceneType CURRENT_SCENE
    {
        get
        {
            return CurrentState;
        }
    }

    public void LoadScene(eSceneType type, bool isAsyc = true)
    {
        if (CurrentState == type)
        {
            return;
        }

        NextState = type;
        this.IsAsyc = isAsyc;
    }

    private void Update()
    {
        if (this.Operation != null)
        {
            this.StackTime += Time.deltaTime;

            UI_Tools.Instance.ShowLoadingUI(StackTime / 2f);
            UI_Tools.Instance.SubRootCreate();

            if (this.Operation.isDone == true
                        && this.StackTime >= 2.0f)
            {
                this.CurrentState = this.NextState;
                ComplatedLoad(this.CurrentState);

                this.Operation = null;
                this.NextState = eSceneType.Scene_None;

                UI_Tools.Instance.HideUI(eUIType.Pf_UI_Loading, true);
            }
            else
            {
                return;
            }
        }

        if (CurrentState == eSceneType.Scene_None)
        {
            return;
        }

        if (NextState != eSceneType.Scene_None 
                                && CurrentState != NextState)
        {
            DisableScene(CurrentState);

            // Scene Chanage
            if (this.IsAsyc)
            {
                // Asyc
                Operation = 
                    SceneManager.LoadSceneAsync(NextState.ToString());

                this.StackTime = 0.0f;

                // 로딩바 생성
                UI_Tools.Instance.ShowLoadingUI(Operation.progress);
                
            }
            else
            {
                // Sys
                SceneManager.LoadScene(NextState.ToString());
                this.CurrentState = NextState;
                this.NextState = eSceneType.Scene_None;

                ComplatedLoad(CurrentState);
            }
        }
    }

    void ComplatedLoad(eSceneType type)
    {
        UI_Tools.Instance.SubRootCreate();

        switch (type)
        {
            case eSceneType.Scene_None:
                break;
            case eSceneType.Scene_Logo:
                break;
            case eSceneType.Scene_Lobby:
                {
                    GameManager.Instance.GameInit();
                }
                break;
            case eSceneType.Scene_Game:
                {
                    GameManager.Instance.LoadGame();
                }
                break;
            default:
                break;
        }
    }

    void DisableScene(eSceneType type)
    {
        UI_Tools.Instance.Clear();

        switch (type)
        {
            case eSceneType.Scene_None:
                break;
            case eSceneType.Scene_Logo:
                break;
            case eSceneType.Scene_Lobby:
                break;
            case eSceneType.Scene_Game:
                SkillManager.Instance.ClearSkill();
                break;
            default:
                break;
        }
    }
}