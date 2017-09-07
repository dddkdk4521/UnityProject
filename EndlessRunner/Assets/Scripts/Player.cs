using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 10f;

    private Vector3 dir;

	// Use this for initialization
	void Start ()
    {
        this.dir = Vector3.zero;	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(Input.GetMouseButtonDown(0))
        {
            this.dir = (this.dir == Vector3.forward) ? Vector3.left : Vector3.forward;
        }

        float amoutToMove = speed * Time.deltaTime;
        transform.Translate(dir * amoutToMove); 
	}

    private void FixedUpdate()
    {
        
    }
}
