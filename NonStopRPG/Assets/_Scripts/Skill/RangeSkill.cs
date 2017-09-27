using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeSkill : BaseSkill
{
	GameObject ModelPrefab = null;

	public override void InitSkill()
	{
		ModelPrefab = Resources.Load("Prefabs/RangeModel") as GameObject;

		if (ModelPrefab == null)
			return;

		GameObject go = Instantiate(ModelPrefab, 
			Vector3.zero, 
			Quaternion.identity);

		go.transform.SetParent(this.transform, false);
	}

	public override void UpdateSkill()
	{
		if (TARGET == null)
		{
			END = true;
			return;
		}

		Vector3 targetPosition = SelfTransform.position +
			(TARGET.SelfTransform.position - SelfTransform.position).normalized
			* 10 * Time.deltaTime;

		SelfTransform.position = targetPosition;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (END == true)
			return;

		GameObject colObject = other.gameObject;
		BaseObject actorObject = colObject.GetComponent<BaseObject>();

		if (actorObject != null && actorObject != TARGET)
			return;

		TARGET.ThrowEvent(ConstValue.EventKey_Hit,
			OWNER.GetData(ConstValue.ActorData_Character),
			SKILL_TEMPLATE);

		END = true;

	}


}
