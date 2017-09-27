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

	GameCharacter _SelfCharacter = null;
	public GameCharacter SelfCharacter
	{ get { return _SelfCharacter; } }



	// AI
	[SerializeField]
	eAIType _AIType;
	public eAIType AIType
	{
		get { return _AIType; }
	}

	BaseAI _AI = null;
	public BaseAI AI
	{ get { return _AI; }	}


	[SerializeField]
	bool bEnableBoard = true;

	BaseObject HitTarget;	// 공격대상

	private void Awake()
	{
		switch (AIType)
		{
			case eAIType.NormalAI:
				{
					GameObject go =
						new GameObject(AIType.ToString(), typeof(NormalAI));

					go.transform.SetParent(SelfTransform);
					_AI = go.GetComponent<NormalAI>();
				}
				break;
		}

		AI.TargetComponent = this;

		GameCharacter character =
			CharacterManager.Instance.AddCharacter(TemplateKey);

        character.TargetComponenet = this;
		_SelfCharacter = character;


		if(bEnableBoard)
		{
			BaseBoard board = BoardManager.Instance.AddBoard(
				this, eBoardType.BOARD_HP);

			board.SetData(ConstValue.SetData_HP,
				GetStatusData(eStatusData.MAX_HP),
				SelfCharacter.CURRENT_HP);
		}

		ActorManager.Instance.AddActor(this);

	}

	protected virtual void Update()
	{
		AI.UpdateAI();
		if (AI.END)
			Destroy(SelfObject);
	}

	public double GetStatusData(eStatusData statusData)
	{
		return SelfCharacter.
			GetCharacterStatus.GetStatusData(statusData);
	}

	public override object GetData(string keyData, params object[] datas)
	{
		switch (keyData)
		{
			case ConstValue.ActorData_Team:
				{
					return TeamType;
				}
			case ConstValue.ActorData_Character:
				{
					return SelfCharacter;
				}
			case ConstValue.ActorData_GetTarget:
				{
					return HitTarget;
				}
			case ConstValue.ActorData_SkillData:
				{
					int index = (int)datas[0];
					return SelfCharacter.GetSkillDataByIndex(index);
				}
			default:
				return base.GetData(keyData, datas);

		}

	}

	public override void ThrowEvent(string keyData, params object[] datas)
	{
		switch (keyData)
		{
			case ConstValue.ActorData_SetTarget:
				{
					HitTarget = datas[0] as BaseObject;
				}
				break;
			case ConstValue.EventKey_Hit:
				{
					if (ObjectState == eBaseObjectState.STATE_DIE)
						return;

					GameCharacter casterCharacter = datas[0] as GameCharacter;
					SkillTemplate skilltemplate = datas[1] as SkillTemplate;

					casterCharacter.GetCharacterStatus.AddStatusData(
						"SKILL", skilltemplate.STATUS_DATA);

					double attackDamage =
						casterCharacter.
						GetCharacterStatus.
						GetStatusData(eStatusData.ATTACK);

					casterCharacter.GetCharacterStatus.RemoveStatusData(
						"SKILL");

					SelfCharacter.IncreaseCurrentHP(-attackDamage);

					//Debug.Log(SelfObject.name + "가 데미지 " + 
					//	attackDamage +" 피해를 입었습니다. ");

					// DamageBoard
					BaseBoard board = BoardManager.Instance.AddBoard(this, eBoardType.BOARD_DAMAGE);
					if (board != null)
						board.SetData(ConstValue.SetData_Damage, attackDamage);

					// HpBoard
					board = BoardManager.Instance.GetBoardData(
						this, eBoardType.BOARD_HP);
					if(board != null)
					{
						board.SetData(ConstValue.SetData_HP,
								GetStatusData(eStatusData.MAX_HP),
								SelfCharacter.CURRENT_HP);
					}

                    if (IsPlayer == true)
                    {
                        CameraManager.Instance.Shake();
                    }


					// 죽었나 안죽었나
					if(ObjectState == eBaseObjectState.STATE_DIE)
					{
						Debug.Log(gameObject.name + "죽음!");
						GameManager.Instance.KillCheck(this);
					}
		
					AI.ANIMATOR.SetInteger("Hit", 1);
				}
				break;

			case ConstValue.EventKey_SelectSkill:
				{
					int index = (int)datas[0];					
					if(SelfCharacter.EquipSkillByIndex(index) == false)
					{
						Debug.LogError(this.gameObject + " 의" +
							"Skill Index : " + index + "스킬 구동 실패");
					}
				}
				break;

			default:
				base.ThrowEvent(keyData, datas);
			break;
		}
	}

	public void RunSkill()
	{
		SkillData selectSkill = SelfCharacter.SELECT_SKILL;

		if (selectSkill == null)
			return;

		for(int i = 0; i < selectSkill.SKILL_LIST.Count; i++)
		{
			SkillManager.Instance.RunSkill(this,
								selectSkill.SKILL_LIST[i]);
		}

		SelfCharacter.SELECT_SKILL = null;


		//GameCharacter gc = TargetComponent.GetData(
		//		ConstValue.ActorData_Character)
		//		as GameCharacter;

		//Debug.Log(this.gameObject.name + "가 "
		//	+ HitTarget.name + "를 "
		//	+ SelfCharacter.GetCharacterStatus.GetStatusData
		//	(eStatusData.ATTACK) +
		//	" 공격력으로 때림.");

		//HitTarget.ThrowEvent(ConstValue.EventKey_Hit);
	}

	private void OnEnable()
	{
		if (BoardManager.Instance != null)
			BoardManager.Instance.ShowBoard(this, true);
	}

	private void OnDisable()
	{
		if (BoardManager.Instance != null)
			if (GameManager.Instance.GAME_OVER == false)
				BoardManager.Instance.ShowBoard(this, false);
	}

	public virtual void OnDestroy()
	{
		if (BoardManager.Instance != null)
			BoardManager.Instance.ClearBoard(this);

		// ActorManager RemoveActor
		if(ActorManager.Instance != null)
			ActorManager.Instance.RemoveActor(this);
	}

	//Animator Anim;
	//protected eAIStateType CurrentState;

	//void Awake ()
	//{
	//	Anim = this.GetComponentInChildren<Animator>();
	//	if(Anim == null)
	//	{
	//		Debug.Log("Animator is null");
	//		return;
	//	}

	//	ChangeState(eAIStateType.AI_STATE_IDLE);
	//}

	//void SetAnimtion(eAIStateType eAIState)
	//{
	//	Anim.SetInteger("State", (int)eAIState);
	//}

	//public void ChangeState(eAIStateType state)
	//{
	//	if (CurrentState == state)
	//		return;

	//	CurrentState = state;

	//	switch (CurrentState)
	//	{
	//		case eAIStateType.AI_STATE_IDLE:
	//			break;
	//		case eAIStateType.AI_STATE_ATTACK:
	//			{

	//			}
	//			break;
	//		case eAIStateType.AI_STATE_MOVE:
	//			break;
	//		case eAIStateType.AI_STATE_DIE:
	//			break;
	//	}


	//	SetAnimtion(CurrentState);
	//}

	//protected virtual void Update()
	//{

	//}

	//public override object GetData(string keyData, params object[] datas)
	//{
	//	return base.GetData(keyData, datas);
	//}

	//public override void ThrowEvent(string keyData, params object[] datas)
	//{
	//	switch(keyData)
	//	{
	//		case ConstValue.EventKey_Hit:
	//			{
	//				// Die
	//				Destroy(SelfObject);
	//			}
	//			break;

	//		default:
	//			{
	//				base.ThrowEvent(keyData, datas);
	//			}
	//			break;
	//	}

	//}
}

