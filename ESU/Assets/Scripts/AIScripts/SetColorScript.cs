using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetColorScript : MonoBehaviour
{
    private static List<Color> colors = new List<Color>
    {
        Color.black,
        Color.cyan,
        Color.gray,
        Color.white,
        Color.yellow,
        new Color(0.8f,1f,0.7568f),
        new Color(0.6117f,0.6392f,1f),
        new Color(0.949f,0.6705f,1f),
        new Color(1f,0.5529f,0.5529f)
    };

    public static void SetColor(GameObject character)
    {
        Renderer ren = character.GetComponent<Renderer>();
        float randUp = colors.Count - 0.1f;
        ren.materials[2].color = colors[Mathf.FloorToInt(Random.Range(0, randUp))];
        ren.materials[4].color = colors[Mathf.FloorToInt(Random.Range(0, randUp))];
        ren.materials[6].color = colors[Mathf.FloorToInt(Random.Range(0, randUp))];
        ren.materials[7].color = colors[Mathf.FloorToInt(Random.Range(0, randUp))];
        ren.materials[8].color = colors[Mathf.FloorToInt(Random.Range(0, randUp))];
    }
}
