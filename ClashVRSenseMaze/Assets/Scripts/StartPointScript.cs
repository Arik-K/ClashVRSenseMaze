// StartPointScript.cs attached to the Cube/Trigger GameObject at the maze start point

using UnityEngine;

public class StartPointScript : MonoBehaviour
{
    public GameObject instructionPanel; // Reference to the panel containing instructions

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Show instruction panel
            instructionPanel.SetActive(true);

            // Call the NextLevel function in TrainingManager
            TrainingManager trainingManager = FindObjectOfType<TrainingManager>();
            if (trainingManager != null)
            {
                trainingManager.NextLevel();
            }

            // Optionally, disable the cube/trigger after triggering once
            gameObject.SetActive(false);
        }
    }*/
}
