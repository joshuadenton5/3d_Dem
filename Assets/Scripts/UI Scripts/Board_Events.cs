using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board_Events : GI_Events
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void OnMouseEnter()
    {
        base.OnMouseEnter();
    }

    protected override void IsHolding()
    {
        if (genericInteraction.CellAvailable())
        {
            SelectColour("G");
        }
        else
        {
            SelectColour("R");
        }
    }

    protected override void NotHolding()
    {
        base.NotHolding();
    }
}
