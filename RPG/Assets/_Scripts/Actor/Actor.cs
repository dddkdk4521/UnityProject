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

    protected eAIStateType CurrentState;
    Animator Anim;
    
	void Awake ()
    {
        this.Anim = this.GetComponentInChildren<Animator>();
        if (Anim == null)
        {
            Debug.Log("Animator is null");
            return;
        }

        ChangeState(eAIStateType.AI_STATE_IDLE);
	}
	
	// Update is called once per frame
	protected virtual void Update ()
    {
                    
    }

    public void SetAnimation(eAIStateType eAIState)
    {
        this.Anim.SetInteger("State", (int)eAIState);
    }

    public void ChangeState(eAIStateType state)
    {
        if (this.CurrentState == state)
        {
            return;
        }

        switch (state)
        {
            case eAIStateType.AI_STATE_NONE:
                break;
            case eAIStateType.AI_STATE_IDLE:
                break;
            case eAIStateType.AI_STATE_ATTACK:
                break;
            case eAIStateType.AI_STATE_MOVE:
                break;
            case eAIStateType.AI_STATE_DIE:
                break;
            default:
                break;
        }

        SetAnimation(this.CurrentState = state);
    }

    public override object GetData(string keyData, params object[] datas)
    {
        return base.GetData(keyData, datas);
    }

    public override void ThrowEvent(string keyData, params object[] datas)
    {
        base.ThrowEvent(keyData, datas);
    }
}
