// Tristan Caetano, Samuel Rouillard, Elijah Karpf
// Descend Project
// CIS 464 Project 1

using UnityEngine;

// Script that enables the camera to follow the player
public class CameraFollow : MonoBehaviour
{

    // Target that the camera will follow, ie the player
    public Transform target;

    // Speed at which the camera will smooth out
    public float smoothSpeed = 0.125f;

    // Offset of the camera so the camera doesnt seem to strict
    public Vector3 offset;

    // Method that moves the camera to the player with the desired smoothing and offset
    void LateUpdate(){
        Vector3 desiredPosition = target.position + offset;
		Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
		transform.position = smoothedPosition;
		transform.LookAt(target);
    }
}
