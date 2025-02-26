using UnityEngine;
using UnityEngine.Events;

public class AnimationEventTool : MonoBehaviour
{
    public UnityEvent Use; // UnityEvent to be called in AnimationEvent

    public void TriggerAnimationEvent()
    {
        Use.Invoke(); // Invoke the event
    }
}
