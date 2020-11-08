using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utensil : GenericInteraction
{
    [SerializeField]
    private Cell localCell; //This is the position that objects can be placed 
    private Vector3 bottom, yDist;
    private int limit = 3;

    public override void Start()
    {
        base.Start();
        bottom = new Vector3(0, -(transform.localScale.y / 2), 0);
        yDist = new Vector3(0, .15f, 0);
        localCell = new Cell();
        localCell.interactions.Add(this);
    }

    public Cell LocalCell() { return localCell; }
      
    override public void OnLeftMouseButton(RaycastHit hit)
    {
        if (!interaction.Holding()) //not holding an object
        {
            if (localCell.interactions.Count > 1)
            {
                StartCoroutine(DelayedPickUp(localCell.interactions));
            }
            else
            {
                base.NothingInHand();
            }
        }
        else
        {
            if(interaction.Currents().Count > 1)
            {

            }
            else
            {
                GenericInteraction current = interaction.Currents()[0];
                current.SetParent(this);
                Vector3 buffer = new Vector3(0, current.transform.localScale.y / 2f, 0);
                localCell.SetPosition(transform.position);
                localCell.interactions.Add(current); //adding current to the utensils cell list 
                StartCoroutine(interaction.OnPutDown(current, localCell, buffer + yDist));
            }           
        }
    }

    public override void CheckForParent(Utensil _parent){}

    public IEnumerator DelayedPickUp(List<GenericInteraction> interactions)
    {
        for (int i = 0; i < interactions.Count; i++)
        {
            StartCoroutine(interaction.OnPickUp(interactions[i]));
            yield return new WaitForSeconds(.05f);
        }
        yield return null;
    }

    public override void CheckForUtensil()//if holding and looking at pan 
    {

    }  
}
