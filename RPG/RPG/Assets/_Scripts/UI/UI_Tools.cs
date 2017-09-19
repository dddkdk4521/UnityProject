using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Tools : MonoSingleton<UI_Tools>
{
    Dictionary<eUIType, BaseObject> DicUI = new Dictionary<eUIType, BaseObject>();
    GameObject SubRoot = null;

    // DontDestroy private
    GameObject SubUIRoot = null;
    Dictionary<eUIType, BaseObject> DicSubUI = new Dictionary<eUIType, BaseObject>();

    Camera _UICamera = null;
    Camera UI_Camera
    {
        get
        {
            if (_UICamera == null)
            {
                this._UICamera = NGUITools.FindCameraForLayer(LayerMask.NameToLayer("UI"));

                //this._UICamera = 
                //    GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
            }

            return _UICamera;
        }
    }
    
    BaseObject GetUI(eUIType uIType, bool isDontDestroy = false)
    {
        if (isDontDestroy == false)
        {
            if (DicUI.ContainsKey(uIType))
            {
                return DicUI[uIType];
            }
        }
        else
        {
            if (DicSubUI.ContainsKey(uIType))
            {
                return DicSubUI[uIType];
            }
        }

        GameObject makeUI = null;
        BaseObject baseObject = null;

        GameObject prefabUI = Resources.Load("Prefabs/UI/" + uIType.ToString()) as GameObject;

        if (prefabUI != null)
        {
            if (isDontDestroy == false)
            {
                // UICamera Child
                // Instantiate() -> Initailize -> Parent
                makeUI = NGUITools.AddChild(UI_Camera.gameObject, prefabUI);

                baseObject = GetComponent<BaseObject>();
                if (baseObject == null)
                {
                    Debug.Log(uIType.ToString() + " 오브젝트에 " + " BaseObject가 연결되어 있지 않습니다.");
                    baseObject = makeUI.AddComponent<BaseObject>();
                }

                DicUI.Add(uIType, baseObject);
            }
            else
            {
                // SubRoot Child
                if (this.SubUIRoot == null)
                {
                    SubRootCreate();
                }

                makeUI = NGUITools.AddChild(SubUIRoot, prefabUI);

                baseObject = makeUI.GetComponent<BaseObject>();
                if (baseObject == null)
                {
                    Debug.Log(uIType.ToString() + " 오브젝트에 " + " BaseObject가 연결되어 있지 않습니다.");
                    baseObject = makeUI.AddComponent<BaseObject>();
                }

                DicSubUI.Add(uIType, baseObject);
            }
        }

        return null;
    }

    public void SubRootCreate()
    {
        if (SubUIRoot == null)
        {
            GameObject subRoot = new GameObject("Loading...");
            subRoot.transform.SetParent(this.transform);
            this.SubUIRoot = subRoot;

            this.SubUIRoot.layer = LayerMask.NameToLayer("UI");
        }

        UIRoot uIRoot = UI_Camera.GetComponentInParent<UIRoot>();
        {
            SubUIRoot.transform.position = uIRoot.transform.position;
            SubUIRoot.transform.localScale = uIRoot.transform.localScale;
        }
    }

    public BaseObject ShowUI(eUIType uIType, bool isSub = false)
    {
        BaseObject uiObject = GetUI(uIType, isSub);
        if (uiObject != null 
            && uiObject.SelfObject.activeSelf == false)
        {
            uiObject.SelfObject.SetActive(true);
        }

        return uiObject;
    }

    public void HideUI(eUIType uIType, bool isSub = false)
    {
        BaseObject uiObject = GetUI(uIType, isSub);
        if (uiObject != null 
            && uiObject.SelfObject.activeSelf == true)
        {
            uiObject.SelfObject.SetActive(false);
        }
    }

    public void ShowLoadingUI(float value)
    {
        BaseObject loadingUI = GetUI(eUIType.Pf_UI_Loading, true);
        if (loadingUI == null)
        {
            return;
        }

        if (loadingUI.gameObject.activeSelf == false)
        {
            loadingUI.gameObject.SetActive(true);
        }


        loadingUI.ThrowEvent("LoadingValue", value);
    }

    public void Clear()
    {
        foreach(KeyValuePair<eUIType, BaseObject> pair in DicUI)
        {
            Destroy(pair.Value.gameObject);
        }
    }
}
