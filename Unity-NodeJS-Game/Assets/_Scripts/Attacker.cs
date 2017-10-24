using UnityEngine;
using System.Collections;

public class Attacker : MonoBehaviour
{
	public float attackDistance = 1;
	public float attackRate = 2;

    float lastAttackTime = 0;
	Targeter targeter;

	void Start ()
    {
		this.targeter = GetComponent<Targeter> ();	
	}
	
	void Update ()
    {
		if (!isReadyToAttack ())
			return;
		
		if (isTargetDead ())
        {
			this.targeter.ResetTarget();
			return;
		}

		if (this.targeter.IsInRange (attackDistance))
        {
			Debug.Log("attacking " + targeter.target.name);

            string targetId = targeter.target.GetComponent<NetworkEntity> ().id;
			Network.Attack (targetId);

            this.lastAttackTime = Time.time;
		}
	}

	bool isTargetDead ()
	{
		return this.targeter.target.GetComponent<Hittable> ().IsDead;
	}

	private bool isReadyToAttack()
	{
		return Time.time - lastAttackTime > attackRate && targeter.target;
	}
}
