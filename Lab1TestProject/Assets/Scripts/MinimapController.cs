using UnityEngine;

public class MinimapController : MonoBehaviour
{
    [SerializeField] private Transform player; // Reference to the player's transform
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.position.x, transform.position.y, player.position.z); // Update the minimap's position to follow the player's position while keeping its own y-coordinate
    }
}
