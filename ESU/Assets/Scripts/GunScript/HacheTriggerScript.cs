using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HacheTriggerScript : MonoBehaviour
{
    public List<GameObject> playersHit = new List<GameObject>();

    void OnTriggerEnter(Collider other) {
         if (other.tag == "Player") 
         {
             playersHit.Add(other.gameObject);
         }
     }
     
     void OnTriggerExit(Collider other) {
         if (other.tag == "Player")
         {
             playersHit.Remove(other.gameObject);
         }
     }
}
