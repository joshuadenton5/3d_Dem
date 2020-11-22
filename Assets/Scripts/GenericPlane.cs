using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericPlane : MonoBehaviour, IInteract
{
    [SerializeField]
    private DynamicCell[,] cells;
    public GameObject dot;
    private Vector3 yDist;

    public virtual void Start()
    {
        yDist = new Vector3(0, .15f, 0);
        PlaceCells(transform, 1);
    }

    public virtual void OnLeftMouseButton(RaycastHit hit, Interaction main)
    {
        if(main.Holding()) //holding an object 
        {
            GetCellAndMove(hit, main);
        }
    }

    protected virtual void GetCellAndMove(RaycastHit hit, Interaction main)
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

    void AssignInteraction(GenericInteraction interaction, DynamicCell cell)
    {
        interaction.SetDesination(cell.transform.position);
        interaction.SetCell(cell);
    }

    private IEnumerator DelayedDrop(List<GenericInteraction> interactions, DynamicCell cell, Interaction main) //experimental function that drops items in an ordered fashion
    {
        List<GenericInteraction> tempList = new List<GenericInteraction>(interactions); //creating a temp list as 'interactions' is modified in the 'OnPutDown' function 
        AssignInteraction(tempList[0], cell);
        yield return StartCoroutine(main.OnPutDown(tempList[0]));//placing the first element down 
        for (int i = 1; i < tempList.Count; i++)//then placing the other items stored 
        {
            tempList[i].SetDesination(tempList[i].GetCell().transform.position);
            StartCoroutine(main.ArcMotionPutDown(tempList[i]));
            yield return new WaitForSeconds(.05f);
        }
        yield return null;
    }

    DynamicCell GetPosition(DynamicCell[,] dynamicCells, Vector3 hitPoint)
    {
        float distance = float.MaxValue;
        DynamicCell cell = null;
        foreach (DynamicCell pos in dynamicCells)
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
        cells = new DynamicCell[x, z];

        for (int i = 0; i < z; i++)
        {
            for (int j = 0; j < x; j++)
            {
                Vector3 startPos = trans.position + (trans.forward / mult * z / 2 / decZ) - (trans.right / mult * x / 2 / decX) + (trans.right / mult / 2 / decX) - (trans.forward / mult / 2 / decZ); //starting at the top right of the object
                Vector3 newPos = startPos - (trans.forward / mult / decZ * i) + (trans.right / mult / decX * j) + top; //moving according to dimensions 

                GameObject obj = Instantiate(dot, newPos, dot.transform.rotation); //debug
                obj.transform.SetParent(trans);//debug 

                DynamicCell cell = obj.AddComponent<DynamicCell>();
                cells[j, i] = cell;
            }
        }
    }

    /*void PlaceCells(Transform trans) //spawning the cells on load
    {
        Vector3 top = new Vector3(0, (trans.localScale.y / 2) + .02f, 0);
        float x = trans.localScale.x;
        float z = trans.localScale.z;

        cells = new DynamicCell[(int)x, (int)z];
        for(int i = 0; i < z; i++)
        {
            for(int j = 0; j < x; j++)
            {
                Vector3 startPos = trans.position + (trans.forward * z / 2) - (trans.right * x / 2) + (trans.right / 2) - (trans.forward / 2); //starting at the top right of the object
                Vector3 newPos = startPos - (trans.forward * i) + (trans.right * j) + top; //moving according to dimensions 
                Cell c = new Cell(newPos);
                cells[j, i] = c;
                Instantiate(dot, newPos, dot.transform.rotation); //debug
            }
        }
    }*/

    //unused functions

    void DrawOnGrid() //debug 
    {
        float xScale = transform.localScale.x;
        float zScale = transform.localScale.z;

        Vector3 toTop = new Vector3(0, (transform.localScale.y / 2) + .02f, 0);
        for (float i = xScale; i < xScale * xScale; i += xScale)
        {
            Vector3 position = transform.position + toTop;
            position += (transform.right / xScale * i) - (transform.right * xScale / 2);
            float yRot = transform.rotation.eulerAngles.y - 90;
            Quaternion q = Quaternion.AngleAxis(yRot, transform.up);
            //GameObject newLine = Instantiate(line, position, q);
            //Instantiate(dot, position, dot.transform.rotation);
            //newLine.transform.localScale = new Vector3(zScale, newLine.transform.localScale.y, newLine.transform.localScale.z);
        }

        for (float i = zScale; i < zScale * zScale; i += zScale)
        {
            Vector3 position = transform.position + toTop;
            position += (transform.forward / zScale * i) - (transform.forward * zScale / 2);
            //GameObject newLine = Instantiate(line, position, transform.rotation);
            //Instantiate(dot, position, dot.transform.rotation);
            //newLine.transform.localScale = new Vector3(xScale, newLine.transform.localScale.y, newLine.transform.localScale.z);
        }
    }
}
