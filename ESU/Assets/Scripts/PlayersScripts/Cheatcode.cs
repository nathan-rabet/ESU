﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheatcode : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("k"))
        {
            transform.GetComponent<Player_Manager>().Death("le Vide", 5);
        }
    }
}
