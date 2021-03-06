﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System;

public class StageManager : MonoSingleton<StageManager> 
{
	Dictionary<int, StageInfo> DicStageInfo = new Dictionary<int, StageInfo>();
    public Dictionary<int, StageInfo> DIC_STAGEINFO
    {
        get
        {
            return DicStageInfo;
        }
    }

    Generators generators;
    EnemyRegenerator currentRegenerator;

	public void StageInit()
	{
		TextAsset stageInfo = 
			Resources.Load(ConstValue.StageData_Path) as TextAsset;

		JSONNode rootNode = JSON.Parse(stageInfo.text);

		foreach (KeyValuePair<string, JSONNode> pair in
			rootNode[ConstValue.StageData_Key] as JSONObject)
		{
			StageInfo info = new StageInfo(pair.Key, pair.Value);
			DicStageInfo.Add(int.Parse(info.KEY), info);
		}
	}

	public StageInfo LoadStage(int selectStage)
	{
		StageInfo info = null;
		DicStageInfo.TryGetValue(selectStage, out info);

		if(info == null)
		{
			Debug.LogError("#1 JSON 정상 로드 확인" + "  #2 JSON Key 값 확인");

			return null;
		}

		GameObject go = Resources.Load("Prefabs/Stages/" + info.MODEL) as GameObject;
		Debug.Assert(go != null, "스테이지 리소스 로드 실패");

		Instantiate(go, Vector3.zero, Quaternion.identity);

        // Test
        this.generators = go.GetComponentInChildren<Generators>();

        return info;
	}

    public BaseObject GetSearchRegenerator(BaseObject actor, out float dist, float radius = 100.0f)
    {
        Vector3 myPosition = actor.SelfTransform.position;

        float nearDistance = radius;
        this.currentRegenerator = null;

        for (int i = 0; i < this.generators.listEnemyRegenerator.Count; i++)
        {
            float distance = Vector3.Distance(myPosition, this.generators.listEnemyRegenerator[i].SelfTransform.position);

            if (distance < nearDistance)
            {
                nearDistance = distance;
                this.currentRegenerator = generators.listEnemyRegenerator[i];
            }
        }
        
        dist = nearDistance;

        return this.currentRegenerator;
    }

    public bool FindRegenerator()
    {
        if (this.currentRegenerator.ActiveRegen)
        {
            // 몬스터 있을때
            while (EnemyRegenerator.IsMonsters())
            {
                return true;
            }

            if (this.generators.listEnemyRegenerator.Contains(this.currentRegenerator))
            {
                this.generators.listEnemyRegenerator.Remove(this.currentRegenerator);
            }
            this.currentRegenerator.ActiveRegen = false;

            return false;
        }

        return false;
    }
}
