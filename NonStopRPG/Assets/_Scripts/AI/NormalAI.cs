using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAI : BaseAI
{
	protected override IEnumerator Idle()
	{
		// 탐지 범위
		float distance = 0f;
        //BaseObject targetObject = ActorManager.Instance.GetSearchEnemy(TargetComponent, out distance);

        BaseObject targetObject =
                StageManager.Instance.GetSearchRegenerator(TargetComponent, out distance);

        string TagName = this.TargetComponent.transform.tag.ToString();
        switch (TagName)
        {
            case "Player":
                {
                    //targetObject = StageManager.Instance.GetSearchRegenerator(TargetComponent, out distance);

                    targetObject =
                       //EnemyRegenerator.IsActiveRegen ?
                       StageManager.Instance.Is_ActiveRenerator() ?
                             ActorManager.Instance.GetSearchEnemy(TargetComponent, out distance)
                                : StageManager.Instance.GetSearchRegenerator(TargetComponent, out distance);
                }
                break;

            case "Monster":
                {
                    targetObject =
                        ActorManager.Instance.GetSearchEnemy(TargetComponent, out distance);
                }
                break;

            default:
                break;
        }

        if (targetObject != null)
		{
			// 공격 범위
			float attackRange = 1f;

			//float distance =
			//	Vector3.Distance
			//	(
			//		targetObject.SelfTransform.position,
			//		SelfTransform.position
			//	);

            // 스킬
			SkillData skillData = 
				TargetComponent.GetData(ConstValue.ActorData_SkillData, 0)
				as SkillData;

			if (skillData != null)
			{
				attackRange = skillData.RANGE;
			}

			if(distance < attackRange)
			{
				Stop();
				AddNextAI(eAIStateType.AI_STATE_ATTACK, targetObject);
			}
			else
			{
				AddNextAI(eAIStateType.AI_STATE_MOVE);
			}

		}

		yield return StartCoroutine(base.Idle());
	}

	protected override IEnumerator Move()
	{
		// 탐지 범위
		float distance = 0f;
        //BaseObject targetObject = ActorManager.Instance.GetSearchEnemy(TargetComponent, out distance);

        BaseObject targetObject =
                StageManager.Instance.GetSearchRegenerator(TargetComponent, out distance);

        string TagName = this.TargetComponent.transform.tag.ToString();
        switch (TagName)
        {
            case "Player":
                {
                    //targetObject = StageManager.Instance.GetSearchRegenerator(TargetComponent, out distance);

                    targetObject =
                        //EnemyRegenerator.IsActiveRegen ?
                        StageManager.Instance.Is_ActiveRenerator() ?
                         ActorManager.Instance.GetSearchEnemy(TargetComponent, out distance)
                            : StageManager.Instance.GetSearchRegenerator(TargetComponent, out distance);
                }
                break;

            case "Monster":
                {
                    targetObject =
                        ActorManager.Instance.GetSearchEnemy(TargetComponent, out distance);
                }
                break;

            default:
                break;
        }


        if (targetObject != null)
		{
			// 공격 범위
			float attackRange = 1f;

			SkillData skillData =
				TargetComponent.GetData(ConstValue.ActorData_SkillData, 0)
				as SkillData;

			if (skillData != null)
			{
				attackRange = skillData.RANGE;
			}

			if (distance < attackRange)
			{
				Stop();
				AddNextAI(eAIStateType.AI_STATE_ATTACK,
					targetObject);
			}
			else
			{
				SetMove(targetObject.SelfTransform.position);
			}

		}
		yield return StartCoroutine(base.Move());
	}

	protected override IEnumerator Attack()
	{
		yield return new WaitForEndOfFrame();

		while(IsAttack)
		{
			if (ObjectState == eBaseObjectState.STATE_DIE)
				break;

            yield return new WaitForEndOfFrame();
		}

		AddNextAI(eAIStateType.AI_STATE_IDLE);

		yield return StartCoroutine(base.Attack());

	}


	protected override IEnumerator Die()
	{
		END = true;

		yield return StartCoroutine(base.Die());
	}

}
