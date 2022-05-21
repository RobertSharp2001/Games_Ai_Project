using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour{

    //Home behaviour
    public GhostHome homeBehaviour;
    //Initial Behaviour is virtual and can be changed.
    public GhostBehaviour initBehaviour;
    //Chasing behaviour is virutal, can change.
    public GhostBehaviour chaseBehaviour;
    //Scatter behavour
    public GhostScatter scatterBehaviour;
    //Flee behaviour
    public GhostFlee fleeBehaviour;
    //Idle behaviour
    public GhostIdle idleBehaviour;

    public GhostBehaviour activeBehaviour;

    public State init;
    public State overrideState;
    public State myState;
    public State newState;

    // Start is called before the first frame update
    void Start(){
        this.homeBehaviour = GetComponent<GhostHome>();
        this.chaseBehaviour = GetComponent<GhostChase>();
        this.scatterBehaviour = GetComponent<GhostScatter>();
        this.fleeBehaviour = GetComponent<GhostFlee>();
        this.idleBehaviour = GetComponent<GhostIdle>();
        ResetToInit();
    }

    public void ResetToInit(){
        //How do i reset these fuckers
        this.activeBehaviour = this.initBehaviour;
        overrideState = init;
        this.newState = init;
        ChangeBehaviour();
    }


    public void ChangeBehaviour(){
        if (overrideState != State.Null){
            myState = overrideState;
            overrideState = State.Null;
        } else {
            myState = newState;
        }

        this.activeBehaviour.Disable();

        switch (myState) {
            case State.Home:
                this.activeBehaviour = this.homeBehaviour;
                break;
            case State.Flee:
                this.activeBehaviour = this.fleeBehaviour;
                break;
            case State.Scatter:
                this.activeBehaviour = this.scatterBehaviour;
                break;
            case State.Chase:
                this.activeBehaviour = this.chaseBehaviour;
                break;
            case State.Idle:
                this.activeBehaviour = this.idleBehaviour;
                break;
        }

        this.activeBehaviour.Enable(this.activeBehaviour.duration);
    }

    // Update is called once per frame
    void Update(){
        //Debug.Log("" + myState);
        //if the state has changed.

        if (newState != myState) {
            ChangeBehaviour();
        }       
    }
}
