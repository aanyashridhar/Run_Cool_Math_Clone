using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Quit()
    {
        GetComponent<ScoreManager>().AddHighScore();

        if (GetComponent<EnvironmentManager>().GetGamePlay() == 1) {
            int curr = GetComponent<EnvironmentManager>().GetLevel();
            int saved = PlayerPrefs.GetInt("Level", 1);

            if (curr > saved) {
                PlayerPrefs.SetInt("Level", GetComponent<EnvironmentManager>().GetLevel());
                PlayerPrefs.Save();
            }
        }
        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; //editor runs
#else
        Application.Quit(); //build
#endif
    }

    public void Restart()
    {
        if (GetComponent<EnvironmentManager>().GetGamePlay() == 1) {
            int curr = GetComponent<EnvironmentManager>().GetLevel();
            int saved = PlayerPrefs.GetInt("Level", 1);

            if (curr > saved) {
                PlayerPrefs.SetInt("Level", GetComponent<EnvironmentManager>().GetLevel());
                PlayerPrefs.Save();
            }
        }

        SceneManager.LoadScene("SampleScene");
    }

    public void ChooseExplore() {
        GetComponent<EnvironmentManager>().GamePlayChosen(1);
        GetComponent<UIManager>().SetStartPanels(1);
    }

    public void ChooseInfinite() {
        GetComponent<EnvironmentManager>().GamePlayChosen(2);
        GetComponent<UIManager>().SetStartPanels(2);
    }
}
