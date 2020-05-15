using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon;

public class ExtincteurScript : MonoBehaviour
{
    public float ammo = 100;
    public bool inHand = false;
    private bool reloading = false;
    private bool canShoot = true;

    private ExtTriggerScript ETS;
    private ParticleSystem smoke;
    private GameObject inHandExt;
    private GameObject stackExt;
    private Animator anim;
    public GameObject HUD;
    PhotonView view;
    private GameManagerScript ManagerScript;

    void Start()
    {
        view = GetComponent<PhotonView>(); //Cherche la vue
        anim = GetComponent<Animator>(); //Cherche Animator
        ManagerScript = GameObject.Find("/GAME/GameManager").GetComponent<GameManagerScript>();

        smoke = transform.Find("Model/mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm/mixamorig:RightHand/mixamorig:RightHandIndex1/mixamorig:RightHandIndex2/InHandExtincteur/FuméeExtincteur").GetComponent<ParticleSystem>();
        inHandExt = transform.Find("Model/mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm/mixamorig:RightHand/mixamorig:RightHandIndex1/mixamorig:RightHandIndex2/InHandExtincteur").gameObject;
        stackExt = transform.Find("Model/mixamorig:Hips/mixamorig:Spine/StackExtincteur").gameObject;
        ETS = transform.Find("Model/mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm/mixamorig:RightHand/mixamorig:RightHandIndex1/mixamorig:RightHandIndex2/InHandExtincteur/SmokeHitTrigger").GetComponent<ExtTriggerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (view.IsMine && inHand && ManagerScript.StadeGame == "INGAME")
        {

            if (canShoot && ammo > 0f && Input.GetKey("mouse 0")) //Si clic gauche (ajout: du recul, temps entre les tirs et munition)
            {
                smoke.Play();
                Shoot(Time.deltaTime); //Tir
                ammo -= Time.deltaTime;
            }

            if (Input.GetKeyUp("mouse 0"))
               smoke.Stop();

            if (!reloading && ammo < 100f && Input.GetKeyDown(KeyCode.R))
            {
                reloading = true;
                canShoot = false;
                StartCoroutine(reloadingIE(3));
            }
        }
    }

    private void Shoot(float damage)
    {
        foreach (GameObject player in ETS.Hits)
        {
            player.GetComponent<PhotonView>().RPC("SetFire", RpcTarget.All, -damage * 3); //Envoi des dégâts
        }
    }

    IEnumerator reloadingIE(int reloadtime)
    {
        yield return new WaitForSeconds(reloadtime);
        ammo = 100f;
        reloading = false;
        canShoot = true;
    }

    public void ChangeWeapon()
    {
        //HUD.SetActive(false);
        inHand = false;
        view.RPC("SyncExtincteur", RpcTarget.All, false); //Set display arme

        // /!\ Set Layer Anim pompier
    }

    [PunRPC]
    public void SyncExtincteur(bool in_hand)
    {
        if (in_hand)
        {
            inHandExt.SetActive(true);
            stackExt.SetActive(false);
        }
        else
        {
            inHandExt.SetActive(false);
            stackExt.SetActive(true);
        }
    }
}
