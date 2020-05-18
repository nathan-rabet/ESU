using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healing : MonoBehaviour
{
    public int ToHeal = 100;
    public GameObject pickupEffect;
    public float respawn = 10f;
    public int setAmmo = 20;
    private ParticleSystem ps;
    private AmmoCount ammoCount;

    void OnTriggerEnter(Collider other)
    {
        ammoCount = GameObject.Find("/GAME/Menu/InGameHUD/Weapon Info").GetComponent<AmmoCount>();
        ps = GetComponent<ParticleSystem>();
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Pickup(other));
        }
    }
    IEnumerator Pickup(Collider player)
    {
        Player_Manager manager = player.GetComponent<Player_Manager>();
        Instantiate(pickupEffect, transform.position, transform.rotation);
        if (ToHeal != 0)
            manager.healing(manager.view.ViewID, ToHeal);
        if (setAmmo != 0)
            ammoCount.SetAmmo(setAmmo);
        
        var emission = ps.emission;
        emission.enabled = false;
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(respawn);
        emission.enabled = true;
        GetComponent<Collider>().enabled = true;
    }
}
