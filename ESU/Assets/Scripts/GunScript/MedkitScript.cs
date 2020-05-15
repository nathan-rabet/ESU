using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon;

public class MedkitScript : MonoBehaviour
{
    public bool inHand = true;
    private int ammo = 3;
    private bool canShoot = true;
    private bool reloading = false;
    public GameObject mainCam;
    public GameObject inHandMedkit;
    public GameObject stackMedkit;
    public GameObject Medkit;
    public GameObject MedkitSpawnPosition;
    private Animator anim;
    public GameObject HUD;
    PhotonView view;


    void Start()
    {
        view = GetComponent<PhotonView> (); //Cherche la vue
        mainCam = GameObject.FindWithTag("MainCamera"); //Cherche camera
        HUD = GameObject.Find("/GAME/Menu/InGameHUD/Medecin Info"); // Cherche Medecin HUD
    }

    // Update is called once per frame
    void Update()
    {
        if (view.IsMine && inHand)
        {
            
            if (canShoot && ammo>0 && Input.GetKey("mouse 0")) //Si clic gauche (ajout: du recul, temps entre les tirs et munition)
            {
                canShoot = false;
                Shoot(); //Tir
                ammo--;
                if (!reloading)
                {
                    reloading = true;
                    StartCoroutine(reloadingIE(1));
                }
                StartCoroutine(recoil(0.1f));
            }
        }
        
    }

    private void Shoot()
    {
        GameObject MyMedkit = PhotonNetwork.Instantiate(Medkit.name, MedkitSpawnPosition.transform.position, MedkitSpawnPosition.transform.rotation, 0);
        MyMedkit.GetComponent<Rigidbody>().AddForce(mainCam.transform.forward * 700);
    }

    //Function de inHandfalse
    public void ChangeWeapon()
    {
        HUD.SetActive(false); // Désactive le HUD
        inHand = false;
    }

    //Function de rechargement
    IEnumerator reloadingIE(int reloadtime)
        {
            //  Jouer l'anim de rechargement /!\

            
            yield return new WaitForSeconds(reloadtime);
            ammo++;
            if (ammo < 3)
                StartCoroutine(reloadingIE(1));
            else
                reloading = false;
        }

    //Function de recoil
    IEnumerator recoil(float recoiltime)
        {

            yield return new WaitForSeconds(recoiltime);
            canShoot = true;
        }
}
