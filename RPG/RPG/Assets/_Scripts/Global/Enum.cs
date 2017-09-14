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



