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
    private float LerpSpeed = 3;
    private int Ammo;
    public Text Textvalue;

    public void Start()
    {
        _ammoAnimation = GameObject.Find("Circle Reload").GetComponent<Ammo_Animation>();
    }

    public void Update()
    {
        ChangeAmmoValue();
        // Affiche le nombre de balle restante
        Textvalue.text = Convert.ToString(Ammo);
    }

    public void SetAmmo(int ammo)
    {
        Ammo = ammo;
    }

    public void ChangeAmmoValue()
    {
        // Set la valeur du fill de l'image à partir de l'Ammo
        if (bar.fillAmount != Ammo)
        {
            float res = (float) Ammo;
            bar.fillAmount = Mathf.Lerp( bar.fillAmount, (res / 100) * 5, LerpSpeed * Time.deltaTime);
        }
    }

    public void ReloadAnim()
    { // Appel la fonction Playanimation dans le script du circle
        _ammoAnimation.PlayAnimation();
    }
    
}
