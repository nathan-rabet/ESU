using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon;

public class PistoletScript : MonoBehaviour
{
    public bool inHand = true;
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
        if (inHand)
        {
            if (Input.GetKeyDown("mouse 0")) //Si clic gauche (ajout: du recul, temps entre les tirs et munition)
            {
                Shoot(); //Tir
            }
            if (Input.GetKeyDown("mouse 1"))
            {
                mainCam.GetComponent<CameraCollision>().Scope(1f, 5f);
            }
            if (Input.GetKeyUp("mouse 1")) 
            {
                mainCam.GetComponent<CameraCollision>().Scope(2.5f, 5f);
            }
        }
    }

    //Function de tir
    private void Shoot()
    {
        RaycastHit hit; //Set de la variable de type RaycastHit(donné sur l'object toucher)
        if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, range)) //Envoi du tir
        {
            if (hit.transform.gameObject.tag == "Player") //Si l'objet touché est un joueur
            {
                view.RPC("dealDammage", RpcTarget.Others,hit.transform.gameObject.GetComponent<PhotonView>().ViewID, damage, PhotonNetwork.NickName); //Envoi des dégâts
            }
        }
    }

    //Function de inHandfalse
    public void ChangeWeapon()
    {
        inHand = false;
        mainCam.GetComponent<CameraCollision>().Scope(2.5f, 5f);
    }
}
