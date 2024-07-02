using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PitchDistanceRaycast : MonoBehaviour
{
    public Transform xrOrigin; // The XR Origin (e.g., VR camera or player rig)
    public AudioSource continuousAudioSource; // The Audio Source with changing pitch
    public AudioSource collisionAudioSource; // The Audio Source for collision sound
    public AudioClip collisionClip; // The collision sound clip
    public float minDistance = 3f; // The maximum distance to consider for pitch adjustment
    public float minPitch = 0.5f; // Minimum pitch value
    public float maxPitch = 2.0f; // Maximum pitch value
    public float collisionDistanceThreshold = 0.1f; // Distance threshold for collision sound

    private bool hasCollided = false; // Flag to track if a collision has occurred

    void Start()
    {
        if (continuousAudioSource != null && !continuousAudioSource.isPlaying)
        {
            continuousAudioSource.Stop(); // Ensure the continuous audio source is playing
        }
    }

    void Update()
    {
        AdjustPitchBasedOnDistance();
    }

    void AdjustPitchBasedOnDistance()
    {
        if (xrOrigin == null || continuousAudioSource == null || collisionAudioSource == null || collisionClip == null)
        {
            Debug.LogWarning("XR Origin, Continuous Audio Source, Collision Audio Source, or Collision Clip not set.");
            return;
        }

        RaycastHit hit;
        Vector3 originPosition = xrOrigin.position;

        // Perform a raycast from the XR origin forward to detect walls
        if (Physics.Raycast(originPosition, xrOrigin.forward, out hit))
        {
            // Calculate the distance to the wall
            float distance = hit.distance;

            // Check if the distance is within the minDistance range
            if (distance <= minDistance)
            {
                // Check if the distance is very small, indicating a collision
                if (distance < collisionDistanceThreshold)
                {
                    // If a collision was not previously detected, play the collision sound
                    if (!hasCollided)
                    {
                        if (continuousAudioSource.isPlaying)
                        {
                            continuousAudioSource.Stop();
                        }
                        collisionAudioSource.PlayOneShot(collisionClip);
                        hasCollided = true; // Set the flag to indicate a collision has occurred
                    }
                }
                else
                {
                    // Reset the collision flag if not in collision range
                    hasCollided = false;

                    // Ensure the continuous audio source is playing
                    if (!continuousAudioSource.isPlaying)
                    {
                        continuousAudioSource.Play();
                    }

                    // Map the distance to the pitch range
                    float pitch = Mathf.Lerp(maxPitch, minPitch, distance / minDistance);

                    // Set the pitch of the continuous audio source
                    continuousAudioSource.pitch = pitch;
                }
            }
            else
            {
                // Stop the continuous audio source if the player is out of range
                if (continuousAudioSource.isPlaying)
                {
                    continuousAudioSource.Stop();
                }
            }
        }
        else
        {
            // Stop the continuous audio source if no wall is detected
            if (continuousAudioSource.isPlaying)
            {
                continuousAudioSource.Stop();
            }

            // Reset the collision flag if no wall is detected
            hasCollided = false;
        }
    }
}
