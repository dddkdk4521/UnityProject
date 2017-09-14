using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionArea : BaseObject
{
    public List<Actor> ListActor = new List<Actor>();

    CapsuleCollider CapColl;

    public void Init(float radius)
    {
        CapColl = SelfComponent<CapsuleCollider>();

        if (CapColl == null)
        {
            CapColl = SelfObject.AddComponent<CapsuleCollider>();
        }

        CapColl.isTrigger = true;
        CapColl.radius = radius;
    }

    private void OnTriggerEnter(Collider other)
    {
        Actor actor = other.GetComponent<Actor>();

        if (actor == null)
            return;

        if (ListActor.Contains(actor) == true)
            return;

        ListActor.Add(actor);
    }

    private void OnTriggerExit(Collider other)
    {
        Actor actor = other.GetComponent<Actor>();

        if (actor == null)
            return;

        if (ListActor.Contains(actor) == true)
        {
            ListActor.Remove(actor);
        }
    }

    public Actor GetActor()
    {
        Actor actor = null;

        while(actor == null)
        {
            if(ListActor.Count <= 0)
            {
                break;
            }

            actor = ListActor[0];

            if (actor == null)
            {
                ListActor.RemoveAt(0);
            }           
        }

        return actor;
    }

}
