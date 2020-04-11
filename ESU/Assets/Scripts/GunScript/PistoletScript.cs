using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon;

public class PistoletScript : MonoBehaviour
{

    // UMP 47 SCRIPT //


    public bool inHand = true;
    public int damage = 10;
    public float range = 100f;
    private int ammo = 20;
    private bool canShoot = true;
    public GameObject mainCam;
    private GameObject inHandGun;
    private GameObject stackGun;
    private Animator anim;
    PhotonView view;


    void Start()
    {
        view = GetComponent<PhotonView> (); //Cherche la vue
        mainCam = GameObject.FindWithTag("MainCamera"); //Cherche camera
        anim = GetComponent<Animator>(); //Cherche Animator

        inHandGun = transform.Find("Model/mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm/mixamorig:RightHand/mixamorig:RightHandIndex1/InHand_ump47").gameObject;
        stackGun = transform.Find("Model/mixamorig:Hips/mixamorig:Spine/Stack_ump47").gameObject;
    }
    void Update()
    {
        if (view.IsMine && inHand)
        {
            inHandGun.SetActive(true);
            stackGun.SetActive(false);
            anim.SetLayerWeight(anim.GetLayerIndex("Gun Pose"), 1f); //Set du layer de visé a true


            if (canShoot && ammo>0 && Input.GetKeyDown("mouse 0")) //Si clic gauche (ajout: du recul, temps entre les tirs et munition)
            {
                Shoot(); //Tir
                ammo--;
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                canShoot = false;
            }

            if (Input.GetKeyDown("mouse 1")) //Si clic droit ZOOM and ANIM
            {
                mainCam.GetComponent<CameraCollision>().Scope(1f, 5f);
                anim.SetLayerWeight(anim.GetLayerIndex("Rifle Up"), 1f);
            }
            if (Input.GetKeyUp("mouse 1")) //Si non clic droit ZOOM and ANIM
            {
                mainCam.GetComponent<CameraCollision>().Scope(2.5f, 5f);
                anim.SetLayerWeight(anim.GetLayerIndex("Rifle Up"), 0f);
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
                view.RPC("dealDammage", RpcTarget.Others, view.ViewID, damage, PhotonNetwork.NickName); //Envoi des dégâts
            }
        }
    }

    //Function de inHandfalse
    public void ChangeWeapon()
    {
        anim.SetLayerWeight(anim.GetLayerIndex("Gun Pose"), 0f);
        
        inHand = false;
        mainCam.GetComponent<CameraCollision>().Scope(2.5f, 5f);

        inHandGun.SetActive(false);
        stackGun.SetActive(true);
    }
}
