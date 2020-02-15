using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon;

public class AppelRCP : MonoBehaviour
{


    public PhotonView view;
    void Start()
    {
    }

    [PunRPC]
    void rpcDeath (int viewID, string Killer) 
    {
        Destroy(PhotonView.Find(viewID).gameObject); //Détruit le perso mort
    }

    [PunRPC]
    public void dealDammage (int viewID, int damage, string Killer)
    {
        GetComponent<Player_Manager>().TakeDamage(viewID, damage, Killer); //Envoie les dégâts aux autres
    }
}
