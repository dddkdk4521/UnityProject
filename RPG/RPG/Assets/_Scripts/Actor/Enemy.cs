using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Actor
{
    NavMeshAgent Agent;
    Transform Target = null;

    EnemyRegenerator Generator;

	void Start () {
        Agent = SelfComponent<NavMeshAgent>();
	}

    public override void ThrowEvent(string KeyData, params object[] datas)
    {
        switch (KeyData)
        {
            case ConstValue.EventKey_EnemyInit:
                {
                    Generator = datas[0] as EnemyRegenerator;
                }
                break;
        default:
                {
                    base.ThrowEvent(KeyData, datas);
                }
        break;
        }
    }

    private void OnDisable()
    {
        if(Generator != null)
        {
            Generator.RemoveActor(this);
        }
    }

    private void OnDestroy()
    {
        if(Generator != null)
        {
            Generator.RemoveActor(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name.Contains("Player"))
        {
            Target = other.transform;

            //InvokeRepeating("FollowTarget", 0.5f, 0.5f);
            //Coroutine c = StartCoroutine(FollowTarget());
            //StopCoroutine(c);

            StartCoroutine("FollowTarget");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.name.Contains("Player"))
        {
            Target = null;
            Agent.isStopped = true;
            StopCoroutine("FollowTarget");
        }
        
    }

    //float StackTime = 0;

    //private void Update()
    //{
    //    StackTime += Time.deltaTime;

    //    if(StackTime >= 1f)
    //    {
    //        StackTime = 0f;
    //        Agent.isStopped = false;
    //        Agent.SetDestination(Target.position);
    //    }
    //}

    //void FollowTarget()
    //{
    //    if (Target != null)
    //    {
    //        Agent.isStopped = false;
    //        Agent.SetDestination(Target.position);
    //    }
    //    else
    //    {
    //        CancelInvoke("FollowTarget");
    //    }
    //}

    IEnumerator FollowTarget()
    {
        while(Target != null)
        {
            Agent.isStopped = false;
            Agent.SetDestination(Target.position);
            yield return new WaitForSeconds(0.5f);
        }

        yield return null;
    }
}
