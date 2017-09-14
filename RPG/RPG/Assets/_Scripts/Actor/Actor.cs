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

    [SerializeField]
    eTeamType _TeamType;

    public eTeamType TeamType
    {
        get { return _TeamType; }
    }

    // AI
    [SerializeField]
    eAIType _AIType;
    public eAIType AIType
    {
        get { return _AIType; }
    }

    BaseAI _AI = null;
    public BaseAI AI
    {
        get { return _AI; }
    }

    // 공격대상
    BaseObject HitTarget;

    private void Awake()
    {
        // ActorManager AddActor
        ActorManager.Instance.AddActor(this);

        switch (AIType)
        {
            case eAIType.NormalAI:
                {
                    GameObject go = new GameObject(AIType.ToString(), typeof(NormalAI));

                    go.transform.SetParent(SelfTransform);
                    _AI = go.GetComponent<NormalAI>();
                }
                break;
        }

        AI.TargetComponent = this;

    }

    public virtual void Update()
    {
        AI.UpdateAI();
        if (AI.END)
        {
            Destroy(SelfObject);
        }
    }

    public override object GetData(string KeyData, params object[] datas)
    {
        switch (KeyData)
        {
            case ConstValue.ActorData_Team:
                {
                    return TeamType;
                }
            default:
                return base.GetData(KeyData, datas);
        }

        return base.GetData(KeyData, datas);
    }

    public override void ThrowEvent(string KeyData, params object[] datas)
    {
        switch (KeyData)
        {
            case ConstValue.ActorData_SetTarget:
                {
                    HitTarget = datas[0] as BaseObject;
                }
                break;
            default:
                base.ThrowEvent(KeyData, datas);
                break;
        }
        
    }

    private void OnDestroy()
    {
        // ActorManager RemoveActor
        if (ActorManager.Instance != null)
        {
            ActorManager.Instance.RemoveActor(this);
        }     
    }

    //   Animator Anim;

    //   protected eAIStateType CurrentState;

    //void Awake ()
    //   {
    //       Anim = this.GetComponentInChildren<Animator>();

    //       if(Anim == null)
    //       {
    //           Debug.Log("Animator is null");
    //           return;
    //       }

    //       ChangeState(eAIStateType.AI_STATE_IDLE);
    //}

    //   protected virtual void Update()
    //   {

    //   }

    //   void SetAnimation(eAIStateType eAIState)
    //   {    
    //       Anim.SetInteger("State", (int)eAIState);
    //   }

    //   public void ChangeState(eAIStateType state)
    //   {
    //       if(CurrentState == state)
    //       {
    //           return;
    //       }
    //       CurrentState = state;

    //       switch (CurrentState)
    //       {
    //           case eAIStateType.AI_STATE_IDLE:
    //               break;
    //           case eAIStateType.AI_STATE_ATTACK:
    //               {

    //               }
    //               break;
    //           case eAIStateType.AI_STATE_MOVE:
    //               break;
    //           case eAIStateType.AI_STATE_DIE:
    //               break;           
    //       }


    //       SetAnimation(CurrentState);
    //   }

    //   public override object GetData(string KeyData, params object[] datas)
    //   {
    //       return base.GetData(KeyData, datas);
    //   }

    //   public override void ThrowEvent(string KeyData, params object[] datas)
    //   {
    //       switch (KeyData)
    //       {
    //           case ConstValue.EventKey_Hit:
    //               {
    //                   // DIE
    //                   Destroy(SelfObject);
    //               }
    //               break;
    //           default:
    //               {
    //                   base.ThrowEvent(KeyData, datas);
    //               }
    //               break;
    //       }

    //   }

}
