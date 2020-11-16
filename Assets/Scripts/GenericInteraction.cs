using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericInteraction : MonoBehaviour,IInteract, IEquatable<GenericInteraction>
{
    private new Collider collider;
    private Rigidbody rb;
    [SerializeField]
    private Cell surfaceCell;

    protected Interaction interaction;
    public int genericCookTime = 30;

    [SerializeField]
    private Utensil parent;
    private Vector3 destination;
    private Vector3 addedHeight;

    public virtual void Start()
    {
        addedHeight = new Vector3(0, transform.localScale.y / 1.5f, 0);
        interaction = FindObjectOfType<Interaction>();
        collider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }
    public Vector3 AddedHeight() {
        Vector3 extra = addedHeight;
        return extra;
    }
    public Vector3 Destination() { return destination; }
    public void SetDesination(Vector3 _destination) { destination = _destination; }

    public void SetCell(Cell cell) { surfaceCell = cell; }

    public virtual void CheckForParent()
    {
        if (parent != null)
        {
            parent.RemoveFromLocal(this);
            parent = null;
        }      
    }
    public virtual void CheckCell()
    {
        if (surfaceCell != null)
        {
            surfaceCell.SetOccupied(false);
        }
    } 

    public virtual void SetParent(Utensil utensil) { parent = utensil; }

    protected virtual void NothingInHand()
    {
        CheckForParent();
        CheckCell();
        StartCoroutine(interaction.OnPickUp(this));
    }

    public virtual void OnLeftMouseButton(RaycastHit hit)
    {
        if (!interaction.Holding()) //not holding an object
        {
            NothingInHand();
        }
        else
        {
            CheckForUtensil();
        }
    }

    public virtual void CheckForUtensil()
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

    public bool Equals(GenericInteraction other)
    {
        return other==this;
    }
}
