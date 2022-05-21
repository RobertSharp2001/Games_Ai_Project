using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Target : MonoBehaviour{
    // Start is called before the first frame update

    public GameObject player;
    public TarState state;
    public BoxCollider2D scatterTransform;
    public List<Vector3> Waypoints;
    public int waypointIndex = 0;
    public Vector3 Temp;

    public float time;

    void Start(){
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate(){
        Waypoints.Clear();
        
        if (scatterTransform != null) {
            Bounds boxBounds = scatterTransform.bounds;

            Waypoints.Add(new Vector3(boxBounds.center.x + boxBounds.extents.x, boxBounds.center.y + boxBounds.extents.y, 0)); //Top right corner
            Waypoints.Add(new Vector3(boxBounds.center.x, boxBounds.center.y + boxBounds.extents.y, 0)); //Mid Center Top
            Waypoints.Add(new Vector3(boxBounds.center.x - boxBounds.extents.x, boxBounds.center.y + boxBounds.extents.y, 0));//Top Left corner
            Waypoints.Add(new Vector3(boxBounds.center.x - boxBounds.extents.x, boxBounds.center.y + (boxBounds.extents.y * 0.4f), 0));//Mid Left Upper
            Waypoints.Add(new Vector3(boxBounds.center.x - (boxBounds.extents.x * 0.4f), boxBounds.center.y + (boxBounds.extents.y * 0.4f), 0));//Mid left inner
            Waypoints.Add(new Vector3(boxBounds.center.x - (boxBounds.extents.x * 0.4f), boxBounds.center.y - (boxBounds.extents.y * 0.4f), 0));//Mid left inner
            Waypoints.Add(new Vector3(boxBounds.center.x - boxBounds.extents.x, boxBounds.center.y - (boxBounds.extents.y * 0.4f), 0));//Mid Left Lower
            Waypoints.Add(new Vector3(boxBounds.center.x - boxBounds.extents.x, boxBounds.center.y - boxBounds.extents.y, 0));//Bottom Left corner
            Waypoints.Add(new Vector3(boxBounds.center.x, boxBounds.center.y - boxBounds.extents.y, 0));//Mid Center Bottom
            Waypoints.Add(new Vector3(boxBounds.center.x + boxBounds.extents.x, boxBounds.center.y - boxBounds.extents.y, 0));//Bottom Right corner
            Waypoints.Add(new Vector3(boxBounds.center.x + boxBounds.extents.x, boxBounds.center.y - (boxBounds.extents.y * 0.4f), 0));//Mid Right Lower
            Waypoints.Add(new Vector3(boxBounds.center.x + (boxBounds.extents.x * 0.4f), boxBounds.center.y - (boxBounds.extents.y * 0.4f), 0));//Mid Right Inner
            Waypoints.Add(new Vector3(boxBounds.center.x + (boxBounds.extents.x * 0.4f), boxBounds.center.y + (boxBounds.extents.y * 0.4f), 0));//Mid Right Inner
            Waypoints.Add(new Vector3(boxBounds.center.x + boxBounds.extents.x, boxBounds.center.y + (boxBounds.extents.y * 0.4f), 0));//Mid Right Upper

        }


        if (state == TarState.Chase) {
            ChaseBehaviour();
        } else if(state==TarState.Scatter){
            ScatterBehaviour();
        }
    }

    public abstract void ChaseBehaviour();
    public abstract void ScatterBehaviour();


    public Vector3 RandomPointInBox() {
        if (Waypoints == null) {
            return Vector3.zero;
        }

        if (waypointIndex > Waypoints.Count || waypointIndex < 0) {
            waypointIndex = 0;
        }

        Vector3 output = Waypoints[0];

        for (int i = 0; i < Waypoints.Count; i++){
            if(i == waypointIndex){
                output = Waypoints[i];
                break;
            }
        }
        waypointIndex++;

        return output;
    }


    // public abstract void FleeBehaviour();
}
