using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Actor
{
	NavMeshAgent Agent;
	Transform Target = null;

	EnemyRegenerator Generator;

	void Start ()
	{
		Agent = SelfComponent<NavMeshAgent>();			
	}

	public override void ThrowEvent(string keyData, params object[] datas)
	{
		switch (keyData)
		{

			case ConstValue.EventKey_EnemyInit:
				{
					Generator = datas[0] as EnemyRegenerator;
				}
				break;

			default:
				{
					// Parent Method Process
					base.ThrowEvent(keyData, datas);
				}
				break;
		}

		
	}

	//private void OnDisable()
	//{
	//	if(Generator != null)
	//	{
	//		Generator.RemoveActor(this);
	//	}

	//}

	public override void OnDestroy()
	{
		if (Generator != null)
		{
			Generator.RemoveActor(this);
		}

		base.OnDestroy();
	}




	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.name.Contains("Player"))
		{
			Target = other.transform;
			// Invoke("FollowTarget", 0.5f);
			//	InvokeRepeating("FollowTarget", 0.5f, 0.5f);
			
			// FollowTarget();

			// Coroutine c = StartCoroutine(FollowTarget());
			// StopCoroutine(c);

			StartCoroutine("FollowTarget");	
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.name.Contains("Player"))
		{
			Target = null;
			Agent.isStopped = true;
			StopCoroutine("FollowTarget");
		}
	}


	// #1 Update
	//float StackTime = 0;
	//protected override void Update()
	//{
	//	if (Target != null)
	//	{
	//		StackTime += Time.deltaTime;
	//		if (StackTime >= 0.5f)
	//		{
	//			StackTime = 0f;
	//			Agent.isStopped = false;
	//			Agent.SetDestination(Target.position);
	//		}
	//	}
	//}

	// #2 Invoke
	//void FollowTarget()
	//{
	//	if (Target != null)
	//	{
	//		Agent.isStopped = false;
	//		Agent.SetDestination(Target.position);
	//	}
	//	else
	//		CancelInvoke("FollowTarget");
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
