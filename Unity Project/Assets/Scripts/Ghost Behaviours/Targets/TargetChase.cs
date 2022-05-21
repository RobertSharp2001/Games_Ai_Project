using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetChase : Target{
    public GameObject target;
    public bool unset;

    /// <summary>
    /// RED GHOST TARGET CONTROL
    /// </summary>

    public void Awake(){
        target = GameObject.FindGameObjectWithTag("Player");
        this.scatterTransform = GameObject.FindGameObjectWithTag("RedBounds").GetComponent<BoxCollider2D>();
    }

    public override void ChaseBehaviour(){
        transform.position = target.transform.position;
    }

    public override void ScatterBehaviour(){
        if (!unset) {
            transform.position = RandomPointInBox();
            unset = true;
            Invoke(nameof(Reset), time);
        }


    }


    public void Reset(){
        unset = false;
    }
}
