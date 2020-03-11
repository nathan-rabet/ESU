using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using Photon.Pun;

public class Animator_Controller : MonoBehaviour
{

    private Animator anim;
    private bool anim_chute = true;
    private Rigidbody MyRigidBody;
    PhotonView view;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        MyRigidBody = GetComponent <Rigidbody> ();
        view = GetComponent<PhotonView> ();
    }

    // Update is called once per frame
    void Update()
    {
        if (view.IsMine)
        {
            if (Input.GetKey(KeyCode.W))
            {
                anim.SetBool("walk", true);
            }
            else
            {
                anim.SetBool("walk", false);
            }
            
            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
            {
                anim.SetBool("run", true);
            }
            else
            {
                anim.SetBool("run", false);
            }
            
            if (Input.GetKey(KeyCode.S))
            {
                anim.SetBool("back", true);
            }
            else
            {
                anim.SetBool("back", false);
            }
            
            if (Input.GetKey(KeyCode.A))
            {
                anim.SetBool("left", true);
            }
            else
            {
                anim.SetBool("left", false);
            }
            
            if (Input.GetKey(KeyCode.D))
            {
                anim.SetBool("right", true);
            }
            else
            {
                anim.SetBool("right", false);
            }

            if (Input.GetKey("space"))
            {
                anim.SetBool("jump", true);
            }
            else
            {
                anim.SetBool("jump", false);
            }
            
            

            if (anim_chute && MyRigidBody.velocity.y > -2 && MyRigidBody.velocity.y < -1)
            {
                anim.SetBool("chute", true);
                anim_chute = false;
            }
            else
            {
                anim.SetBool("chute", false);
                anim_chute = true;
            }
        }
    }
}
