using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitAnimation : StateMachineBehaviour 
{
	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (stateInfo.normalizedTime >= 1.0f)
			animator.SetInteger("Hit", 0);
	}
}
