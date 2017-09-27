using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : Actor
{
	JoyStick Stick;

	private void Start()
	{
		IsPlayer = true;
		Stick = JoyStick.Instance;
	}

	protected override void Update()
	{
		//if (GameManager.Instance.GAME_OVER == false)
		//{
		//	if (this.ObjectState == eBaseObjectState.STATE_DIE)
		//	{
		//		GameManager.Instance.SetGameOver();
		//	}
		//}
		//else
		//	return;

		if (Stick.IsPressed)
		{
			Vector3 movePosition = transform.position;
			movePosition +=
				new Vector3(Stick.Axis.x, 0, Stick.Axis.y);
			AI.JoyMove(movePosition);
		}
		else
		{
			base.Update();      // Actor::Update();
		}
	}

	//void ProcessAttack()
	//{
	//	if (CurrentState == eAIStateType.AI_STATE_ATTACK)
	//		return;

	//	ChangeState(eAIStateType.AI_STATE_ATTACK);

	//	if(DetectArea != null)
	//	{
	//		Actor actor = DetectArea.GetActor();

	//		if (actor == null)
	//			return;

	//		Vector3 dir = actor.SelfTransform.position
	//								- SelfTransform.position;

	//		dir.Normalize();
	//		// dir = dir.normalized;

	//		SelfTransform.rotation =
	//			Quaternion.LookRotation(dir);
	//		actor.SelfTransform.rotation =
	//			Quaternion.LookRotation(-dir);

	//		actor.ThrowEvent(ConstValue.EventKey_Hit);

	//	}


	//}
}
