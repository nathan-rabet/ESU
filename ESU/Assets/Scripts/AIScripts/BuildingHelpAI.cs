using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using UnityEngine.AI;

public class BuildingHelpAI : MonoBehaviour
{
    private bool look;
    private bool isHelp = false;
    private GameObject Text;
    private GameObject mainCam;
    private GameStat GameStat;

    private GameObject[] Dests;
    private NavMeshAgent agent;
    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
        Dests = GameObject.FindGameObjectsWithTag("AISRT");
        agent = GetComponent<NavMeshAgent>();

        Text = transform.Find("Model/HelpText").gameObject;
        Text.SetActive(false);
        mainCam = GameObject.FindWithTag("MainCamera"); //Cherche camera
        GameStat = GameObject.Find("/GAME/GameManager").GetComponent<GameStat>();

        if (PhotonNetwork.IsMasterClient)
            agent.Warp(new Vector3(transform.position.x + Random.Range(-2f, 2f), 0, transform.position.z + Random.Range(-2f, 2f)));
    }

    void Update()
    {
        if (isHelp && PhotonNetwork.IsMasterClient && agent.remainingDistance <= 1)
        {
            GameStat.changeScore(0, 20);
            Destroy(gameObject);
        }
        if (!isHelp && look)
        {
            Text.transform.LookAt(mainCam.transform);
            Text.transform.eulerAngles = new Vector3(Text.transform.eulerAngles.x, Text.transform.eulerAngles.y + 180, Text.transform.eulerAngles.z);

            RaycastHit hit;
            if (Input.GetKeyDown(KeyCode.E))
            {
                //if (hit.collider.gameObject == gameObject)
                //{
                    GameStat.changeScore(0, 20);
                    isHelp = true;
                    Destination();
                    _animator.SetBool("soin", true);
                //}
            }
        }
    }

    void Destination()
    {
        int rd = Mathf.FloorToInt(Random.Range(0, Dests.Length - 1));
        Transform dest = Dests[rd].transform;
        agent.avoidancePriority = 99;
        agent.speed = 6;
        agent.SetDestination(dest.position);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isHelp && other.tag == "Player" && other.GetComponent<PhotonView>().IsMine && (other.GetComponent<Player_Manager>().myClass == Player_Manager.Classe.Pompier || other.GetComponent<Player_Manager>().myClass == Player_Manager.Classe.Medecin))
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
