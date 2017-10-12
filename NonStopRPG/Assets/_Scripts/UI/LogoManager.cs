using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoManager : MonoBehaviour 
{
	void Start () 
	{
		UI_Tools.Instance.ShowUI(eUIType.Pf_UI_Logo);
	}
}
