using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnimatedSprite : MonoBehaviour{

    public SpriteRenderer spriteRenderer { get; private set; }

    public Sprite[] sprites;

    public float animTime = 0.5f;
    public int animFrame { get; private set; }

    public bool loop = true;

    private void Awake(){
        this.spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start(){
        InvokeRepeating(nameof(Advance), this.animTime, this.animTime);
    }

    private void Advance(){
        if (!this.spriteRenderer.enabled){
            return;
        }
        
        this.animFrame++;

        if(this.animFrame >= this.sprites.Length && this.loop){
            this.animFrame = 0;
        }
        //make sure it's within the limits for error prevention purposes.
        if (this.animFrame >= 0 && this.animFrame < this.sprites.Length){
            this.spriteRenderer.sprite = this.sprites[this.animFrame];
        }

    }
    //reset animation to 0
    public void Restart(){
        this.animFrame = -1;
        Advance();
    }
}
