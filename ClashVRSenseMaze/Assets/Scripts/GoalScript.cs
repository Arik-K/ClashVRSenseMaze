// GoalScript.cs attached to the Goal GameObject

using UnityEngine;

public class GoalScript : MonoBehaviour
{
    private TrainingManager trainingManager;

    void Start()
    {
        // Find the TrainingManager script in the scene
        trainingManager = FindObjectOfType<TrainingManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Player reached the goal, notify TrainingManager
            trainingManager.ReachedGoal();
        }
    }
}
