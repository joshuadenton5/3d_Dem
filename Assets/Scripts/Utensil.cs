using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utensil : GenericInteraction
{
    [SerializeField]
    private Cell localCell; //This is the position that.... 
    private Vector3 bottom, yDist;
    private int limit = 3;

    public override void Start()
    {
        base.Start();
        bottom = new Vector3(0, -(transform.localScale.y / 2), 0);
        yDist = new Vector3(0, .15f, 0);
        localCell = new Cell();
    }

    public Cell LocalCell() { return localCell; }
      
    override public void OnLeftMouseButton(RaycastHit hit)
    {
        if (!Interaction.Holding()) //not holding an object
        {
            base.NothingInHand();
            if (localCell.interactions.Count > 0) //if this item has other items associated with it 
            {
                foreach (GenericInteraction i in localCell.interactions)
                    guide.Interactions().Add(i);
                StartCoroutine(DelayedPickUp(localCell.interactions));
            }
        }
        else //check to see if items can be placed on this object
        {
            CheckForUtensil();
        }
    }

    public IEnumerator DelayedPickUp(List<GenericInteraction> interactions)
    {
        int i = 0;
        Debug.Log(interactions.Count);
        do
        {
            yield return new WaitForSeconds(.3f);
            interactions[i].DisableRb();
            interactions[i].SetColliderTrigger(true);
            StartCoroutine(Interaction.OnPickUp(interactions[i]));
            i++;
        } while (i < interactions.Count);
        yield return null;
    }

    public override void CheckForUtensil()//if holding and looking at pan 
    {
        if(localCell.interactions.Count < limit)
        {
            localCell.SetPosition(transform.position + bottom);
            foreach(GenericInteraction i in guide.Interactions())
            {
                localCell.interactions.Add(i);
                i.SetSurfaceCell(localCell);
            }
            Vector3 buffer = new Vector3(0, transform.position.y / 2, 0);
            buffer += yDist;
            StartCoroutine(Interaction.DelayThePhysics(localCell.Position() + buffer, guide.Interactions()));
            //current.SetSurfaceCell(localCell);           
        }
        else
        {
            localCell.SetOccupied(true);
            Debug.Log("Pan full");
            //localCell.SetOccupied(true);
        }
    }  
}
