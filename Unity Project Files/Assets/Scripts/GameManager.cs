using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour{
    public GameObject[] GhostsPrefab;

    public List<GameObject> ghosts = new List<GameObject>();
    public bool LevelDone = false;

    public TileData levelMapper;

    //For specifics.
    public GameObject targetBlue;
    public GameObject targetPink;
    public GameObject targetRed;
    public GameObject targetOrange;

    public bool[] ghostsEnabled;

    public int scoreCheck = 2000;
    public int scoreMult = 1;

    public Player player;
    bool firstLevel = true;

    public Transform pellets;
    public Tilemap tilemap;

    public int score { get; private set; }
    public int lives { get; private set; }
    public int startLives;

    public bool allowGenerateLevels = false;
    public bool showTargets = false;

    public int GhostMultiplier { get; private set; } = 1;

    private void Start(){
        NewGame();
    }

    #region restart
    private void NewGame(){
        SetScore(0);
        SetLives(startLives);
        NewRound();
    }

    private void NewRound(){
        foreach(Transform pellet in pellets){
            pellet.gameObject.SetActive(true);
        }
        if (!firstLevel){
            levelMapper.noise.Trigger();
            levelMapper.createNewLevel();
        }

        resetRound();

    }

    private void Update(){
        if (Input.anyKeyDown && this.lives <= 0){
            NewGame();
        }

        if (levelMapper.done){
            LevelDone = true;
        }

        if (Input.GetMouseButtonDown(2)&& allowGenerateLevels) {
            levelMapper.noise.Trigger();
            levelMapper.createNewLevel();
            resetRound();
        }

        if(this.score > (scoreCheck*scoreMult)){
            this.lives++;
            scoreMult++;
        }


        //Method for disabling ghosts
        for (int i = 0; i < this.GhostsPrefab.Length; i++){
            if(this.ghostsEnabled[i] == false){
                this.ghosts[i].SetActive(false);
            } else {
                this.ghosts[i].SetActive(true);
            }
        }

        if (showTargets){
            targetBlue.GetComponent<SpriteRenderer>().enabled = true;
            targetRed.GetComponent<SpriteRenderer>().enabled = true;
            targetPink.GetComponent<SpriteRenderer>().enabled = true;
            targetOrange.GetComponent<SpriteRenderer>().enabled = true;
        } else {
            targetBlue.GetComponent<SpriteRenderer>().enabled = false;
            targetRed.GetComponent<SpriteRenderer>().enabled = false;
            targetPink.GetComponent<SpriteRenderer>().enabled = false;
            targetOrange.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    #endregion

    private void resetRound(){
        resetGhostMultiplier();
        while (ghosts.Count != 0){
            for (int i = 0; i < ghosts.Count; i++){
                GameObject Ghost = ghosts[i];
                this.ghosts.Remove(Ghost);
                Destroy(Ghost);
            }
        }


        for (int i = 0; i < GhostsPrefab.Length; i++){
            GameObject newGhost = Instantiate(GhostsPrefab[i]);
            this.ghosts.Add(newGhost);
        }
        //ghosts[0].GetComponent<GhostV2>().movement.SetDirection(new Vector2(-1,0),true);

        #region TargetVariables
        TargetAmbush x = targetBlue.GetComponent(typeof(TargetAmbush)) as TargetAmbush;
        x.blinky = this.ghosts[0];
        TargetPatrol y = targetOrange.GetComponent(typeof(TargetPatrol)) as TargetPatrol;
        y.player = player.gameObject;
        y.clyde = this.ghosts[3];
        #endregion

        this.player.ResetState();
    }

    private void gameOver(){

        for (int i = 0; i < this.ghosts.Count -1; i++){
            this.ghosts[i].gameObject.SetActive(false);
        }

        this.player.gameObject.SetActive(false);
        firstLevel = false;
    }

    #region setters
    private void SetScore(int score){
        this.score = score;
    }

    private void SetLives(int lives){
        this.lives = lives;
    }

    public void FruitEaten(Fruit fruit){
        Debug.Log("Fruit eaten");
        int points = fruit.points;

        SetScore(this.score + points);
        //fruit.gameObject.SetActive(false);
    }
    #endregion

    #region ghosts
    public void GhostEaten(GhostV2 ghost){
        int points = ghost.points * this.GhostMultiplier;

        SetScore(this.score + points);

        this.GhostMultiplier++;
    }

    public void resetGhostMultiplier(){
        this.GhostMultiplier = 1;

        //Reset the ghosts layer here.
        for (int i = 0; i < this.ghosts.Count -1; i++){

        }
    }
    #endregion



    public void PlayerEaten(){
        this.player.gameObject.SetActive(false);
        SetLives(this.lives - 1);

        if(this.lives > 0){
            Invoke(nameof(resetRound),3.0f);
        } else {
            gameOver();
        }
    }

    public void Quit(){
        Application.Quit();
    }

    #region pellets
    public void PelletEaten(Pellet pellet){
        pellet.gameObject.SetActive(false);
        SetScore(this.score + pellet.points);

        //WIN CONDITION CHNAGE LATER
        if (!hasPelletsRemaining()) {
            this.player.gameObject.SetActive(false);
            Invoke(nameof(NewRound),3.0f);
            firstLevel = false;
        }
    }

    public void PowerPelletEaten(PowerPellet pellet){

        //TODO ghosts state change
        for(int i = 0; i < this.ghosts.Count; i++){
            //If the ghost isn't in the ghost house
            FSM temp = this.ghosts[i].gameObject.GetComponent(typeof(FSM)) as FSM;
            if (temp.myState == State.Idle || temp.myState == State.Home){
            }else{
                temp.newState = State.Flee;
            }
            //this.ghosts[i].fsm.ChangeBehaviour();
            //this.ghosts[i].fsm.SetPelletDuration(pellet.duration);
        }

        PelletEaten(pellet);
        //In case the player eats other pellet
        CancelInvoke();
        //Start the timer to reset the multiplier
        Invoke(nameof(resetGhostMultiplier), pellet.duration);  
    }

    private bool hasPelletsRemaining(){
        GameObject[] pelletsLeft = GameObject.FindGameObjectsWithTag("Pellets");
        if(pelletsLeft.Length == 0){
            return false;
        } else {
            return true;
        }
        /*
        foreach (Transform pellet in pellets){
            //pellet.gameObject.SetActive(true);
            if (pellet.gameObject.activeSelf){
                return true;
            }
        }
        return false;
        */
    }
    #endregion

}
