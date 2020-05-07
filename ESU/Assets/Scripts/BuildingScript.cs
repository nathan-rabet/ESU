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
    private Animation anim;

    private void Start()
    {
        anim = gameObject.GetComponent<Animation>();
        view = GetComponent<PhotonView>(); //Cherche la vue
        GameStat = GameObject.Find("/GAME/GameManager").GetComponent<GameStat>();
    }
    
    public void TakeDamage(int viewID, int damage, Photon.Realtime.Player Killer)
    {
        if (viewID == view.ViewID) //Test si on est le bâtiment
        {
            if (health>damage) //Diminution de la vie
            {
                health-=damage;
                view.RPC("SyncBat", RpcTarget.Others, health);
            }
            else
            {
                if (health>0)
                {
                    string myClass = (string)Killer.CustomProperties["Team"];
                    if (myClass == "DEF")
                    {
                        GameStat.changeScore(0, 10);
                    }else
                    {
                        GameStat.changeScore(10, 0);
                    }
                    view.RPC("SyncBat", RpcTarget.Others, health);
                    //jouer l'anim de mort
                    anim.Play("batiment");
                    Destroy(gameObject, 3f);
                }
                health = 0;
            }
        }
    }

    [PunRPC]
    public void SyncBat(int health)
    {
        this.health = health;
        if (this.health == 0)
        {
            anim.Play("batiment");
            Destroy(gameObject, 3f);
        }
    }
}
