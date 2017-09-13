using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : Actor
{
    public float AttackRange = 3.0f;

    JoyStick Stick;
    NavMeshAgent Agent; // UnityEngine.AI 포함

    DetectionArea DetectArea;

    private void Start()
    {
        base.IsPlayer = true;

        this.Stick = JoyStick.Instance;
        this.Agent = this.GetComponent<NavMeshAgent>();
        this.DetectArea = 
            SelfObject.GetComponentInChildren<DetectionArea>();
        if (DetectArea == null)
        {
            Debug.Log("Detection Area is null");
            return;
        }

        this.DetectArea.Init(this.AttackRange);
    }

    /*
    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeState(eAIStateType.AI_STATE_ATTACK);
        }

        if (Stick.IsPressed)
        {
            Vector3 movePosition = transform.position;
            //movePosition += new Vector3(Stick.Axis.x, 0, Stick.Axis.y);
            movePosition += new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

            // 위치 값 강제이동x
            // transform.position = movePosition;
            Agent.isStopped = false;
            Agent.SetDestination(movePosition);

            ChangeState(eAIStateType.AI_STATE_MOVE);
        }
        else
        {
            if ( CurrentState != eAIStateType.AI_STATE_ATTACK)
            {
                ChangeState(eAIStateType.AI_STATE_IDLE);
            }
        }

        ProcessAttack();
    }

    void ProcessAttack()
    {
        if (CurrentState == eAIStateType.AI_STATE_ATTACK)
        {
            return;
        }

        ChangeState(eAIStateType.AI_STATE_ATTACK);

        if (DetectArea != null)
        {
            Actor actor = this.DetectArea.GetActor();
            if (actor == null)
            {
                return;
            }

            Vector3 dir = actor.SelfTransform.position - SelfTransform.position;
            dir.Normalize();

            SelfTransform.rotation = Quaternion.LookRotation(dir);
            actor.SelfTransform.rotation = Quaternion.LookRotation(-dir);
            actor.ThrowEvent(ConstValue.EventKey_Hit);
        }
    }
    */
}
