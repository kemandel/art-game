using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    Canvas levelCanvas;
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<SwordController>().GameLose += LoseGame;
        FindObjectOfType<PedestalController>().GameWin += WinGame;

        levelCanvas = FindObjectOfType<Canvas>();
        levelCanvas.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void LoseGame(){
        levelCanvas.gameObject.SetActive(true);
        levelCanvas.GetComponentInChildren<Text>().text = "Your sword broke! Press <color=orange>r</color> to restart.";
    }

    void WinGame(){
        levelCanvas.gameObject.SetActive(true);
        levelCanvas.GetComponentInChildren<Text>().text = "You Win! Press <color=orange>r</color> to play again.";
    }
}
