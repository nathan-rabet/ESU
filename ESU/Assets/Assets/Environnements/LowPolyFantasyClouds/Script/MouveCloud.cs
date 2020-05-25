using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouveCloud : MonoBehaviour
{
    public int maxPosition = 930;
    public int respawnPosition = -624;
    public float speed = 0.2f;
    float time = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (MoveBack(speed, gameObject) != 0)
        {
            transform.Translate(0, 0, MoveBack(speed, gameObject) * Time.deltaTime);
            time = Time.deltaTime;
        }

        else
        {
            transform.SetPositionAndRotation(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, respawnPosition), new Quaternion());
        }
        
    }
    public float MoveBack(float speed, GameObject gameObject)
    {
        if (gameObject.transform.position.z >= maxPosition)
        {
                speed = 0;
        }
        return speed;
    }
}
