using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimation : StateMachineBehaviour
{
    Actor TargetActor;
    bool bIsAttack = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        this.TargetActor = animator.GetComponentInParent<Actor>();
        if (TargetActor != null)
        {
            this.bIsAttack = true;
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (this.bIsAttack == true)
        {
            if (animatorStateInfo.normalizedTime >= 1.0f)
            {
                this.TargetActor.ChangeState(eAIStateType.AI_STATE_IDLE);
            }
        }           
    }
}
