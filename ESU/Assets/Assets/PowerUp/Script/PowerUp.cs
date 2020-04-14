using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

    public GameObject pickupEffect;
    public float duration = 4f;
    public float multiplierPlayerScale = 1f;
    public float multiplierSpeed = 1.4f;
    public float multiplierJump = 1.4f;
    public GameObject light = GameObject.Find("LightShape");

    void OnTriggerEnter (Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine( Pickup(other) );
        }
    }

    IEnumerator Pickup(Collider player) 
    {
        bool isJumpModif = false;
        bool isSpeedModif = false;
        bool isScaleModif = false;
        //Spawn a cool effect
        GameObject effect = Instantiate(pickupEffect, transform.position, transform.rotation);

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


        //Destroy PowerUp
        Destroy(gameObject);
        Destroy(light);
    }
   
}
