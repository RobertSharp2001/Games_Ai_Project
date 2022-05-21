using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetAmbush : Target{
    // Start is called before the first frame update
    //public GameObject player;
    public Movement playerMovement;
    public GameObject blinky;
    public bool unset;

    /// <summary>
    /// BLUE GHOST TARGET CONTROL
    /// </summary>

    public void Awake(){
        this.scatterTransform = GameObject.FindGameObjectWithTag("BlueBounds").GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    public override void ChaseBehaviour() {

        //Inky tries to move to a location that is calculated by taking the tile two spaces ahead of Pac-Man and doubling the distance Blinky is away from it.
        Vector3 temp = (playerMovement.direction * 2f);
        Vector3 TwoTiles = (player.transform.position + temp);
        //Get the angle from blinky to the player
        float angle = Vector2.Angle(blinky.transform.position, TwoTiles);
        //get distance between the two
        float distance = Vector3.Distance(blinky.transform.position, TwoTiles);
        //quaternion
        var q = Quaternion.AngleAxis(angle, Vector3.forward);
        //update
        transform.position = player.transform.position + q * Vector3.right * (distance / 2);

        //Debug.Log(distance);
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
