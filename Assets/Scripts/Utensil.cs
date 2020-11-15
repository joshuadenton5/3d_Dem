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

    override public void OnLeftMouseButton(RaycastHit hit)
    {
        if (!interaction.Holding()) //not holding an object
        {
            if (localInteractions.Count > 1)
            {
                StartCoroutine(DelayedPickUp(localInteractions));
            }
            else
            {
                base.NothingInHand();
            }
        }
        else//holding 
        {
            if (interaction.Currents().Count > 1 || interaction.Currents()[0].CompareTag(tag))
            {
                Debug.Log("Not Allowed!");
            }
            else
            {
                GenericInteraction current = interaction.Currents()[0];//will always be this as the first element 
                current.SetDesination(transform.position);
                current.SetParent(this);
                //adding current to the utensils cell list 
                localInteractions.Add(current);
                StartCoroutine(interaction.OnPutDown(current));
            }           
        }
    }

    public override void CheckForParent(){  }

    public override void CheckCell(){ base.CheckCell();}

    public IEnumerator DelayedPickUp(List<GenericInteraction> interactions)
    {
        StartCoroutine(interaction.OnPickUp(interactions[0]));
        for (int i = 1; i < interactions.Count; i++)
        {
            StartCoroutine(interaction.ArcMotionPickUp(interactions[i]));
            yield return new WaitForSeconds(.05f);
        }
        yield return null;
    }

    public override void CheckForUtensil()//if holding and looking at pan 
    {

    }  
}
