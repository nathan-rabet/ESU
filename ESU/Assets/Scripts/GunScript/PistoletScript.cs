using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon;

public class PistoletScript : MonoBehaviour
{
    public int damage = 10;
    public float range = 100f;
    public GameObject mainCam;
    PhotonView view;


    void Start()
    {
        view = GetComponent<PhotonView> (); //Cherche la vue
        mainCam = GameObject.FindWithTag("MainCamera"); //Cherche camera
    }
    void Update()
    {
        if (Input.GetKeyDown("mouse 0")) //Si clic gauche (ajout: du recul, temps entre les tirs et munition)
        {
            Shoot(); //Tir
        }
    }

    private void Shoot()
    {
        RaycastHit hit; //Set de la variable de type RaycastHit(donné sur l'object toucher)
        if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, range)) //Envoi du tir
        {
            if (hit.transform.gameObject.tag == "Player") //Si l'objet touché est un joueur
            {
                view.RPC("dealDammage", RpcTarget.Others,hit.transform.gameObject.GetComponent<PhotonView>().ViewID, damage, "Jacky Tuning"); //Envoi des dégâts
            }
        }
    }
}
