using UnityEngine;
using System.Collections;

public class UI_LoadingBar : BaseObject
{
    UIProgressBar ProgressBar;

    private void OnEnable()
    {
        if (this.ProgressBar == null)
        {
            this.ProgressBar = this.GetComponentInChildren<UIProgressBar>();
        }        
    }

    public override void ThrowEvent(string KeyData, params object[] datas)
    {
        if (KeyData.Equals("LoadingValue"))
        {
            ProgressBar.value = (float)datas[0];
        }
        else
        {
            base.ThrowEvent(KeyData, datas);
        }
    }
}
