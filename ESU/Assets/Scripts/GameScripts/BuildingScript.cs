using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Animations;
using Photon;
using UnityEditor;
using Object = UnityEngine.Object;

public class BuildingScript : MonoBehaviour
{
<<<<<<< HEAD
    public float health = 1000f;
    public float fire = 0;
    private float maxfire = 50f;
=======
    public int health = 1000;
    public int fire = 0;
>>>>>>> 78ccc66ba71b00794b33edde8730ec9692518fd4
    private PhotonView view;
    private GameStat GameStat;
    private GameObject fireParticule;
    private GameObject smokeParticule;
<<<<<<< HEAD

    private GameObject firePar;
    private GameObject smokePar;


=======
    
    
>>>>>>> 78ccc66ba71b00794b33edde8730ec9692518fd4
    private void Start()
    {
        fireParticule = (GameObject)Resources.Load("FireArea", typeof(GameObject));
        smokeParticule = (GameObject)Resources.Load("Smoke02_HighPerformance", typeof(GameObject));
        view = GetComponent<PhotonView>(); //Cherche la vue
        GameStat = GameObject.Find("/GAME/GameManager").GetComponent<GameStat>();
    }

    private bool smokeP = false;
    private bool fireP = false;
    


    public void Update()
    {
<<<<<<< HEAD
        if (!smokeP && fire > maxfire/2) // Instancie les particules de fumée
        {
            smokePar = Instantiate(smokeParticule, transform.position, transform.rotation * Quaternion.Euler (270f, 0, 0f));
            smokeP = true;
        }

        if (smokePar != null && smokeP && fire < maxfire / 2) // Instancie les particules de fumée
        {
            Destroy(smokePar);
            smokeP = false;
        }

        if (!fireP && fire > maxfire/1.5f) // Instancie les particules de feux
        {
            firePar = Instantiate(fireParticule, transform.position, transform.rotation * Quaternion.Euler (270f, 0, 0f));
            fireP = true;
        }

        if (firePar != null && fireP && fire < maxfire / 1.5f) // Instancie les particules de fumée
        {
            Destroy(firePar);
            fireP = false;
        }
=======
        if (fire > 5 && !smokeP) // Instancie les particules de fumée
        {
            Instantiate(smokeParticule, transform.position, transform.rotation * Quaternion.Euler (270f, 0, 0f));
            smokeP = true;
        }
            
        if (fire > 20 && !fireP) // Instancie les particules de feux
        {
            Instantiate(fireParticule, transform.position, transform.rotation * Quaternion.Euler (270f, 0, 0f));
            fireP = true;
        }
>>>>>>> 78ccc66ba71b00794b33edde8730ec9692518fd4
    }

    public void TakeDamage(int viewID, int damage, Photon.Realtime.Player Killer)
    {
        if (viewID == view.ViewID && health > 0) //Test si on est le bâtiment
        {
            if (health>damage) //Diminution de la vie
            {
                health-=damage;
                view.RPC("SyncBat", RpcTarget.Others, health, fire);
            }
            else
            {
                GameStat.changeScore(50, 0);
                
                health = 0;
                view.RPC("SyncBat", RpcTarget.All, health, fire);
            }
        }
    }

<<<<<<< HEAD
=======
    public void SetFire(int amount)
    {
        if (fire + amount > 10)
            fire = 10;
        if (fire + amount < 0)
            fire = 0;
        fire += amount;
    }
>>>>>>> 78ccc66ba71b00794b33edde8730ec9692518fd4

    IEnumerator Fire()
    {
        if (fire > 0)
        {
<<<<<<< HEAD
=======
            (bool smokeP, bool fireP) = (false, false);
>>>>>>> 78ccc66ba71b00794b33edde8730ec9692518fd4
            health -= fire;


            view.RPC("SyncBat", RpcTarget.Others, health, fire);
            yield return new WaitForSeconds(1f);
<<<<<<< HEAD

            StartCoroutine(Fire());
=======
            StartCoroutine(Fire());

            if (fire > 5 && !smokeP)
            {
                Instantiate(smokeParticule, transform.position, transform.rotation);
                smokeP = true;
            }
            
            if (fire > 20 && !fireP)
            {
                Instantiate(fireParticule, transform.position, transform.rotation);
                fireP = true;
            }
            
            
            
>>>>>>> 78ccc66ba71b00794b33edde8730ec9692518fd4
        }
    }
    

    IEnumerator Anims()
    {
        int dir = 1;
        for (int i = 0; i < 50; i++)
        {
            if (dir == 1)
                dir = 0;
            else
                dir = 1;
            if (dir==1)
            {
                transform.position = transform.position + new Vector3(0.1f, 0.1f, 0.1f);
            }
            else
            {
                transform.position = transform.position - new Vector3(0.1f, 0.1f, 0.1f);
            }
            yield return new WaitForSeconds(0.01f);
        }
        for (int i = 0; i < 200; i++)
        {
            transform.Translate(Vector3.down * 0.1f);
            yield return new WaitForSeconds(0.05f);
        }
    }

    [PunRPC]
<<<<<<< HEAD
    public void SetFire(float amount)
    {
        if (fire + amount > maxfire)
            fire = maxfire;
        if (fire + amount < 0)
            fire = 0;
        if (fire == 0)
            StartCoroutine(Fire());
        fire += amount;
    }

    [PunRPC]
=======
>>>>>>> 78ccc66ba71b00794b33edde8730ec9692518fd4
    public void SyncBat(int health, int fire)
    {
        this.health = health;
        this.fire = fire;
        if (this.health == 0)
        {
            StartCoroutine(Anims());
        }
    }
}
