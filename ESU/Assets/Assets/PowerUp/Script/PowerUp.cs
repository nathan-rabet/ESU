using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

    public GameObject pickupEffect;
    public float duration = 4f;
    public float respawn = 10f;
    public float multiplierPlayerScale = 1f;
    public float multiplierSpeed = 1.4f;
    public float multiplierJump = 1.4f;

    void OnTriggerEnter (Collider other) 
    {
        // Get light
        GameObject originalGameObject = this.gameObject;
        GameObject light = this.transform.GetChild(0).gameObject;

        if (other.CompareTag("Player"))
        {
            light.GetComponent<MeshRenderer>().enabled = false;
            StartCoroutine( Pickup(other) );
            StartCoroutine(ActivePower(light.GetComponent<MeshRenderer>()));
        }
    }

    IEnumerator Pickup(Collider player) 
    {
        bool isJumpModif = false;
        bool isSpeedModif = false;
        bool isScaleModif = false;

        //Spawn a cool effect
        Instantiate(pickupEffect, transform.position, transform.rotation);
        

        //Apply an effect to the player
        if (multiplierPlayerScale > 0)
        {
            player.transform.localScale *= multiplierPlayerScale;
            isScaleModif = true;
        }
        PlayerMouvement mouv = player.GetComponent<PlayerMouvement>();
        if (multiplierSpeed > 0)
        {
            mouv.MaxSpeed *= multiplierSpeed;
            isSpeedModif = true;
        }
        if (multiplierJump > 0)
        {
            mouv.JumpHight *= multiplierJump;
            isJumpModif = true;
        }
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        

        yield return new WaitForSeconds(duration);
        
        if (isJumpModif)
            mouv.JumpHight /= multiplierJump;
        if (isSpeedModif)
            mouv.MaxSpeed /= multiplierSpeed;
        if (isScaleModif)
            player.transform.localScale /= multiplierPlayerScale;

    }
    IEnumerator ActivePower(MeshRenderer light) 
    {
        yield return new WaitForSeconds(respawn+duration);

        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<Collider>().enabled = true;
        light.enabled = true;

    }
   
}
