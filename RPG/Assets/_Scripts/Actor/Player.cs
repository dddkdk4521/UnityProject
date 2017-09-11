using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : Actor
{
    JoyStick Stick;
    NavMeshAgent Agent;

    private void Start()
    {
        base.IsPlayer = true;

        this.Stick = JoyStick.Instance;
        this.Agent = this.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeAnimation(eAIStateType.AI_STATE_ATTACK);
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

            ChangeAnimation(eAIStateType.AI_STATE_MOVE);
        }
        else
        {
            ChangeAnimation(eAIStateType.AI_STATE_IDLE);
            base.Update();
        }
    }
}
