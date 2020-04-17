using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon;

public class MedkitTriggerScript : MonoBehaviour
{
    private bool canHeal = false;
    public GameObject medKit;
    void OnTriggerEnter(Collider other) {
         if (canHeal && other.tag == "Player") 
         {
            PhotonView view = other.GetComponent<PhotonView> ();
            view.RPC("healing", RpcTarget.All, view.ViewID, 20);
            Destroy(medKit);
         }
     }

    private void Start()
    {
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
        {

            yield return new WaitForSeconds(0.1f);
            canHeal = true;
        }
}
