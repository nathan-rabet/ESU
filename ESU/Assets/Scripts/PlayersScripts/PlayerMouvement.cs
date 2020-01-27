using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMouvement : MonoBehaviour
{
    public int speed = 5;
    PhotonView view;

    void Start()
    {
        view = GetComponent<PhotonView> ();
    }

    // Update is called once per frame
    void Update()
    {
        if (view.IsMine)
        {
            transform.Translate (Vector3.forward * Input.GetAxis ("Vertical") * speed * Time.deltaTime);
            transform.Rotate (Vector3.up * Input.GetAxis("Horizontal") * speed * 40 * Time.deltaTime);
        }
    }
}
