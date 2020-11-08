using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IInteract
{
    void OnLeftMouseButton(RaycastHit hit);
}

public class Interaction : MonoBehaviour
{
    public static Guide guide;
    private IInteract interact;

    [SerializeField]
    private List<GenericInteraction> currents = new List<GenericInteraction>();
    private bool canClick = true;

    void Start()
    {
        guide = GetComponentInChildren<Guide>();
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
                    interact.OnLeftMouseButton(hit);
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

    public List<GenericInteraction> Currents() { return currents; }

    public IEnumerator OnPickUp(GenericInteraction current)
    {
        current.DisableRb();
        current.SetColliderTrigger(true);
        current.SurfaceCellTaken(false);
        current.SetSurfaceCell(null);
        currents.Add(current);
        current.transform.SetParent(guide.transform);
        yield return PickUp(current, 6f);
    }

    public IEnumerator OnPutDown(GenericInteraction current, Cell pos, Vector3 buff)
    {       
        current.SetSurfaceCell(pos);
        currents.Remove(current);
        current.transform.SetParent(null);
        yield return PutDown(current.transform, pos.Position() + buff, 6f);
        current.EnableRb();
        current.SetColliderTrigger(false);
    }

    public static IEnumerator PickUp(GenericInteraction fromPos, float vel) 
    {
        float counter = 0;
        fromPos.transform.rotation = guide.transform.rotation;
        Vector3 startPos = fromPos.transform.position;
        float distance = Vector3.Distance(guide.transform.position, startPos); //distance - speed=distance/time
        float time = distance / vel;
        while (counter < time)
        {
            counter += Time.deltaTime;
            fromPos.transform.position = Vector3.Slerp(startPos, guide.transform.position, counter / time); //guide - so the object will always end up in the same position
            //update toPos in case its changed 
            yield return null;
        }
        yield return new WaitForSeconds(.01f);
    }

    public static IEnumerator PutDown(Transform fromPos, Vector3 toPos, float vel) 
    {
        float counter = 0;
        Quaternion q = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
        Vector3 startPos = fromPos.position;
        float distance = Vector3.Distance(fromPos.position, toPos); //distance - vel=distance/time
        float time = distance / vel;
        while (counter < time)
        {
            counter += Time.deltaTime;
            fromPos.position = Vector3.Slerp(startPos, toPos, counter / time);
            fromPos.rotation = Quaternion.Slerp(fromPos.rotation, q, counter / time);
            yield return null;
        }
        yield return new WaitForSeconds(.1f);
        //newColider = false;
        //isMoving = false;
    }

    public static IEnumerator Rotate(Transform fromPos, float dur)
    {
        float counter = 0;
        Quaternion q = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
        while(counter < dur)
        {
            counter += Time.deltaTime;
            fromPos.rotation = Quaternion.Slerp(fromPos.rotation, q, counter / dur);
            yield return null;
        }
        yield return new WaitForSeconds(.1f);
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
