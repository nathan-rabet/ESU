using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FireParticule : MonoBehaviour
{
    public ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;

    void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    void OnParticleCollision(GameObject other)
    {
        if (other.tag == "Batiment" && other.GetComponent<BuildingScript>().fire < BuildingScript.maxfire)
        {
            other.GetComponent<PhotonView>().RPC("SetFire", RpcTarget.All, 0.1f);
        }
    }
}
