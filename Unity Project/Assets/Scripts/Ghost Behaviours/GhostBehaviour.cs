using System;
using UnityEngine;

[RequireComponent(typeof(GhostV2))]
public abstract class GhostBehaviour : MonoBehaviour{
    // Start is called before the first frame update
    public GhostV2 ghost { get; private set; }
    public float duration;

    public void Awake(){
        this.ghost = GetComponent<GhostV2>();
        this.enabled = false;
    }

    public void Enable() {
        Enable(this.duration);
    }

    public virtual void Enable(float duration){
        this.enabled = true;

        //Remove existing timers
        CancelInvoke();
        //Start new timer
        Invoke(nameof(Disable),duration);
    }

    public virtual void Disable(){
        this.enabled = false;
        CancelInvoke();
    }

    public abstract State getState();

}
