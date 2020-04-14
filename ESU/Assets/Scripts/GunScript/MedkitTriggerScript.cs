using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon;

public class MedkitTriggerScript : MonoBehaviour
{
    public GameObject medKit;
    void OnTriggerEnter(Collider other) {
         if (other.tag == "Player") 
         {
            PhotonView view = other.GetComponent<PhotonView> ();
            view.RPC("healing", RpcTarget.All, view.ViewID, 20);
            Destroy(medKit);
         }
     }
}
