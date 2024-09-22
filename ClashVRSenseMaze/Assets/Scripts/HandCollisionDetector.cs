using UnityEngine;

public class HandCollisionDetector : MonoBehaviour
{
    public LoggerScript logger;
    public string handName; // "LeftHand" or "RightHand"

    private void OnCollisionEnter(Collision collision)
    {
        logger.LogCollision(collision.gameObject, handName);
    }

    private void OnTriggerEnter(Collider other)
    {
        logger.LogCollision(other.gameObject, handName);
    }
}
