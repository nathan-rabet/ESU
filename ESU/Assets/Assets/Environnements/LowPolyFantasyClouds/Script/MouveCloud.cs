using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouveCloud : MonoBehaviour
{
    public float speed = 0.2f;
    
    // Start is called before the first frame update
    void Start()
    {
        float zCoord = this.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        bool come = true;
        
        transform.Translate(0, 0, MoveBack(speed * Time.deltaTime,gameObject));
        
    }
    public static float MoveBack(float speed, GameObject gameObject)
    {
        if (gameObject.transform.position.z == 295 || gameObject.transform.position.z == -165)
        {
            speed *= -1;
            return speed;
        }
        return speed;
    }
}
