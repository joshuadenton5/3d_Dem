using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utensil : GenericInteraction
{
    private Vector3 yDist;
    private List<GenericInteraction> localInteractions = new List<GenericInteraction>(); //objects in utensil 

    public override void Start()
    {
        base.Start();
        yDist = new Vector3(0, .15f, 0);
        localInteractions.Add(this);
    }

    public List<GenericInteraction> LocalInteractions()
    {
        List<GenericInteraction> tmp = new List<GenericInteraction>(localInteractions);
        return tmp;
    }

    public void AddToLocal(GenericInteraction interaction)
    {
        if (interaction != null)
            localInteractions.Add(interaction);
    }
    public void RemoveFromLocal(GenericInteraction interaction)
    {
        if(interaction!=null)
            localInteractions.Remove(interaction);
    }

    override public void OnLeftMouseButton(RaycastHit hit, Interaction main)
    {
        if (!main.Holding()) //not holding an object
        {
            if (localInteractions.Count > 1)
            {
                base.CheckCell();
                StartCoroutine(DelayedPickUp(localInteractions, main));
            }
            else
            {
                base.NothingInHand(main);
            }
        }
        else//holding 
        {
            if (main.Currents().Count > 1 || main.Currents()[0].CompareTag(tag))
            {
                Debug.Log("Not Allowed!");
            }
            else
            {
                GenericInteraction current = main.Currents()[0];//will always be this as the first element 
                current.SetDesination(transform.position);
                current.SetParent(this);
                //adding current to the utensils cell list 
                localInteractions.Add(current);
                StartCoroutine(main.OnPutDown(current));
            }           
        }
    }

    public override void CheckForParent(){  }

    public IEnumerator DelayedPickUp(List<GenericInteraction> interactions, Interaction main)
    {
        StartCoroutine(main.OnPickUp(interactions[0]));
        for (int i = 1; i < interactions.Count; i++)
        {
            StartCoroutine(main.ArcMotionPickUp(interactions[i]));
            interactions[i].CheckCell();
            yield return new WaitForSeconds(.05f);
        }
        yield return null;
    }

    public override void CheckForOther()//if holding and looking at pan 
    {

    }  
}
