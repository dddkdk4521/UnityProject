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

        //Status


    }



}
