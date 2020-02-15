using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMouvement : MonoBehaviour
{
    private Rigidbody MyRigidBody;



    private float speed = 0.08f;
    private float jumpHight = 10.0f;
    private bool canJump = true;
    public GameObject mainCamera;
    PhotonView view;

    void Start()
    {
        view = GetComponent<PhotonView> (); //Cherche la vue
        MyRigidBody = GetComponent <Rigidbody> (); //Cherche du rigidbody

        mainCamera = GameObject.FindWithTag("MainCamera"); //Cherche de la camera
    }

    // Update is called once per frame
    void Update()
    {


        if (view.IsMine) //Si ma vue
        {
            
            if (!canJump && MyRigidBody.velocity.y == 0) //Si on peut pas sauter et on tombe pas
            {
                canJump = true; //peut sauter
            }

            if (Input.GetKey(KeyCode.LeftShift)) //Si Maj enfoncé
            {
                speed = 0.15f; //vitesse
            }
            else
            {
                speed = 0.08f; //vitesse
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
            }
        }
    }
}
