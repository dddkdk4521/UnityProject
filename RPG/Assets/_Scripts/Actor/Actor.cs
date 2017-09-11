using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : BaseObject
{
    bool _IsPlayer = false;
    public bool IsPlayer
    {
        get { return _IsPlayer; }
        set { _IsPlayer = value; }
    }

    Animator Anim;
    eAIStateType CurrentState;
    
    // Use this for initialization
	void Start ()
    {
        this.Anim = this.GetComponentInChildren<Animator>();
        if (Anim == null)
        {
            Debug.Log("Animator is null");
            return;
        }

        this.CurrentState = eAIStateType.AI_STATE_IDLE;
	}
	
	// Update is called once per frame
	protected virtual void Update ()
    {
            
    }

    public void ChangeAnimation(eAIStateType eAIState)
    {
        this.CurrentState = eAIState;
        this.Anim.SetInteger("State", (int)CurrentState);
    }


    public override object GetData(string keyData, params object[] datas)
    {
        return base.GetData(keyData, datas);
    }

    public override object ThrowEvent(string keyData, params object[] datas)
    {
        return base.ThrowEvent(keyData, datas);
    }
}
