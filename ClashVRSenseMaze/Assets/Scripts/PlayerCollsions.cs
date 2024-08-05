using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class CollisionEvent : UnityEvent {}
[System.Serializable]
public class FinishEvent : UnityEvent {}
[System.Serializable]
public class TimeoutEvent : UnityEvent {}


public class PlayerCollsions : MonoBehaviour
{

    public CollisionEvent onWallCollision;
    public CollisionEvent onStartingPointCollision;
    public CollisionEvent onAudioGoalCollision;
    public FinishEvent onFinish;
    public TimeoutEvent onTimeout;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Goal"))
        {
            onFinish?.Invoke();
        }

        if(other.gameObject.CompareTag("StartPoint"))
        {
            onStartingPointCollision?.Invoke();
        }

    }

}

