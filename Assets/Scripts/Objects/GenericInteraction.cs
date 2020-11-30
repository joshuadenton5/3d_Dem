using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericInteraction : MonoBehaviour,IInteract, IEquatable<GenericInteraction>
{
    private new Collider collider;
    private Rigidbody rb;
    [SerializeField]
    private DynamicCell surfaceCell;
    [SerializeField]
    protected List<GenericInteraction> localInteractions = new List<GenericInteraction>(); //objects in utensil 

    public int genericCookTime = 30;

    [SerializeField]
    private GenericInteraction parent;
    private Vector3 destination;
    private Vector3 addedHeight;

    public virtual void Start()
    {
        addedHeight = new Vector3(0, transform.localScale.y / 1.5f, 0);
        collider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }
    public Vector3 AddedHeight() {
        Vector3 extra = addedHeight;
        return extra;
    }
    public Vector3 Destination() { return destination; }

    public void SetDesination(Vector3 _destination) { destination = _destination; }

    public void SetCell(DynamicCell cell) { surfaceCell = cell; }

    public DynamicCell GetCell() { return surfaceCell; }

    public List<GenericInteraction> LocalInteractions()
    {
        List<GenericInteraction> tmp = new List<GenericInteraction>(localInteractions);
        return tmp;
    }

    public void AddToLocal(GenericInteraction interaction)
    {
        if (interaction != null)
            localInteractions.Add(interaction);
    }
    public void RemoveFromLocal(GenericInteraction interaction)
    {
        if (interaction != null)
            localInteractions.Remove(interaction);
    }

    protected virtual void CheckForParent()
    {
        if (parent != null)
        {
            parent.RemoveFromLocal(this);
            parent = null;
        }      
    }
    protected virtual void CheckCell()
    {
        if (surfaceCell != null)
        {
            surfaceCell.SetTaken(false);
        }
    } 

    public virtual void SetParent(GenericInteraction current) { parent = current; }

    protected virtual void DefaultInteraction(Interaction main)
    {
        CheckForParent();
        CheckCell();
        StartCoroutine(main.OnPickUp(this));
    }

    public virtual void OnLeftMouseButton(RaycastHit hit, Interaction main)
    {
        if (!main.Holding()) //not holding an object
        {
            DefaultInteraction(main);
        }
        else//holding
        {
            CheckCanPlace(hit, main);
        }
    }

    protected virtual void CheckCanPlace(RaycastHit hit, Interaction main)
    {
        Debug.Log("Can't do that");
    } 

    protected IEnumerator DelayedPickUp(List<GenericInteraction> interactions, Interaction main)
    {
        StartCoroutine(main.OnPickUp(interactions[0]));
        for (int i = 1; i < interactions.Count; i++)
        {
            StartCoroutine(main.ArcMotionPickUp(interactions[i]));
            yield return null;
        }
    }

    protected DynamicCell GetPosition(DynamicCell[,] dynamicCells, Vector3 hitPoint)
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

    protected void AssignInteraction(GenericInteraction interaction, DynamicCell cell)
    {
        interaction.SetDesination(cell.transform.position);
        interaction.SetCell(cell);
    }

    protected IEnumerator DelayedDrop(List<GenericInteraction> interactions, DynamicCell cell, Interaction main) //experimental function that drops items in an ordered fashion
    {
        List<GenericInteraction> tempList = new List<GenericInteraction>(interactions); //creating a temp list as 'interactions' is modified in the 'OnPutDown' function 
        AssignInteraction(tempList[0], cell);
        StartCoroutine(main.OnPutDown(tempList[0]));//placing the first element down 
        for (int i = 1; i < tempList.Count; i++)//then placing the other items stored 
        {
            tempList[i].SetDesination(tempList[i].GetCell().transform.position);
            StartCoroutine(main.ArcMotionPutDown(tempList[i]));
            yield return null;
        }
        yield return null;
    }

    public virtual bool CellAvailable() { return false; }

    protected DynamicCell[,] InitialiseCells(Transform trans, GameObject dot, int mult) //spawning the cells on load, still working on this one .....
    {
        int decX = GetDec(trans.localScale.x);
        int decZ = GetDec(trans.localScale.z);
        Vector3 top = new Vector3(0, (trans.localScale.y / 2) + .02f, 0);
        int x = Mathf.CeilToInt(trans.localScale.x) * mult;
        int z = Mathf.CeilToInt(trans.localScale.z) * mult;
        DynamicCell[,] cells = new DynamicCell[x, z];

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
        return cells;
    }

    int GetDec(float input)
    {
        if (input % 1 == 0)
            return 1;
        return 2;
    }

    public void SetColliderTrigger(bool set)
    {
        collider.isTrigger = set;
    }

    public void DisableRb()
    {
        rb.useGravity = false;
        rb.isKinematic = true;
    }

    public void EnableRb()
    {
        rb.useGravity = true;
        rb.isKinematic = false;
    }

    public bool Equals(GenericInteraction other)
    {
        return other==this;
    }

}
