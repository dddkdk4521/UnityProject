using UnityEngine;
using System.Collections;

public class Navigator : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent agent;
    private Animator animator;
	private Targeter targeter;
    
	void Awake ()
    {
	    this.agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
	    this.animator = GetComponent<Animator>();
		this.targeter = GetComponent<Targeter>();
	}

    void Update()
    {
        this.animator.SetFloat("Distance", agent.remainingDistance);
    }
	
	public void NavigateTo (Vector3 position)
	{
	    this.agent.SetDestination(position);
		this.targeter.target = null;
		this.animator.SetBool ("Attack", false);
	}
}
