// GoalScript.cs attached to the Goal GameObject

using UnityEngine;

public class GoalScript : MonoBehaviour
{
    
    public GameManager gm;
    public AudioSource audioSource;
    
    public float maxDistance = 0.25f; // Maximum distance at which the sound starts playing
    public float minDistance = 0.01f;  // Distance at which the sound is at full volume
    public string listenerTag = "MainCamera"; // Tag of the object with the audio listener (usually the player)

    void Start()
    {
        // Get or add AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Configure AudioSource
        audioSource.spatialBlend = 1f; // Make the sound fully 3D
        audioSource.rolloffMode = AudioRolloffMode.Linear; // Use linear rolloff for simplicity
        audioSource.maxDistance = maxDistance;
        audioSource.minDistance = minDistance;
        audioSource.loop = true; // Loop the sound continuously
        audioSource.playOnAwake = true; // Start playing immediately

        // Make sure the GameObject has the correct tag
    }

    void Update()
    {
        
        if (gm.conditions[gm.ConditionCount-1] == "visual_only"|| gm.conditions[gm.ConditionCount-1] == "haptic_only" || gm.conditions[gm.ConditionCount-1] == "audio_off")
        {
            audioSource.volume = 0;
        }

        else{
            // Find the listener (player)
            GameObject listener = GameObject.FindGameObjectWithTag(listenerTag);
            if (listener != null)
            {
                float distance = Vector3.Distance(transform.position, listener.transform.position);
                
                // Adjust volume based on distance
                if (distance <= maxDistance && gameObject.tag == "Goal")
                {
                    // Ensure audio is playing
                    if (!audioSource.isPlaying)
                    {
                        audioSource.Play();
                    }

                    // Calculate volume (1 at minDistance, 0 at maxDistance)
                    float volume = 0.7f - Mathf.Clamp01((distance - minDistance) / (maxDistance - minDistance));
                    audioSource.volume = volume;
                }
                else
                {
                    // Stop audio if listener is too far
                    audioSource.Stop();
                }
            }
        }
        
    }
}
