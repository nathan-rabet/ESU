using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource source;

    public AudioClip[] music;

    void Start()
    {
        int r = Random.Range(0, music.Length);

        StartCoroutine(PlayerAudio(-1));

    }

    IEnumerator PlayerAudio(int temp) // temp : indince de la musique précédente
    {
        int r = Random.Range(0, music.Length);
        if (temp != -1)
        {
            while (r == temp) // Si la nouvelle piste est celle d'avant : refait un random
            {
                r = Random.Range(0, music.Length);
            }
        }
        
        source.clip = music[r]; // défini le clip comme étant celui de l'indice r
        source.Play(); // joue le clip audio
        yield return new WaitForSeconds(music[r].length);
        PlayerAudio(r);
    }


}
