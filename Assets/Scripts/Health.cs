using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{

    Shoot player;
    Slider slider;

    [Tooltip("How much health you start with. Between 1 and 100 HP.")]
    [Range (1, 100)]
    [SerializeField]
    int health;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Shoot>();
        slider = GameObject.Find("Health").GetComponent<Slider>();
        slider.maxValue = health;
    }

    public void TakeDamage(int dmg) {
        health -= dmg;
        slider.value = health;
        if (health <= 0) {
            int score = player.IncreaseScore(0);
            GameManager.GameOver(false, score);
        }
    }
}
