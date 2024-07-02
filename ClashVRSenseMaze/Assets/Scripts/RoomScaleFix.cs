
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(XROrigin))]

public class RoomScaleFix : MonoBehaviour
{
   private CharacterController _characrter;
    private XROrigin _xrOrigin;

    
    
    // Start is called before the first frame update
    void Start()
    {
         _characrter = GetComponent<CharacterController>();
         _xrOrigin = GetComponent<XROrigin>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var centerPoint = transform.InverseTransformPoint(_xrOrigin.Camera.transform.position);
        _characrter.center = new Vector3(centerPoint.x, _characrter.height/2 + _characrter.skinWidth, centerPoint.z);

        //_characrter.Move(new Vector3(0.001f, -0.001f, 0.001f));
        //_characrter.Move(new Vector3(-0.001f, -0.001f, -0.001f));
    }

    
}
