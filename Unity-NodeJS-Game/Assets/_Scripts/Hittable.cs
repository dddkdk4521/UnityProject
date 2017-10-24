using UnityEngine;
using System.Collections;

public class Hittable : MonoBehaviour
{
	public float health = 100;
    public bool IsDead
    {
		get
        {
            return health <= 0;
        }
	}

	public float respawnTime = 5;

	private Animator animator;

	void Start ()
    {
		this.animator = GetComponent<Animator> ();
	}

	public void GetHit(float damage)
    {
		this.health -= damage;
		if (IsDead)
        {
			this.animator.SetTrigger ("Dead");
			Invoke ("Spawn", respawnTime);
		}
	}

	void Spawn()
    {
		Debug.Log ("Spawn the player");

        this.transform.position = Vector3.zero;
		this.health = 100;
		this.animator.SetTrigger ("Spawn");
	}
}
