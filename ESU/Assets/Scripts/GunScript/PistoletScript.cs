using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistoletScript : MonoBehaviour
{
    public int damage = 10;
    public float range = 100f;
    public GameObject mainCam;


    void Start()
    {
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
                transform.GetComponent<Player_Manager>().TakeDamage(damage);
            }
        }
    }
}
