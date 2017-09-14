using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRegenerator : BaseObject
{
    private GameObject MonsterPrefab = null;
    List<Actor> listAttacMonster = new List<Actor>();

    public eRegeneratorType RegenType = eRegeneratorType.NONE;
    public eEnemyType EnemyType = eEnemyType.A_Monster;

    public int MaxObjectNum = 0;

    // RegenTime Event
    public float RegenTime = 300f;
    private float CurrTime = 0;

    // Trigger Event
    public float Radius = 15;

    private void OnEnable()
    {
        MonsterPrefab = ActorManager.Instance.GetEnemyPrefab(EnemyType);
        // Resources.Load("Prefabs/Actor/" + EnemyType.ToString()) as GameObject;

        if(MonsterPrefab == null)
        {
            Debug.LogError("몬스터 프리팹 로드 실패");
            return;
        }

        switch (RegenType)
        {
            case eRegeneratorType.REGENTIME_EVENT:
                CurrTime = 0;
                break;
            case eRegeneratorType.TRIGGER_EVENT:
                {
                    SphereCollider sc = this.gameObject.AddComponent<SphereCollider>();

                    sc.isTrigger = true;
                    sc.radius = Radius;
                }
                break;
        }
    }

    private void Update()
    {
        switch (RegenType)
        {
            case eRegeneratorType.REGENTIME_EVENT:
                {
                    if(this.RegenTime> this.CurrTime)
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
        }
    }

    void RegenMonster()
    {
        for(int i = listAttacMonster.Count; i < MaxObjectNum; i++)
        {
            //GameObject go = Instantiate(MonsterPrefab, SelfTransform.position + GetRandomPos(), Quaternion.identity) as GameObject;
            //Actor actor = go.GetComponent<Actor>();

            Actor actor = ActorManager.Instance.InstantiateOnce(MonsterPrefab, SelfTransform.position + GetRandomPos());

            // Enemy Init
            actor.ThrowEvent(ConstValue.EventKey_EnemyInit, this);

            listAttacMonster.Add(actor);
        }
    }

    Vector3 GetRandomPos()
    {
        Vector3 dir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));

        return dir.normalized * Random.Range(1, Radius);
    }

    public void RemoveActor(Actor actor)
    {
        if(listAttacMonster.Contains(actor) == true)
        {
            listAttacMonster.Remove(actor);
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
                    if(actor!= null && actor.IsPlayer == true)
                    {
                        RegenMonster();
                    }
                }
                break;
        }

    }
}
