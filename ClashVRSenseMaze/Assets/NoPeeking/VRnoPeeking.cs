using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;



public class VRnoPeeking : MonoBehaviour
{
    [SerializeField] LayerMask CollisionLayer;
    [SerializeField] float fadeSpeed;
    [SerializeField] float sphereCheckSize = .15f;

    private Material cameraFadeMat;
    private bool isCameraFade = false;
    
    private void Awake()
    {
        cameraFadeMat = GetComponent<Renderer>().material;
    }
    // Update is called once per frame
    void Update()
    {
        if(Physics.CheckSphere(transform.position, sphereCheckSize, CollisionLayer, QueryTriggerInteraction.Ignore)){
            CameraFade(1f);
            isCameraFade=true;
        }
        else{
            if(!isCameraFade)
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
