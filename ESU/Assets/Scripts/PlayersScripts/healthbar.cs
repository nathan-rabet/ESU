using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class healthbar : MonoBehaviour
{
    public Slider slider;
    private float lerpSpeed = 2;
    private float t = 0;
    private int Health;
    public float LerpSpeed = 3;
    public Text Textvalue;


    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        Health = health;
    }

    public void Update()
    {
        // Vérifie si la vie actuelle correspond à la valeur de la vie théorique
        ChangeHealthValue();
        // Remplace la valeur du text par la vie actuelle
        Textvalue.text = Convert.ToString(Health) + "%";
    }


    public void SetHealth(int health)
    {
        // Met à jour la valeur que doit prendre la bar de vie
        Health = health;
    }

    public void ChangeHealthValue()
    {
        // Change la valeur de la vie dans le cas ou Health ne correspond pas à la valeur du slider
        if (slider.value != Health)
        {
            slider.value = Mathf.Lerp( slider.value, Health, LerpSpeed * Time.deltaTime);
        }
    }
}
