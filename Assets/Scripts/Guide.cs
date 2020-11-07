using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guide : MonoBehaviour
{
    [SerializeField]
    private GenericInteraction interaction;
    [SerializeField]
    private List<GenericInteraction> interactions = new List<GenericInteraction>();

    void Start()
    {
        
    }
    public Transform GetTransform() { return transform; }

    public void AddInteraction(GenericInteraction _interaction) { interactions.Add(_interaction); }
   
    public GenericInteraction GetInteraction() { return interaction; }

    public List<GenericInteraction> Interactions() { return interactions; }

    public void RemoveInteraction(GenericInteraction _interaction) { interactions.Remove(_interaction); }

}
