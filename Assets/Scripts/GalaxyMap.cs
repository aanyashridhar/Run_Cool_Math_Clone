using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class GalaxyMap : MonoBehaviour
{
    public GameObject levelBlock;
    public GameObject line;
    public GameObject playerIcon;
    public RectTransform map;
    private GameObject player;
    public EnvironmentManager envManager;
    public UIManager uiManager;

    private List<GameObject> levelBlocks;
    public List<GameObject> LevelBlocks {
        get {return levelBlocks;}
        set {levelBlocks = value;}
    }
    private List<GameObject> lines;
    public List<GameObject> Lines {
        get {return lines;}
        set {lines = value;}
    }

    private int nextLevel;
    public int NextLevel {
        get {return nextLevel;}
        set {nextLevel = value;}
    }

    private float startCoords;
    public float StartCoords {
        get {return startCoords;}
        set {startCoords = value;}
    }
    
    void Start()
    {
        LevelBlocks = new List<GameObject>();
        Lines = new List<GameObject>();
        NextLevel = Mathf.Max(PlayerPrefs.GetInt("Level"), 1);

        //start of screen is negative half of screen width
        StartCoords = map.rect.width / -2;

        RefreshMap();
    }

    void Update() {
        
    }

    public void CleanMap()
    {
        foreach (Transform child in map)
        {
            //destroy all except for the close button
            if (!child.gameObject.tag.Equals("closebutton")) {
                Destroy(child.gameObject);
            }
        }

        LevelBlocks.Clear();
        Lines.Clear();
        player = null;
    }

    public void BuildMap()
    {
        for (int i = 1; i <= NextLevel; i++)
        {
            //create a level block for each level
            GameObject lb = Instantiate(levelBlock, map);

            Button lbButton = lb.GetComponentInChildren<Button>();
            lbButton.GetComponentInChildren<TMP_Text>().text = i + "";
            int level = i;
            lbButton.onClick.AddListener(() => OnLevelSelected(level));

            RectTransform lbRect = lb.GetComponent<RectTransform>();
            lbRect.anchoredPosition = new Vector2(StartCoords + (i - 1) * 120f + 90f, 0);
            LevelBlocks.Add(lb);


            //create a dashed line for each level (to the left of the block)
            GameObject l = Instantiate(line, map);
            RectTransform lRect = l.GetComponent<RectTransform>();
            lRect.anchoredPosition = new Vector2(StartCoords + (i - 1) * 120f + 30f, 0);
            Lines.Add(l);
        }
    }

    public void Player()
    {
        if (Lines.Count > 0)
        {
            player = Instantiate(playerIcon, map);

            //put the center of the player icon at the center of the last dashed line
            player.GetComponent<RectTransform>().anchoredPosition = Lines[Lines.Count - 1].GetComponent<RectTransform>().anchoredPosition;
        }
    }

    public void OnLevelSelected(int level)
    {
        Debug.Log("clicked");
        uiManager.SetStartPanels(1);
        uiManager.CloseMap();
        
        if (level < NextLevel) {
            uiManager.UpdateLevel(level);
            envManager.GamePlayChosen(1, true, level);
        }
        else if (level == NextLevel) {
            uiManager.UpdateLevel(level);
            envManager.GamePlayChosen(1);
        }
    }

    public void RefreshMap() {
        NextLevel = Mathf.Max(PlayerPrefs.GetInt("Level"), 1);

        CleanMap();
        BuildMap();
        Player();
    }
}