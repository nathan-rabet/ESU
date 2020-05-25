using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMouvement : MonoBehaviour
{
    private Rigidbody MyRigidBody;
    private float distToGround;
    
    private Animator anim;
    private float speed = 0.08f;
    //WARNING: Change normal variable and control variable in the same time !
    private float maxspeed = 0.16f;
    private float maxspeedControl = 0.16f;
    private float jumpHight = 10.0f;
    private float jumpHightControl = 10.0f;
    private bool canJump = true;
    private AudioSource walk;
    private AudioSource run;
    public GameObject mainCamera;
    PhotonView view;

    public float MaxSpeed
    {
        get => maxspeed;
        set => maxspeed = value;
    }
    public float MaxSpeedControl
    {
        get => maxspeedControl;
    }

    public float JumpHight
    {
        get => jumpHight;
        set => jumpHight = value;
    }
    public float JumpHightControl
    {
        get => jumpHightControl;
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        view = GetComponent<PhotonView> (); //Cherche la vue
        MyRigidBody = GetComponent <Rigidbody> (); //Cherche du rigidbody

        mainCamera = GameObject.FindWithTag("MainCamera"); //Cherche de la camera
        distToGround = GetComponent<Collider>().bounds.extents.y;

        AudioSource[] sources = GetComponents<AudioSource>();
        walk = sources[0];
        run = sources[1];
    }

    public bool IsGrounded()
    {
        Debug.DrawRay(transform.position + Vector3.up * 0.2f, -Vector3.up * (distToGround - 0.3f), Color.white);
        return Physics.Raycast(transform.position + Vector3.up * 0.2f, -Vector3.up, distToGround - 0.3f);
    }

    // Update is called once per frame
    void Update()
    {


        if (view.IsMine) //Si ma vue
        {
            
            if (IsGrounded()) //Si on peut pas sauter et on tombe pas
            {
                canJump = true; //peut sauter
                anim.SetBool("landing", true);
            }
            else
            {
                canJump = false;
                anim.SetBool("landing", false);
            }

            if (Input.GetKey(KeyCode.LeftShift)) //Si Maj enfoncé
            {
                speed = maxspeed; //vitesse
                if (canJump && !Input.GetKey("space") && (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0))
                {
                    if (!run.isPlaying)
                        run.Play();
                    walk.Stop();
                }
            }
            else
            {
                speed = maxspeed/2; //vitesse
                if (canJump && !Input.GetKey("space") && (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0))
                {
                    if (!walk.isPlaying)
                        walk.Play();
                    run.Stop();
                }
            }

            if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
            {
                walk.Stop();
                run.Stop();
            }

            //Création du vecteur direction du mouvement
            Vector3 dirVector = (Input.GetAxis("Horizontal") * MyRigidBody.transform.right + Input.GetAxis("Vertical") * MyRigidBody.transform.forward).normalized;
            
            //Ajout de la pos le vercteur dir * la vitesse
            MyRigidBody.MovePosition (transform.position + dirVector * speed);

            //Création d'un vecteur rotation qui suit la cam lentement
            Quaternion newrot = Quaternion.Slerp(transform.rotation, mainCamera.transform.rotation, 10f * Time.deltaTime);
            if ( Input.GetKey("mouse 1") ) //Si clic droit
            {
                newrot.Set(0, mainCamera.transform.rotation.y , 0, mainCamera.transform.rotation.w); //Suivi instantané
            }else
            {
                newrot.Set(0, newrot.y , 0, newrot.w); //Suivi lent
            }
            transform.rotation = newrot;

            if (canJump && Input.GetKeyDown("space")) //Si peut sauté et touche espace
            {
                MyRigidBody.AddForce (transform.up * (int)Mathf.Sqrt(3.0f*jumpHight), ForceMode.VelocityChange); //Ajout d'une force vertical
                canJump = false; //peut pas sauter
                walk.Stop();
                run.Stop();
            }
        }
    }
}
