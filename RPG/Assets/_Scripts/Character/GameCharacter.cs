using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCharacter
{
	public BaseObject TargetComponenet = null;

	CharacterTemplateData TemplateData = null;
	CharacterStatusData CharacterStatus = new CharacterStatusData();

	public CharacterTemplateData GetCharacterTemplate
	{ get { return TemplateData; } }
	public CharacterStatusData GetCharacterStatus
	{ get { return CharacterStatus; } }

	double CurrentHP = 0;
	public double CURRENT_HP
	{ get { return CurrentHP; } }

	public SkillData SELECT_SKILL
	{
		get;
		set;
	}
	List<SkillData> ListSkill = new List<SkillData>();
	
	public void IncreaseCurrentHP(double valueData)
	{
		CurrentHP += valueData;

		if (CurrentHP < 0)
			CurrentHP = 0;

		double maxHP =
			CharacterStatus.GetStatusData(eStatusData.MAX_HP);
		if (CurrentHP > maxHP)
			CurrentHP = maxHP;

		if (CurrentHP == 0)
		{
			TargetComponenet.ObjectState =
				eBaseObjectState.STATE_DIE;
		}
	}

	public void SetTemplate(CharacterTemplateData _templateData)
	{
		TemplateData = _templateData;
		CharacterStatus.AddStatusData(
			ConstValue.CharacterStatusDataKey,
			TemplateData.STATUS);
		CurrentHP =
			CharacterStatus.GetStatusData(eStatusData.MAX_HP);


		// Skill Setting
		for (int i = 0; i < TemplateData.LIST_SKILL.Count; i++)
		{
			SkillData data =
				SkillManager.Instance.GetSkillData(
								TemplateData.LIST_SKILL[i]);

			if (data == null)
			{
				Debug.LogError(TemplateData.LIST_SKILL[i]
					+ " 스킬 키를 찾을수 없습니다.");
				return;
			}
			else
			{
				AddSkill(data);
			}
		}

	}

	public bool EquipSkillByIndex(int index)
	{
		if (ListSkill.Count > index)
		{
			SELECT_SKILL = ListSkill[index];
		}
		else
			return false;

		return true;
	}

	public SkillData GetSkillDataByIndex(int index)
	{
		if (ListSkill.Count > index)
		{
			return ListSkill[index];
		}
		else
			return null;
	}


	void AddSkill(SkillData data)
	{
		ListSkill.Add(data);
	}


}
