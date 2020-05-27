using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DeathZone : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PhotonView>().IsMine)
        {
            Player_Manager manager = other.GetComponent<Player_Manager>();
            manager.isDeathZone = true;
        }
    }
}
