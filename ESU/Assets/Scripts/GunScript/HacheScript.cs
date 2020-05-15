using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon;


public class HacheScript : MonoBehaviour
{
    public int damage = 40;
    public bool inHand = false;
    public bool canHit = true;
    private HacheTriggerScript HTS;
    private GameObject inHandAxe;
    private GameObject stackAxe;
    private Animator anim;
    PhotonView view;
    public GameObject HUD;


    void Start()
    {
        HUD = GameObject.Find("/GAME/Menu/InGameHUD/Pompier Info");
        view = GetComponent<PhotonView> (); //Cherche la vue
        anim = GetComponent<Animator>(); //Cherche Animator
        HTS = transform.Find("AxeHitTrigger").GetComponent<HacheTriggerScript>();

        inHandAxe = transform.Find("Model/mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm/mixamorig:RightHand/mixamorig:RightHandIndex1/mixamorig:RightHandIndex2/InHandAxe").gameObject;
        stackAxe = transform.Find("Model/mixamorig:Hips/mixamorig:Spine/StackAxe").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (view.IsMine && inHand)
        {
            if (canHit && Input.GetKey("mouse 0")) //Si clic gauche (ajout: du recul, temps entre les tirs et munition)
            {
                canHit = false;
                anim.SetBool("Attack", true);
                Shoot(); //Tir
                StartCoroutine(recoil(0.2f));
            }
            else
            {
                anim.SetBool("Attack", false);
            }
        }
        
    }

    private void Shoot()
    {
        foreach (GameObject player in HTS.playersHit)
        {
            player.GetComponent<PhotonView>().RPC("dealDammage", RpcTarget.All, player.GetComponent<PhotonView>().ViewID, damage, PhotonNetwork.LocalPlayer); //Envoi des dégâts
        }
        
    }

    public void ChangeWeapon()
    {
        HUD.SetActive(false);
        inHand = false;
        view.RPC("SyncHache", RpcTarget.All, false); //Set display arme

        // /!\ Set Layer Anim pompier
    }

    IEnumerator recoil(float recoiltime)
    {

        yield return new WaitForSeconds(recoiltime);
        canHit = true;
    }

    [PunRPC]
    public void SyncHache(bool in_hand)
    {
        if (in_hand)
        {
            inHandAxe.SetActive(true);
            stackAxe.SetActive(false);
        }
        else
        {
            inHandAxe.SetActive(false);
            stackAxe.SetActive(true);
        }
    }
}
