using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitManager : MonoBehaviour{
    // Start is called before the first frame update
    public Sprite[] FruitSprites;
    public GameObject[] Fruits;
    public GameManager gameManager;
    public float fruitDelay;

    void Start(){
        gameManager = GetComponent<GameManager>();
        Fruits = GameObject.FindGameObjectsWithTag("Fruit");
    }

    public void StartTrigger(){
        for (int i = 0; i < Fruits.Length; i++){
            if (Fruits[i] != null) {
                Fruits[i].GetComponent<Fruit>().sprite.enabled = true;
                Fruits[i].GetComponent<Fruit>().box.enabled = true;
                Invoke(nameof(StartTrigger), 5f);
            }
        }
    }

    public void SwitchFruit(Fruit fruit){
        if(fruit == null){
            return;
        }

        //If the fruit type is out of range, turn it off
        if(fruit.index > 7 || fruit.index < 0){
            fruit.gameObject.SetActive(false);
        }

        for(int i = 0; i<FruitSprites.Length;i++)
            switch (fruit.index) {
                case 0:
                    fruit.sprite.sprite = FruitSprites[0]; //Cherry
                    fruit.points = 100;
                    break;
                case 1:
                    fruit.sprite.sprite = FruitSprites[1]; //Strawberry
                    fruit.points = 300;
                    break;
                case 2:
                    fruit.sprite.sprite = FruitSprites[2]; //Peach
                    fruit.points = 500;
                    break;
                case 3:
                    fruit.sprite.sprite = FruitSprites[3]; //Apple
                    fruit.points = 700;
                    break;
                case 4:
                    fruit.sprite.sprite = FruitSprites[4]; //Grapes
                    fruit.points = 1000;
                    break;
                case 5:
                    fruit.sprite.sprite = FruitSprites[5]; //Galaxian
                    fruit.points = 2000;
                    break;
                case 6:
                    fruit.sprite.sprite = FruitSprites[6]; //Bell
                    fruit.points = 3000;
                    break;
                case 7:
                    fruit.sprite.sprite = FruitSprites[7]; //Key
                    fruit.points = 5000;
                    break;
            }

    }

    public void FruitEaten(Fruit fruit){
        if (fruit != null) {
            gameManager.FruitEaten(fruit);
            fruit.index++;
            fruit.sprite.enabled = false;
            fruit.box.enabled = false;
            SwitchFruit(fruit);
            Invoke(nameof(RespawnFruit), fruitDelay);
        }

    }

    private void RespawnFruit(){
        for(int i =0; i < Fruits.Length; i++){
            if (Fruits[i] != null) {
                if (Fruits[i].GetComponent<Fruit>().sprite.enabled == false){
                    Fruits[i].GetComponent<Fruit>().sprite.enabled = true;
                    Fruits[i].GetComponent<Fruit>().box.enabled = true;
                }
            }

        }
    }

    // Update is called once per frame
    void Update(){
        for (int i = 0; i < Fruits.Length; i++){
            if (Fruits[i] != null){
                if (Fruits[i].GetComponent<Fruit>().on == false){
                    SwitchFruit(Fruits[i].GetComponent<Fruit>());
                    Fruits[i].GetComponent<Fruit>().sprite.enabled = false;
                    Fruits[i].GetComponent<Fruit>().box.enabled = false;
                    Fruits[i].GetComponent<Fruit>().on = true;
                    Invoke(nameof(StartTrigger), 5f);
                }
            }

        }
    }
}
