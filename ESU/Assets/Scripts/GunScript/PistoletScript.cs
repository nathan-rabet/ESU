using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon;

public class PistoletScript : MonoBehaviour
{
    public int damage = 10;
    public float range = 100f;
    public GameObject mainCam;
    PhotonView view;


    void Start()
    {
        view = GetComponent<PhotonView> ();
        mainCam = GameObject.FindWithTag("MainCamera");
    }
    void Update()
    {
        if (Input.GetKeyDown("mouse 0"))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, range))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * range, Color.white);
            if (hit.transform.gameObject.tag == "Player")
            {
                view.RPC("dealDammage", RpcTarget.Others,hit.transform.gameObject.GetComponent<PhotonView>().ViewID, damage, "Jacky Tuning");
            }
        }
    }
}
