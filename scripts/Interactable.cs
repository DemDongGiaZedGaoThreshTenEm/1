using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface Interactable 
{
    public string interactionPromt { get; }
    public bool Interact(PlayerInteractions interactor);
}
