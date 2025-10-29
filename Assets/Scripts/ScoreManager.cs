using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private List<int> highScores;
    public List<int> HighScores {
        get {return highScores;}
        set {highScores = value;}
    }

    private int score;
    public int Score {
        get {return score;}
        set {score = value;}
    }

    // Start is called before the first frame update
    void Start()
    {
        HighScores = new List<int>();
        Score = 0;

        for (int i = 0; i < 3; i++) {
            string key = "HighScore " + i;

            if (PlayerPrefs.HasKey(key))
            {
                int score = PlayerPrefs.GetInt(key);
                if (!HighScores.Contains(score))
                {
                    HighScores.Add(score);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void End() {
        if (GameObject.FindGameObjectsWithTag("trail").Length > 0) {
            Destroy(GameObject.FindGameObjectsWithTag("trail")[0]);
        }

        Time.timeScale = 0;

        AddHighScore();

        string leaderboard = "Leaderboard:";
        for (int i = 0; i < HighScores.Count; i++)
        {
            leaderboard += "\n " + (i + 1) + ". " + HighScores[i] + " coins";
        }

        if (GetComponent<EnvironmentManager>().GetGamePlay() == 1) {
            int curr = GetComponent<EnvironmentManager>().GetLevel();
            int saved = PlayerPrefs.GetInt("Level", 1);

            if (curr > saved) {
                PlayerPrefs.SetInt("Level", GetComponent<EnvironmentManager>().GetLevel());
                PlayerPrefs.Save();
            }
        }

        GetComponent<UIManager>().SetEndPanels(leaderboard);
    }

    public void AddScore(int curr)
    {
        Score += curr;
    }

    public void AddHighScore() {
        if (!HighScores.Contains(Score))
        {
            HighScores.Add(Score);
        }

        HighScores.Sort((x, y) => y.CompareTo(x));

        while (HighScores.Count > 3)
        {
            HighScores.RemoveAt(HighScores.Count - 1);
        }

        for (int i = 0; i < HighScores.Count; i++)
        {
            PlayerPrefs.SetInt("HighScore " + i, HighScores[i]);
        }
        for (int i = HighScores.Count; i < 3; i++)
        {
            PlayerPrefs.DeleteKey("HighScore " + i);
        }
        PlayerPrefs.Save();
    }

    public int GetScore() {
        return Score;
    }
}
