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

    [SerializeField]
    string TemplateKey = string.Empty;

    GameCharacter _selfCharacter = null;
    public GameCharacter SelfCharacter
    {
        get
        {
            return _selfCharacter;
        }
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

        GameCharacter character = CharacterManager.Instance.AddCharacter(TemplateKey);
        character.TargetComponent = this;
        _selfCharacter = character;

        ActorManager.Instance.AddActor(this);
    }

    public virtual void Update()
    {
        AI.UpdateAI();
        if (AI.END)
        {
            Destroy(SelfObject);
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

    public override object GetData(string KeyData, params object[] datas)
    {
        switch (KeyData)
        {
            case ConstValue.ActorData_Team:
                {
                    return TeamType;
                }
            case ConstValue.ActorData_Character:
                {
                    return SelfCharacter
                        ;
                }
            case ConstValue.ActorData_GetTarget:
                {
                    return HitTarget;
                }
            default:
                return base.GetData(KeyData, datas);
        }
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
            case ConstValue.EventKey_Hit:
                {
                    AI.ANIMATOR.SetInteger("Hit", 1);
                }
                break; 
            default:
                base.ThrowEvent(KeyData, datas);
                break;
        }
        
    }

    public void RunSkill()
    {
        //GameCharacter gc = TargetComponent.GetData(ConstValue.ActorData_Character) as GameCharacter;
        Debug.Log(this.gameObject.name + "가 "
            + HitTarget.name + "를 "
            + SelfCharacter.GetCharacterStatus.GetStatusData(eStatusData.ATTACK)                
            + " 공격력으로 때림.");

        HitTarget.ThrowEvent(ConstValue.EventKey_Hit);
    }

    public double GetStatusData(eStatusData statusData)
    {
        return SelfCharacter.GetCharacterStatus.GetStatusData(statusData);
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
