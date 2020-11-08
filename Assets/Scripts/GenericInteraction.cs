using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericInteraction : MonoBehaviour,IInteract
{
    private new Collider collider;
    private Rigidbody rb;
    private Cell surfaceCell;

    protected Interaction interaction;
    public int genericCookTime = 30;

    [SerializeField]
    private Utensil parent;

    public virtual void Start()
    {
        interaction = FindObjectOfType<Interaction>();
        collider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }

    public virtual void CheckForParent(Utensil _parent)
    {
        if (_parent != null)
        {
            _parent.LocalCell().interactions.Remove(this);
            _parent = null;
        }
    }

    public virtual void SetParent(Utensil utensil) { parent = utensil; }

    protected virtual void NothingInHand()
    {
        CheckForParent(parent);
        StartCoroutine(interaction.OnPickUp(this));
    }

    public virtual void OnLeftMouseButton(RaycastHit hit)
    {
        if (!interaction.Holding()) //checking if an object is not being held
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

    public void SurfaceCellTaken(bool setTaken)
    {
        if(GetSurfaceCell() != null)
        {
            GetSurfaceCell().SetOccupied(setTaken);
        }
    }

    public Cell GetSurfaceCell()
    {
        return surfaceCell;
    }

    public void SetSurfaceCell(Cell _surfaceCell)
    {
        surfaceCell = _surfaceCell;
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
}
