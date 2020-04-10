using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

    public GameObject pickupEffect;
    public float duration = 4f;
    public float multiplierPlayerScale = 1f;
    public float multiplierSpeed = 1.4f;
    public float multiplierJump = 1.4f;

    void OnTriggerEnter (Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine( Pickup(other) );
        }
    }

    IEnumerator Pickup(Collider player) 
    {
        //Spawn a cool effect
        Instantiate(pickupEffect, transform.position, transform.rotation);

        //Apply an effect to the player
        if (multiplierPlayerScale > 0)
            player.transform.localScale *= multiplierPlayerScale;
        
        PlayerMouvement mouv = player.GetComponent<PlayerMouvement>();
        if (multiplierSpeed > 0)
            mouv.MaxSpeed *= multiplierSpeed;
        if (multiplierJump > 0)
        mouv.MaxJumpHight *= multiplierJump;

        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;


        yield return new WaitForSeconds(duration);
        //Destroy PowerUp
        
        Destroy(gameObject);
        Debug.Log("Power up picked up!");
    }
   
}
