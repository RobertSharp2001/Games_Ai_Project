using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPatrol : Target{
    public GameObject clyde;
    public GameObject target;
    /*
    public GameObject player;
    public BoxCollider2D scatterTransform;
    Vector3 Temp;
    bool lockOnTemp;
    */
    public bool unset;
    public float distValue;

    /// <summary>
    /// ORANGE GHOST TARGET CONTROL
    /// </summary>


    public void Awake() {
        this.scatterTransform = GameObject.FindGameObjectWithTag("OrangeBounds").GetComponent<BoxCollider2D>();
        target = GameObject.FindGameObjectWithTag("Player");
    }

    public override void ChaseBehaviour(){
        float distance = Vector3.Distance(clyde.transform.position, target.transform.position);
        //float distanceToTemp = Vector3.Distance(clyde.transform.position, Temp);

        //If he is farther than eight tiles away, his targeting is identical to Blinky's
        if (distance > distValue && !unset){
            transform.position = player.transform.position;
        } else {
            GhostV2 clydeGhost = clyde.gameObject.GetComponent<GhostV2>();
            if (clydeGhost != null){
                clydeGhost.fsm.overrideState = State.Scatter;
                ScatterBehaviour();
            }

        }
    }


    public override void ScatterBehaviour(){
        if (!unset) {
            transform.position = RandomPointInBox();
            unset = true;
            Invoke(nameof(Reset), time);
        }


    }


    public void Reset() {
        unset = false;
    }
}
