using UnityEngine;
using UnityEngine.Tilemaps;

public class GroundCheck : MonoBehaviour
{
    public bool isOnGround { get; private set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Map"))
            isOnGround = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Map"))
            isOnGround = false;
    }

}
