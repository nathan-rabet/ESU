using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetColorScript : MonoBehaviour
{
    private static List<Color> colors = new List<Color> 
    { 
        Color.black,
        Color.blue,
        Color.cyan,
        Color.gray,
        Color.green,
        Color.grey,
        Color.magenta,
        Color.red,
        Color.white,
        Color.yellow
    };

    public static void SetColor(GameObject character)
    {
        Renderer ren = character.GetComponent<Renderer>();
        string t = "";
        foreach (Material mat in ren.materials)
            t += mat.name + " ";
        Debug.Log(t);
        ren.materials[2].color = colors[Mathf.FloorToInt(Random.Range(0, 9.9f))];
        ren.materials[4].color = colors[Mathf.FloorToInt(Random.Range(0, 9.9f))];
        ren.materials[6].color = colors[Mathf.FloorToInt(Random.Range(0, 9.9f))];
        ren.materials[7].color = colors[Mathf.FloorToInt(Random.Range(0, 9.9f))];
        ren.materials[8].color = colors[Mathf.FloorToInt(Random.Range(0, 9.9f))];
    }
}
