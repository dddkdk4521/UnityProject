using UnityEngine;
using System.Collections;

public class Follower : MonoBehaviour
{
	public Targeter targeter;

    public float scanFrequency = 0.5f;
    public float stopFollowDistance = 2f;

    private float lastScanFrequency = 0;
    private UnityEngine.AI.NavMeshAgent agent;

    // Use this for initialization
    void Start()
    {
        this.agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		this.targeter = GetComponent<Targeter> ();
    }

    // Update is called once per frame
    void Update()
    {
		if (isReadyToScan() && !targeter.IsInRange(stopFollowDistance))
        {
			this.agent.SetDestination(targeter.target.position);
        }
    }

    private bool isReadyToScan()
    {
		return Time.time - lastScanFrequency > scanFrequency && targeter.target;
    }
}
