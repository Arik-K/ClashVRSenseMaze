using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WallTouch : MonoBehaviour
{
    public XRBaseController controller;
    public float vibrationIntensity = 0.2f;
    public float vibrationDuration = 0.1f;
    private Coroutine hapticCoroutine;

   public bool isWallTouchEnabled = true;
    private void OnTriggerEnter(Collider other)
    {
        if (!isWallTouchEnabled) return;

        if(other.gameObject.CompareTag("Goal")|| other.gameObject.CompareTag("FalseGoal"))
        {
           StopHapticFeedback(controller); 
        }
        
        if (other.gameObject.CompareTag("Wall") ||other.gameObject.CompareTag("Mute") || other.gameObject.CompareTag("HapticGhost")|| other.gameObject.CompareTag("Invisible"))
        {
            if (hapticCoroutine == null)
            {
                hapticCoroutine = StartCoroutine(ContinuousHapticFeedback());
            }
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Wall") ||other.gameObject.CompareTag("Mute") || other.gameObject.CompareTag("HapticGhost")|| other.gameObject.CompareTag("Invisible"))
         
        {
            if (hapticCoroutine != null)
            {
                StopCoroutine(hapticCoroutine);
                hapticCoroutine = null;
            }
            StopHapticFeedback(controller);
        }
    }

    private IEnumerator ContinuousHapticFeedback()
    {
        while (true)
        {
            TriggerHapticFeedback(controller);
            yield return new WaitForSeconds(vibrationDuration);
        }
    }

    private void TriggerHapticFeedback(XRBaseController controller)
    {
        if (controller != null)
        {
            controller.SendHapticImpulse(vibrationIntensity, vibrationDuration);
        }
    }

    public void StopHapticFeedback(XRBaseController controller)
    {
        if (controller != null)
        {
            controller.SendHapticImpulse(0, 0);
        }
    }
}
