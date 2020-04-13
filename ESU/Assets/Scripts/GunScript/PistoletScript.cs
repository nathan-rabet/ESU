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
    private bool reloading = false;
    private bool canShoot = true;
    public GameObject mainCam;
    private ParticleSystem muzzleFlash;
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
        muzzleFlash = transform.Find("Model/mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm/mixamorig:RightHand/mixamorig:RightHandIndex1/InHand_ump47/default/Muzzle Flash").GetComponent<ParticleSystem>();
    }
    void Update()
    {
        if (view.IsMine && inHand)
        {
            anim.SetLayerWeight(anim.GetLayerIndex("Gun Pose"), 1f); //Set du layer de visé a true


            if (canShoot && ammo>0 && Input.GetKey("mouse 0")) //Si clic gauche (ajout: du recul, temps entre les tirs et munition)
            {
                canShoot = false;
                muzzleFlash.Play();
                Shoot(); //Tir
                ammo--;
                StartCoroutine(recoil(0.2f));
            }

            if (!reloading && Input.GetKeyDown(KeyCode.R))
            {
                reloading = true;
                canShoot = false;
                StartCoroutine(reloadingIE(3));
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
                hit.transform.gameObject.GetComponent<PhotonView>().RPC("dealDammage", RpcTarget.Others, hit.transform.gameObject.GetComponent<PhotonView>().ViewID, damage, PhotonNetwork.NickName); //Envoi des dégâts
            }
        }
    }

    //Function de inHandfalse
    public void ChangeWeapon()
    {
        anim.SetLayerWeight(anim.GetLayerIndex("Gun Pose"), 0f);
        
        inHand = false;
        mainCam.GetComponent<CameraCollision>().Scope(2.5f, 5f);

        view.RPC("SyncPistolet", RpcTarget.All, false); //Set display arme
    }

    //Function de rechargement
    IEnumerator reloadingIE(int reloadtime)
        {
            //  Jouer l'anim de rechargement /!\

            
            yield return new WaitForSeconds(reloadtime);
            ammo = 20;
            reloading = false;
            canShoot = true;
        }

    //Function de recoil
    IEnumerator recoil(float recoiltime)
        {

            yield return new WaitForSeconds(recoiltime);
            canShoot = true;
        }

    [PunRPC]
    public void SyncPistolet(bool in_hand)
    {
        if (in_hand)
        {
            inHandGun.SetActive(true);
            stackGun.SetActive(false);
        }
        else
        {
            inHandGun.SetActive(false);
            stackGun.SetActive(true);
        }
    }
}
