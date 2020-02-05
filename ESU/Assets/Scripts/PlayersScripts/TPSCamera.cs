using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TPSCamera : MonoBehaviour
{
    private const float Y_ANGLE_MIN = -50.0f;
    private const float Y_ANGLE_MAX = 50.0f;
    
    public Transform lookAt;
    private Transform camTransform;

    private Camera cam;

    private float distance = 10.0f;
    private float currentY = 0.0f;
    private float currentRotation = 0.0f;

    private void Start()
    {
        camTransform = transform;
        cam = Camera.main;
        
    }

    private void Update(){
        currentY += Input.GetAxis("Mouse Y");
        currentY = Mathf.Clamp(currentY,Y_ANGLE_MIN,Y_ANGLE_MAX);
    }

    private void LateUpdate()
    {
        if (lookAt != null) //Mouvement de camera pour le perso
        {
            Vector3 dir = new Vector3(0,0,-distance);
            Quaternion rotation = Quaternion.Euler(currentY, lookAt.transform.eulerAngles.y,0);
            camTransform.position = lookAt.position + rotation * dir;
            
            
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
