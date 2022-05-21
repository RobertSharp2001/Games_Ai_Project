using UnityEngine;

public class GhostChase : GhostBehaviour{
    private void OnDisable(){
        this.ghost.fsm.newState = State.Scatter;
    }

    public void OnEnable(){
        if (ghost.playerTarget != null)
        {
            ghost.playerTarget.state = TarState.Chase;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        
        Node node = other.GetComponent<Node>();

        if (node != null && this.enabled && this.ghost.fsm.myState != State.Flee){
            Vector2 direction = Vector2.zero;
            float minDistance = float.MaxValue;

            // Find the available direction that moves closet to pacman
            foreach (Vector2 availableDirection in node.availableDirections){
                Vector3 newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y);
                float distance = (ghost.player.position - newPosition).sqrMagnitude;

                if (distance < minDistance){
                    direction = availableDirection;
                    minDistance = distance;
                }
            }

            ghost.movement.SetDirection(direction);
        }
    }

    public override State getState() {
        return State.Chase;
    }
}
