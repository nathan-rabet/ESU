using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon;
using UnityEngine.UI;

public class ExtincteurScript : MonoBehaviour
{
    public float ammo;
    private float MaxAmmo = 10f;
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
    private Image bar;
    

    void Start()
    {
        ammo = MaxAmmo;
        bar = GameObject.Find("/GAME/Menu/InGameHUD/Pompier Extinct/Standard/Background/Loading Bar").GetComponent<Image>();
        HUD = GameObject.Find("/GAME/Menu/InGameHUD/Pompier Extinct");
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
                if (!smoke.isPlaying)
                    smoke.Play();
                Shoot(Time.deltaTime); //Tir
                ammo -= Time.deltaTime;
                bar.fillAmount = Mathf.Lerp(bar.fillAmount, ammo / MaxAmmo, 3 * Time.deltaTime);
            }

            if (Input.GetKeyUp("mouse 0"))
               smoke.Stop();

            if (!reloading && ammo < MaxAmmo && Input.GetKeyDown(KeyCode.R))
            {
                smoke.Stop();
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
        ammo = MaxAmmo;
        reloading = false;
        canShoot = true;
        while (bar.fillAmount < 1)
            bar.fillAmount += 3 * Time.deltaTime;
    }

    public void ChangeWeapon()
    {
        HUD.SetActive(false);
        inHand = false;
        view.RPC("SyncExtincteur", RpcTarget.All, false); //Set display arme
        
        // /!\ Set Layer Anim pompier
        anim.SetLayerWeight(anim.GetLayerIndex("Pompier Extincteur"), 0f);
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
