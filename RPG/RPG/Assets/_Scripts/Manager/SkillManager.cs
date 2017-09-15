using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class SkillManager : MonoSingleton<SkillManager>
{
    Dictionary<BaseObject, List<BaseSkill>> DicUseSkill =
                                    new Dictionary<BaseObject, List<BaseSkill>>();

    Dictionary<string, SkillData> DicSkillData =
                                    new Dictionary<string, SkillData>();

    Dictionary<string, SkillTemplate> DicSkillTemplate =
                                    new Dictionary<string, SkillTemplate>();

    private void Awake()
    {
        LoadSkillData();
        LoadSkillTemplate();
    }

    void LoadSkillData()
    {
        TextAsset skillAssetData = Resources.Load(ConstValue.SkillDataPath) as TextAsset;
        if (skillAssetData == null)
        {
            Debug.Log(ConstValue.SkillDataPath + " 이 파일이 존재하지 않음.");
            return;
        }

        JSONNode rootNode = JSON.Parse(skillAssetData.text);
        if (rootNode == null)
        {
            return;
        }

        JSONObject skillDataNode = rootNode[ConstValue.SkillDataKey] as JSONObject;
        foreach (KeyValuePair<string, JSONNode> pair in skillDataNode)
        {
            SkillData skillData = new SkillData(pair.Key, pair.Value);
            DicSkillData.Add(pair.Key, skillData);
        }
    }

    void LoadSkillTemplate()
    {
        TextAsset skillAssetTemplate = Resources.Load(ConstValue.SkillTemplatePath) as TextAsset;
        if (skillAssetTemplate == null)
        {
            Debug.Log(ConstValue.SkillTemplatePath + " 이 파일이 존재하지 않음.");
            return;
        }

        JSONNode rootNode = JSON.Parse(skillAssetTemplate.text);
        if (rootNode == null)
        {
            return;
        }

        JSONObject skillTemplateNode = rootNode[ConstValue.SkillTemplateKey] as JSONObject;
        foreach (KeyValuePair<string, JSONNode> pair in skillTemplateNode)
        {
            SkillTemplate skillTemplate = new SkillTemplate(pair.Key, pair.Value);
            DicSkillTemplate.Add(pair.Key, skillTemplate);
        }
    }

    public SkillData GetSkillData(string strkey)
    {
        SkillData skillData = null;
        DicSkillData.TryGetValue(strkey, out skillData);
        return skillData;
    }

    public SkillTemplate GetSkillTemplate(string strkey)
    {
        SkillTemplate skillTEmplate = null;
        DicSkillTemplate .TryGetValue(strkey, out skillTEmplate);
        return skillTEmplate;
    }
    
    public void RunSkill(BaseObject keyObject, string strSkillTemplateKey)
    {
        SkillTemplate template = GetSkillTemplate(strSkillTemplateKey);
        if (template == null)
        {
            Debug.Log(strSkillTemplateKey +
                " Skill Template 키를 찾을수 없습니다.");
            return;
        }

        BaseSkill runSkill = CreateSkill(keyObject, template);
        RunSkill(keyObject, runSkill);
    }

    public void RunSkill(BaseObject keyObject, BaseSkill runSkill)
    {
        List<BaseSkill> listSkill = null;
        if (DicUseSkill.ContainsKey(keyObject) == false)
        {
            listSkill = new List<BaseSkill>();
            DicUseSkill.Add(keyObject, listSkill);
        }
        else
        {
            listSkill = DicUseSkill[keyObject];
        }

        listSkill.Add(runSkill);
    }

    BaseSkill CreateSkill(BaseObject owner, SkillTemplate skillTemplate)
    {
        BaseSkill makeSkill = null;

        return makeSkill;
    }
}
