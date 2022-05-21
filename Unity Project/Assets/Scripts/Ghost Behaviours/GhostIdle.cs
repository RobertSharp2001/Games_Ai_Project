using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostIdle : GhostBehaviour{
    // Start is called before the first frame update
    public Transform StartPoint;
    public Transform LeftStart;
    public Transform MidStart;
    public Transform RightStart;
    public int index;

    public void Start(){

        this.LeftStart = GameObject.FindGameObjectWithTag("leftStart").transform;
        this.MidStart = GameObject.FindGameObjectWithTag("midStart").transform;
        this.RightStart = GameObject.FindGameObjectWithTag("rightStart").transform;

        switch (index){
            case 0:
                StartPoint = LeftStart;
                break;
            case 1:
                StartPoint = MidStart;
                break;
            case 2:
                StartPoint = RightStart;
                break;
        }
    }

    // Update is called once per frame
    private void OnEnable(){
        //Invoke(nameof(TriggerNext), duration);    
    }

    private void OnDisable(){
        ghost.fsm.newState = State.Home;
        //this.ghost.fsm.ChangeBehaviour();
    }

    void Update(){
        ghost.transform.position = StartPoint.position;
    }

    public override State getState() {
        return State.Idle;
    }
}
