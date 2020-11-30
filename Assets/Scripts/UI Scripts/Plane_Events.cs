using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane_Events : GI_Events
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void OnMouseEnter()
    {
        base.OnMouseEnter();
    }

    protected override void NotHolding()
    {
        SelectColour(null);
    }

    protected override void IsHolding()
    {
        CheckForSpace();
    }

    void CheckForSpace()
    {
        if (genericInteraction.CellAvailable())
        {
            SelectColour("G");
        }
    }
}
