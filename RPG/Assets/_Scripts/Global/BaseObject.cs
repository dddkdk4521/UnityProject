using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObject : MonoBehaviour
{
    Dictionary<string, UnityEngine.Component> DicComponent = new Dictionary<string, Component>();

    BaseObject Target = null;
    public BaseObject TargetComponent
    {
        get { return Target; }
        set { Target = value;}
    }

    eBaseObjectState _ObjectState = eBaseObjectState.STATE_NORMAL;
    public eBaseObjectState ObjectState
    {
        get
        {
            return TargetComponent == null ? _ObjectState : TargetComponent._ObjectState;
        }

        set
        {
            if(TargetComponent == null)
            {
                _ObjectState = value;
            }
            else
            {
                TargetComponent._ObjectState = value;
            }
        }
    }

    public GameObject SelfObject
    {
        get
        {
            return TargetComponent == null ? this.gameObject : TargetComponent.gameObject;
        }
    }

    public Transform SelfTransform
    {
        get
        {
            return TargetComponent == null ? this.transform : TargetComponent.transform;
        }
    }

    virtual public object GetData(string keyData, params object[] datas)
    {
        return null;
    }

    virtual public object ThrowEvent(string keyData, params object[] datas)
    {
        return null;
    }

    public Transform GetChild(string strname)
    {
        return _GetChild(strname, SelfTransform);
    }

    private Transform _GetChild(string strName, Transform trans)
    {
        if(trans.name == strName)
        {
            return trans;
        }

        for(int i = 0; i < trans.childCount; i++)
        {
            Transform returnTrans = _GetChild(strName, trans.GetChild(i));
            if(returnTrans != null)
            {
                return returnTrans;
            }
        }

        return null;
    }

    public T SelfComponent<T>() where T : UnityEngine.Component
    {
        string objectName = "";
        string typeName = typeof(T).ToString();

        T tempComponent = default(T);

        if(TargetComponent == null)
        {
            objectName = SelfObject.name;
            // typename 포합여부 확인
            if (DicComponent.ContainsKey(typeName))
            {
                tempComponent = DicComponent[typeName] as T;
            }
            else
            {
                tempComponent = this.GetComponent<T>();
                if(tempComponent != null)
                {
                    DicComponent.Add(typeName, tempComponent);
                }
            }
        }
        else
        {
            tempComponent = TargetComponent.SelfComponent<T>();
        }

        if(tempComponent  == null)
        {
            Debug.LogError("GameObject Name  :  " + objectName + " null Component : " + typeName);
        }

        return tempComponent;
    }
}
