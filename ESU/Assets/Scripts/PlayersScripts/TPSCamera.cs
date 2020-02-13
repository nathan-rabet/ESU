using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TPSCamera : MonoBehaviour
{
    public GameObject cursorHUD;
    private const float Y_ANGLE_MIN = -50.0f;
    private const float Y_ANGLE_MAX = 50.0f;
    
    public Transform lookAt;
    private Transform camTransform;

    private Camera cam;
    private float currentY = 0.0f;
    private float currentX = 0.0f;
    private float currentRotation = 0.0f;

        #region Viser
        float currentTime;
        float startTime;
        Vector3 startVise;
        private float tempsDeVise = 0.2f;
        private bool scope = false;
        public bool Scope() => scope;
        #endregion
    private Vector3 dirNoScope = new Vector3(0,0,-2.5f);
    private Vector3 dirScope = new Vector3(0,0,-1.0f);

    private void Start()
    {
        camTransform = transform;
        cam = Camera.main;
        startVise = transform.position;
        currentTime = Time.time;
        startTime = Time.time;
    }

    private void Update(){
        currentY -= Input.GetAxis("Mouse Y");
        currentY = Mathf.Clamp(currentY,Y_ANGLE_MIN,Y_ANGLE_MAX);
        currentX += Input.GetAxis("Mouse X") * 20.0f;

        
    }

    private void LateUpdate()
    {
        if (lookAt != null) //Mouvement de camera pour le perso
        {
            //Recup du clic droit
            if (Input.GetKeyDown("mouse 1"))
            {
                scope = true;
                startVise = transform.position;
            
                startTime = Time.time;
                currentTime = startTime;
            }
            if (Input.GetKeyUp("mouse 1"))
            {
                scope = false;
                startVise = transform.position;
            
                startTime = Time.time;
                currentTime = startTime;
            }

            //Posiotion de la cam
            Quaternion rotation = Quaternion.Euler(currentY, currentX,0);
            currentTime += Time.deltaTime;
            float time = (currentTime - startTime) / tempsDeVise;
            if (scope)
            {
                cursorHUD.SetActive(true);
                if (time>1)
                {
                    camTransform.position = lookAt.position + rotation * dirScope;
                }else
                {
                    camTransform.position = Vector3.Slerp(startVise, lookAt.position + rotation * dirScope, (currentTime - startTime) / tempsDeVise);
                }
            }else
            {
                cursorHUD.SetActive(false);
                if (time>1)
                {
                    camTransform.position = lookAt.position + rotation * dirNoScope;
                }else
                {
                    camTransform.position = Vector3.Slerp(startVise, lookAt.position + rotation * dirNoScope, (currentTime - startTime) / tempsDeVise);
                }
            }
            
            
            camTransform.LookAt(lookAt.position);
        }
        else //Mouvement de camera pour le menu
        {
            Vector3 poscentre = new Vector3(325,0,250);
            Vector3 dir = new Vector3(0,0,-50);
            Quaternion rotation = Quaternion.Euler(50, currentRotation,0);
            camTransform.position = poscentre + rotation * dir;
            camTransform.LookAt(poscentre);
            if (currentRotation<360)
            {
                currentRotation += 10 * Time.deltaTime;
            }else
            {
                currentRotation = 0;
            }
        }
    }

}
