using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroy : MonoBehaviour
{
    public GameObject particle;
    public int minDuration;
    public int maxDuration;
    private float duration;
    // Start is called before the first frame update
    void Start()
    {
        duration = Random.Range(minDuration, maxDuration);
    }

    // Update is called once per frame
    void Update()
    {
        if (duration > 0)
        {
            duration -= Time.deltaTime;
        }
        else
            Destroy(particle);
    }
}
