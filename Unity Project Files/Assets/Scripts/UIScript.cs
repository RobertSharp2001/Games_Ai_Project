using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour{

    public GameManager gameManager;
    public int score;
    public int lives;

    public Text scoreText;
    public Text livesText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update(){
        score = gameManager.score;
        lives = gameManager.lives;

        scoreText.text = score.ToString();
        livesText.text = lives.ToString();
    }
}
