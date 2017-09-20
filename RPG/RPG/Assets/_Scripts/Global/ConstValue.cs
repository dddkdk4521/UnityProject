using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConstValue
{
    // StatusData 
    public const string SetData_Damage = "BOARD_DAMAGE";
    public const string SetData_HP     = "BOARD_HP";

    // StatusData Key
    public const string CharacterStatusDataKey = "CHARACTER_TEMPLATE_STATUS";
        
    // Path Key
    public const string CharacterTemplatePath = "JSON/CHARACTER_TEMPLATE";
    public const string CharacterTemplateKey  = "CHARACTER_TEMPLATE";

    public const string SkillTemplatePath = "JSON/SKILL_TEMPLATE";
    public const string SkillTemplateKey = "SKILL_TEMPLATE";

    public const string SkillDataPath = "JSON/SKILL_DATA";
    public const string SkillDataKey  = "SKILL_DATA";

    public const string UI_PATH_HP     = "Prefabs/UI/HP_Board";
    public const string UI_PATH_DAMAGE = "Prefabs/UI/Damage_Board";

    // ThrowEvent
    public const string EventKey_EnemyInit = "E_INIT";
    public const string EventKey_Hit       = "ACTOR_HIT";
    public const string EventKey_SelectSkill = "SELECT_SKILL";

    public const string ActorData_SetTarget = "SET_TARGET";

    // GetData Key
    public const string ActorData_GetTarget = "GET_TARGET";
    public const string ActorData_Team      = "TEAM_TYPE";
    public const string ActorData_Character = "CHARACTER";
    public const string ActorData_SkillData = "SKILL_DATA";
}

