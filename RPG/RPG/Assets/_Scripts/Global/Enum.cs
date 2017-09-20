public enum eBaseObjectState
{
    STATE_NORMAL,
    STATE_DIE,
}

// AI, Animation
public enum eAIStateType
{
    AI_STATE_NONE = 0,
    AI_STATE_IDLE,
    AI_STATE_ATTACK,
    AI_STATE_MOVE,
    AI_STATE_DIE
}

// Enemy 관련
public enum eRegeneratorType
{
    NONE,
    REGENTIME_EVENT,
    TRIGGER_EVENT
}

public enum eEnemyType
{
    A_Monster,
    B_Monster,
    C_Monster,
    MAX
}

public enum eTeamType
{
    TEAM_1,
    TEAM_2
}

public enum eAIType
{
    NormalAI
}

public enum eStatusData
{
    MAX_HP,
    ATTACK,
    DEFFENCE,
    MAX
}

public enum eSkillTemplateType
{
    TARGET_ATTACK,
    RANGE_ATTACK
}

public enum eSkillAttackRangeType
{
    RANGE_BOX,
    RANGE_SPHERE
}

public enum eSceneType
{
    Scene_None,
    Scene_Logo,
    Scene_Lobby,
    Scene_Game
}

public enum eUIType
{
    Pf_UI_Logo,
    Pf_UI_Loading,
    Pf_UI_Lobby,
    Pf_UI_Inventory,
    Pf_UI_Popup,
    Pf_UI_Stage,
    Pf_UI_Gacha,
}

public enum eBoardType
{
    BOARD_NONE,
    BOARD_HP,
    BOARD_DAMAGE
}