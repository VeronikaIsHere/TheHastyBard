using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Running : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform endPoint;

    private bool isMoving = true;

    void FixedUpdate()
    {
        if (isMoving)
        {
            // Move the player to the right with a constant speed
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);

            // Check if the player has reached the endpoint
            if (transform.position.x >= endPoint.position.x)
            {
                isMoving = false;
                Debug.Log("Reached endpoint");
            }
        }
    }
}
