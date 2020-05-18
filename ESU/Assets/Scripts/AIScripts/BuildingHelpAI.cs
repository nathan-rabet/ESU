using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using UnityEngine.AI;

public class BuildingHelpAI : MonoBehaviour
{
    private bool look;
    private GameObject Text;
    private GameObject mainCam;
    private GameStat GameStat;

    private GameObject[] Dests;
    private NavMeshAgent agent;

    void Start()
    {
        Dests = GameObject.FindGameObjectsWithTag("AIDEST");

        Text = transform.Find("Model/HelpText").gameObject;
        Text.SetActive(false);
        mainCam = GameObject.FindWithTag("MainCamera"); //Cherche camera
        GameStat = GameObject.Find("/GAME/GameManager").GetComponent<GameStat>();

        //RaycastHit[] f = Physics.RaycastAll(transform.position + new Vector3(0, 2, 0), Vector3.up, 100.0f);
        //int i = 0;
        //Debug.Log(f.Length);
        //while (i < f.Length)
        //{
        //    if (f[i].transform.tag == "Batiment")
        //        i++;
        //    else
        //    {
        //        transform.position = new Vector3(transform.position.x, f[i].transform.position.y, transform.position.z);
        //        i = f.Length + 1;
        //    }
        //}
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }

    void Update()
    {
        if (look)
        {
            Text.transform.LookAt(mainCam.transform);
            Text.transform.eulerAngles = new Vector3(Text.transform.eulerAngles.x, Text.transform.eulerAngles.y + 180, Text.transform.eulerAngles.z);

            RaycastHit hit;
            if (Input.GetKeyDown(KeyCode.E))
            {
                //if (hit.collider.gameObject == gameObject)
                //{
                    GameStat.changeScore(0, 20);
                    Destination();
                //}
            }
        }
    }

    void Destination()
    {
        int rd = (int)Random.Range(0, Dests.Length);
        Transform dest = Dests[0].transform;
        agent.SetDestination(dest.position);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other.GetComponent<PhotonView>().IsMine && (other.GetComponent<Player_Manager>().myClass == Player_Manager.Classe.Pompier || other.GetComponent<Player_Manager>().myClass == Player_Manager.Classe.Medecin))
        {
            look = true;
            Text.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && other.GetComponent<PhotonView>().IsMine)
        {
            look = false;
            Text.SetActive(false);
        }
    }
}
