using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon;
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
        Pistolet = 0,
        Hache = 1,
        Extincteur = 2,
        Medpack = 3,
        LanceFlamme = 4
    }
    
    public PhotonView view;
    public Classe myClass;
    public int health;
    
    private List<Armes> weaponsInventory;
    private int selectedWeapon = 0;

    

    private healthbar healthBar;
    public GameObject gamemanager;

    void Start()
    {
        //Set des variables
        gamemanager = GameObject.Find("/GAME/GameManager");
        healthBar = GameObject.Find("/GAME/Menu/InGameHUD/Health Bar").GetComponent<healthbar>();
        
        
        weaponsInventory = new List<Armes>();

        //Donne les caractéristiques de la classe
        switch(myClass)
        {
            case Classe.Policier:
                health = 100;
                weaponsInventory.Add(Armes.Pistolet);
                break;
            case Classe.Pompier:
                health = 100;
                weaponsInventory.Add(Armes.Hache);
                weaponsInventory.Add(Armes.Extincteur);
                break;
            case Classe.Medecin:
                health = 100;
                weaponsInventory.Add(Armes.Medpack);
                break;
            case Classe.Mercenaire:
                health = 100;
                weaponsInventory.Add(Armes.Pistolet);
                break;
            case Classe.Pyroman:
                health = 100;
                weaponsInventory.Add(Armes.LanceFlamme);
                break;
            case Classe.Drogueur:
                health = 100;
                weaponsInventory.Add(Armes.Medpack);
                break;

        }
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


    //Protocol de mort
    public void Death(string Killer, int respTime)
    {
        if (view.IsMine)
        {
            //Affichage du HUD 
            gamemanager.GetComponent<GameManagerScript>().HUDMort(Killer, respTime); //Appel de la function HUDMort de GameManagerScript

            //APPEL RPC
            view.RPC("rpcDeath", RpcTarget.Others, view.ViewID, Killer); //Envoi ma mort aux autres

            //Gestion du Player
            transform.GetComponent<PlayerMouvement>().enabled = false; //Désactive les mouvement
            transform.Find("Model").gameObject.SetActive(false); //Cache le model
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.detectCollisions = true; //désactive les collision
        }
    }

    //Destruction
    public void DestroyMe()
    {
        if (view.IsMine)
        {
            Destroy(gameObject); //Détruit le gameObject coté client
        }
    }
}
