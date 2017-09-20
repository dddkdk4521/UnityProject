using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBoard : BaseBoard
{
    [SerializeField]
    UIProgressBar ProgressBar = null;

    [SerializeField]
    UILabel Label;

    public override eBoardType BOARD_TYPE
    {
        get
        {
            return eBoardType.BOARD_HP;
        }
    }

    public override void SetData(string strKey, params object[] datas)
    {
        if (strKey.Equals(ConstValue.SetData_HP))
        {
            double maxHP = (double)datas[0];
            double curHp = (double)datas[1];

            ProgressBar.value = (float)(curHp / maxHP);
            Label.text = curHp + " / " + maxHP;
        }
        else
        {
            base.SetData(strKey, datas);
        }
    }
}
