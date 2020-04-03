using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon;

public class Cheatcode : MonoBehaviour
{
    public GameObject GameManager;
    public PhotonView view;
    // Start is called before the first frame update
    void Start()
    {
        GameManager = GameObject.Find("/GAME/GameManager");
        view = GameManager.GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (view.IsMine)
        {
            if (Input.GetKeyDown("k"))
            {
                transform.GetComponent<Player_Manager>().Death("le Vide", 5);
            }
            if (Input.GetKeyDown("l"))
            {
                view.RPC("changeScore", RpcTarget.Others, 10, 0);
                GameManager.GetComponent<GameStat>().changeScore(10,0);
            }
            if (Input.GetKeyDown("m"))
            {
                view.RPC("changeScore", RpcTarget.Others, 0, 10);
                GameManager.GetComponent<GameStat>().changeScore(0,10);
            }
        }
    }
}
