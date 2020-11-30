using UnityEngine;

public class Utensil : GenericInteraction
{
    private Vector3 yDist;
    private DynamicCell localCell;

    public override void Start()
    {
        base.Start();
        yDist = new Vector3(0, .15f, 0);
        localInteractions.Add(this);
        localCell = gameObject.AddComponent<DynamicCell>();
    }    

    override public void OnLeftMouseButton(RaycastHit hit, Interaction main)
    {
        base.OnLeftMouseButton(hit, main);
    }

    protected override void DefaultInteraction(Interaction main)
    {
        if (localInteractions.Count > 1)
        {
            base.CheckCell();
            StartCoroutine(DelayedPickUp(localInteractions, main));
        }
        else //only one object in hand
        {
            base.DefaultInteraction(main);
        }
    }

    protected override void CheckForParent(){}
    
    protected override void CheckCanPlace(RaycastHit hit, Interaction main)
    {
        if (main.Currents().Count > 1 || main.Currents()[0].CompareTag(tag))
        {
            Debug.Log("Not Allowed!");
        }
        else
        {
            GenericInteraction current = main.Currents()[0];//will always be this as the first element 
            current.SetDesination(transform.position + yDist);
            current.SetCell(localCell);
            current.SetParent(this);
            localInteractions.Add(current);
            StartCoroutine(main.OnPutDown(current));
        }
    }  
}
