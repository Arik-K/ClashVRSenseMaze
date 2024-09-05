using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;



public class VRnoPeeking : MonoBehaviour
{
    [SerializeField] LayerMask CollisionLayer;
    List<string> collisionTags = new List<string> {"Wall", "Mute", "Intangable"}; // List of tags to check
    [SerializeField] float fadeSpeed;
    [SerializeField] float sphereCheckSize = .15f;

    private Material cameraFadeMat;
    private bool isCameraFade = false;
    
    private void Awake()
    {
        cameraFadeMat = GetComponent<Renderer>().material;
    }
    // Update is called once per frame
    // Update is called once per frame
    void Update()
    {
        bool shouldFade = false;

        // Check if there's a collision with the specific layer
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, sphereCheckSize, CollisionLayer, QueryTriggerInteraction.Ignore);

        foreach (var hitCollider in hitColliders)
        {
            // Check if the collided object has any of the required tags
            if (collisionTags.Contains(hitCollider.tag))
            {
                shouldFade = true;
                break;
            }
        }

        // Apply fade if we should fade based on the collision check
        if (shouldFade)
        {
            CameraFade(1f);
            isCameraFade = true;
        }
        else
        {
            if (!isCameraFade)
                return;

            CameraFade(0f);
        }
    }

    public void CameraFade(float targetAlpha){
        var fadeValue = Mathf.MoveTowards(cameraFadeMat.GetFloat("_AlphaValue"), targetAlpha, Time.deltaTime * fadeSpeed);
        cameraFadeMat.SetFloat("_AlphaValue", fadeValue);

        if(fadeValue<=0.01f){
            isCameraFade = false;
        }


    }

}
