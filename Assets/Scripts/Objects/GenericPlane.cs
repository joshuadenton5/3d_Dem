using UnityEngine;

public class GenericPlane : GenericInteraction
{
    [SerializeField]
    private DynamicCell[,] cells;
    public GameObject dot;
    private Vector3 yDist;

    public override void Start()
    {
        yDist = new Vector3(0, .15f, 0);
        cells = InitialiseCells(transform, dot, 1);
    }

    public override void OnLeftMouseButton(RaycastHit hit, Interaction main)
    {
        base.OnLeftMouseButton(hit, main);
    }

    protected override void DefaultInteraction(Interaction main)
    {
        Debug.Log("Nothing in hand!!");
    }

    protected override void CheckCanPlace(RaycastHit hit, Interaction main)
    {
        DynamicCell cell = GetPosition(cells, hit.point);
        if (cell != null)//position is not taken
        {
            if(main.Currents().Count > 1)//if the player is holding more than one item 
            {
                StartCoroutine(DelayedDrop(main.Currents(), cell, main));
            }
            else
            {
                GenericInteraction current = main.Currents()[0];
                AssignInteraction(current, cell);
                StartCoroutine(main.OnPutDown(current));
            }           
        }
        else
        {
            Debug.Log("This slot is taken!!");
        }       
    }

    public override bool CellAvailable()
    {
        foreach(DynamicCell cell in cells)
        {
            if (!cell.Taken()) return true;
        }
        return false;
    }
}
