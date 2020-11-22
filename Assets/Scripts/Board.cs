using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : GenericInteraction
{
    private DynamicCell[,] dynamicPositions;
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

    DynamicCell GetPosition(DynamicCell[,] dynamicCells, Vector3 hitPoint)
    {
        float distance = float.MaxValue;
        DynamicCell cell = null;
        foreach(DynamicCell pos in dynamicCells)
        {
            float tempDist = Vector3.Distance(hitPoint, pos.transform.position);
            if (tempDist < distance)
            {
                distance = tempDist;
                cell = pos;
            }
        }
        if (!cell.Taken())
        {
            cell.SetTaken(true);
            return cell;
        }
        return null;
    }

    protected override void CheckCanPlace(RaycastHit hit, Interaction main)
    {
        if (main.Currents().Count > 1)//more than one object in hand 
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

    protected override void CheckForParent(){}

    int GetDec(float input)
    {
        if (input % 1 == 0)
            return 1;
        return 2;
    } 

    void PlaceCells(Transform trans, int mult) //spawning the cells on load, still working on this one .....
    {
        int decX = GetDec(trans.localScale.x);
        int decZ = GetDec(trans.localScale.z);
        Vector3 top = new Vector3(0, (trans.localScale.y / 2) + .02f, 0);
        int x = Mathf.CeilToInt(trans.localScale.x) * mult;
        int z = Mathf.CeilToInt(trans.localScale.z) * mult;
        dynamicPositions = new DynamicCell[x, z];

        for (int i = 0; i < z; i++)
        {
            for (int j = 0; j < x; j++)
            {
                Vector3 startPos = trans.position + (trans.forward/mult * z/2/decZ) - (trans.right/mult * x/2/decX ) + (trans.right/mult/2/decX ) - (trans.forward/mult/2/decZ ); //starting at the top right of the object
                Vector3 newPos = startPos - (trans.forward/mult/decZ * i) + (trans.right/mult/decX * j) + top; //moving according to dimensions 

                GameObject obj = Instantiate(dot, newPos, dot.transform.rotation); //debug
                obj.transform.SetParent(trans);//debug 

                DynamicCell cell = obj.AddComponent<DynamicCell>();
                dynamicPositions[j, i] = cell;
            }
        }
    }

    /*void ResetCellPositions()
    {
        for (int i = 0; i < cells.GetLength(0); i++)
        {
            for (int j = 0; j < cells.GetLength(1); j++)
            {
                cells[j, i].SetPosition(dynamicPositions[j, i].transform.position);
            }
        }
    }*/
}
