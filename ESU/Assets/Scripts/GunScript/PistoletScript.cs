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
    public ParticleSystem muzzleFlash;
    public ParticleSystem hitPlayer;
    public ParticleSystem hitGround;
    public GameObject inHandGun;
    public GameObject stackGun;
    private Animator anim;
    PhotonView view;


    void Start()
    {
        view = GetComponent<PhotonView> (); //Cherche la vue
        mainCam = GameObject.FindWithTag("MainCamera"); //Cherche camera
        anim = GetComponent<Animator>(); //Cherche Animator
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
                Instantiate(hitPlayer, hit.point, new Quaternion(0,0,0,0)); //Spawn Particule
                view.RPC("SyncParticules", RpcTarget.Others, 0, hit.point); //Envoie aux autres

                hit.transform.gameObject.GetComponent<PhotonView>().RPC("dealDammage", RpcTarget.Others, hit.transform.gameObject.GetComponent<PhotonView>().ViewID, damage, PhotonNetwork.NickName); //Envoi des dégâts
            }
            else
            {
                Instantiate(hitGround, hit.point, new Quaternion(0,0,0,0)); //Spawn Particule
                view.RPC("SyncParticules", RpcTarget.Others, 1, hit.point); //Envoie aux autres
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

    [PunRPC]
    public void SyncParticules(int type, Vector3 hitpoint)
    {
        switch (type)
        {
            case 0:
                Instantiate(hitPlayer, hitpoint, new Quaternion(0,0,0,0));
                break;
            case 1:
                Instantiate(hitGround, hitpoint, new Quaternion(0,0,0,0));
                break;
        }
    }
}
