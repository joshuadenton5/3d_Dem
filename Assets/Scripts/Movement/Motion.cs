using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motion : MonoBehaviour
{
    public static Transform guide;
    private void Start()
    {
        guide = transform.Find("Guide");
    }

    public static IEnumerator PickUp(Transform fromPos, float dur)
    {
        float counter = 0;
        fromPos.rotation = guide.transform.rotation;
        Vector3 startPos = fromPos.position;
        float distance = Vector3.Distance(guide.transform.position, startPos); //distance - speed=distance/time
        float time = distance / dur;
        while (counter < dur)
        {
            counter += Time.deltaTime;
            fromPos.position = Vector3.Slerp(startPos, guide.transform.position, counter / dur); //guide - so the object will always end up in the same position
            //update toPos in case its changed 
            yield return null;
        }
        yield return new WaitForSeconds(.01f);
    }

    public static IEnumerator PutDown(Transform fromPos, Vector3 toPos, float dur)
    {
        float counter = 0;
        Quaternion q = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
        Vector3 startPos = fromPos.position;
        float distance = Vector3.Distance(fromPos.position, toPos); //distance - vel=distance/time
        float time = distance / dur;
        while (counter < dur)
        {
            counter += Time.deltaTime;
            fromPos.position = Vector3.Slerp(startPos, toPos, counter / dur);
            fromPos.rotation = Quaternion.Slerp(fromPos.rotation, q, counter / dur);
            yield return null;
        }
        yield return new WaitForSeconds(.1f);
    }

    public static IEnumerator ArcPickUp(Transform fromPos, GenericInteraction inter, float dur)
    {
        float counter = 0;
        fromPos.rotation = guide.transform.rotation;
        Vector3 start = fromPos.position;
        Vector3 toPos = inter.GetCell().transform.position;
        Vector3 arc = start + (toPos - start) / 2 + Vector3.up * .5f;
        float distance = Vector3.Distance(fromPos.position, toPos); //distance - vel=distance/time
        float time = distance / dur;
        while (counter < dur)
        {
            counter += Time.deltaTime;
            toPos = inter.GetCell().transform.position + new Vector3(0, inter.transform.localScale.y/2, 0);
            Vector3 m1 = Vector3.Lerp(start, arc, counter / dur);
            Vector3 m2 = Vector3.Lerp(arc, toPos, counter / dur);
            fromPos.position = Vector3.Lerp(m1, m2, counter / dur);
            yield return null;
        }
    }

    public static IEnumerator ArcPutDown(Transform fromPos, GenericInteraction inter, float dur)
    {
        float counter = 0;
        Quaternion q = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
        Vector3 toPos = inter.GetCell().transform.position;
        Vector3 start = fromPos.position;
        Vector3 arc = start + (toPos - start) / 2 + Vector3.up * .5f;
        float distance = Vector3.Distance(fromPos.position, toPos); //distance - vel=distance/time
        float time = distance / dur;
        while (counter < dur)
        {
            counter += Time.deltaTime;
            toPos = inter.GetCell().transform.position;
            Vector3 m1 = Vector3.Lerp(start, arc, counter / dur);
            Vector3 m2 = Vector3.Lerp(arc, toPos, counter / dur);
            fromPos.position = Vector3.Lerp(m1, m2, counter / dur);
            fromPos.rotation = Quaternion.Slerp(fromPos.rotation, q, counter / dur);
            yield return null;
        }
    }

    public static IEnumerator Rotate(Transform fromPos, float dur)
    {
        float counter = 0;
        Quaternion q = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
        while (counter < dur)
        {
            counter += Time.deltaTime;
            fromPos.rotation = Quaternion.Slerp(fromPos.rotation, q, counter / dur);
            yield return null;
        }
        yield return new WaitForSeconds(.1f);
    }
}
