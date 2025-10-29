using UnityEngine;
using System.Collections.Generic;

public class EnvironmentManager : MonoBehaviour
{
    public GameObject player;
    public GameObject tile;
    public GameObject coin;
    public GameObject powerup;
    private GameObject p;

    private float zPosition;
    public float ZPosition {
        get {return zPosition;}
        set {zPosition = value;}
    }

    private int gamePlay;
    public int GamePlay {
        get {return gamePlay;}
        set {gamePlay = value;}
    }

    private int level;
    public int Level {
        get {return level;}
        set {level = value;}
    }

    private List<GameObject> spawned;
    public List<GameObject> Spawned {
        get {return spawned;}
        set {spawned = value;}
    }

    private int currentColorIndex;
    public int CurrentColorIndex {
        get {return currentColorIndex;}
        set {currentColorIndex = value;}
    }

    private float startZ;
    public float StartZ {
        get {return startZ;}
        set {startZ = value;}
    }

    void Start()
    {
        //Level = 1;
        GamePlay = 0;
        Spawned = new List<GameObject>();
        CurrentColorIndex = 0;
    }

    void Update() {
        if (GamePlay == 0) {
            return;
        }

        if (GamePlay == 1) {
            if (Level == 0) {
                Level = 1;
            }

            int currentLevel = Mathf.FloorToInt((p.transform.position.z + StartZ) / 300f) + 1;

            if (currentLevel > Level) {
                Level = currentLevel;

                foreach (GameObject obj in Spawned) {
                    if (obj != null && obj.tag.Equals("tile")) {
                        obj.GetComponent<TileColorChange>().ColorChange(CurrentColorIndex);
                    }
                }

                CurrentColorIndex++;
                if (CurrentColorIndex >= 8) {
                    CurrentColorIndex = 0;
                }

                p.GetComponent<PlayerMovement>().LevelChange();
                GetComponent<UIManager>().LevelChange(Level);
                Debug.Log("New Level: " + Level);
            }         
        }
        
        if (ZPosition < p.transform.position.z + 50f) {
            Coords(ZPosition);
            ZPosition += 1.5f;
        }

        CleanUp();
    }

    public void Coords(float z)
    {
        z = Mathf.Round(z * 100f) / 100f;   //account for floating-point imprecision over time

        //bottom faces
        SpawnTile(new Vector3(-3f, -3.75f, z), Quaternion.Euler(0,0,0));
        SpawnTile(new Vector3(-1.5f, -3.75f, z), Quaternion.Euler(0,0,0));
        SpawnTile(new Vector3(0f, -3.75f, z), Quaternion.Euler(0,0,0));
        SpawnTile(new Vector3(1.5f, -3.75f, z), Quaternion.Euler(0,0,0));
        SpawnTile(new Vector3(3f, -3.75f, z), Quaternion.Euler(0,0,0));

        //top faces
        SpawnTile(new Vector3(-3f, 3.75f, z), Quaternion.Euler(0,0,180));
        SpawnTile(new Vector3(-1.5f, 3.75f, z), Quaternion.Euler(0,0,180));
        SpawnTile(new Vector3(0f, 3.75f, z), Quaternion.Euler(0,0,180));
        SpawnTile(new Vector3(1.5f, 3.75f, z), Quaternion.Euler(0,0,180));
        SpawnTile(new Vector3(3f, 3.75f, z), Quaternion.Euler(0,0,180));

        //left faces
        SpawnTile(new Vector3(-3.75f, -3f, z), Quaternion.Euler(0,0,270));
        SpawnTile(new Vector3(-3.75f, -1.5f, z), Quaternion.Euler(0,0,270));
        SpawnTile(new Vector3(-3.75f, 0f, z), Quaternion.Euler(0,0,270));
        SpawnTile(new Vector3(-3.75f, 1.5f, z), Quaternion.Euler(0,0,270));
        SpawnTile(new Vector3(-3.75f, 3f, z), Quaternion.Euler(0,0,270));

        //right faces
        SpawnTile(new Vector3(3.75f, -3f, z), Quaternion.Euler(0,0,90));
        SpawnTile(new Vector3(3.75f, -1.5f, z), Quaternion.Euler(0,0,90));
        SpawnTile(new Vector3(3.75f, 0f, z), Quaternion.Euler(0,0,90));
        SpawnTile(new Vector3(3.75f, 1.5f, z), Quaternion.Euler(0,0,90));
        SpawnTile(new Vector3(3.75f, 3f, z), Quaternion.Euler(0,0,90));
    }

    public void SpawnTile(Vector3 position, Quaternion rotation)
    {
        bool inRange = false;
        if (GamePlay == 1)
        {
            float localZ = position.z % 300f;
            inRange = localZ > 15f && localZ < 285f;
        }
        else if (GamePlay == 2)
        {
            inRange = position.z > 15f;
        }

        //create hole at random
        //higher levels -> more holes, max difficulty at level 5
        if (Random.Range(0, 10) < Mathf.Min(Level, 5) && inRange) {
            return;
        }

        GameObject t = Instantiate(tile, position, rotation);
        t.GetComponent<TileColorChange>().SetColor(CurrentColorIndex);
        Spawned.Add(t);

        int spawn = Random.Range(0,200);

        //spawn coin or power-up at random
        if (inRange && !OnSeam(position, rotation))
        {
            Vector3 objPosition = Vector3.zero;
            
            //floor
            if (rotation == Quaternion.Euler(0,0,0)) {
                objPosition = t.transform.position + new Vector3(0, 0.5f, 0);
            }
            //ceiling
            else if (rotation == Quaternion.Euler(0,0,180)) {
                objPosition = t.transform.position + new Vector3(0, -0.5f, 0);
            }
            //left wall
            else if (rotation == Quaternion.Euler(0,0,270)) {
                objPosition = t.transform.position + new Vector3(0.5f, 0, 0);
            }
            //right wall
            else if (rotation == Quaternion.Euler(0,0,90)) {
                objPosition = t.transform.position + new Vector3(-0.5f, 0, 0);
            }
            
            if (spawn == 0) {
                GameObject c = Instantiate(coin, objPosition, rotation);
                Spawned.Add(c);
            }
            else if (spawn == 1) {
                GameObject s = Instantiate(powerup, objPosition, rotation);
                Spawned.Add(s);
            }
        }
    }

    public bool OnSeam(Vector3 position, Quaternion rotation) {
        //floor or ceiling
        if (rotation == Quaternion.Euler(0,0,0) || rotation == Quaternion.Euler(0,0,180)) {
            return position.x > 2.75f || position.x < -2.75f;
        }
        //left or right wall
        else if (rotation == Quaternion.Euler(0,0,270) || rotation == Quaternion.Euler(0,0,90)) {
            return position.y > 2.75f || position.y < -2.75f;
        }

        return false;
    }

    public void GamePlayChosen(int g, bool past = false, int l = 0) {
        GamePlay = g;
        ZPosition = 0;

        if (GamePlay == 1 && PlayerPrefs.HasKey("Level") && !past) {
            Level = PlayerPrefs.GetInt("Level");
            CurrentColorIndex = (Level - 1) % 8;
            StartZ = (Level - 1) * 300f;
        }
        else if (GamePlay == 1 && past) {
            Level = l;
            CurrentColorIndex = (Level - 1) % 8;
            StartZ = (Level - 1) * 300f;
        }
        else {
            Level = 1;
            CurrentColorIndex = 0;
            StartZ = 0f;
        }

        p = Instantiate(player, new Vector3(0, -3.75f, 0), Quaternion.identity);
    }

    public void CleanUp() {
        for (int i = Spawned.Count - 1; i >= 0; i--) {
            if (Spawned[i] == null) {
                Spawned.RemoveAt(i);
            }

            //destroy obj if its behind player
            if (Spawned[i].transform.position.z < (p.transform.position.z - 15f)) {
                Destroy(Spawned[i]);
                Spawned.RemoveAt(i);
            }
        }
    }

    public void CleanGame() {
        PlayerPrefs.DeleteKey("Level");
        Level = 1;
        CurrentColorIndex = 0;
    }

    public int GetLevel() {
        return Level;
    }

    public int GetGamePlay() {
        return GamePlay;
    }
}