using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouveCloud : MonoBehaviour
{
    public float speed = 0.2f;
    
    // Start is called before the first frame update
    void Start()
    {
        float zCoord = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {

        transform.Translate(0,0,speed * Time.deltaTime);
    }
}
