using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ActorManager : MonoSingleton<ActorManager>
{
    // Hierachy managed
    Transform ActorTransRoot = null;

    // 모든 액터관리
    Dictionary<eTeamType, List<Actor>> DicActor = new Dictionary<eTeamType, List<Actor>>();

    Dictionary<eEnemyType, GameObject> DicEnemyPrefab = new Dictionary<eEnemyType, GameObject>();

    private void Awake()
    {
        EnemyPrefabLoad();           
    }

    private void EnemyPrefabLoad()
    {
        for (int i = 0; i < (int)eEnemyType.MAX; i++)
        {
            GameObject go = Resources.Load("Prefabs/Actor/" + 
                                            ((eEnemyType)i).ToString()) as GameObject;
            if (go == null)
            {
                Debug.LogError(((eEnemyType)i).ToString("F") +
                                    "Load Failed.");
                continue;
            }
            else
            {
                DicEnemyPrefab.Add((eEnemyType)i, go);
            }
        }
    }

    public GameObject GetEnemyPrefab(eEnemyType type)
    {
        if (DicEnemyPrefab.ContainsKey(type) == true)
        {
            return DicEnemyPrefab[type];
        }
        else
        {
            Debug.LogError(type.ToString() + " 타입의 적 프리팹이 없습니다.");
            return null;
        }
    }

    public Actor PlayerLoad()
    {
        // Player
        GameObject playerPrefab = Resources.Load("Prefabs/Actor/" + "Player") as GameObject;

        // Create Clone in Scene
        GameObject go = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);

        return go.GetComponent<Actor>();
    }

    public Actor InstantiateOnce(GameObject prefab, Vector3 pos)
    {
        if (prefab == null)
        {
            Debug.LogError("프리팹이 null 입니다," +
                            "[ ActorManager.InstantiateOnce() ]");
            return null;
        }

        GameObject go = Instantiate(prefab, pos, Quaternion.identity);
        if (ActorTransRoot == null)
        {
            GameObject temp = new GameObject("ActorRoot");
            ActorTransRoot = temp.transform;
        }
        go.transform.SetParent(ActorTransRoot);

        return go.GetComponent<Actor>();
    }

    public void AddActor(Actor actor)
    {
        List<Actor> listActor = null;
        eTeamType teamType = actor.TeamType;

        if (this.DicActor.ContainsKey(teamType) == false)
        {
            listActor = new List<Actor>();
            this.DicActor.Add(teamType, listActor);
        }
        else
        {
            DicActor.TryGetValue(teamType, out listActor);
        }

        listActor.Add(actor);
    }

    public void RemoveActor(Actor actor, bool bDelete = false)
    {
        eTeamType teamType = actor.TeamType;

        if (DicActor.ContainsKey(teamType) == true)
        {
            List<Actor> listActor = null;
            DicActor.TryGetValue(teamType, out listActor);
            listActor.Remove(actor);
        }
        else
        {
            Debug.LogError("존재 하지 않는 액터를 삭제하려고 한다");
        }

        if (bDelete)
        {
            Destroy(actor.SelfObject);
        }
    }
}
