using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps; //KEEP THIS YOU FUCK
//using Unity2D;
//using Tilemap;

public class GhostV2 : MonoBehaviour{
    public Movement movement { get; private set; }
    //public GhostChase chaseBehaviour { get; private set; }
    //public GhostFlee fleeBehaviour { get; private set; }
    //public GhostHome homeBehaviour { get; private set; }
    //public GhostScatter scatterBehaviour { get; private set; }
    //public GhostBehaviour initialBehavior;

    public FSM fsm;

    public SpriteRenderer body;
    public SpriteRenderer eyes;
    public SpriteRenderer blue;
    public SpriteRenderer white;

    public Transform player; //Use tagrtes instead
    public Target playerTarget; //Use tagrtes instead
    public string Tagname;
    public int points = 200;
    public Transform inside;
    public Transform outside;


    public void Awake(){
        this.movement = GetComponent<Movement>();
        this.fsm = GetComponent<FSM>();

        this.inside = GameObject.FindGameObjectWithTag("Inside").transform;
        this.outside = GameObject.FindGameObjectWithTag("Outside").transform;
        this.player = GameObject.FindGameObjectWithTag(Tagname).transform;
        this.playerTarget = player.gameObject.GetComponent<Target>();
        //Setup finite state machine;
        //fsm = new FSM();
        //fsm.Setup(inside, outside);
        /*
        this.chaseBehaviour = GetComponent<GhostChase>();
        this.fleeBehaviour = GetComponent<GhostFlee>();
        this.scatterBehaviour = GetComponent<GhostScatter>();
        */
        //this.homeBehaviour = GetComponent<GhostHome>();
    }

    private void Start() {
        //ResetState();
        this.movement.enabled = true; //Should be true
    }


    public void ResetState(){
        this.gameObject.SetActive(true);
        this.fsm.ResetToInit();
        this.movement.ResetState();

    }

    public void SetPosition(Vector3 position){
        // Keep the z-position the same since it determines draw depth
        position.z = transform.position.z;
        transform.position = position;
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")){
            if (fsm.myState == State.Flee){
                FindObjectOfType<GameManager>().GhostEaten(this);
            }
            else{
                FindObjectOfType<GameManager>().PlayerEaten();
            }
        }
    }
}
