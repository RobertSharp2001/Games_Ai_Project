using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFour : Target{
    // Start is called before the first frame update
    public GameObject target;
    public Movement movement;
    public bool unset;

    /// <summary>
    /// PINK GHOST TARGET CONTROL
    /// </summary>

    public void Awake(){
        target = GameObject.FindGameObjectWithTag("Player");
        this.scatterTransform = GameObject.FindGameObjectWithTag("PinkBounds").GetComponent<BoxCollider2D>();
    }

    public override void ChaseBehaviour(){
        Vector3 temp = (movement.direction * 4f);
        transform.position = (target.transform.position + temp);
    }

    public override void ScatterBehaviour(){
        if (!unset){
            transform.position = RandomPointInBox();
            unset = true;
            Invoke(nameof(Reset), time);
        }


    }


    public void Reset(){
        unset = false;
    }
}
