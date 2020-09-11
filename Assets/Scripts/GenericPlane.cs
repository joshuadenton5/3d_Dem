using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericPlane : MonoBehaviour, IInteract
{
    private float ratio;
    private Cell[,] cells;
    public GameObject dot;
    private Vector3 yDist;
    private float test;

    public virtual void Start()
    {
        yDist = new Vector3(0, .15f, 0);
        PlaceCells(transform);
    }

    public virtual void OnLeftMouseButton(RaycastHit hit)
    {
        if(Interaction.IsHolding()) //holding an object 
        {
            GetCellAndMove(hit);
        }
    }

    protected virtual void GetCellAndMove(RaycastHit hit)
    {
        Cell cellPos = MoveTo(cells, hit.point);
        if (cellPos != null)//position is not taken
        {
            GenericInteraction obj = Interaction.GetCurrent();
            Vector3 buffer = new Vector3(0, obj.transform.localScale.y / 2f, 0);
            buffer += yDist;
            obj.SetSurfaceCell(cellPos);
            //needs reviewing 
            if(obj.LocalCell() != null)
                StartCoroutine(DelayedDrop(obj.LocalCell().interactions, obj, cellPos.Position() + buffer));
            else
                StartCoroutine(Interaction.DelayThePhysics(cellPos.Position() + yDist, obj));
        }
        else
        {
            string s = "HelloWorld";
            string newS = "";

            //reversing a string 
            for(int i = s.Length - 1; i >= 0; i--)
            {
                newS += s[i];
            }
            Debug.Log(newS);
            Debug.Log("This slot is taken!!");
        }
    }

    private IEnumerator DelayedDrop(List<GenericInteraction> interactions, GenericInteraction obj, Vector3 pos)
    {
        List<GenericInteraction> itemsAndUtensil = new List<GenericInteraction> {obj};    
        foreach (GenericInteraction inter in interactions)
            itemsAndUtensil.Add(inter);

        int i = 0;
        while(i < itemsAndUtensil.Count)
        {
            StartCoroutine(Interaction.DelayThePhysics(pos, itemsAndUtensil[i]));
            i++;
            yield return new WaitForSeconds(.3f);
        }
        yield return null;
    }

    private Cell MoveTo(Cell[,] _cells, Vector3 clickPoint)
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
        if (!cell.Taken())
        {
            cell.SetOccupied(true);
            return cell;
        }
        return null;
    }

    void PlaceCells(Transform trans)
    {
        Vector3 top = new Vector3(0, (trans.localScale.y / 2) + .02f, 0);
        float x = trans.localScale.x;
        float z = trans.localScale.z;

        cells = new Cell[(int)x, (int)z];
        for(int i = 0; i < z; i++)
        {
            for(int j = 0; j < x; j++)
            {
                Vector3 startPos = trans.position + (trans.forward * z / 2) - (trans.right * x / 2) + (trans.right / 2) - (transform.forward / 2);
                Vector3 newPos = startPos - (transform.forward * i) + (transform.right * j) + top;
                Cell c = new Cell();
                c.SetPosition(newPos);
                cells[j, i] = c;
                Instantiate(dot, newPos, dot.transform.rotation); //debug
            }
        }
    }

    //unused functions
    Vector3 GetTopPlane(Transform thisTrans, Vector3 rayPoint, float buffer)
    {
        float lowerY = thisTrans.transform.position.y - thisTrans.localScale.y / 2;
        float yScale = thisTrans.localScale.y;
        float dif = yScale - rayPoint.y + lowerY + buffer;
        SetRatio((rayPoint.y - lowerY) / yScale);
        return new Vector3(rayPoint.x, rayPoint.y + dif, rayPoint.z);
    }

    void SetRatio(float _ratio)
    {
        ratio = _ratio;
    }

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
