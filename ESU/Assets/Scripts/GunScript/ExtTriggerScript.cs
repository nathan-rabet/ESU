using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtTriggerScript : MonoBehaviour
{
    public List<GameObject> Hits;
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Batiment")
        {
            Hits.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Batiment")
        {
            Hits.Remove(other.gameObject);
        }
    }
}
