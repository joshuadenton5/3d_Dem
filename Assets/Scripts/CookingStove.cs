using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingStove : GenericPlane
{
    public override void Start()
    {
        base.Start();
    }

    public override void OnLeftMouseButton(RaycastHit hit, Interaction main)
    {
        if(main.Holding())
        {
            
        }
    }
}
