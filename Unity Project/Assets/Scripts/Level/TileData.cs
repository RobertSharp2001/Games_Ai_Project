using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileVisual{
    None,
    Vertical,
    Horizontal,
    SWCorner,
    NWCorner,
    SECorner,
    NECorner,
    Black,
    Circle,
    TSouth,
    TNorth,
    TEast,
    TWest
}
public class TileData : MonoBehaviour {
    // Start is called before the first frame update
    public bool notActive;

    Tilemap tilemap; 
    Tilemap pellets;
    Tilemap nodes;

    GameManager gm;

    public Tile _wall;
    public Tile[] _alltiles;
    public RuleTile _portalLeft;
    public RuleTile _tunnelLeft;
    public RuleTile _portalRight;
    public RuleTile _tunnelRight;
    public RuleTile _node;
    public RuleTile _pellet;
    public RuleTile _powerpellet;
    public RuleTile _fruit;
    public Tile _empty;
    public PhysicsMaterial2D _physicals;
    public GameObject grid;

    public MazeGenerator noise;

    public int[,] perlinMap;

    public float perlinSize;
    public float perlinSize2;
    public float perlinMagification;

    private int colorIndex = 0;

    public int _SizeX = 13;
    public int _SizeY = 30;

    public bool done = false;

    public void Awake(){
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    //only have to do half of the level becasue it's mirrored
    private int[,] defaultArray = { //Bottom should be on top REVERSE IT
        { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, //
        { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } ,//
        { 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1 } ,//
        { 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1 } ,//
        { 1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1, 1 } ,//5
        { 1, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 1, 1, 1 } ,//
        { 1, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 1, 1, 1 } ,//
        { 1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 } ,//
        { 1, 0, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1 } ,//
        { 1, 0, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1 } ,//10
        { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1 } ,//
        { 1, 1, 1, 1, 1, 1, 0, 1, 1, 0, 1, 1, 1, 1, 1 } ,//
        { 7, 7, 7, 7, 7, 1, 0, 1, 1, 0, 1, 1, 1, 1, 1 } ,//
        { 7, 7, 7, 7, 7, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0 } ,//
        { 7, 7, 7, 7, 7, 1, 0, 1, 1, 0, 1, 1, 1, 1, 1 } , //End of ghost house //15
        { 1, 1, 1, 1, 1, 1, 0, 1, 1, 0, 1, 7, 7, 7, 1 } ,
        { 4, 5, 0, 0, 0, 0, 0, 0, 0, 0, 1, 7, 7, 7, 1 } , //Portal section
        { 1, 1, 1, 1, 1, 1, 0, 1, 1, 0, 1, 7, 7, 7, 1 } ,
        { 7, 7, 7, 7, 7, 1, 0, 1, 1, 0, 1, 1, 1, 3, 1 } , //Start if ghost house
        { 7, 7, 7, 7, 7, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0 } ,//20
        { 7, 7, 7, 7, 7, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1 } ,//
        { 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1 } ,//
        { 1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1, 1 } ,//
        { 1, 0, 1, 1, 1, 1, 0, 1, 1, 0, 1, 1, 1, 1, 1 } ,//
        { 1, 0, 1, 1, 1, 1, 0, 1, 1, 0, 1, 1, 1, 1, 1 } ,//25
        { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } ,//
        { 1, 0, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1 } ,//
        { 1, 0, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1 } ,//
        { 1, 0, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1 } ,//
        { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1 } ,//30
        { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 } //
        //2 is node
        //1 is walls
        //4 is portal tiles
        //5 is portal return points
        //3 is gates
        //7 is empty tiles that shouldn't have pellts
    };

    private int[,] blankLevel = {
        { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, //
        { 1, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 1 } ,//
        { 1, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 1 } ,//
        { 1, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 1 } ,//
        { 1, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 1 } ,//5
        { 1, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 1 } ,//
        { 1, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 1 } ,//
        { 1, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 0, 0, 1 } ,//
        { 1, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 0, 6, 1 } ,//
        { 1, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 0, 6, 1 } ,//10
        { 1, 6, 6, 6, 6, 6, 6, 6, 6, 0, 0, 0, 0, 6, 1 } ,//
        { 1, 6, 6, 6, 6, 6, 6, 6, 6, 0, 6, 6, 6, 6, 1 } ,//
        { 1, 6, 6, 6, 6, 6, 6, 6, 6, 0, 6, 6, 6, 6, 1 } ,//
        { 1, 6, 6, 6, 6, 6, 6, 6, 6, 0, 6, 6, 6, 6, 1 } ,//
        { 1, 6, 6, 6, 6, 6, 6, 6, 6, 0, 1, 1, 1, 1, 1 } , //End of ghost house
        { 1, 6, 6, 6, 6, 6, 6, 6, 6, 0, 1, 7, 7, 7, 1 } ,
        { 4, 5, 0, 0, 0, 0, 0, 0, 0, 0, 1, 7, 7, 7, 1 } , //Portal section
        { 1, 6, 6, 6, 6, 6, 6, 6, 6, 0, 1, 7, 7, 7, 1 } ,
        { 1, 6, 6, 6, 6, 6, 6, 6, 6, 0, 1, 1, 1, 3, 1 } , //Start if ghost house
        { 1, 6, 6, 6, 6, 6, 6, 6, 6, 0, 0, 0, 0, 0, 1 } ,//
        { 1, 6, 6, 6, 6, 6, 6, 6, 6, 0, 6, 6, 6, 6, 1 } ,//
        { 1, 6, 6, 6, 6, 6, 6, 6, 6, 0, 6, 6, 6, 6, 1 } ,//
        { 1, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 1 } ,//
        { 1, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 1 } ,//
        { 1, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 1 } ,//
        { 1, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 1 } ,//
        { 1, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 1 } ,//
        { 1, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 1 } ,//
        { 1, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 1 } ,//
        { 1, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 1 } ,//
        { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 } //
        //2 is segnemnts
        //1 is walls
        //4 is portal tiles
        //5 is portal return points
        //3 is gates
        //6 are tile that can be replaced
        //7 is empty tiles that shouldn't have pellts
    };
   
    #region displayChecks;
    public bool CheckIfNode(int[,] array, int x, int y){
        int[] intarr = new int[9];
        intarr = GetSurroudings(array, x, y);

        if(isEmpty(intarr[5]) && isEmpty(intarr[3]) && !isEmpty(intarr[1]) && !isEmpty(intarr[7])){
            return false;
        }

        if (!isEmpty(intarr[5]) && !isEmpty(intarr[3]) && isEmpty(intarr[1]) && isEmpty(intarr[7])){
            return false;
        }



        return true;
    }

    public int[] GetSurroudings(int[,] array, int x, int y){
        int[] intarr = new int[9];

        intarr[4] = array[y, x];

        #region Cardinal
        if (y - 1 >= 0){ //Bottom Tile
            intarr[7] = array[y - 1, x];
        }else{
            intarr[7] = 9;
        }

        if (y + 1 <= _SizeY) { //Top Tile
            intarr[1] = array[y + 1, x];
        } else{
            intarr[1] = 9;
        }

        if (x + 1 <= _SizeX + 1){ //Right Tile
            intarr[5] = array[y, x + 1];
        }else{
            intarr[5] = 9;
        }

        if (x - 1 >= 0) { //Left Tile
            intarr[3] = array[y, x - 1];
        }else{
            intarr[3] = 9;
        }
        #endregion

        #region diagonal
        if (y - 1 >= 0 && x - 1 >= 0){//Bottom left
            intarr[6] = array[y - 1, x - 1];
        }else{
            intarr[6] = 9;
        }
        if (y - 1 >= 0 && x + 1 <= _SizeX){//Bottom right
            intarr[8] = array[y - 1, x + 1];
        }else{
            intarr[8] = 9;
        }
        if (x - 1 >= 0 && y + 1 <= _SizeY){ //Top left
            intarr[0] = array[y + 1, x - 1];
        }else{
            intarr[0] = 9;
        }
        if (x + 1 <= _SizeX && y + 1 <= _SizeY){ //Top right
            intarr[2] = array[y + 1, x + 1];
        }else{
            intarr[2] = 9;
        }

        return intarr;

    }

    public Tile[] GetTilefromCode(int[,] array, int x, int y){
        Tile[] arr1 = new Tile[2];
        int[] intarr = new int[9];
        intarr = GetSurroudings(array, x, y);
        //Prevent the code from going out of bounds

        //Debug.Log(intarr[6] + " Self:" + x + "," + y + " Target:" + (x - 1) + "," + (y - 1));
        //Debug.Log(intarr[8] + " Self:" + x + "," + y + " Target:" + (x + 1) + "," + (y - 1));
        //Debug.Log(intarr[0] + " Self:" + x + "," + y + " Target:" + (x - 1) + "," + (y + 1));
        //Debug.Log(intarr[2] + " Self:" + x + "," + y + " Target:" + (x + 1) + "," + (y + 1));
        #endregion


        TileVisual t = GetTileVisual(intarr);

        switch (t){
            case TileVisual.None:
                break;
            case TileVisual.Vertical:
                arr1[0] = _alltiles[1];
                arr1[1] = _alltiles[1];
                break;
            case TileVisual.Horizontal:
                arr1[0] = _alltiles[0];
                arr1[1] = _alltiles[0];
                break;
            case TileVisual.Black:
                arr1[0] = _alltiles[6];
                arr1[1] = _alltiles[6];
                break;
            case TileVisual.Circle:
                arr1[0] = _alltiles[8];
                arr1[1] = _alltiles[8];
                break;
            case TileVisual.SECorner:
                arr1[0] = _alltiles[3];
                arr1[1] = _alltiles[4];
                break;
            case TileVisual.SWCorner:
                arr1[0] = _alltiles[4];
                arr1[1] = _alltiles[3];
                break;
            case TileVisual.NECorner:
                arr1[0] = _alltiles[2];
                arr1[1] = _alltiles[5];
                break;
            case TileVisual.NWCorner:
                arr1[0] = _alltiles[5];
                arr1[1] = _alltiles[2];
                break;
            case TileVisual.TNorth:
                arr1[0] = _alltiles[9];
                arr1[1] = _alltiles[9];
                break;
        }

        //5 is right tile
        //intarr[4] = defaultArray[x, y];

        //Calculate corners


        return arr1;
    }

    public bool isEmpty(int x){
        if (x == 0 || x == 2|| x == 4 || x == 7 || x==6 || x == 9){
            return true;
        }

        return false;
    }
    #endregion
    #region get tile type
    public TileVisual GetTileVisual(int[] x){
        TileVisual t=TileVisual.None;
        if (x[4] == 1){
            if (!isEmpty(x[1]) && !isEmpty(x[7])){
                t = TileVisual.Vertical;
            }

            if (!isEmpty(x[5]) && !isEmpty(x[3])){
                t = TileVisual.Horizontal;
            }

            if (!isEmpty(x[5]) && !isEmpty(x[3]) && !isEmpty(x[7]) && !isEmpty(x[1])){
                t = TileVisual.Black;
            }

            /*
            if ((!isEmpty(x[1]) && !isEmpty(x[5]) && !isEmpty(x[3])) && (isEmpty(x[0]) ^ isEmpty(x[2]))){
                t = TileVisual.TNorth;
            }
            */

            //South east corner pieces
            #region SE
            if (isEmpty(x[0]) && isEmpty(x[1]) && isEmpty(x[2]) && isEmpty(x[3]) && isEmpty(x[6]) && !(isEmpty(x[4]) && isEmpty(x[5]) && isEmpty(x[7]) && isEmpty(x[8])))
            {
                t = TileVisual.SECorner;
            }
            if (!isEmpty(x[1]) && !isEmpty(x[2]) && !isEmpty(x[5]) && !isEmpty(x[7]) && isEmpty(x[8]))
            {
                t = TileVisual.SECorner;
            }
            #endregion
            //North east corner pieces
            #region NE
            if (!isEmpty(x[1]) && !isEmpty(x[5]) && isEmpty(x[0]) && isEmpty(x[3]) && isEmpty(x[6]) && !isEmpty(x[7]) && !isEmpty(x[8]) && isEmpty(x[2]))
            {
                t = TileVisual.NECorner;
            }

            if (isEmpty(x[0]) && !isEmpty(x[1]) && isEmpty(x[7]) && !isEmpty(x[5]))
            {
                t = TileVisual.NECorner;
            }

            if (isEmpty(x[2]) && !isEmpty(x[0]) && !isEmpty(x[1]) && !isEmpty(x[3]) && !isEmpty(x[4]) && !isEmpty(x[6]) && !isEmpty(x[8]) && !isEmpty(x[7]) && !isEmpty(x[5]))
            {
                t = TileVisual.NECorner;
            }
            #endregion
            //North west corner pieces
            #region NW
            if (!isEmpty(x[1]) && !isEmpty(x[3]) && isEmpty(x[6]) && isEmpty(x[7]) && isEmpty(x[8]) && isEmpty(x[5]))
            {
                t = TileVisual.NWCorner;
            }

            if (isEmpty(x[0]) && !isEmpty(x[1]) && !isEmpty(x[3]))
            {
                t = TileVisual.NWCorner;
            }
            #endregion
            //South west corner pieces
            #region SW
            if (!isEmpty(x[7]) && !isEmpty(x[3]) && isEmpty(x[2]) && isEmpty(x[1]) && isEmpty(x[5])){
                t = TileVisual.SWCorner;
            }

            if (isEmpty(x[6]) && !isEmpty(x[7]) && !isEmpty(x[3])){
                t = TileVisual.SWCorner;
            }
            #endregion


            if (isEmpty(x[5]) && isEmpty(x[3]) && isEmpty(x[7]) && isEmpty(x[1])){
                t = TileVisual.Circle;
            }

            if (isEmpty(x[5]) && isEmpty(x[3]) && isEmpty(x[7]) && !isEmpty(x[1])){
                t = TileVisual.Circle;
            }

            if (isEmpty(x[5]) && !isEmpty(x[3]) && isEmpty(x[7]) && isEmpty(x[1])){
                t = TileVisual.Circle;
            }

            if (!isEmpty(x[5]) && isEmpty(x[3]) && isEmpty(x[7]) && isEmpty(x[1])){
                t = TileVisual.Circle;
            }


            if (isEmpty(x[5]) && isEmpty(x[3]) && !isEmpty(x[7]) && isEmpty(x[1])){
                t = TileVisual.Circle;
            }

        }


        return t;
    }
    #endregion
    #region chnage colours
    //Make the map changhe colours for fun.
    private Color ShiftColour(){
        colorIndex++;

        if (colorIndex > 8){
            colorIndex = 0;
        }
        Color orange = new Color(1.0f, 0.5f, 0.0f);
        Color purple = new Color(0.5f, 0f, 1.0f);


        switch (colorIndex)
        {
            case 0:
                return Color.white;
            case 1:
                return Color.red;
            case 2:
                return orange;
            case 3:
                return Color.yellow;
            case 4:
                return Color.green;
            case 5:
                return Color.cyan;
            case 6:
                return Color.blue;
            case 7:
                return purple;
            case 8:
                return Color.magenta;
            default:
                return Color.white;
        }

    }
    #endregion

    public void Start() {
        perlinMap = new int[_SizeX, _SizeY];

        if (notActive) {

            tilemap = createTilemap("Tilemap");
            pellets = createTilemap("Pellets");
            pellets.transform.position = new Vector3(pellets.transform.position.x, pellets.transform.position.y, -1);
            nodes = createTilemap("Nodes");

            loadFromArray(tilemap, pellets, nodes, defaultArray);
            //tilemap.SetTile(new Vector3Int(0, 0, 0), _wall);
            //tilemap.SetTile(new Vector3Int(13, 1, 0), _wall);
            //tilemap.SetTile(new Vector3Int(27, 30, 0), _wall);
            /*
            tilemap.ClearAllTiles();
            pellets.ClearAllTiles();
            */

        }

    }

    public void loadFromArray(Tilemap map, Tilemap pellets, Tilemap nodes, int[,] Array) {
        //Generating the locations of pellets

        for (int i = 0; i <= _SizeY; i++){
            for (int j = 0; j <= _SizeX; j++){
                

                if (Array[i, j] == 1){//Remove 4 later
                    Tile[] tiles = GetTilefromCode(Array, j, i);
                    if (tiles[0] != null) {
                        map.SetTile(new Vector3Int(j, i, 0), tiles[0]);
                        map.SetTile(new Vector3Int(27 - j, i, 0), tiles[1]);
                    }else{
                        map.SetTile(new Vector3Int(j, i, 0), _alltiles[8]);
                        map.SetTile(new Vector3Int(27 - j, i, 0), _alltiles[8]);
                    }

                }
                //
                if (Array[i, j] == 0 || Array[i, j] == 2){//setting nodes
                    bool x = CheckIfNode(Array, j, i);
                    if (x){
                        nodes.SetTile(new Vector3Int(j, i, 0), _node);
                        nodes.SetTile(new Vector3Int(27 - j, i, 0), _node);
                    }
                    int e = getPerlinValue(j,i);
                    if (e == 0){
                        pellets.SetTile(new Vector3Int(j, i, 0), _pellet);
                        pellets.SetTile(new Vector3Int(27 - j, i, 0), _pellet);
                    }else if (e == 1){
                        pellets.SetTile(new Vector3Int(j, i, 0), _powerpellet);
                        pellets.SetTile(new Vector3Int(27 - j, i, 0), _powerpellet);
                    } else{
                        pellets.SetTile(new Vector3Int(j, i, 0), _fruit);
                        pellets.SetTile(new Vector3Int(27 - j, i, 0), _fruit);
                    }
                    /*
                    if (pPellets <= MaxpPellets && (j == PelletsX|| j == PelletsX2) && (i == PelletsY||i==PelletsY2||i==PelletsY3)) { //Setting p
                        pellets.SetTile(new Vector3Int(j, i, 0), _powerpellet);
                        pellets.SetTile(new Vector3Int(27 - j, i, 0), _powerpellet);
                        pPellets++;
                    } else if (fruits <= MaxFruits && i % 4 == 0 && j == FruitX) {
                        pellets.SetTile(new Vector3Int(j, i, 0), _fruit);
                        pellets.SetTile(new Vector3Int(27 - j, i, 0), _fruit);
                        fruits++;
                    } else{
                        pellets.SetTile(new Vector3Int(j, i, 0), _pellet);
                        pellets.SetTile(new Vector3Int(27 - j, i, 0), _pellet);
                    }
                    */


                }
                if (Array[i, j] == 3){//Remove 4 later
                    map.SetTile(new Vector3Int(j, i, 0), _alltiles[7]);
                    map.SetTile(new Vector3Int(27 - j, i, 0), _alltiles[7]);
                }
                if (Array[i, j] == 4) {//Remove 4 later
                    map.SetTile(new Vector3Int(j, i, 0), _portalLeft);
                    map.SetTile(new Vector3Int(27 - j, i, 0), _portalRight);
                }
                if (Array[i, j] == 5){//Remove 4 later
                    map.SetTile(new Vector3Int(j, i, 0), _tunnelLeft);
                    map.SetTile(new Vector3Int(27 - j, i, 0), _tunnelRight);
                }
            }

            done = true;
           
            /*
            map.SetTile(new Vector3Int(0, 0, 0), _wall); // Or use SetTiles() for multiple tiles.
            map.SetTile(new Vector3Int(27, 0, 0), _wall); // Or use SetTiles() for multiple tiles.
            map.SetTile(new Vector3Int(27, 30, 0), _wall); // Or use SetTiles() for multiple tiles.
            map.SetTile(new Vector3Int(0, 30, 0), _wall); // Or use SetTiles() for multiple tiles.
            */
        }
    }

    private void CleanUp(int[,] array){
        for (int i = 0; i <= _SizeY; i++){
            for (int j = 0; j <= _SizeX; j++){
                int[] surroundings = GetSurroudings(array, j, i);
                //If the array square is empty, but surrounded by falls fill it in
                if (array[i, j] == 0){
                    if (surroundings[1]== 1 && surroundings[3]==1 && surroundings[5]==1 && surroundings[7]==1){
                        array[i, j] = 1;
                    }
                }
                //If the array square is empty on all sides, but surrounded by empty fill it in
                if (array[i, j] == 0){
                    if (surroundings[1] == 0 && surroundings[3] == 0 && surroundings[5] == 0 && surroundings[7] == 0 && surroundings[0] == 0 && surroundings[2] == 0 && surroundings[6] == 0 && surroundings[8] == 0){
                        array[i, j] = 1;
                    }
                }

                //If the array square is a wall, but surrounded by empty eraase it
                if (array[i, j] == 1){
                    if (surroundings[1] == 0 && surroundings[3] == 0 && surroundings[5] == 0 && surroundings[7] == 0){
                        array[i, j] = 0;
                    }
                }
            }
        }

        
        //If it's empty and surrounded by 
        
    }

    public void createNewLevel(){

        GameObject[] allPerlin = GameObject.FindGameObjectsWithTag("Perlin");
        for (int i = 0; i < allPerlin.Length; i++){
            Destroy(allPerlin[i]);
        }


        int[,] tempArray = new int[blankLevel.GetLength(0), blankLevel.GetLength(1)];
        System.Array.Copy(blankLevel, tempArray, blankLevel.Length);

        gm.LevelDone = false;
        done = false;

        tilemap.ClearAllTiles();
        tilemap.color = ShiftColour();
        pellets.ClearAllTiles();
        nodes.ClearAllTiles();

        for (int i = 0; i <= tempArray.GetLength(1) -1; i++){
            for (int j = 0; j <= tempArray.GetLength(0) -1; j++) {
                if (tempArray[j, i] == 6){
                    if (noise.grid[i-1, j-1] == 1){
                        //Load the new level from the maze generator
                        tempArray[j, i] = noise.grid[i-1, j-1] == 1 ? 0 : 1;
                    } else {
                        tempArray[j, i] = 1;
                    }
                }
            }
        }
        CleanUp(tempArray);
        loadFromArray(tilemap, pellets, nodes, tempArray);
    }

    public Tilemap createTilemap(string name){
        var tilemap = new GameObject(name).AddComponent<Tilemap>();
        tilemap.gameObject.AddComponent<TilemapRenderer>();
        //Create and setup collider
        tilemap.gameObject.AddComponent<TilemapCollider2D>();
        tilemap.GetComponent<TilemapCollider2D>().sharedMaterial = _physicals;
        //Set up rigidBody or the nopdes cannot detect it. 
        tilemap.gameObject.AddComponent<Rigidbody2D>();
        tilemap.GetComponent<Rigidbody2D>().gravityScale = 0f;
        tilemap.GetComponent<Rigidbody2D>().isKinematic = true;//For some reason this fixes a bug?
        tilemap.GetComponent<Rigidbody2D>().constraints = (RigidbodyConstraints2D)(RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationZ);
        //Setup transforms
        tilemap.transform.SetParent(grid.transform);
        // Set the grid source to the bottom left corner
        tilemap.transform.position = new Vector3(-14f, -16, 0);
        //Set tilemap to correct layer
        tilemap.gameObject.layer = LayerMask.NameToLayer("Obstacle");

        return tilemap;
    }


    public int getPerlinValue(int x, int y){
        int x_offset = Random.Range(0, 7);
        int y_offset = Random.Range(0, 8);


        float store = Mathf.PerlinNoise((x - x_offset) / perlinMagification, (y - y_offset) / perlinMagification);
        //Debug.Log(store);
        if (store > .1f) {
            return 0;
        } else if (store < .1f && store > .05f){
            return 1;
        } else{
            return 2;
        }

    }
}