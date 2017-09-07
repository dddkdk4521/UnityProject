using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCode : MonoBehaviour
{
    private void Update()
    {
        Vector3 axis;
        axis.x = JoyStick.Instance.Axis.x;
        axis.y = JoyStick.Instance.Axis.y;
        axis.z = 0;

        transform.position += axis* 10f * Time.deltaTime;
    }
}
