using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float moveSpeed;
    PlayerController player;

    float zOffset;

    private void Start() {
        player = FindObjectOfType<PlayerController>();
        zOffset = transform.position.z;
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, zOffset);
    }
    private void FixedUpdate() {
        // Get the player position plus the vertical offset.
        float distance = Vector2.Distance(transform.position, player.transform.position); // Calculate the distance between the camera and the player;
        Vector2 direction = (player.transform.position - transform.position).normalized; // Find the direction of the player.

        // Calculate the velocity of the camera
        Vector2 velocity = direction * distance * Time.deltaTime * moveSpeed;
        if (velocity.magnitude > .1 * Time.deltaTime)
        {
            // Move the camera by the velocity
            transform.Translate(velocity); // Move the camera towards the player location.   
            transform.position = new Vector3(transform.position.x, transform.position.y, zOffset);
        }
    }
}
