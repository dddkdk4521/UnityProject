using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class SkillManager : MonoSingleton<SkillManager> 
{
	Dictionary<BaseObject, List<BaseSkill>> DicUseSkill = new Dictionary<BaseObject, List<BaseSkill>>();

	Dictionary<string, SkillData> DicSkillData = new Dictionary<string, SkillData>();

	Dictionary<string, SkillTemplate> DicSkillTemplate = new Dictionary<string, SkillTemplate>();

    #region Unity
    void Awake()
	{
		LoadSkillData();
		LoadSkillTemplate();
	}

	public void Update()
	{
		if (GameManager.Instance.GAME_OVER)
			return;

		foreach(KeyValuePair<BaseObject, List<BaseSkill> > pair
			in DicUseSkill)
		{
			List<BaseSkill> list = pair.Value;

			for(int i = 0; i< list.Count; i++)
			{
				BaseSkill updateSkill = list[i];
				updateSkill.UpdateSkill();

				if(updateSkill.END)
				{
					list.Remove(updateSkill);
					Destroy(updateSkill.gameObject);
				}
			}
		}
	}
    #endregion

    public SkillData GetSkillData(string strKey)
	{
		SkillData skillData = null;
		DicSkillData.TryGetValue(strKey, out skillData);

        return skillData;
	}

	public SkillTemplate GetSkillTemplate(string strKey)
	{
		SkillTemplate skillTemplate = null;
		DicSkillTemplate.TryGetValue(strKey, out skillTemplate);
		return skillTemplate;
	}

	public void RunSkill(BaseObject keyObject, string strSkillTemplateKey)
	{
		SkillTemplate template = GetSkillTemplate(strSkillTemplateKey);
		if(template == null)
		{
			Debug.Log(strSkillTemplateKey + " Skill Template 키를 찾을수 없습니다.");

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
			listSkill = DicUseSkill[keyObject];

		listSkill.Add(runSkill);
	}

	public void ClearSkill()
	{
		foreach(KeyValuePair<BaseObject, List<BaseSkill>> pair
			in DicUseSkill)
		{
			List<BaseSkill> list = pair.Value;

			for(int i = 0; i< list.Count; i++)
			{
				BaseSkill updateSkill = list[i];
				list.Remove(updateSkill);
				Destroy(updateSkill.gameObject);
			}
		}
		DicUseSkill.Clear();
	}

	private BaseSkill CreateSkill(BaseObject owner, SkillTemplate skillTemplate)
	{
		BaseSkill makeSkill = null;

		GameObject skillObject = new GameObject();
		Transform firePos = null;

		switch (skillTemplate.SKILL_TYPE)
		{
			case eSkillTemplateType.TARGET_ATTACK:
				{
					makeSkill = skillObject.AddComponent<MeleeSkill>();
					firePos = owner.SelfTransform;
				}
				break;
			case eSkillTemplateType.RANGE_ATTACK:
				{
					makeSkill = skillObject.AddComponent<RangeSkill>();
					firePos = owner.GetChild("FirePos");

					if (firePos == null)
					{
						Debug.Log("FirePos 가 없습니다.");
						firePos = owner.SelfTransform;
					}
				}
				break;
		}

		skillObject.name = owner.name + " " + skillTemplate.SKILL_TYPE.ToString();

		if (makeSkill != null)
		{
			makeSkill.transform.position = firePos.position;
			makeSkill.transform.rotation = firePos.rotation;

			makeSkill.OWNER = owner;
			makeSkill.SKILL_TEMPLATE = skillTemplate;
			makeSkill.TARGET =
				owner.GetData(ConstValue.ActorData_GetTarget) as BaseObject;

			makeSkill.InitSkill();
		}

		switch (skillTemplate.RANGE_TYPE)
		{
			case eSkillAttackRangeType.RANGE_BOX:
				{
					BoxCollider collider =
						skillObject.AddComponent<BoxCollider>();
					collider.size =
						new Vector3(
							skillTemplate.RANGE_DATA_1, 
							1, 
							skillTemplate.RANGE_DATA_2);

					collider.center =
						new Vector3(
							0, 0.5f,
							skillTemplate.RANGE_DATA_2 * 0.5f);
					collider.isTrigger = true;
				}
				break;
			case eSkillAttackRangeType.RANGE_SPHERE:
				{
					SphereCollider collider = 
						skillObject.AddComponent<SphereCollider>();
					collider.radius = skillTemplate.RANGE_DATA_1;
					collider.isTrigger = true;
				}
				break;
		}

		return makeSkill;
	}

	private void LoadSkillData()
	{
		TextAsset skillAssetData = 
			Resources.Load(ConstValue.SkillDataPath) as TextAsset;

		if( skillAssetData == null)
		{
			Debug.Log(ConstValue.SkillDataPath + " 이 파일이 존재 하지 않습니다.");
			return;
		}

		JSONNode rootNode = JSON.Parse(skillAssetData.text);
		if (rootNode == null)
			return;

		JSONObject skillDataNode = 
			rootNode[ConstValue.SkillDataKey] as JSONObject;

		foreach(KeyValuePair<string ,JSONNode> pair in skillDataNode)
		{
			SkillData skillData = new SkillData(pair.Key, pair.Value);
			DicSkillData.Add(pair.Key, skillData);
		}
	}

	private void LoadSkillTemplate()
	{
		TextAsset skillAssetData =
			Resources.Load(ConstValue.SkillTemplatePath) as TextAsset;

		if (skillAssetData == null)
		{
			Debug.Log(ConstValue.SkillTemplatePath + " 이 파일이 존재 하지 않습니다.");
			return;
		}

		JSONNode rootNode = JSON.Parse(skillAssetData.text);
		if (rootNode == null)
			return;

		JSONObject skillDataNode =
			rootNode[ConstValue.SkillTemplateKey] as JSONObject;

		foreach (KeyValuePair<string, JSONNode> pair in skillDataNode)
		{
			SkillTemplate skillTemplate = new SkillTemplate(pair.Key, pair.Value);
			DicSkillTemplate.Add(pair.Key, skillTemplate);
		}
	}
}
