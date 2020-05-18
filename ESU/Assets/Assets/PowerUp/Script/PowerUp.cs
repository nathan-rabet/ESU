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
    private ParticleSystem ps;

    void OnTriggerEnter (Collider other) 
    {
        // Get light
        ps = GetComponent<ParticleSystem>();

        if (other.CompareTag("Player"))
        {

            StartCoroutine( Pickup(other) );
            StartCoroutine(ActivePower());
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
        var emission = ps.emission;
        emission.enabled = false;
        GetComponent<Collider>().enabled = false;
        

        yield return new WaitForSeconds(duration);
        
        if (isJumpModif)
            mouv.JumpHight /= multiplierJump;
        if (isSpeedModif)
            mouv.MaxSpeed /= multiplierSpeed;
        if (isScaleModif)
            player.transform.localScale /= multiplierPlayerScale;

    }
    IEnumerator ActivePower() 
    {
        yield return new WaitForSeconds(respawn+duration);
        var emission = ps.emission;
        emission.enabled = true;
        GetComponent<Collider>().enabled = true;
       

    }
   
}
