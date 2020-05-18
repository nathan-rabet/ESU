using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float radius = 3f;

    //AMMO
    
    //SHIELD
    
    //SPEED

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
