using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRegnerator : BaseObject
{
    public eRegeneratorType RegenType = eRegeneratorType.NONE;
    public eEnemyType       EnemyType = eEnemyType.A_Monster;

    public int MaxObjectNum = 0;

    // RegenTime Event
    public float RegenTime = 300f;
    public float CurrTime = 0f;

    // Trigger Event
    public float Radius = 15f;

    private GameObject MonsterPrefab = null;
    private List<Actor> listAttachMonster = new List<Actor>();

    private void OnEnable()
    {
        MonsterPrefab = ActorManager.Instance.GetEnemyPrefab(EnemyType);
            //Resources.Load("Prefabs/Actor/" + EnemyType.ToString()) as GameObject;
        if (MonsterPrefab == null)
        {
            Debug.LogError("몬스터 로드 실패");
            return;
        }

        switch (RegenType)
        {
            case eRegeneratorType.REGENTIME_EVENT:
                CurrTime = 0f;
                break;
            case eRegeneratorType.TRIGGER_EVENT:
                {
                    SphereCollider sc = this.gameObject.AddComponent<SphereCollider>();
                    {
                        sc.isTrigger = true;
                        sc.radius = Radius;
                    }
                }
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        switch (RegenType)
        {
            case eRegeneratorType.REGENTIME_EVENT:
                {
                    if (this.RegenTime > this.CurrTime)
                    {
                        this.CurrTime += Time.deltaTime;
                        RegenMonster();
                    }
                    else
                    {
                        this.CurrTime = 0;
                    }
                }
                break;
            case eRegeneratorType.TRIGGER_EVENT:
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (RegenType)
        {
            case eRegeneratorType.REGENTIME_EVENT:
                break;
            case eRegeneratorType.TRIGGER_EVENT:
                {
                    Actor actor = other.GetComponent<Actor>();
                    if (actor != null && actor.IsPlayer == true)
                    {
                        RegenMonster();
                    }
                }
                break;
            default:
                break;
        }
    }

    private void RegenMonster()
    {
        for (int i = listAttachMonster.Count; i < MaxObjectNum; i++)
        {
            Actor actor = ActorManager.Instance.InstantiateOnce(MonsterPrefab, 
                                                                SelfTransform.position + GetRandomPos());
            actor.ThrowEvent(ConstValue.EventKey_EnemyInit, this);

            listAttachMonster.Add(actor);
        }
    }

    private Vector3 GetRandomPos()
    {
        Vector3 dir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        return dir.normalized * Random.Range(1, Radius);
    }
    
    public void RemoveActor(Actor actor)
    {
        if (listAttachMonster.Contains(actor) == true)
        {
            this.listAttachMonster.Remove(actor);
        }
    }

}
