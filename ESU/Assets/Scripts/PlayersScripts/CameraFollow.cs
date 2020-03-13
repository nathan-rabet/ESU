using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public float CameraMoveSpeed = 120.0f;
	public Transform CameraFollowObj;
	public float clampAnglemax = 80.0f;
	public float clampAnglemin = 60.0f;
	public float inputSensitivity = 150.0f;
	public float finalInputX;
	public float finalInputZ;
	private float rotY = 0.0f;
	private float rotX = 0.0f;

	private float currentRotation = 0.0f;



	// Use this for initialization
	void Start () {
		Vector3 rot = transform.localRotation.eulerAngles;
		rotY = rot.y;
		rotX = rot.x;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update () {

		// We setup the rotation of the sticks here
		float mouseX = Input.GetAxis ("Mouse X");
		float mouseY = Input.GetAxis ("Mouse Y");

		rotY += mouseX * inputSensitivity * Time.deltaTime;
		rotX += mouseY * inputSensitivity * Time.deltaTime;

		rotX = Mathf.Clamp (rotX, -clampAnglemin, clampAnglemax);

		Quaternion localRotation = Quaternion.Euler (rotX, rotY, 0.0f);
		transform.rotation = localRotation;


	}

	void LateUpdate () {
		if (CameraFollowObj != null) //Mouvement de camera pour le perso
        {
			CameraUpdater ();
		}
		else //Mouvement de camera pour le menu
        {
            Vector3 poscentre = new Vector3(325,0,250); //Position du centre
            Vector3 dir = new Vector3(0,0,-50); //Distance du centre
            Quaternion rotation = Quaternion.Euler(50, currentRotation,0); //Rotation
            transform.position = poscentre + rotation * dir; //Set la pos de la cam
            transform.LookAt(poscentre); //regarde le centre
            if (currentRotation<360) 
            {
                currentRotation += 10 * Time.deltaTime; //Tourne la cam
            }else
            {
                currentRotation = 0; //Reset du tour
            }
        }
	}

	void CameraUpdater() {
		// set the target object to follow
		Transform target = CameraFollowObj;

		//move towards the game object that is the target
		float step = CameraMoveSpeed * Time.deltaTime;
		transform.position = Vector3.MoveTowards (transform.position, target.position, step);
	}
}
