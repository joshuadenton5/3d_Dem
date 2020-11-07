using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cell
{
    private Vector3 position;
    [SerializeField]
    private bool isTaken;
    
    [SerializeField]
    public List<GenericInteraction> interactions = new List<GenericInteraction>();

    public void SetPosition(Vector3 pos)
    {
        position = pos;    
    }

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

    public void RemoveInteraction(GenericInteraction interaction)
    {
        interactions.Remove(interaction);
    }
}
