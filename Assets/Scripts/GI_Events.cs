using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GenericInteraction))]
public class GI_Events : MonoBehaviour
{
    private Interaction interaction;
    public delegate void OnSee(Color val);
    public event OnSee OnLook;

    void Start()
    {
        interaction = FindObjectOfType<Interaction>();
    }

    private void OnMouseEnter()
    {
        if (interaction.Holding())
            OnLook(Color.red);
        else
            OnLook(Color.green);
    }

    private void OnMouseExit()
    {
        OnLook(Color.grey);
    }
}
