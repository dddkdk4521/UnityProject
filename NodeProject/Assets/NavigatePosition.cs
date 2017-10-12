using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NavigatePosition : MonoBehaviour
{
    NavMeshAgent agent;           
    
    // Use this for initialization
	void Start ()
    {
        this.agent = GetComponent<NavMeshAgent>();
	}
	
	public void NavigateTo(Vector3 position)
    {
        this.agent.SetDestination(position);
	}
}
