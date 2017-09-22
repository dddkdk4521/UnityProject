using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObject : MonoBehaviour
{
	Dictionary<string, UnityEngine.Component> DicComponent
		 = new Dictionary<string, Component>();

	BaseObject TargetObject = null;

	public BaseObject TargetComponent
	{
		get { return TargetObject; }
		set { TargetObject = value; }
	}

	eBaseObjectState _ObjectState = eBaseObjectState.STATE_NORMAL;
	public eBaseObjectState ObjectState
	{
		get
		{
			if (TargetComponent == null)
				return _ObjectState;
			else
				return TargetComponent._ObjectState;
		}

		set
		{
			if (TargetComponent == null)
				_ObjectState = value;
			else
				TargetComponent._ObjectState = value;
		}
	}

	public GameObject SelfObject
	{
		get
		{
			if (TargetComponent == null)
				return this.gameObject;
			else
				return TargetComponent.gameObject;
		}
	}

	public Transform SelfTransform
	{
		get
		{
			if (TargetComponent == null)
				return this.transform;
			else
				return TargetComponent.transform;
		}
	}

	virtual public object GetData(string keyData, params object[] datas)
	{
		return null;
	}

	virtual public void ThrowEvent(string keyData, params object[] datas)
	{
	
	}

	public Transform GetChild(string strName)
	{
		// this.GetChild(string); -> BaseObject
		// transform.GetChild(int);

		return _GetChild(strName, SelfTransform);
	}

	private Transform _GetChild(string strName, Transform trans)
	{
		if (trans.name == strName)
			return trans;

		for(int i = 0; i < trans.childCount; i++)
		{
			Transform returnTrans = _GetChild(strName, trans.GetChild(i));
			if (returnTrans != null)
				return returnTrans;
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
			// typeName 키를 포함하고 있는지.
			if(DicComponent.ContainsKey(typeName))
			{
				tempComponent = DicComponent[typeName] as T;
			}
			else
			{
				tempComponent = this.GetComponent<T>();
				if (tempComponent != null)
					DicComponent.Add(typeName, tempComponent);
			}
		}
		else
		{
			tempComponent = TargetComponent.SelfComponent<T>();
		}

		if(tempComponent == null)
		{
			Debug.LogError("GameObject Name : " + objectName
				+ " null Component : " + typeName);
		}

		return tempComponent;
	}

}
