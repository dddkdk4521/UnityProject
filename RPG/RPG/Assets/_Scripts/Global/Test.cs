using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    Animation Anim;

    private void Start()
    {
        Anim = this.GetComponent<Animation>();
        Anim.Play("Wait");
    }

    void Update ()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Anim.Play("Attack");
        }

        Vector3 axis;

        axis.x = JoyStick.Instance.Axis.x;
        axis.y = 0f;
        axis.z = JoyStick.Instance.Axis.y;

        transform.position += axis * Time.deltaTime * 10f;
	}
}
