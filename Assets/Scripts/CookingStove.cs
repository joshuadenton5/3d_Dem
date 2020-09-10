using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingStove : GenericPlane
{
    public override void Start()
    {
        base.Start();
    }

    public override void OnLeftMouseButton(RaycastHit hit)
    {
        if(Interaction.IsHolding())
        {
            GenericInteraction obj = Interaction.GetCurrent();
            base.GetCellAndMove(hit);
            int timeToCook = 0;          
            if(obj.tag == "Utensil")
            {
                Cell cell = obj.LocalCell();
                if (cell.interactions.Count > 0)
                {
                    timeToCook += obj.genericCookTime * cell.interactions.Count;
                }
                else
                {
                    Debug.Log("Nothing in the pan!!");
                }
            }        
            Debug.Log(timeToCook);
        }
    }
}
