using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndMenu : MonoBehaviour
{

    bool win;
    int score;
    TMP_Text winTxt, diedTxt, scoreTxt;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        win = GameManager.win;
        score = GameManager.score;

        scoreTxt = GameObject.Find("ScoreTxt").GetComponent<TMP_Text>();
        winTxt = GameObject.Find("WinTxt").GetComponent<TMP_Text>();
        diedTxt = GameObject.Find("DiedTxt").GetComponent<TMP_Text>();

        if (win) {
            winTxt.enabled = true;
            diedTxt.enabled = false;
        }
        else {
            winTxt.enabled = false;
            diedTxt.enabled = true;
        }

        scoreTxt.text = $"YOUR SCORE: {score.ToString()}/9";

    }

    public void Quit() {
        Application.Quit();
    }

    public void PlayAgain() {
        GameManager.PlayAgain();
    }

}
