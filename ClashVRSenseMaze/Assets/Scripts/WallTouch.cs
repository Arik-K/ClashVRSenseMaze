using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WallTouch : MonoBehaviour
{
    public float hapticDuration = 0.1f; // Duration of the haptic feedback
    public float hapticAmplitude = 0.5f; // Intensity of the haptic feedback

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object that collided with the wall is the player's hand
        if (collision.gameObject.CompareTag("Hand"))
        {
            // Get the XRController component from the colliding object
            XRBaseController controller = collision.gameObject.GetComponent<XRBaseController>();

            if (controller != null)
            {
                // Send haptic feedback to the controller
                SendHapticFeedback(controller, hapticDuration, hapticAmplitude);
            }
        }
    }

    private void SendHapticFeedback(XRBaseController controller, float duration, float amplitude)
    {
        if (controller != null)
        {
            controller.SendHapticImpulse(amplitude, duration);
        }
    }
}
