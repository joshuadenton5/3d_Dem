using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : GenericInteraction
{
    private Cell[,] cells;
    public GameObject dot;

    public override void Start()
    {
        base.Start();
        PlaceCells(transform, 3);
    }

    public override void OnLeftMouseButton(RaycastHit hit, Interaction main)
    {
        base.OnLeftMouseButton(hit, main);
    }
    public override void CheckForOther()
    {

    }
    public override void CheckForParent(){}

    void PlaceCells(Transform trans, int mult) //spawning the cells on load, still working on this one .....
    {
        Vector3 top = new Vector3(0, (trans.localScale.y / 2) + .02f, 0);
        int x = Mathf.CeilToInt(trans.localScale.x) * mult;
        int z = Mathf.CeilToInt(trans.localScale.z) * mult;
        Debug.Log(trans.localScale.x);

        float _x = trans.localScale.x * mult;
        float _z = trans.localScale.z * mult;

        float flotX = trans.localScale.x * mult;
        float flotZ = trans.localScale.z * mult;

        int ratX = x / mult;
        int ratZ = z / mult;

        cells = new Cell[x, z];
        for (int i = 0; i < _z; i++)
        {
            for (int j = 0; j < _x; j++)
            {
                Vector3 startPos = trans.position + (trans.forward/mult * _z ) - (trans.right/mult * _x ) + (trans.right/mult ) - (trans.forward/mult ); //starting at the top right of the object
                Vector3 newPos = startPos - (trans.forward/mult * i) + (trans.right/mult * j) + top; //moving according to dimensions 
                Cell c = new Cell();
                c.SetPosition(newPos);
                cells[j, i] = c;
                GameObject obj = Instantiate(dot, newPos, dot.transform.rotation); //debug
                obj.transform.SetParent(trans);
            }
        }
    }
}
