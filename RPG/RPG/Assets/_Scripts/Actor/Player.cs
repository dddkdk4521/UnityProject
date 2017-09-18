using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : Actor
{ 
    JoyStick Stick;

    private void Start()
    {
        IsPlayer = true;
        Stick = JoyStick.Instance;
    }

    // Player JoyStick
    public override void Update()
    {
        if (Stick.IsPressed)
        {
            Vector3 movePosition = transform.position;

            movePosition += new Vector3(Stick.Axis.x, 0, Stick.Axis.y);

            AI.JoyMove(movePosition);
        }
        else
        {
            base.Update();
        }
    }
}

