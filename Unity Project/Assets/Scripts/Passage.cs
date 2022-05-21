using UnityEngine;

public class Passage : MonoBehaviour{
    public Transform connection;
    public int type;

    public void Start(){
        if(type == 0){
            connection = GameObject.FindGameObjectWithTag("ConnRight").transform;
        } else{
            connection = GameObject.FindGameObjectWithTag("ConnLeft").transform;
        }
    }
    public void OnTriggerEnter2D(Collider2D other) {
        Vector3 position = other.transform.position;
        position.x = this.connection.position.x;
        position.y = this.connection.position.y;

        other.transform.position = position;
    }
}
