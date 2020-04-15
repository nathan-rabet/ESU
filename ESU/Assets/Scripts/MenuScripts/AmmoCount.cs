using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class AmmoCount : MonoBehaviour
{
    // Start is called before the first frame update
    public Image bar;
    private Ammo_Animation _ammoAnimation;

    public void Start()
    {
        _ammoAnimation = GameObject.Find("Circle Reload").GetComponent<Ammo_Animation>();
    }

    public void SetAmmo(int ammo)
    {
        // Set la valeur du fill de l'image à partir de l'ammo
        float res = (float) ammo;
        bar.fillAmount = (res / 100) * 5;
    }

    public void ReloadAnim()
    { // Appel la fonction Playanimation dans le script du circle
        _ammoAnimation.PlayAnimation();
    }
    
}
