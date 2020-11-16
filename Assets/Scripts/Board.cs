using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : GenericInteraction
{
    private Cell[,] cells;
    private GameObject[,] dynamicPositions;
    public GameObject dot;

    public override void Start()
    {
        base.Start();
        PlaceCells(transform, 2);
        localInteractions.Add(this);
    }

    public override void OnLeftMouseButton(RaycastHit hit, Interaction main)
    {
        base.OnLeftMouseButton(hit, main);
    }

    protected override void NothingInHand(Interaction main)
    {
        if (localInteractions.Count > 1)//more than one object in hand
        {
            base.CheckCell();
            StartCoroutine(DelayedPickUp(localInteractions, main));
        }
        else //only one object in hand
        {
            base.NothingInHand(main);
        }
    }

    protected override void CheckCanPlace(RaycastHit hit, Interaction main)
    {
        ResetCellPositions();
        Cell cell = SelectCell(cells, hit.point);
        if (cell != null)
        {
            if (main.Currents().Count > 1)//more than one object in hand 
            {
                Debug.Log("Cant do that");
            }
            else
            {
                GenericInteraction current = main.Currents()[0];
                AssignInteraction(current, cell);
                current.SetParent(this);
                localInteractions.Add(current);
                StartCoroutine(main.OnPutDown(current));
            }
        }
    }

    void AssignInteraction(GenericInteraction interaction, Cell cell)
    {
        interaction.SetDesination(cell.Position());
        interaction.SetCell(cell);
    }


    private Cell SelectCell(Cell[,] _cells, Vector3 clickPoint) //function that moves item in hand to cell position 
    {
        float distance = float.MaxValue;
        Cell cell = new Cell(clickPoint);
        foreach (Cell c in _cells)
        {
            float tempDist = Vector3.Distance(clickPoint, c.Position());
            if (tempDist < distance)
            {
                distance = tempDist;
                cell = c;
            }
        }
        if (!cell.Taken()) //checking if there is already an item placed on the cell
        {
            cell.SetOccupied(true);
        }
        return cell;
    }

    protected override void CheckForParent(){}

    int GetDec(float input)
    {
        if (input % 1 == 0)
            return 1;
        return 2;
    }

    void ResetCellPositions()
    {
        for (int i = 0; i < cells.GetLength(0); i++)
        {
            for (int j = 0; j < cells.GetLength(1); j++)
            {
                cells[j, i].SetPosition(dynamicPositions[j, i].transform.position);
            }
        }
    }

    void PlaceCells(Transform trans, int mult) //spawning the cells on load, still working on this one .....
    {
        int decX = GetDec(trans.localScale.x);
        int decZ = GetDec(trans.localScale.z);
        Vector3 top = new Vector3(0, (trans.localScale.y / 2) + .02f, 0);
        int x = Mathf.CeilToInt(trans.localScale.x) * mult;
        int z = Mathf.CeilToInt(trans.localScale.z) * mult;
        cells = new Cell[x, z];
        dynamicPositions = new GameObject[x, z];

        for (int i = 0; i < z; i++)
        {
            for (int j = 0; j < x; j++)
            {
                Vector3 startPos = trans.position + (trans.forward/mult * z/2/decZ ) - (trans.right/mult * x/2/decX ) + (trans.right/mult/2/decX ) - (trans.forward/mult/2/decZ ); //starting at the top right of the object
                Vector3 newPos = startPos - (trans.forward/mult/decZ * i) + (trans.right/mult/decX * j) + top; //moving according to dimensions 
                Cell c = new Cell(newPos);
                cells[j, i] = c;
                GameObject obj = Instantiate(dot, newPos, dot.transform.rotation); //debug
                obj.transform.SetParent(trans);
                dynamicPositions[j, i] = obj;
            }
        }
    }
}
