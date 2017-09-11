using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCode : MonoBehaviour
{
    Animation Anim;

    private void Start()
    {
        this.Anim = this.GetComponent<Animation>();
        this.Anim.Play("Wait");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.Anim.Play("Attack");
        }
        
        Vector3 axis;
        axis.x = JoyStick.Instance.Axis.x;
        axis.y = JoyStick.Instance.Axis.y;
        axis.z = 0;

        transform.position += axis* 10f * Time.deltaTime;
    }
}
