using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class BasicAIScripts : MonoBehaviour
{
    private int behevbehaviour;
    private int nbEardShot;
    private int hide;
    private float wait;

    private GameObject[] Dests;
    private GameObject[] Outs;
    private NavMeshAgent agent;
    private GameStat GameStat;
    private Animator _animator;
    private PhotonView view;

    private AudioSource walk;
    private AudioSource run;


    void Start()
    {
        SetColorScript.SetColor(transform.Find("Model/Character").gameObject);

        _animator = GetComponent<Animator>();
        Outs = GameObject.FindGameObjectsWithTag("AISRT");
        Dests = GameObject.FindGameObjectsWithTag("AIDEST");
        agent = GetComponent<NavMeshAgent>();
        GameStat = GameObject.Find("/GAME/GameManager").GetComponent<GameStat>();
        view = GetComponent<PhotonView>();

        AudioSource[] sources = GetComponents<AudioSource>();
        walk = sources[0];
        run = sources[1];

        behevbehaviour = 0;
        nbEardShot = 0;

        if (PhotonNetwork.IsMasterClient)
        {
            hide = Mathf.FloorToInt(UnityEngine.Random.Range(5, 30));

            Transform dest = Dests[Mathf.FloorToInt(UnityEngine.Random.Range(0, Dests.Length - 1))].transform;
            agent.Warp(dest.position);

            agent.speed = 3;
            agent.avoidancePriority = 99;
            dest = Dests[Mathf.FloorToInt(UnityEngine.Random.Range(0, Dests.Length - 1))].transform;
            agent.SetDestination(dest.position);
        }
        view.RPC("SyncAnim", RpcTarget.All, true, false);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (PhotonNetwork.IsMasterClient && !agent.pathPending)
        {
            if (behevbehaviour == 0 && agent.remainingDistance <= 2)
            {
                Transform dest = Dests[Mathf.FloorToInt(UnityEngine.Random.Range(0, Dests.Length - 1))].transform;
                agent.SetDestination(dest.position);
            }
            if (behevbehaviour == 1 && agent.remainingDistance <= 2)
            {
                GameStat.changeScore(0, 20);
                PhotonNetwork.Instantiate("BasicPNJ", new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                PhotonNetwork.Destroy(gameObject);
            }
            if (behevbehaviour == 2)
            {
                wait -= Time.deltaTime;
                if (wait <= 0)
                {
                    _animator.SetBool("peur", false);
                    _animator.SetBool("cours", true);
                    nbEardShot = 0;
                    headingShots();
                    agent.isStopped = false;
                }
            }
        }
    }

    public void headingShots()
    {
        nbEardShot++;
        if (nbEardShot == 1)
        {
            _animator.SetBool("cours", true);
            float minDist = float.MaxValue;
            Transform closeOut = null;
            foreach (GameObject Out in Outs)
            {
                float dist = Vector3.Distance(Out.transform.position, transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closeOut = Out.transform;
                }
            }
            agent.speed = 8;
            agent.avoidancePriority = 0;
            agent.SetDestination(closeOut.position);

            view.RPC("SyncAnim", RpcTarget.All, false, true);
            behevbehaviour = 1;
        }

        if (nbEardShot == hide)
        {
            _animator.SetBool("peur", true);
            agent.isStopped = true;


            view.RPC("SyncAnim", RpcTarget.All, false, false);


            wait = UnityEngine.Random.Range(3f, 10f);
            behevbehaviour = 2;
        }
    }

    [PunRPC]
    public void SyncAnim(bool walk, bool run)
    {
        if (walk)
            this.walk.Play();
        else
            this.walk.Stop();


        if (run)
            this.run.Play();
        else
            this.run.Stop();
    }
}
