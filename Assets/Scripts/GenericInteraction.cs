using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericInteraction : MonoBehaviour,IInteract
{
    private new Collider collider;
    private Rigidbody rb;
    private Cell surfaceCell;

    protected Cell localCell;
    public int genericCookTime = 30;

    public virtual void Start()
    {
        collider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void CheckHolding()
    {
        if (surfaceCell != null)
        {
            surfaceCell.SetOccupied(false);
            if (surfaceCell.interactions.Contains(this))
            {
                surfaceCell.interactions.Remove(this);
            }
            SetSurfaceCell(null);
        }
        StartCoroutine(Interaction.OnPickUp(transform));
        DisableRb();
        SetColliderTrigger(true);
        transform.SetParent(Interaction.GetGuide());
    }

    public virtual void OnLeftMouseButton(RaycastHit hit)
    {
        if (!Interaction.IsHolding()) //checking if an object is being held
        {
            CheckHolding();
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

    public bool GetHolding()
    {
        return Interaction.IsHolding();
    }

    public Cell SurfaceCell()
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

    public Cell LocalCell()
    {
        return localCell;
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
