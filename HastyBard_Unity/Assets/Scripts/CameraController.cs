using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float offsetX = 2f;

    void LateUpdate()
    {
        // Move the camera horizontally to follow the player's position
        Vector3 newPos = transform.position;
        newPos.x = player.position.x + offsetX;
        transform.position = newPos;
    }
}