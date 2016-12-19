using UnityEngine;
using System.Collections;

public class TriggerAnimation : MonoBehaviour {
    public string AnimationName;
    public Animator stateMachine;

    public void Trigger()
    {
        stateMachine.SetTrigger(AnimationName);
    }

}