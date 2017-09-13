using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionArea : BaseObject
{
    public List<Actor> ListActor = new List<Actor>();
    CapsuleCollider CapColl;

    public void Init(float radius)
    {
        this.CapColl = SelfComponent<CapsuleCollider>();
        if (CapColl == null)
        {
            this.CapColl = SelfObject.AddComponent<CapsuleCollider>();
        }

        this.CapColl.isTrigger = true;
        this.CapColl.radius = radius;
    }

    public Actor GetActor()
    {
        Actor actor = null;

        while (actor == null)
        {
            if (this.ListActor.Count <= 0)
            {
                break;
            }

            actor = ListActor[0];

            if (actor == null)
            {
                this.ListActor.RemoveAt(0);
            }
        }

        return actor;
    }

    private void OnTriggerEnter(Collider other)
    {
        Actor actor = other.GetComponent<Actor>();
        if (actor == null)
        {
            return;
        }

        if (ListActor.Contains(actor) == true)
        {
            return;
        }

        ListActor.Add(actor);
    }

    private void OnTriggerExit(Collider other)
    {
        Actor actor = other.GetComponent<Actor>();
        if (actor == null)
        {
            return;
        }

        if (ListActor.Contains(actor) == true)
        {
            ListActor.Remove(actor) ;
        }
    }

}
