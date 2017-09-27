using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager> 
{
	UILabel TopLabel;

	public override void Init()
	{
		TopLabel = this.transform.Find("TopLabel").GetComponent<UILabel>();
	}

	public void SetText(bool isKill, float data)
	{
		if(isKill)
		{
			TopLabel.text = "KILL COUNT : " + ((int)data).ToString();
		}
		else
		{
			TopLabel.text = string.Format(
				"Time {0} : {1}",
				(int)(data / 60), (int)(data % 60));
		}
	}
}
