using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    Animator levelChangeAni;

    public GameObject runInstructions;
    public GameObject menu;
    private GameObject instructions;
    public GameObject startPanel;
    public GameObject endPanel;
    public GameObject scorePanel;
    public GameObject levelPanel;
    public GameObject galaxyMap;
    public TMP_Text scoreText;
    public TMP_Text levelText;
    public TMP_Text leaderboardText;
    public TMP_Text levelChangeText;

    private bool menuOn;
    public bool MenuOn {
        get {return menuOn;}
        set {menuOn = value;}
    }

    private bool instructionsOn;
    public bool InstructionsOn {
        get {return instructionsOn;}
        set {instructionsOn = value;}
    }

    // Start is called before the first frame update
    void Start()
    {
        MenuOn = false;
        InstructionsOn = false;

        levelChangeAni = levelChangeText.GetComponent<Animator>();

        startPanel.SetActive(true);
        endPanel.SetActive(false);
        scorePanel.SetActive(false);
        levelPanel.SetActive(false);
        galaxyMap.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Menu();
    }

    public void Menu() {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MenuOn = !MenuOn;
        }

        menu.SetActive(MenuOn);
        Time.timeScale = !MenuOn ? 1 : 0;

        if (!MenuOn && InstructionsOn)
        {
            Destroy(instructions);
            InstructionsOn = false;
        }

        int score = GetComponent<ScoreManager>().GetScore();
        if (score == 1) {
            scoreText.text = "Score: " + score + " coin";
        }
        else {
            scoreText.text = "Score: " + score + " coins";
        }
    }

    public void ShowInstructions()
    {
        InstructionsOn = !InstructionsOn;

        if (InstructionsOn)
        {
            instructions = Instantiate(runInstructions);
        }
        else
        {
            Destroy(instructions);
        }
    }

    public void SetStartPanels(int gamePlay)
    {
        if (gamePlay == 1) {
            startPanel.SetActive(false);
            scorePanel.SetActive(true);
            levelPanel.SetActive(true);
            levelText.text = "Level: " + GetComponent<EnvironmentManager>().GetLevel();
        }
        else if (gamePlay == 2) {
            startPanel.SetActive(false);
            scorePanel.SetActive(true);
        }
    }

    public void SetEndPanels(string leaderboard) {
        leaderboardText.text = leaderboard;
        endPanel.SetActive(true);
    }

    public void UpdateLevel(int level) {
        levelText.text = "Level: " + level;
    }

    public void LevelChange(int level) {
        levelText.text = "Level: " + level;
        levelChangeText.text = "Level " + level;

        levelChangeAni.SetTrigger("move");
        Invoke("ClearLevelChange", 1);
    }

    public void ClearLevelChange() {
        levelChangeText.text = "";
        levelChangeAni.SetTrigger("reset");
    }

    public void ShowMap() {
        galaxyMap.SetActive(true);
    }

    public void CloseMap() {
        galaxyMap.SetActive(false);
    }
}
