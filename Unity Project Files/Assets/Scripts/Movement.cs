using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour{
    public Rigidbody2D rb { get; private set; }

    public float speed = 8.0f;
    public float speedMult = 1.0f;

    public Vector2 initalDirection;
    public Vector2 direction { get; private set; }
    public Vector2 showDirection { get; private set; }
    public Vector2 nextDirection { get; private set; }

    public LayerMask obstacleLayer;

    private Vector3 startingPos;
    private Vector3 overrideCheck;
    public Vector3 check;

    public bool toggle = true;
    public bool takeover;

    private void Awake(){
        this.rb = GetComponent<Rigidbody2D>();
        this.startingPos = this.transform.position;
    }

    private void Start(){
        ResetState();
    }

    public void ResetState(){
        this.speedMult = 1.0f;
        this.direction = initalDirection;
        this.nextDirection = Vector2.zero;
        this.transform.position = this.startingPos;
        this.rb.isKinematic = false;

        this.enabled = true;
    }
    //Use fixed update for phyics to allow the game's phyiscs to be consistant.
    private void FixedUpdate(){
        Vector2 position = this.rb.position;
        Vector2 translation = this.direction * this.speed * this.speedMult * Time.fixedDeltaTime;
        this.rb.MovePosition(position+translation);

        overrideCheck = this.transform.position;
        Invoke(nameof(CheckOveride), 0.05f);

        if (toggle){
            check = this.transform.position;
            Invoke("InvertDirection", 1.0f);
        }
    }

    private void Update(){
        showDirection = direction;   

        if (this.nextDirection != Vector2.zero){
            SetDirection(this.nextDirection, takeover);
        }
    }

    public void CheckOveride(){
        //Allow the player to change direction if they get suck on something.
        if (this.transform.position == overrideCheck && this.gameObject.tag== "Player"){
            takeover = true;
        } else {
            takeover = false;
        }
    }

    public void SetDirection(Vector2 direction, bool forced = false){
        if (forced || (!Occupied(direction))){
            this.direction = direction;
            this.nextDirection = Vector2.zero;
        } else {
            this.nextDirection = direction;
        }
    }

    public void InvertDirection(){
        //If the ghost is this in the same postion after 3 seconds, meaning it's stuck.
        if(check == this.transform.position && this.gameObject.tag != "Player"){
            //times direction by minus 1 to invert it;
            this.direction *= -1;
        }
    }

    public bool Occupied(Vector2 direction){
        RaycastHit2D hit = Physics2D.BoxCast(this.transform.position, Vector2.one * 0.75f, 0.0f, direction, 1.5f, this.obstacleLayer);
        return hit.collider != null;
    }
}
