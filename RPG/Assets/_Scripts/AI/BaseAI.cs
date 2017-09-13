using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NextAI
{
    public eAIStateType StateType;
    public BaseObject TargetObject;
    public Vector3 Position;
}

public class BaseAI : BaseObject
{
    protected List<NextAI> ListNextAI = new List<NextAI>();

    protected eAIStateType _CurrentAIState = eAIStateType.AI_STATE_IDLE;
    public eAIStateType CurrentAIState
    {
        get { return _CurrentAIState;  }
    }

    protected Vector3 MovePosition = Vector3.zero;

    Vector3 PreMovePosition = Vector3.zero;

    Animator Anim = null;
    NavMeshAgent NavAgent = null;
    public Animator ANIMATOR
    {
        get
        {
            if (Anim == null)
            {
                Anim = SelfObject.GetComponentInChildren<Animator>();
            }

            return Anim;
        }
    }
    public NavMeshAgent NAV_MESH_AGENT
    {
        get
        {
            if (NavAgent == null)
            {
                NavAgent = SelfObject.GetComponent<NavMeshAgent>();
            }

            return NavAgent;
        }
    }

    bool bUpdateAI = false;
    bool bAttack = false;
    public bool IsAttack
    {
        get { return bAttack;  }
        set { bAttack = value; }
    }

    bool bEnd = false;
    public bool END
    {
        get { return bEnd; }
        set { bEnd = value; }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void ChangeAnimation()
    {
        if (ANIMATOR == null)
        {
            Debug.LogError(SelfObject.name +
                " 에게 Animator가 없음");
            return;
        }

        ANIMATOR.SetInteger("State", (int)CurrentAIState);
    }

    protected bool MoveCheck()
    {
        if (NAV_MESH_AGENT.pathStatus == NavMeshPathStatus.PathComplete)
        {
            if (NAV_MESH_AGENT.hasPath == false ||
                NAV_MESH_AGENT.pathPending == false)
            {
                return true;
            }
        }

        return false;
    }

    protected void SetMove(Vector3 position)
    {
        if (PreMovePosition == position)
        {
            return;
        }

        this.PreMovePosition = position;
        NAV_MESH_AGENT.isStopped = false;
        NAV_MESH_AGENT.SetDestination(position);
    }

    protected void Stop()
    {
        this.MovePosition = Vector3.zero;
        NAV_MESH_AGENT.isStopped = true;
    }

    protected virtual void ProcessIdle()
    {
        this._CurrentAIState = eAIStateType.AI_STATE_IDLE;
        ChangeAnimation();
    }

    protected virtual void ProcessMove()
    {
        this._CurrentAIState = eAIStateType.AI_STATE_MOVE;
        ChangeAnimation();
    }

    protected virtual void ProcessAttack()
    {
        this._CurrentAIState = eAIStateType.AI_STATE_ATTACK;
        ChangeAnimation();
    }

    protected virtual void ProcessDie()
    {
        this._CurrentAIState = eAIStateType.AI_STATE_DIE;
        ChangeAnimation();
    }

    protected virtual IEnumerator Idle()
    {
        this.bUpdateAI = false;
        yield break;
    }

    protected virtual IEnumerator Move()
    {
        this.bUpdateAI = false;
        yield break;
    }

    protected virtual IEnumerator Attack()
    {
        this.bUpdateAI = false;
        yield break;
    }

    protected virtual IEnumerator Die()
    {
        this.bUpdateAI = false;
        yield break;
    }

    public virtual void AddNextAI(eAIStateType nextStateType, BaseObject targetObject, Vector3 position = new Vector3())
    {
        NextAI nextAI = new NextAI
        {
            StateType = nextStateType,
            TargetObject = targetObject,
            Position = position,
        };

        ListNextAI.Add(nextAI);
    }

    void SetNextAI(NextAI nextAI)
    {
        if (nextAI.TargetObject != null)
        {

        }

        if (nextAI.Position != Vector3.zero)
        {
            this.MovePosition = nextAI.Position;
        }

        switch (nextAI.StateType)
        {
            case eAIStateType.AI_STATE_NONE:
                break;
            case eAIStateType.AI_STATE_IDLE:
                ProcessIdle();
                break;
            case eAIStateType.AI_STATE_ATTACK:
                {
                    if (nextAI.TargetObject != null)
                    {
                        SelfTransform.forward = 
                            (nextAI.TargetObject.SelfTransform.position - SelfTransform.position).normalized;
                    }

                    ProcessAttack();
                }
                break;
            case eAIStateType.AI_STATE_MOVE:
                ProcessMove();
                break;
            case eAIStateType.AI_STATE_DIE:
                ProcessDie();
                break;
            default:
                break;
        }
    }

    public void UpdateAI()
    {
        if (this.bUpdateAI == true)
        {
            return;
        }

        if (this.ListNextAI.Count > 0)
        {
            SetNextAI(ListNextAI[0]);
        }

        if (ObjectState == eBaseObjectState.STATE_DIE)
        {
            this.ListNextAI.Clear();
            ProcessDie();
        }

        this.bUpdateAI = true;

        switch (CurrentAIState)
        {
            case eAIStateType.AI_STATE_NONE:
                break;
            case eAIStateType.AI_STATE_IDLE:
                StartCoroutine("Idle");
                break;
            case eAIStateType.AI_STATE_ATTACK:
                StartCoroutine("Attack");
                break;
            case eAIStateType.AI_STATE_MOVE:
                StartCoroutine("Move");
                break;
            case eAIStateType.AI_STATE_DIE:
                StartCoroutine("Die");
                break;
            default:
                break;
        }
    }

    public void ClearAI()
    {
        this.ListNextAI.Clear();
    }

    public void ClearAI(eAIStateType stateType)
    {
        // #1
        List<NextAI> removeAI = new List<NextAI>();
        for (int i = 0; i < removeAI.Count; i++)
        {
            if (ListNextAI[i].StateType == stateType)
            {
                removeAI.Add(ListNextAI[i]);
            }
        }

        for (int i = 0; i < removeAI.Count; i++)
        {
            ListNextAI.Remove(removeAI[i]);
        }
        removeAI.Clear();

        // #2 Predicate

        // #3 Lamda

    }
}
