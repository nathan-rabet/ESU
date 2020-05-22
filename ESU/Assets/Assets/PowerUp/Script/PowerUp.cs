using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUp : MonoBehaviour {

    public GameObject pickupEffect;
    public Text myText;
    public Image slot0;
    public Image slot1;
    public Image slot2;
    public Image slot3;
    public Sprite icon;
    //Choose effect
    public bool Heal;
    public bool Ammo;
    public bool Shield;
    public bool Speed;
    public bool Jump;

    public float duration = 4f;
    public float respawn = 10f;
    public float multiplierSpeed = 1.4f;
    public float multiplierJump = 1.4f;

    private bool toActivate = false;
    private Collider playerPick = null;
    private Inventory inventory;
    private ParticleSystem ps;
    private int slot;
    


    void OnTriggerEnter (Collider other) 
    {
        // Get light
        ps = GetComponent<ParticleSystem>();

        if (other.CompareTag("Player"))
        {
            inventory = other.GetComponent<Inventory>();
            PlayerMouvement mouv = other.GetComponent<PlayerMouvement>();
            for (int i = 0; i < inventory.slots.Length; i++)
            {
                if (inventory.isFull[i] == false)
                {
                    // Item can be added to Inventory !
                    inventory.isFull[i] = true;
                    Pickup(other, mouv, i);
                    StartCoroutine(displayText());
                    StartCoroutine(RespawnPower());
                    toActivate = true;
                    playerPick = other;
                    slot = i;
                    break;
                }
            }

        }
    }

    public void Update()
    {
        switch (slot)
        {
            case 0:
                toLaunchActivePower(KeyCode.W);
                break;
            case 1:
                toLaunchActivePower(KeyCode.X);
                break;
            case 2:
                toLaunchActivePower(KeyCode.C);
                break;
            case 3:
                toLaunchActivePower(KeyCode.V);
                break;
        }
        
    }

    void Pickup(Collider player, PlayerMouvement mouv, int i) 
    {

        //Spawn a cool effect
        Instantiate(pickupEffect, transform.position, transform.rotation);
        
        var emission = ps.emission;
        emission.enabled = false;
        GetComponent<Collider>().enabled = false;
        switch (i)
        {
            case 0:
                slot0.enabled = true;
                slot0.sprite = icon;
                break;
            case 1:
                slot1.enabled = true;
                slot1.sprite = icon;
                break;
            case 2:
                slot2.enabled = true;
                slot2.sprite = icon;
                break;
            case 3:
                slot3.enabled = true;
                slot3.sprite = icon;
                break;
        }

        


    }
    public IEnumerator ActivePower(Collider player, PlayerMouvement mouv, int i) 
    {


        //Consumme PowerUP
        inventory.isFull[i] = false;
        switch (i)
        {
            case 0:
                slot0.enabled = false;
                break;
            case 1:
                slot1.enabled = false;
                break;
            case 2:
                slot2.enabled = false;
                break;
            case 3:
                slot3.enabled = false;
                break;
        }
        //Apply an effect to the player

        //Heal
        if (Heal)
        {
            Player_Manager manager = player.GetComponent<Player_Manager>();
            manager.healing(manager.view.ViewID, 100);
        }
        //Ammo
        if (Ammo) 
        {
            AmmoCount ammoCount;
            ammoCount = GameObject.Find("/GAME/Menu/InGameHUD/Weapon Info").GetComponent<AmmoCount>();
            ammoCount.SetAmmo(20);
        }
        //Shield
        if (Shield)
        {
            Player_Manager manager = player.GetComponent<Player_Manager>();
            manager.isShieldActive = true;
        }

        //Speed
        if (Speed)
            mouv.MaxSpeed *= multiplierSpeed;
        

        //Jump
        if (Jump)
            mouv.JumpHight *= multiplierJump;


        yield return new WaitForSeconds(duration);

        if (Jump)
            mouv.JumpHight /= multiplierJump;

        if (Speed)
            mouv.MaxSpeed /= multiplierSpeed;

       

    }
    IEnumerator RespawnPower() 
    {
        yield return new WaitForSeconds(respawn);
        var emission = ps.emission;
        emission.enabled = true;
        GetComponent<Collider>().enabled = true;
       

    }

    IEnumerator displayText()
    {
        myText.text = "You picked a "+icon.name+"!!";
        yield return new WaitForSeconds(5);
        myText.text = "";
    }
    void toLaunchActivePower(KeyCode code)
    {
        if (Input.GetKeyUp(code) && toActivate)
        {
            PlayerMouvement mouv = playerPick.GetComponent<PlayerMouvement>();
            StartCoroutine(ActivePower(playerPick, mouv, slot));
            toActivate = false;
        }
    }

}
