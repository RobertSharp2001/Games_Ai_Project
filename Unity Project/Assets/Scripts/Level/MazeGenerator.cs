using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Create a texture and fill it with Perlin noise.
// Try varying the xOrg, yOrg and scale values in the inspector
// while in Play mode to see the effect they have on the noise.

public class MazeGenerator : MonoBehaviour{
    // Width and height of the texture in pixels.
    public GameObject dirtPrefab;
    private GameObject C;
    public int _SizeX = 14;
    public int _SizeY = 30;
    private int seed;

    public bool showPrefabs;

    public int[,] grid;

    public void GenerateMaze(){

        int startX = Random.Range(0, _SizeX);
        int startY = Random.Range(0, _SizeY);
        grid = new int[_SizeX, _SizeY];
        /*
        Debug.Log(Mathf.PerlinNoise(0f, 0f));
        Debug.Log(Mathf.PerlinNoise(0.1f, 0.1f));
        Debug.Log(Mathf.PerlinNoise(0.5f, 0.5f));
        */

        grid[startX, startY] = 1;
        MazeDigger(startX, startY);
        //cleanUp();

        if (showPrefabs) {
            for (int i = 0; i < grid.GetLength(0); i++) {
                for (int j = 0; j < grid.GetLength(1); j++){
                    if (grid[i,j]==0) {
                        float width = dirtPrefab.transform.lossyScale.x / 2.2f;
                        float height = dirtPrefab.transform.lossyScale.y / 2.2f;

                        C = (GameObject)Instantiate(dirtPrefab, new Vector3(i * width - 14f, j * height - 16f, 0), Quaternion.identity);
                        C.tag = "Maze";
                        SpriteRenderer Sr1 = C.GetComponent<SpriteRenderer>();
                        Sr1.color = Color.green;
                    }
                }
            }
        }
    }

    public void Trigger(){
        GameObject[] allMaze = GameObject.FindGameObjectsWithTag("Maze");
        for (int i = 0; i < allMaze.Length; i++){
            Destroy(allMaze[i]);
        }

        GenerateMaze();
            //RegeneratePerlin();
    }


    //Shuffling directiosn for the maze;
    public void Shuffle<T>(T[] array){
        System.Random _random = new System.Random();
        for (int i = array.Length; i > 1; i--) {
            // Pick random element to swap.
            int j = _random.Next(i); // 0 <= j <= i-1
                                     // Swap.
            T tmp = array[j];
            array[j] = array[i - 1];
            array[i - 1] = tmp;
        }
        //Thread.Sleep(1);
    }

    #region implementation 2
    public void recursiveGeneration(int x, int y){
        // 4 random directions
        int[] directions = new int[] { 1, 2, 3, 4 };

        //directions = generateRandomDirections();
        Shuffle(directions);

        // Examine each direction
        for (int i = 0; i < directions.Length; i++){

            switch (directions[i]) {
                case 1: // Left
                        //?Whether 2 cells up is out or not
                    if (x - 2 <= 0)
                        continue;
                    if (grid[x - 2, y] != 1){ //This is the only difference, the algorithm will actively avoid walls.
                        grid[x - 2, y] = 1;
                        grid[x - 1, y] = 1;
                        recursiveGeneration(x - 2, y);
                    }
                    break;
                case 2: // Up
                        // Whether 2 cells to the right is out or not
                    if (y + 2 >= _SizeY - 1)
                        continue;
                    if (grid[x, y + 2] != 1){
                        grid[x, y + 2] = 1;
                        grid[x, y + 1] = 1;
                        recursiveGeneration(x, y + 2);
                    }
                    break;
                case 3: // Right
                        // Whether 2 cells down is out or not
                    if (x + 2 >= _SizeX - 1)
                        continue;
                    if (grid[x + 2, y] != 1) {
                        grid[x + 2, y] = 1;
                        grid[x + 1, y] = 1;
                        recursiveGeneration(x + 2, y);
                    }
                    break;
                case 4: // Down
                        // Whether 2 cells to the left is out or not
                    if (y - 2 <= 0)
                        continue;
                    if (grid[x, y - 2] != 1){
                        grid[x, y - 2] = 1;
                        grid[x, y - 1] = 1;
                        recursiveGeneration(x, y - 2);
                    }
                    break;
            }
        }

    }
    #endregion


    #region Implemntation 1
    void MazeDigger(int x, int y){
        int[] directions = new int[] { 1, 2, 3, 4 };
        //Shuffle the values randomly
        Shuffle(directions);

        for (int i = 0; i < directions.Length; i++){
            //For the cell specified by x and y
            //Check each of it directions. 
            if (directions[i] == 1){ //South
                if (y - 3 <= 0) {// IF the Y value would go below 0, then skip it
                    continue;
                }  //Else
                    //if the cell is currently empty
                if (grid[x, y - 3] == 0){
                    //Set those values to walls
                    grid[x, y - 2] = 1;
                    grid[x, y - 1] = 1;
                    grid[x, y - 3] = 1;

                    MazeDigger(x, y - 3);
                }
            }

            if (directions[i] == 2){
                if (x - 2 <= 0)// IF the X value would go below 0, then skip it
                    continue;
                //if the cell is currently empty
                if (grid[x - 2, y] == 0){ //East
                    //Set those values to walls
                    grid[x - 2, y] = 1;
                    grid[x - 1, y] = 1;
                    
                    MazeDigger(x - 2, y);//Then Do the same for for x-2, y
                }
            }

            if (directions[i] == 3){
                if (x + 2 >= _SizeX - 1)// IF the X value would above the size of the maze, then skip it
                    continue;
                //if the cell is currently empty
                if (grid[x + 2, y] == 0) { //West
                    //Set those values to walls
                    grid[x + 2, y] = 1;
                    grid[x + 1, y] = 1;

                    MazeDigger(x + 2, y);//Then Do the same for for x + 2, y
                }
            }

            if (directions[i] == 4){
                if (y + 3 >= _SizeY - 1)// IF the Y value would above the size of the maze, then skip it
                    continue;
                //if the cell is currently empty
                if (grid[x, y + 3] == 0){ //North
                    //Set those values to walls
                    grid[x, y + 3] = 1;
                    grid[x, y + 2] = 1;
                    grid[x, y + 1] = 1;

                    //MazeDigger(x, y + 1);//Then Do the same for for x , y + 2
                    MazeDigger(x, y + 3);//Then Do the same for for x , y + 2
                }
            }
        }
    }
    #endregion

//I don't remember what this was for.
    public void cleanUp(){
        bool[] cardinal = new bool[] { false, false, false, false }; //East, South, North, West
        bool[] diagonal = new bool[] { false, false, false, false }; //SouthEast, SouthWest, NorthWest, NorthEast

        for (int i = 0; i < grid.GetLength(0); i++){
            for (int j = 0; j < grid.GetLength(1); j++){
                #region calcDirections;
                if (i - 1 >= 0){
                    if (grid[i - 1, j] == 1) {
                        cardinal[0] = true;
                    }
                    if (j - 1 >= 0){
                        if (grid[i - 1, j - 1] == 1){
                            diagonal[0] = true;
                        }
                    }
                }
                if (j - 1 >= 0){
                    if (grid[i, j - 1] == 1){
                        cardinal[1] = true;
                    }
                    if (i + 1 <= _SizeX - 1){
                        if (grid[i + 1, j - 1] == 1) {
                            diagonal[1] = true;
                        }
                    }
                }
                if (i + 1 <= _SizeX - 1){
                    if (grid[i + 1, j] == 1){
                        cardinal[2] = true;
                    }

                    if (j + 1 <= _SizeY - 1){
                        if (grid[i + 1, j + 1] == 1){
                            diagonal[2] = true;
                        }
                    }
                }
                if (j + 1 <= _SizeY - 1){
                    if (grid[i, j + 1] == 1){
                        cardinal[3] = true;
                    }

                    if (i - 1 >= 0){
                        if (grid[i - 1, j + 1] == 1){
                            diagonal[3] = true;
                        }
                    }
                }
                #endregion

                /*
                } else if(grid[i, j] == 1 && diagCount >= 3 && cardCount >= 3){
                    grid[i, j] = 0;
                }
                */

                //perlinMap[i, j] = getPerlinValue(i, j);
            }
        }
    }
}