using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMouvement : MonoBehaviour
{
    private Rigidbody MyRigidBody;


    private float speed = 0.5f;
    private float jumpHight = 10.0f;
    private bool canJump = true;
    public GameObject mainCamera;
    PhotonView view;

    void Start()
    {
        view = GetComponent<PhotonView> ();
        MyRigidBody = GetComponent <Rigidbody> ();
        mainCamera = GameObject.FindWithTag("MainCamera");
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
            MyRigidBody.MovePosition (transform.position + dirVector * speed);
            Quaternion newrot = newrot = Quaternion.Slerp(transform.rotation, mainCamera.transform.rotation, 10f * Time.deltaTime);
            if ( Input.GetKey("mouse 1") )
            {
                newrot.Set(0, mainCamera.transform.rotation.y , 0, mainCamera.transform.rotation.w);
            }else
            {
                newrot.Set(0, newrot.y , 0, newrot.w);
            }
            transform.rotation = newrot;

            if (canJump && Input.GetKeyDown("space"))
            {
                MyRigidBody.AddForce (transform.up * (int)Mathf.Sqrt(3.0f*jumpHight), ForceMode.VelocityChange);
                canJump = false;
            }
        }
    }
}
