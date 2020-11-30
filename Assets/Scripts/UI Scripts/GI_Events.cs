using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GenericInteraction))]
public class GI_Events : MonoBehaviour
{
    protected Interaction interaction;
    protected GenericInteraction genericInteraction;
    public delegate void OnSee(Color val, string text);
    public event OnSee OnLook;

    protected virtual void Start()
    {
        interaction = FindObjectOfType<Interaction>();
        genericInteraction = GetComponent<GenericInteraction>();
    }

    protected virtual void IsHolding()
    {
        OnLook(Color.red, name);
    }

    protected virtual void NotHolding()
    {
        SelectColour("G");
    }

    protected void SelectColour(string letter)
    {
        switch (letter)
        {
            case "G":
                OnLook(Color.green, name);
                break;
            case "R":
                OnLook(Color.red, name);
                break;
            default:
                OnLook(Color.grey, "");
                break;
        }
    }

    protected virtual void OnMouseEnter()
    {
        if (interaction.Holding())
        {
            IsHolding();
        }
        else
        {
            NotHolding();
        }
    }

    protected virtual void OnMouseExit()
    {
        SelectColour(null);
    }
}
