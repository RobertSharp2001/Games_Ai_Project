using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour{

    public LayerMask obstacleLayer;
    public GameManager gameManager;
    public List<Vector2> availableDirections { get; private set; }

    private void Start(){
        this.availableDirections = new List<Vector2>();
        this.gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        //Debug.Log("Node script is running correctly");
    }

    private void CheckAvailableDirection(Vector2 direction){
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.5f, 0f, direction, 1f, obstacleLayer);
        if (hit.collider == null){ 
            this.availableDirections.Add(direction);
        } else{
            //Debug.Log("Raycast Hit! " + hit.collider.name);
        }
    }

    public void Update(){
        availableDirections = new List<Vector2>();
        
        CheckAvailableDirection(Vector2.up);
        CheckAvailableDirection(Vector2.down);
        CheckAvailableDirection(Vector2.left);
        CheckAvailableDirection(Vector2.right);
    }

    public Vector2 getDirection(int index){
        return availableDirections[index];
    }

    public bool isInAvailable(Vector2 direction){
        for(int i = 0; i <=availableDirections.Count-1; i++){
            if(direction == availableDirections[i]){
                return true;
            }
        }
        return false;
    }
}
