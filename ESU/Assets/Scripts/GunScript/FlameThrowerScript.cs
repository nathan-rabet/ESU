using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon;
using UnityEngine.UI;

public class FlameThrowerScript : MonoBehaviour
{
    private float ammo;
    private float Maxammo = 10f;
    public bool inHand = false;
    private bool reloading = false;
    private bool canShoot = true;

    private ParticleSystem firetrail;
    private AudioSource audiofire;
    private GameObject inHandFT;
    private GameObject stackFT;
    private Animator anim;
    public GameObject HUD;
    PhotonView view;
    private GameManagerScript ManagerScript;
    private Image bar;

    void Start()
    {
        view = GetComponent<PhotonView>(); //Cherche la vue
        anim = GetComponent<Animator>(); //Cherche Animator
        ManagerScript = GameObject.Find("/GAME/GameManager").GetComponent<GameManagerScript>();
        HUD = GameObject.Find("/GAME/Menu/InGameHUD/Lance Flamme"); // Cherche le HUD

        ammo = Maxammo;
        bar = GameObject.Find("/GAME/Menu/InGameHUD/Lance Flamme/Standard/Background/Loading Bar").GetComponent<Image>();
        inHandFT = transform.Find("Model/mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm/mixamorig:RightHand/mixamorig:RightHandIndex1/mixamorig:RightHandIndex2/InHandFlamethrower").gameObject;
        stackFT = transform.Find("Model/mixamorig:Hips/mixamorig:Spine/StackFlamethrower").gameObject;
        firetrail = transform.Find("Model/mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm/mixamorig:RightHand/mixamorig:RightHandIndex1/mixamorig:RightHandIndex2/InHandFlamethrower/FireTrail").gameObject.GetComponent<ParticleSystem>();
        audiofire = transform.Find("Model/mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm/mixamorig:RightHand/mixamorig:RightHandIndex1/mixamorig:RightHandIndex2/InHandFlamethrower/FireTrail").gameObject.GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (view.IsMine && inHand && ManagerScript.StadeGame == "INGAME")
        {

            if (canShoot && ammo > 0f && Input.GetKey("mouse 0")) //Si clic gauche (ajout: du recul, temps entre les tirs et munition)
            {
                if (!firetrail.isPlaying)
                {
                    firetrail.Play();
                    audiofire.Play();
                    view.RPC("SyncFTPar", RpcTarget.Others, true);
                }
                ammo -= Time.deltaTime;
                bar.fillAmount = Mathf.Lerp(bar.fillAmount, ammo / Maxammo, 3 * Time.deltaTime);
            }
            else
            {
                firetrail.Stop();
                audiofire.Stop();
                view.RPC("SyncFTPar", RpcTarget.Others, false);
            }


            if (!reloading && ammo < Maxammo && Input.GetKeyDown(KeyCode.R))
            {
                firetrail.Stop();
                audiofire.Stop();
                view.RPC("SyncFTPar", RpcTarget.Others, false);
                reloading = true;
                canShoot = false;
                StartCoroutine(reloadingIE(3));
            }
        }
    }

    IEnumerator reloadingIE(int reloadtime)
    {
        yield return new WaitForSeconds(reloadtime);
        ammo = Maxammo;
        reloading = false;
        canShoot = true;
        while (bar.fillAmount < 1)
            bar.fillAmount += 3 * Time.deltaTime;
    }

    public void ChangeWeapon()
    {
        anim.SetTrigger("grap"); //Jouer l'amin grap du pistolet
        HUD.SetActive(false); // Désactive le HUD

        inHand = false;

        view.RPC("SyncFT", RpcTarget.All, false); //Set display arme
    }

    [PunRPC]
    public void SyncFT(bool in_hand)
    {
        StartCoroutine(SyncFT_IE(in_hand));
    }

    [PunRPC]
    public void SyncFTPar(bool active)
    {
        if (active)
        {
            firetrail.Play();
            audiofire.Play();
        }
        else
        {
            firetrail.Stop();
            audiofire.Stop();
        }
    }

    IEnumerator SyncFT_IE(bool in_hand)
    {
        yield return new WaitForSeconds(0.4f);
        if (in_hand)
        {
            inHandFT.SetActive(true);
            stackFT.SetActive(false);
        }
        else
        {
            inHandFT.SetActive(false);
            stackFT.SetActive(true);
            yield return new WaitForSeconds(1f);
            if (!inHand)
                anim.SetLayerWeight(anim.GetLayerIndex("Gun Pose"), 0f);
        }
    }
}
