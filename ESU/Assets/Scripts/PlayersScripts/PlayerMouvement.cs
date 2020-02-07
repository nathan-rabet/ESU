using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMouvement : MonoBehaviour
{
    private Rigidbody MyRigidBody;


    private int speed = 25;
    private float jumpHight = 35.0f;
    private bool canJump = true;
    PhotonView view;

    void Start()
    {
        view = GetComponent<PhotonView> ();
        MyRigidBody = GetComponent <Rigidbody> ();
    }

    // Update is called once per frame
    void Update()
    {
        if (!canJump && MyRigidBody.velocity.y == 0) {
            canJump = true;
        }

        if (view.IsMine)
        {
            

            Vector3 dirVector = (Input.GetAxis("Horizontal") * MyRigidBody.transform.right + Input.GetAxis("Vertical") * MyRigidBody.transform.forward).normalized;
            MyRigidBody.MovePosition (transform.position + dirVector * speed * Time.deltaTime);

            Quaternion rotVector = Quaternion.Euler(0,Input.GetAxis("Mouse X") * 20,0);
            MyRigidBody.MoveRotation (transform.rotation * rotVector);

            if (canJump && Input.GetKeyDown("space"))
            {
                MyRigidBody.AddForce (transform.up * (int)Mathf.Sqrt(2.0f*jumpHight), ForceMode.VelocityChange);
                canJump = false;
            }
        }
    }
}
