using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroy : MonoBehaviour
{
    public GameObject particle;
    public int minDuration;
    public int maxDuration;
    private float duration;

    void Start()
    {
        duration = Random.Range(minDuration, maxDuration);
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 10f);
        foreach (Collider hit in hitColliders)
        {
            if (hit.gameObject.tag == "AI")
                hit.GetComponent<PhotonView>().RPC("headingShot", RpcTarget.MasterClient);
        }
    }


    void Update()
    {
        if (duration > 0)
        {
            duration -= Time.deltaTime;
        }
        else
            Destroy(particle);
    }
}
