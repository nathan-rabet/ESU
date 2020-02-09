using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Animator_Controller : MonoBehaviour
{

    public Animator anim;
    private Rigidbody MyRigidBody;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        MyRigidBody = GetComponent <Rigidbody> ();
    }

    // Update is called once per frame
    void Update()
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

        if (Input.GetKey("space"))
        {
            anim.SetBool("jump", true);
        }
        else
        {
            anim.SetBool("jump", false);
        }

        if (MyRigidBody.velocity.y < -2)
        {
            anim.SetBool("chute", true);
        }
        else
        {
            anim.SetBool("chute", false);
        }
    }
}
