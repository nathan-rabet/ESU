using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerJump : MonoBehaviour
{
    public int jumpForce = 20;
    // Start is called before the first frame update
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            other.GetComponent<Rigidbody>().AddExplosionForce(jumpForce, transform.position, 5,1,ForceMode.Impulse);
    }

}