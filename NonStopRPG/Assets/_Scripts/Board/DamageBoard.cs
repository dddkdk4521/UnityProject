using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBoard : BaseBoard 
{
	[SerializeField]
	UILabel DamageLabel = null;

	public AnimationCurve Curve;
	public AnimationCurve ScaleCurve;

	public override eBoardType BOARD_TYPE
	{
		get
		{
			return eBoardType.BOARD_DAMAGE;
		}
	}

	public override void SetData(string strKey, params object[] datas)
	{
		if(strKey.Equals(ConstValue.SetData_Damage) == true)
		{
			double damage = (double)datas[0];
			DamageLabel.text = damage.ToString();

			base.UpdateBoard();
		}
		else
			base.SetData(strKey, datas);
	}

	public override void UpdateBoard()
	{
		CurTime += Time.deltaTime;

		Vector3 pos = transform.localPosition;
		pos += Vector3.up * 10f * Time.deltaTime;

		float x = 5f * Curve.Evaluate(CurTime / DestroyTime);
		pos.x += x;

		transform.localPosition = pos;

		transform.localScale = Vector3.one *
			ScaleCurve.Evaluate(CurTime / DestroyTime); 
	}


}
