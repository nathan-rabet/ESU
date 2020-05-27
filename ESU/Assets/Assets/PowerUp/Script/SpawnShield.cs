using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnShield : MonoBehaviour
{
    public bool forDefender;
    public bool forAttacker;
    void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag("Player") && isDefender(other) == forDefender)
        {
            Player_Manager manager = other.GetComponent<Player_Manager>();
            manager.isShieldActive = true;
            Debug.Log("ShieldActivé!");
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isDefender(other) == forDefender)
        {
            Player_Manager manager = other.GetComponent<Player_Manager>();
            manager.isShieldActive = false;
            Debug.Log("ShieldDesactivé!");
        }
    }


    bool isDefender(Collider player)
    {
        if (player.gameObject.name.Contains("Pompier") || player.gameObject.name.Contains("Policier") || player.gameObject.name.Contains("Medecin"))
            return true;
        return false;
    }

    
}
