using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCell : MonoBehaviour
{
    [SerializeField]
    private bool occupied;

    void Awake()
    {

    }

    public bool Taken()
    {
        return occupied;
    }

    public void SetTaken(bool taken)
    {
        occupied = taken;
    }
}
