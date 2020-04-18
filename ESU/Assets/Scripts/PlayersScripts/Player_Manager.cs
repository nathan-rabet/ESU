using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Animations;
using Photon;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using TMPro;

public class Player_Manager : MonoBehaviour
{
    public enum Classe
    {
        Policier = 0,
        Pompier = 1,
        Medecin = 2,
        Mercenaire = 3,
        Pyroman = 4,
        Drogueur = 5,
    }
    public enum Armes
    {
        Hand = 0,
        Pistolet = 1,
        Hache = 2,
        Extincteur = 3,
        Medpack = 4,
        LanceFlamme = 5
    }
    
    public PhotonView view;
    public Classe myClass;
    public int health;
    public int maxhealth;
    
    
    
    private List<Armes> weaponsInventory;
    private int weaponsInventoryLength;
    private int selectedWeapon = 0;
    
    private Animator anim;
    
    private healthbar healthBar;
    public GameObject gamemanager;
    public GameObject HealingPrefab;

    void Start()
    {
        //Set des variables
        gamemanager = GameObject.Find("/GAME/GameManager");
        healthBar = GameObject.Find("/GAME/Menu/InGameHUD/Health Bar").GetComponent<healthbar>();
        
        anim = GetComponent<Animator>();
        anim.SetLayerWeight(anim.GetLayerIndex("Gun Pose"), 0f);
        anim.SetLayerWeight(anim.GetLayerIndex("Pompier"), 0f);
        
        weaponsInventory = new List<Armes>();

        //Donne les caractéristiques de la classe
        switch(myClass)
        {
            case Classe.Policier:
                maxhealth = 100;
                weaponsInventory.Add(Armes.Hand);
                weaponsInventory.Add(Armes.Pistolet);
                break;
            case Classe.Pompier:
                maxhealth = 100;
                weaponsInventory.Add(Armes.Hand);
                weaponsInventory.Add(Armes.Hache);
                weaponsInventory.Add(Armes.Extincteur);
                break;
            case Classe.Medecin:
                maxhealth = 100;
                weaponsInventory.Add(Armes.Hand);
                weaponsInventory.Add(Armes.Medpack);
                break;
            case Classe.Mercenaire:
                maxhealth = 100;
                weaponsInventory.Add(Armes.Hand);
                weaponsInventory.Add(Armes.Pistolet);
                break;
            case Classe.Pyroman:
                maxhealth = 100;
                weaponsInventory.Add(Armes.Hand);
                weaponsInventory.Add(Armes.LanceFlamme);
                break;
            case Classe.Drogueur:
                maxhealth = 100;
                weaponsInventory.Add(Armes.Hand);
                weaponsInventory.Add(Armes.Medpack);
                break;
        }
        health = maxhealth;
        weaponsInventoryLength = weaponsInventory.Count;
        if (view.IsMine)
        {
            healthBar.SetMaxHealth(health);
        }
    }

    //Fonction perdre de la vie
    public void TakeDamage(int viewID, int damage, string Killer)
    {
        if (viewID==view.ViewID) //Test si on est la personne tuer
        {
            if (health>damage) //Diminution de la vie
            {
                health-=damage;
            }
            else
            {
                health=0;
                Death(Killer, 5);
            }
            
            healthBar.SetHealth(health);
        }
    }

    //Fonction gagner de la vie
    public void Healing(int viewID, int heal)
    {
        if (viewID==view.ViewID) //Test si on est la personne tuer
        {
            HealingPrefab.GetComponent<ParticleSystem>().Play();
            if (health+heal<maxhealth) //Diminution de la vie
            {
                health+=heal;
            }
            else
            {
                health = maxhealth;
            }
            
            healthBar.SetHealth(health);
        }
    }

    //Protocol de mort
    public void Death(string Killer, int respTime)
    {
        if (view.IsMine)
        {
            //Affichage du HUD 
            gamemanager.GetComponent<GameManagerScript>().HUDMort(Killer, respTime); //Appel de la function HUDMort de GameManagerScript
            if (myClass == Classe.Policier)
            {
                GetComponent<PistoletScript>().HUD.SetActive(false);
            }

            //APPEL RPC
            view.RPC("rpcDeath", RpcTarget.Others); //Envoi ma mort aux autres
            if (myClass == Classe.Policier || myClass == Classe.Medecin || myClass == Classe.Pompier)
            {
                view.RPC("changeScore", RpcTarget.All, 0, 10);
            }else
            {
                view.RPC("changeScore", RpcTarget.All, 10, 0);
            }

            //Gestion du Player
            transform.GetComponent<PlayerMouvement>().enabled = false; //Désactive les mouvement
            transform.Find("Model").gameObject.SetActive(false); //Cache le model
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.detectCollisions = true; //désactive les collision
            Destroy(gameObject, 5f);
        }
    }

    void Update()
    {
        if (view.IsMine)
        {
            if (Input.mouseScrollDelta.y < 0.0f && selectedWeapon>0)
            {
                selectedWeapon--;
                Hashtable hash = new Hashtable();
                hash.Add("Weapon", selectedWeapon);
                PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
                updateWeaponScript(weaponsInventory[selectedWeapon]);
            }
            if (Input.mouseScrollDelta.y > 0.0f && selectedWeapon<weaponsInventoryLength-1)
            {
                selectedWeapon++;
                Hashtable hash = new Hashtable();
                hash.Add("Weapon", selectedWeapon);
                PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
                updateWeaponScript(weaponsInventory[selectedWeapon]);
            }
            if (Input.GetKeyDown("1") && weaponsInventoryLength>0)
            {
                selectedWeapon = 0;
                Hashtable hash = new Hashtable();
                hash.Add("Weapon", selectedWeapon);
                PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
                updateWeaponScript(weaponsInventory[selectedWeapon]);
            }
            if (Input.GetKeyDown("2") && weaponsInventoryLength>1)
            {
                selectedWeapon = 1;
                Hashtable hash = new Hashtable();
                hash.Add("Weapon", selectedWeapon);
                PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
                updateWeaponScript(weaponsInventory[selectedWeapon]);
                
            }
        }
    }

    //Update weaponscript
    private void updateWeaponScript(Armes weapon)
    {
        if (myClass == Classe.Policier)
        {
            if (weapon == Armes.Pistolet)
            {
                view.RPC("SyncPistolet", RpcTarget.All, true); //Set display arme
                GetComponent<PistoletScript>().HUD.SetActive(true);
                GetComponent<PistoletScript>().inHand =true;
                anim.SetLayerWeight(anim.GetLayerIndex("Gun Pose"), 1f); //Set du layer de visé a true
                anim.SetTrigger("grap"); //Jouer l'amin grap du pistolet
            }
            else
            {
                GetComponent<PistoletScript>().ChangeWeapon();
            }
        }

        if (myClass == Classe.Pompier)
        {
            if (weapon == Armes.Hache)
            {
                view.RPC("SyncHache", RpcTarget.All, true); //Set display arme
                GetComponent<HacheScript>().inHand = true;

                // /!\ Set Layer Anim pompier
                anim.SetLayerWeight(anim.GetLayerIndex("Pompier"), 1f);
                anim.SetTrigger("grap");
            }
            else
            {
                GetComponent<HacheScript>().ChangeWeapon();
            }
        }

        if (myClass == Classe.Medecin)
        {
            if (weapon == Armes.Medpack)
            {
                GetComponent<MedkitScript>().inHand = true;
            }
            else
            {
                GetComponent<MedkitScript>().ChangeWeapon();
            }
        }
    }


    //RPC
    [PunRPC]
    void rpcDeath () 
    {
        Destroy(gameObject); //Détruit le perso mort
    }

    [PunRPC]
    public void dealDammage (int viewID, int damage, string Killer)
    {
        TakeDamage(viewID, damage, Killer); //Envoie les dégâts aux autres
    }

    [PunRPC]
    public void healing (int viewID, int heal)
    {
        Healing(viewID, heal); //Heal
    }
}
