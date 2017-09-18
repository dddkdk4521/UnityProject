using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCharacter
{
    public BaseObject TargetComponent = null;

    CharacterTemplateData TemplateData = null;
    public CharacterTemplateData GetCharacterTemplate
    {
        get
        {
            return TemplateData;
        }
    }

    CharacterStatusData CharacterStatus = new CharacterStatusData();
    public CharacterStatusData GetCharacterStatus
    {
        get
        {
            return CharacterStatus; 
        }
    }

    double CurrentHP = 0;
    public double CURRENT_HP
    {
        get
        {
            return CurrentHP;
        }
    }

    public SkillData SELECT_SKILL
    {
        get;
        set;
    }
    List<SkillData> ListSkill = new List<SkillData>();

    public void IncreaseCurentHP(double valueData)
    {
        this.CurrentHP += valueData;

        if (this.CurrentHP < 0)
        {
            this.CurrentHP = 0;
        }

        double maxHP = CharacterStatus.GetStatusData(eStatusData.MAX_HP);
        if (this.CurrentHP > maxHP)
        {
            this.CurrentHP = maxHP;
        }

        if (CurrentHP == 0)
        {
            this.TargetComponent.ObjectState = eBaseObjectState.STATE_DIE;
        }
    }

    public void SetTemplate(CharacterTemplateData _templateData)
    {
        this.TemplateData = _templateData;
        this.CharacterStatus.AddStatusData(ConstValue.CharacterStatusDataKey, TemplateData.STATUS);
        this.CurrentHP = CharacterStatus.GetStatusData(eStatusData.MAX_HP);

        // Skill Setting
        for (int i = 0; i < TemplateData.LIST_SKILL.Count; i++)
        {
            SkillData data = SkillManager.Instance.GetSkillData(TemplateData.LIST_SKILL[i]);
            if (data == null)
            {
                Debug.LogError(TemplateData.LIST_SKILL[i] + " 스킬 키를 찾을수 없습니다.");
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
        {
            return false;
        }

        return true;
    }

    public void AddSkill(SkillData data)
    {
        ListSkill.Add(data);
    }

    public SkillData GetSkillDataByIndex(int index)
    {
        if (ListSkill.Count > index)
        {
            return ListSkill[index];
        }
        else
        {
            return null;
        }
    }
}
