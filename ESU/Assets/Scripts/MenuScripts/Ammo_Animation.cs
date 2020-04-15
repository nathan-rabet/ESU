using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo_Animation : MonoBehaviour
{
    // Start is called before the first frame update

    private Animator anim;

    public void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void PlayAnimation()
    {
        // Jour l'anim de rechargement
        anim.SetTrigger("reload");
    }
}
