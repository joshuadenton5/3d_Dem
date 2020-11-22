using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteract
{
    void OnLeftMouseButton(RaycastHit hit, Interaction main);
}

public class Interaction : MonoBehaviour
{
    private Transform guide;
    private IInteract interact;

    [SerializeField]
    private List<GenericInteraction> currents = new List<GenericInteraction>();
    private bool canClick = true;

    void Start()
    {
        guide = transform.Find("Guide");
    }

    void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 5f))
        {
            if (Input.GetMouseButtonDown(0) && canClick)
            {
                interact = hit.collider.GetComponent<IInteract>();
                if (interact != null)
                {
                    interact.OnLeftMouseButton(hit, this);
                    canClick = false;
                    StartCoroutine(DelayClick());
                }
            }      
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (Holding()){ OnDrop(); }
        }
    }

    private IEnumerator DelayClick()
    {
        yield return new WaitForSeconds(.6f);
        canClick = true;
    }

    public List<GenericInteraction> Currents()
    {
        List<GenericInteraction> tmp = new List<GenericInteraction>(currents);
        return tmp;
    }

    public IEnumerator OnPickUp(GenericInteraction current)
    {
        current.DisableRb();
        current.SetColliderTrigger(true);
        currents.Add(current);
        current.transform.SetParent(guide.transform);
        yield return Motion.PickUp(current.transform, .3f);
    }

    public IEnumerator OnPutDown(GenericInteraction current)
    {
        currents.Remove(current);
        current.transform.SetParent(null);
        yield return Motion.PutDown(current.transform, current.Destination() + current.AddedHeight(), .3f);
        current.EnableRb();
        current.SetColliderTrigger(false);
    }

    public IEnumerator ArcMotionPickUp(GenericInteraction current)
    {
        current.DisableRb();
        current.SetColliderTrigger(true);
        currents.Add(current);
        current.transform.SetParent(guide.transform);
        yield return Motion.ArcPickUp(current.transform, current, .55f);
    }

    public IEnumerator ArcMotionPutDown(GenericInteraction current)
    {
        Vector3 buffer = new Vector3(0, current.transform.localScale.y / 2f, 0);
        currents.Remove(current);
        current.transform.SetParent(null);
        yield return Motion.ArcPutDown(current.transform, current.Destination() + buffer, .55f);
        current.EnableRb();
        current.SetColliderTrigger(false);
    }
 
    public void OnDrop()
    {
        foreach(GenericInteraction i in currents)
        {
            i.EnableRb();
            i.SetColliderTrigger(false);
            i.SetParent(null);
            i.transform.SetParent(null);
        }
        currents.Clear();
    }

    public bool Holding()
    {
        if (currents.Count > 0) return true;
        return false;
    }
}
