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
    private bool alive = true;
    public float health = 1000f;
    public float fire = 0;
    public static float maxfire = 50f;
    private PhotonView view;
    private GameStat GameStat;
    private GameObject fireParticule;
    private GameObject smokeParticule;

    private GameObject firePar;
    private GameObject smokePar;


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
        if (!smokeP && fire > maxfire/4) // Instancie les particules de fumée
        {
            smokePar = Instantiate(smokeParticule, transform.position, transform.rotation * Quaternion.Euler (270f, 0, 0f));
            smokeP = true;
        }

        if (smokePar != null && smokeP && fire < maxfire / 4) // Instancie les particules de fumée
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
    }

    public void TakeDamage(int viewID, int damage, Photon.Realtime.Player Killer)
    {
        if ((string)Killer.CustomProperties["Team"] == "ATT" && viewID == view.ViewID && health > 0) //Test si on est le bâtiment
        {
            if (health>damage) //Diminution de la vie
            {
                health-=damage;
                view.RPC("SyncBat", RpcTarget.Others, health, fire, alive);
            }
            else
            {
                GameStat.changeScore(50, 0);
                
                health = 0;
                view.RPC("SyncBat", RpcTarget.All, health, fire, alive);
            }
        }
    }


    IEnumerator Fire()
    {
        if (alive && fire > 0)
        {
            health -= fire;

            if (health <= 0)
            {
                GameStat.changeScore(50, 0);

                health = 0;
                view.RPC("SyncBat", RpcTarget.All, health, fire, alive);
            }
            else
            {
                view.RPC("SyncBat", RpcTarget.Others, health, fire, alive);
                yield return new WaitForSeconds(1f);

                StartCoroutine(Fire());
            }
        }
    }
    

    IEnumerator Anims()
    {
        if (alive && PhotonNetwork.IsMasterClient)
            PhotonNetwork.Instantiate("BuildingHelpPNJ", transform.position - new Vector3(UnityEngine.Random.Range(-5.0f, 5.0f), 50, UnityEngine.Random.Range(-5.0f, 5.0f)), transform.rotation);

        alive = false;
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
        GetComponent<Collider>().enabled = false;
        if (smokePar != null)
            Destroy(smokePar);

        if (firePar != null)
            Destroy(firePar);
    }

    [PunRPC]
    public void SetFire(float amount)
    {
        if (fire + amount > maxfire)
        {
            fire = maxfire;
            return;
        }
        if (fire + amount < 0)
        {
            fire = 0;
            return;
        }
        if (fire == 0 && PhotonNetwork.IsMasterClient)
        {
            fire += amount;
            StartCoroutine(Fire());
            return;
        }
        fire += amount;
        view.RPC("SyncBat", RpcTarget.Others, health, fire, alive);
    }

    [PunRPC]
    public void SyncBat(float health, float fire, bool alive)
    {
        this.health = health;
        this.fire = fire;
        if (this.health == 0)
        {
            if (alive)
                StartCoroutine(Anims());
            else
            {
                alive = false;
                transform.position -= new Vector3(0, 200, 0);
            }
        }
    }
}
