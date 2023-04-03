using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static int score;
    public static bool win;

    public static void GameOver(bool win, int score) {
        GameManager.score = score;
        GameManager.win = win;
        SceneManager.LoadScene(1);
    }

    public static void PlayAgain() {
        SceneManager.LoadScene(0);
    }

}
