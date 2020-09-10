using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utensil : GenericInteraction
{
    //private Cell localCell; //to allow items to be placed 
    private Vector3 bottom, yDist;
    private int limit = 3;

    public override void Start()
    {
        base.Start();
        bottom = new Vector3(0, -(transform.localScale.y / 2), 0);
        yDist = new Vector3(0, .15f, 0);
        localCell = new Cell();
    }

    override public void OnLeftMouseButton(RaycastHit hit)
    {
        if (!Interaction.IsHolding()) //not holding an object
        {
            base.CheckHolding();
            if (localCell.interactions.Count > 0)
            {
                foreach (GenericInteraction gen in localCell.interactions)
                {
                    gen.DisableRb();
                    gen.SetColliderTrigger(true);
                    StartCoroutine(Interaction.OnPickUp(gen.transform));
                }
            }
        }
        else
        {
            CheckForUtensil();
        }
        //base.OnLeftMouseButton(hit);       
    }

    public override void CheckForUtensil()
    {
        if(localCell.interactions.Count < limit)
        {
            GenericInteraction current = Interaction.GetCurrent();
            localCell.SetPosition(transform.position + bottom);
            localCell.interactions.Add(current);
            current.SetSurfaceCell(localCell);
            Vector3 buffer = new Vector3(0, transform.position.y / 2, 0);
            buffer += yDist;
            StartCoroutine(Interaction.DelayThePhysics(localCell.Position() + buffer, current));
        }
        else
        {
            Debug.Log("Pan full");
            //localCell.SetOccupied(true);
        }
    }  
}
