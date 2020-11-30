using UnityEngine;

public class Board : GenericInteraction
{
    private DynamicCell[,] dynamicPositions;
    public GameObject dot;

    public override void Start()
    {
        base.Start();
        dynamicPositions = InitialiseCells(transform, dot, 2, 2);
        localInteractions.Add(this);
    }

    public override void OnLeftMouseButton(RaycastHit hit, Interaction main)
    {
        base.OnLeftMouseButton(hit, main);
    }

    protected override void DefaultInteraction(Interaction main)
    {
        if (localInteractions.Count > 1)//more than one object in hand
        {
            base.CheckCell();
            StartCoroutine(DelayedPickUp(localInteractions, main));
        }
        else //only one object in hand
        {
            base.DefaultInteraction(main);
        }
    }

    protected override void CheckCanPlace(RaycastHit hit, Interaction main)
    {
        if (main.Currents().Count > 1 || main.Currents()[0].CompareTag(tag))//more than one object in hand 
        {
            Debug.Log("Cant do that");
        }
        else
        {
            DynamicCell cell = GetPosition(dynamicPositions, hit.point);
            if(cell != null)
            {
                GenericInteraction current = main.Currents()[0];
                current.SetDesination(cell.transform.position);
                current.SetParent(this);
                current.SetCell(cell);
                localInteractions.Add(current);
                StartCoroutine(main.OnPutDown(current));
            }
            else
            {
                Debug.Log("slot Taken");
            }
        }
    }

    public override bool CellAvailable()
    {
        foreach (DynamicCell cell in dynamicPositions)
        {
            if (!cell.Taken()) return true;
        }
        return false;
    }

    protected override void CheckForParent(){}

    int GetDec(float input)
    {
        if (input % 1 == 0)
            return 1;
        return 2;
    } 
}
