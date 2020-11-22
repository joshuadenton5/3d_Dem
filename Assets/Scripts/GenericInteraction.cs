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

    protected virtual void NothingInHand(Interaction main)
    {
        CheckForParent();
        CheckCell();
        StartCoroutine(main.OnPickUp(this));
    }

    public virtual void OnLeftMouseButton(RaycastHit hit, Interaction main)
    {
        if (!main.Holding()) //not holding an object
        {
            NothingInHand(main);
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

    public IEnumerator DelayedPickUp(List<GenericInteraction> interactions, Interaction main)
    {
        StartCoroutine(main.OnPickUp(interactions[0]));
        for (int i = 1; i < interactions.Count; i++)
        {
            StartCoroutine(main.ArcMotionPickUp(interactions[i]));
            yield return null;
        }
        yield return null;
    }

    public bool Equals(GenericInteraction other)
    {
        return other==this;
    }
}
