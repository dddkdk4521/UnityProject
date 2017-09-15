using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class SkillData 
{
    string DataKey = string.Empty;

    List<string> SkillList = new List<string>();
    public List<string> SKILL_LIST
    {
        get
        {
            return SkillList;
        }
    }
    
    float Range = 0;
    public float RANGE
    {
        get
        {
            return Range;
        }
    }


    public SkillData(string strKey, JSONNode nodeData)
    {
        this.DataKey = strKey;
        this.Range = nodeData["RANGE"].AsFloat;

        JSONArray arrSkill = nodeData["SKILL"].AsArray;
        if (arrSkill != null)
        {
            for (int i = 0; i < arrSkill.Count; i++)
            {
                SkillList.Add(arrSkill[i]);
            }
        }
    }
}
