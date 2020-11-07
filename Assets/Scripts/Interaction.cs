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
    public static Guide _guide;
    //public static Transform guide;
    private IInteract interact;
    private static bool isHolding;
    private GenericInteraction current;

    void Start()
    {
        //guide = transform.Find("Guide");
        _guide = GetComponentInChildren<Guide>();
        //retical.color = Color.white;
    }

    void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 5f))
        {
            if (Input.GetMouseButtonDown(0))
            {
                interact = hit.collider.GetComponent<IInteract>();
                if (interact != null)
                {
                    interact.OnLeftMouseButton(hit);
                    //Debug.Log(interact);
                }
            }      
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (Holding())
            {
                OnDrop();
            }
        }
    }

    public static IEnumerator OnPickUp(GenericInteraction fromPos)
    {
        _guide.AddInteraction(fromPos);
        yield return PickUp(fromPos, .2f);
    }

    public static IEnumerator PickUp(GenericInteraction fromPos, float dur) 
    {
        float counter = 0;
        //fromPos.rotation = guide.rotation;
        fromPos.transform.rotation = _guide.GetTransform().rotation;
        Vector3 startPos = fromPos.transform.position;
        while (counter < dur)
        {
            counter += Time.deltaTime;
            fromPos.transform.position = Vector3.Lerp(startPos, _guide.GetTransform().position, counter / dur); //guide - so the object will always end up in the same position
            //update toPos in case its changed 
            yield return null;
        }
        fromPos.transform.SetParent(_guide.GetTransform());
        //_guide.AddInteraction(fromPos);
        isHolding = true;
        yield return new WaitForSeconds(.01f);
    }

    public static IEnumerator PutDown(Transform fromPos, Vector3 toPos, float dur) 
    {
        //isMoving = true;
        isHolding = false;
        float counter = 0;
        Quaternion q = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
        Vector3 startPos = fromPos.position;
        while (counter < dur)
        {
            counter += Time.deltaTime;
            fromPos.position = Vector3.Lerp(startPos, toPos, counter / dur);
            fromPos.rotation = Quaternion.Slerp(fromPos.rotation, q, counter / dur);
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

    public static IEnumerator DelayThePhysics(Vector3 pos, List<GenericInteraction> objs)
    {
        foreach(GenericInteraction i in objs)
        {
            i.transform.SetParent(null);
            //yield return Rotate(obj.transform, .01f);
            yield return PutDown(i.transform, pos, .2f);
            i.SetColliderTrigger(false);
            i.EnableRb();
        }
        GetCurrent().Clear();
    }

    public void OnDrop()
    {
        isHolding = false;
        foreach(GenericInteraction i in GetCurrent())
        {
            i.EnableRb();
            i.SetColliderTrigger(false);
            i.transform.SetParent(null);
        }
        GetCurrent().Clear();
    }

    public static bool Holding()
    {
        return isHolding;
    }

    public static List<GenericInteraction> GetCurrent()
    {
        return _guide.Interactions();
    }

    public static Guide GetGuide()
    {
        return _guide;
    }
}
