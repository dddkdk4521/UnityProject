using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBoard : BaseBoard
{
    [SerializeField]
    UILabel DamageLabel = null;

    public AnimationCurve Curve;
    public AnimationCurve ScaleToBoardEffect;

    public override eBoardType BOARD_TYPE
    {
        get
        {
            return eBoardType.BOARD_DAMAGE;
        }
    }

    public override void SetData(string strKey, params object[] datas)
    {
        if (strKey.Equals(ConstValue.SetData_Damage) == true)
        {
            double damage = (double)datas[0];
            DamageLabel.text = damage.ToString();

            base.UpdateBoard();
        }
        else
        {
            base.SetData(strKey, datas);
        }
    }

    public override void UpdateBoard()
    {
        base.CurTime += Time.deltaTime;

        Vector3 pos = transform.localPosition;
        pos += Vector3.up * 0.2f * Time.deltaTime;
        //transform.localScale *= 0.9f * Time.deltaTime;

        float x = 5f * Curve.Evaluate(CurTime / DestroyTime);
        pos.x += (x > 4f) ? x : -x;

        transform.localPosition = pos;
        transform.localScale = Vector3.one * 
            ScaleToBoardEffect.Evaluate(CurTime / DestroyTime);
    }
}
