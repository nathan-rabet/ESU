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
    PhotonView view;


    void Start()
    {
        view = GetComponent<PhotonView> (); //Cherche la vue
        mainCam = GameObject.FindWithTag("MainCamera"); //Cherche camera
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
                StartCoroutine(recoil(0.2f));
            }
        }
        
    }

    private void Shoot()
    {
        PhotonNetwork.Instantiate(Medkit.name, MedkitSpawnPosition.transform.position, mainCam.transform.rotation, 0);
    }

    //Function de inHandfalse
    public void ChangeWeapon()
    {
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
