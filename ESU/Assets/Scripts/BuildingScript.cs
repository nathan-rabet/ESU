using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Animations;
using Photon;

public class BuildingScript : MonoBehaviour
{
    public int health = 1000;
    private PhotonView view;
    private GameStat GameStat;

    private void Start()
    {
        view = GetComponent<PhotonView>(); //Cherche la vue
        GameStat = GameObject.Find("/GAME/GameManager").GetComponent<GameStat>();
    }
    
    public void TakeDamage(int viewID, int damage, Photon.Realtime.Player Killer)
    {
        if (viewID == view.ViewID && health > 0) //Test si on est le bâtiment
        {
            if (health>damage) //Diminution de la vie
            {
                health-=damage;
                view.RPC("SyncBat", RpcTarget.Others, health);
            }
            else
            {
                string myClass = (string)Killer.CustomProperties["Team"];
                GameStat.changeScore(50, 0);
                    
                view.RPC("SyncBat", RpcTarget.Others, health);

                StartCoroutine(Anims());
                
                health = 0;
            }
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
    public void SyncBat(int health)
    {
        this.health = health;
        if (this.health == 0)
        {
            StartCoroutine(Anims());
        }
    }
}
