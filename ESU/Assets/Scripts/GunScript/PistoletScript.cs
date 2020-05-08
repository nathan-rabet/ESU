using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon;
using UnityEngine.UI;

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
    public GameObject HUD;
    PhotonView view;
    private GameManagerScript ManagerScript;

    private AmmoCount ammoCount;


    void Start()
    {
        ammoCount = GameObject.Find("/GAME/Menu/InGameHUD/Weapon Info").GetComponent<AmmoCount>();
        HUD = GameObject.Find("/GAME/Menu/InGameHUD/Weapon Info");
        view = GetComponent<PhotonView> (); //Cherche la vue
        mainCam = GameObject.FindWithTag("MainCamera"); //Cherche camera
        anim = GetComponent<Animator>(); //Cherche Animator
        ManagerScript = GameObject.Find("/GAME/GameManager").GetComponent<GameManagerScript>();
    }
    void Update()
    {
        if (view.IsMine && inHand && ManagerScript.StadeGame == "INGAME")
        {
            
            if (canShoot && ammo>0 && Input.GetKey("mouse 0")) //Si clic gauche (ajout: du recul, temps entre les tirs et munition)
            {
                canShoot = false;
                muzzleFlash.Play();
                Shoot(); //Tir
                ammo--;
                ammoCount.SetAmmo(ammo);
                StartCoroutine(recoil(0.2f));
            }

            if (!reloading && ammo<20 && Input.GetKeyDown(KeyCode.R))
            {
                ammoCount.ReloadAnim();
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
            Debug.DrawRay(mainCam.transform.position, mainCam.transform.forward * hit.distance, Color.green, 2.5f);
            switch (hit.transform.gameObject.tag)
            {
                case "Player":
                    Instantiate(hitPlayer, hit.point, new Quaternion(0,0,0,0)); //Spawn Particule
                    view.RPC("SyncParticules", RpcTarget.Others, 0, hit.point); //Envoie aux autres

                    hit.transform.gameObject.GetComponent<PhotonView>().RPC("dealDammage", RpcTarget.Others, hit.transform.gameObject.GetComponent<PhotonView>().ViewID, damage, PhotonNetwork.LocalPlayer); //Envoi des dégâts
                    break;
                case "Batiment":
                    Instantiate(hitGround, hit.point, new Quaternion(0,0,0,0)); //Spawn Particule
                    view.RPC("SyncParticules", RpcTarget.Others, 1, hit.point); //Envoie aux autres
                    hit.transform.gameObject.GetComponent<BuildingScript>().TakeDamage(hit.transform.gameObject.GetComponent<PhotonView>().ViewID, damage, PhotonNetwork.LocalPlayer); //Envoi des dégâts
                    break;
                default:
                    Instantiate(hitGround, hit.point, new Quaternion(0,0,0,0)); //Spawn Particule
                    view.RPC("SyncParticules", RpcTarget.Others, 1, hit.point); //Envoie aux autres
                    break;
            }
            if (hit.transform.gameObject.tag == "Player") //Si l'objet touché est un joueur
            {
                
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
        HUD.SetActive(false); // Desactive le HUD
        anim.SetLayerWeight(anim.GetLayerIndex("Gun Pose"), 0f);

        inHand = false;
        mainCam.GetComponent<CameraCollision>().Scope(2.5f, 5f);

        view.RPC("SyncPistolet", RpcTarget.All, false); //Set display arme
    }

    //Function de rechargement
    IEnumerator reloadingIE(int reloadtime)
        {
            //  Jouer l'anim de rechargement /!\
            anim.SetTrigger("Reload");

            
            yield return new WaitForSeconds(reloadtime);
            ammo = 20;
            ammoCount.SetAmmo(ammo);
            reloading = false;
            canShoot = true;
        }

    //Function de recoil
    IEnumerator recoil(float recoiltime)
        {

            yield return new WaitForSeconds(recoiltime);
            if (!reloading)
            {
                canShoot = true;
            }
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
