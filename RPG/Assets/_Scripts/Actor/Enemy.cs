using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Actor
{
    NavMeshAgent Agent;
    Transform Target = null;

    // Use this for initialization
	void Start ()
    {
        this.Agent = SelfComponent<NavMeshAgent>();
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("Player"))
        {
            this.Target = other.transform;
            this.Agent.isStopped = false;
            this.Agent.SetDestination(Target.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Equals("Player"))
        {
            this.Target = null;
            this.Agent.isStopped = true;
        }
    }
}
