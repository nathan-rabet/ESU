using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

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
    private GameObject shieldParticules;
    private GameObject speedParticules;
    private GameObject jumpParticules;
    //private ParticleSystem jumpParticules;
    private int slot;
    
    

    void OnTriggerEnter (Collider other) 
    {
        // Get light
        ps = GetComponent<ParticleSystem>();

        if (other.CompareTag("Player") && other.GetComponent<PhotonView>().IsMine)
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
        if (Jump == false)
        {
            var emission = ps.emission;
            emission.enabled = false;
        }
        else
            particulesDisable(this.gameObject, "AuraChargePurple");

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

        shieldParticules = player.transform.Find("ShieldBlue").gameObject;
        jumpParticules = player.transform.Find("SwirlAuraPurple").gameObject;
        speedParticules = player.transform.Find("SwirlAuraRed").gameObject;
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
        //Shield
        if (Shield)
        {
            particulesActive(shieldParticules);
            Player_Manager manager = player.GetComponent<Player_Manager>();
            manager.isShieldActive = true;
        }

        //Speed
        float stockSpeed = mouv.MaxSpeedControl;
        if (Speed)
        {
            particulesActive(speedParticules);
            mouv.MaxSpeed = mouv.MaxSpeedControl * multiplierSpeed;
        }

        //Jump
        float stockJump = mouv.JumpHightControl;
        if (Jump)
        {
            particulesActive(jumpParticules);
            mouv.JumpHight = mouv.JumpHightControl * multiplierJump;
        }

        yield return new WaitForSeconds(duration);

        if (Jump)
        {
            particulesDisable(jumpParticules);
            mouv.JumpHight = stockJump;
        }

        if (Speed)
        {
            particulesDisable(speedParticules);
            mouv.MaxSpeed = stockSpeed;
        }
        if (Shield)
        {
            particulesDisable(shieldParticules);
            Player_Manager manager = player.GetComponent<Player_Manager>();
            manager.isShieldActive = false;
        }


       

    }
    IEnumerator RespawnPower() 
    {
        yield return new WaitForSeconds(respawn);
        if (Jump == false)
        {
            var emission = ps.emission;
            emission.enabled = true;
        }
        else
            particulesActive(this.gameObject, "AuraChargePurple");
        GetComponent<Collider>().enabled = true;
       

    }

    IEnumerator displayText()
    {
        myText.text = "+1 "+icon.name;
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

    void particulesActive(GameObject anObject)
    {
        var shelf = anObject.GetComponent<ParticleSystem>().emission;
        shelf.enabled = true;
        foreach (Transform child in anObject.transform)
        {
            var emission = child.GetComponent<ParticleSystem>().emission;
            emission.enabled = true;
        }
    }

    void particulesActive(GameObject anObject,string exception)
    {
        var shelf = anObject.GetComponent<ParticleSystem>().emission;
        shelf.enabled = true;
        foreach (Transform child in anObject.transform)
        {
            if (child.gameObject.name != exception)
            {
                var emission = child.GetComponent<ParticleSystem>().emission;
                emission.enabled = true;
            }
        }
    }

    void particulesDisable(GameObject anObject)
    {
        var shelf = anObject.GetComponent<ParticleSystem>().emission;
        shelf.enabled = false;
        foreach (Transform child in anObject.transform)
        {
            var emission = child.GetComponent<ParticleSystem>().emission;
            emission.enabled = false;
        }
    }

    void particulesDisable(GameObject anObject, string exception)
    {
        var shelf = anObject.GetComponent<ParticleSystem>().emission;
        shelf.enabled = false;
        foreach (Transform child in anObject.transform)
        {
            if (child.gameObject.name != exception)
            {
                var emission = child.GetComponent<ParticleSystem>().emission;
                emission.enabled = false;
            }
        }
    }

}
