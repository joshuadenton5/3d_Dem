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
        if(Interaction.Holding())
        {
            List<GenericInteraction> objs = Interaction.GetCurrent();
            base.GetCellAndMove(hit);
            int timeToCook = 0;          
            foreach(GenericInteraction i in objs)
            {
                if(i.tag == "Utensil")
                {
                    timeToCook += i.genericCookTime * objs.Count;
                }
                else
                {
                    Debug.Log("Nothing in the pan!!");
                }
            }            
            Debug.Log("time to cook: " + timeToCook);
        }
    }
}
