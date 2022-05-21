using System;
using UnityEngine;

public class GhostFlee : GhostBehaviour {
    
    public float eyesduration;

    public bool eaten { get; private set; }

    public GhostFlee(int duration){
        this.duration = duration;
    }

    public override void Enable(float duration){
        base.Enable(duration);

        ghost.body.enabled = false;
        ghost.eyes.enabled = false;
        ghost.blue.enabled = true;
        ghost.white.enabled = false;

        Invoke(nameof(Flash), duration / 10f);
    }

    public override void Disable(){
        base.Disable();

        ghost.body.enabled = true;
        ghost.eyes.enabled = true;
        ghost.blue.enabled = false;
        ghost.white.enabled = false;

        //ghost.fsm.newState = State.Chase;
    }

    private void Eaten(){
        eaten = true;
        ghost.SetPosition(ghost.inside.position);
        ghost.gameObject.layer = LayerMask.NameToLayer("Ignored");
        CancelInvoke();

        ghost.body.enabled = false;
        ghost.eyes.enabled = true;
        ghost.blue.enabled = false;
        ghost.white.enabled = false;

        Invoke(nameof(TriggerNext),eyesduration);

    }

    void TriggerNext() {
        ghost.fsm.newState = State.Idle;
    }

    private void Flash(){
        if (!eaten){
            ghost.blue.enabled = false;
            ghost.white.enabled = true;
            ghost.white.GetComponent<AnimatedSprite>().Restart();
            Invoke(nameof(Flash2),duration/10f);
        }
    }

    private void Flash2(){
        if (!eaten) {
            ghost.blue.enabled = true;
            ghost.white.enabled = false;
            ghost.blue.GetComponent<AnimatedSprite>().Restart();
            Invoke(nameof(Flash), duration / 10f);
        }
    }

    public override State getState() {
        return State.Flee;
    }

    private void OnEnable(){
        ghost.fsm.newState = State.Flee;
        ghost.blue.GetComponent<AnimatedSprite>().Restart();
        ghost.movement.speedMult = 0.5f;
        eaten = false;
    }

    private void OnDisable(){
        ghost.movement.speedMult = 1f;
        ghost.fsm.newState = State.Scatter;
        eaten = false;
        CancelInvoke();
        ghost.gameObject.layer = LayerMask.NameToLayer("Enemy");
    }

    private void OnTriggerEnter2D(Collider2D other){
        Node node = other.GetComponent<Node>();

        if (node != null && enabled){
            Vector2 direction = Vector2.zero;
            float maxDistance = float.MinValue;

            // Find the available direction that moves farthest from pacman
            foreach (Vector2 availableDirection in node.availableDirections) {
                // If the distance in this direction is greater than the current
                // max distance then this direction becomes the new farthest
                Vector3 newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y);
                float distance = (ghost.player.position - newPosition).sqrMagnitude;

                if (distance > maxDistance){
                    direction = availableDirection;
                    maxDistance = distance;
                }
            }

            ghost.movement.SetDirection(direction);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")){
            if (enabled){
                Eaten();
            }
        }
    }


}