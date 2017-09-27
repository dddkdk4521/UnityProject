using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimation : StateMachineBehaviour
{
	Actor TargetActor;
	bool bIsAttack = false;

	public override void OnStateEnter(
		Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
		// TargetActor = animator.transform.parent.GetComponent<Actor>();
		TargetActor = animator.GetComponentInParent<Actor>();

		if(TargetActor != null
			&& TargetActor.AI.CurrentAIState == eAIStateType.AI_STATE_ATTACK)
		{
			TargetActor.AI.IsAttack = true;
			bIsAttack = false;
		}
	}

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
		if (animatorStateInfo.normalizedTime >= 1.0f
			&& TargetActor.AI.IsAttack)
		{
			if(TargetActor.AI.CurrentAIState == eAIStateType.AI_STATE_ATTACK)
			{
				TargetActor.AI.IsAttack = false;
			}
		}

		if(bIsAttack == false 
			&& animatorStateInfo.normalizedTime >= 0.5f)
		{
			bIsAttack = true;
			TargetActor.RunSkill();
		}		
	}
}
