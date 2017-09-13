using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Actor
{
    NavMeshAgent Agent;
    Transform Target = null;

    EnemyRegnerator Generator;

    // Use this for initialization
	void Start ()
    {
        this.Agent = SelfComponent<NavMeshAgent>();
	}

    public override void ThrowEvent(string keyData, params object[] datas)
    {
        switch (keyData)
        {
            case ConstValue.EventKey_EnemyInit:
                {
                    Generator = datas[0] as EnemyRegnerator;
                }
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Player"))
        {
            this.Target = other.transform;
            // Invoke : 예약작업 (이펙트관련), InvokeRepeating()

            /*
            Coroutine c = StartCoroutine(FollowTarget());
            StopCoroutine(c);
            */

            this.Agent.isStopped = false;
            this.Agent.SetDestination(Target.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Contains("Player"))
        {
            this.Target = null;
            this.Agent.isStopped = true;
        }
    }

    private void OnDisable()
    {
        if (this.Generator != null)
        {
            this.Generator.RemoveActor(this);
        }
    }

    private void OnDestroy()
    {
        if (this.Generator != null)
        {
            this.Generator.RemoveActor(this);
        }
    }

    IEnumerator FollowTarget()
    {
        while (Target != null)
        {
            Agent.isStopped = false;
            Agent.SetDestination(Target.position);

            yield return new WaitForSeconds(0.5f);
        }

        yield return null;
    }
}
