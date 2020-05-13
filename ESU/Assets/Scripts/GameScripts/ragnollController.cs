using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ragnollController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        setRigibodyState(true);
        setColliderState(false);
    }
    
    public void die() // Active le ragnoll si le perso meurt
    {
        GetComponent<Animator>().enabled = false;
        setRigibodyState(false);
        setColliderState(true);
    }

    void setRigibodyState(bool state) // Set tout les rigibody du perso en fonction de state
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = state;
        }

        GetComponent<Rigidbody>().isKinematic = !state;
    }
    
    void setColliderState(bool state) // Set tout les colliders du perso en fonction de state
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = state;
        }
        GetComponent<Collider>().enabled = !state;
    }
}
