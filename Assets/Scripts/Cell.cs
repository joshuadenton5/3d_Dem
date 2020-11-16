using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cell
{
    private Vector3 position;
    [SerializeField]
    private bool isTaken;

    GameObject parent;
    Transform transform;

    public Cell(Vector3 _position)
    {
        position = _position;
    }

    public void SetPosition(Vector3 pos) { position = pos; }

    public void SetOccupied(bool taken)
    {
        isTaken = taken;
    }

    public bool Taken()
    {
        return isTaken;
    }

    public Vector3 Position()
    {
        return position;
    }
}
