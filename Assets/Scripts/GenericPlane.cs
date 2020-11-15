using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericPlane : MonoBehaviour, IInteract
{
    [SerializeField]
    private Cell[,] cells;

    public GameObject dot;
    private Vector3 yDist;
    protected Interaction interaction;

    public virtual void Start()
    {
        yDist = new Vector3(0, .15f, 0);
        interaction = FindObjectOfType<Interaction>();
        PlaceCells(transform);
    }

    public virtual void OnLeftMouseButton(RaycastHit hit)
    {
        if(interaction.Holding()) //holding an object 
        {
            GetCellAndMove(hit);
        }
    }

    protected virtual void GetCellAndMove(RaycastHit hit)
    {
        Cell cell = SelectCell(cells, hit.point);
        if (cell != null)//position is not taken
        {
            if(interaction.Currents().Count > 1)//if the player is holding more than one item 
            {
                StartCoroutine(DelayedDrop(interaction.Currents(), cell));
            }
            else
            {
                GenericInteraction current = interaction.Currents()[0];
                AssignInteraction(current, cell);
                StartCoroutine(interaction.OnPutDown(current));
            }           
        }
        else
        {
            Debug.Log("This slot is taken!!");
        }       
    }

    void AssignInteraction(GenericInteraction interaction, Cell cell)
    {
        interaction.SetDesination(cell.Position());
        interaction.SetCell(cell);
    }

    private IEnumerator DelayedDrop(List<GenericInteraction> interactions, Cell cell) //experimental function that drops items in an ordered fashion
    {
        List<GenericInteraction> tempList = new List<GenericInteraction>(interactions); //creating a temp list as 'interactions' is modified in the 'OnPutDown' function 
        AssignInteraction(tempList[0], cell);
        StartCoroutine(interaction.OnPutDown(tempList[0]));//placing the first element down 
        for (int i = 1; i < tempList.Count; i++)//then placing the other items stored 
        {
            AssignInteraction(tempList[i], cell);
            StartCoroutine(interaction.ArcMotionPutDown(tempList[i]));
            yield return new WaitForSeconds(.05f);
        }
        yield return null;
    }

    private Cell SelectCell(Cell[,] _cells, Vector3 clickPoint) //function that moves item in hand to cell position 
    {
        float distance = float.MaxValue;
        Cell cell = new Cell();
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
            return cell;
        }
        return null;
    }

    void PlaceCells(Transform trans) //spawning the cells on load
    {
        Vector3 top = new Vector3(0, (trans.localScale.y / 2) + .02f, 0);
        float x = trans.localScale.x;
        float z = trans.localScale.z;

        cells = new Cell[(int)x, (int)z];
        for(int i = 0; i < z; i++)
        {
            for(int j = 0; j < x; j++)
            {
                Vector3 startPos = trans.position + (trans.forward * z / 2) - (trans.right * x / 2) + (trans.right / 2) - (transform.forward / 2); //starting at the top right of the object
                Vector3 newPos = startPos - (transform.forward * i) + (transform.right * j) + top; //moving according to dimensions 
                Cell c = new Cell();
                c.SetPosition(newPos);
                cells[j, i] = c;
                Instantiate(dot, newPos, dot.transform.rotation); //debug
            }
        }
    }

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
