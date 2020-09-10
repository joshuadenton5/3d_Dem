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
    public static Transform guide;
    //public Image retical;
    IInteract interact;
    private static bool isHolding;

    void Start()
    {
        guide = transform.Find("Guide");
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

    public static IEnumerator OnPickUp(Transform fromPos)
    {
        yield return PickUp(fromPos, .2f);
    }

    public static IEnumerator PickUp(Transform fromPos, float dur) 
    {
        float counter = 0;
        fromPos.rotation = guide.rotation;
        Vector3 startPos = fromPos.position;
        while (counter < dur)
        {
            counter += Time.deltaTime;
            fromPos.position = Vector3.Lerp(startPos, guide.position, counter / dur); //guide - so the object will always end up in the same position
            //update toPos in case its changed 
            yield return null;
        }
        fromPos.SetParent(guide);
        isHolding = true;
        yield return new WaitForSeconds(.01f);
    }

    public static IEnumerator PutDown(Transform fromPos, Vector3 toPos, float dur) 
    {
        //isMoving = true;
        isHolding = false;
        float counter = 0;
        Vector3 startPos = fromPos.position;
        while (counter < dur)
        {
            counter += Time.deltaTime;
            fromPos.position = Vector3.Lerp(startPos, toPos, counter / dur);
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

    public static IEnumerator DelayThePhysics(Vector3 pos, GenericInteraction obj)
    {
        obj.transform.SetParent(null);
        yield return Rotate(obj.transform, .05f);
        yield return PutDown(obj.transform, pos, .2f);
        obj.SetColliderTrigger(false);
        obj.EnableRb();
    }

    public static IEnumerator FollowParent()
    {
        yield return null;
    }

    public void OnDrop()
    {
        isHolding = false;
        GenericInteraction obj = GetCurrent();
        obj.EnableRb();
        obj.SetColliderTrigger(false);
        obj.transform.SetParent(null);
    }

    public static void SetCurrent()
    {

    }

    public static bool IsHolding()
    {
        GenericInteraction obj = guide.GetComponentInChildren<GenericInteraction>();
        if (obj != null)
            return true;
        return false;
    }

    public static bool Holding()
    {
        return isHolding;
    }

    public static GenericInteraction GetCurrent()
    {
        GenericInteraction obj = guide.GetComponentInChildren<GenericInteraction>();
        if(obj != null)
        {           
            return obj;
        }
        return null;
    }

    public static Transform GetGuide()
    {
        return guide;
    }
}
