using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour{
    public int points;
    public int index;
    public SpriteRenderer sprite;
    public Sprite[] FruitSprites;
    public int SpriteIndex;
    public CircleCollider2D box;
    public bool on = false;


    // Start is called before the first frame update
    void Awake(){
        sprite = GetComponent<SpriteRenderer>();
        box = GetComponent<CircleCollider2D>();

        this.sprite.enabled = false;
        this.sprite.sprite = FruitSprites[0];
        this.box.enabled = false;

        Invoke(nameof(Respawn), 10f);
    }

    private void Respawn(){
        if (this.SpriteIndex >= FruitSprites.Length){
            Destroy(gameObject);
            return;
        }

        this.sprite.sprite = FruitSprites[this.SpriteIndex];
        this.SpriteIndex++;
        this.points = this.points + 200;


        this.sprite.enabled = true;
        this.box.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            Eat();
        }
    }

    protected void Eat(){
        FindObjectOfType<GameManager>().FruitEaten(this);
        this.sprite.enabled = false;
        this.box.enabled = false;
        Invoke(nameof(Respawn), 10f);
    }

}


