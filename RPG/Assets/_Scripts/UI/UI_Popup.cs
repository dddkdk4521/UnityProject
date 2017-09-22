using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void YesEvent();
public delegate void NoEvent();

public class UI_Popup : BaseObject
{
    YesEvent Yes;
    NoEvent No;

    UILabel TitleLabel;
    UILabel ContentsLabel;

    UIButton YesBtn;
    UIButton NoBtn;

    private void Awake()
    {
        this.TitleLabel = GetChild("Title").GetComponent<UILabel>();
        this.ContentsLabel = GetChild("Contents").GetComponent<UILabel>();

        this.YesBtn = GetChild("YesBtn").GetComponent<UIButton>();
        this.NoBtn = GetChild("NoBtn").GetComponent<UIButton>();

        EventDelegate.Add(YesBtn.onClick, new EventDelegate(this, "OnClickedYesBtn"));
        EventDelegate.Add(NoBtn.onClick, new EventDelegate(this, "OnClickedNoBtn"));

    }

    public void Set(YesEvent yes, NoEvent no, string title, string contents)
    {
        Yes = yes;
        No = no;
        TitleLabel.text = title;
        ContentsLabel.text = contents;
    }

    public void OnClickedYesBtn()
    {
        if (Yes != null)
        {
            Yes();
        }
    }

    public void OnClickedNoBtn()
    {
        if (No != null)
        {
            No();
        }
    }
}
