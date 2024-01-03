using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerInteractions : MonoBehaviour
{
    [SerializeField] float Radius;
    [SerializeField] LayerMask Interactable;
    [SerializeField] Transform InteractionPoint;

    private readonly Collider[] colliders = new Collider[3];
    [SerializeField] private int numFound;

    [SerializeField] private InteractionPromtUI IPUI;

    private Interactable interactable;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        numFound = Physics.OverlapSphereNonAlloc(InteractionPoint.position, Radius, colliders, Interactable);
        if(numFound > 0)
        {
            interactable = colliders[0].GetComponent<Interactable>();
            if(interactable != null)
            {
                if (!IPUI.IsDisplayed) IPUI.SetUp(interactable.interactionPromt);
                if (Input.GetKeyDown(KeyCode.F)) interactable.Interact(this);
            }    
        }
        else
        {
            if (Interactable != null) interactable = null;
            if (IPUI.IsDisplayed) IPUI.Close();
        }
    }
}
