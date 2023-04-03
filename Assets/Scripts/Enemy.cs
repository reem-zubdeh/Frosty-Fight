using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    GameObject player;
    Animator anim;
    UnityEngine.AI.NavMeshAgent navAgent;
    int currentStatus;
    IEnumerator damageCoroutine;
    bool isDying;

    [Tooltip("How much health the enemy starts with. Between 5 and 25 HP.")]
    [Range (5, 25)]
    [SerializeField]
    int health;

    [Tooltip("How much damage the enemy deals with every attack. Between 1 and 5 HP.")]
    [Range (1, 5)]
    [SerializeField]
    int damage;

    [Tooltip("How far away the enemies notice you and start to follow you. Between 10 and 150 meters.")]
    [Range (10f, 150f)]
    [SerializeField]
    float noticeDistance;

    [Tooltip("How far away the enemies can attack and damage you. Between 3 and 5 meters.")]
    [Range (3f, 5f)]
    [SerializeField]
    float attackDistance;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        anim = transform.Find("SnowmanRoot").GetComponent<Animator>();
        navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        currentStatus = 0;
        isDying = false;
        damageCoroutine = Damage();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance <= attackDistance && currentStatus != 2) {
            StartCoroutine(damageCoroutine);
            anim.SetBool("isWalking", false);
            anim.SetBool("isAttacking", true);
            currentStatus = 2;
        }
        else if (distance <= noticeDistance && distance > attackDistance) {
            navAgent.destination = player.transform.position;
            if (currentStatus != 1) {
                StopCoroutine(damageCoroutine);
                anim.SetBool("isWalking", true);
                anim.SetBool("isAttacking", false);    
            }
            currentStatus = 1;
        }
        else if (distance > noticeDistance && currentStatus != 0) {
            StopCoroutine(damageCoroutine);
            navAgent.destination = transform.position;
            anim.SetBool("isWalking", false);
            anim.SetBool("isAttacking", false);
            currentStatus = 0;
        }
    }

    IEnumerator Damage()
    {
        while (true) {
            yield return new WaitForSecondsRealtime(1f);
            player.GetComponent<Health>().TakeDamage(damage);
        }
    }

    public void TakeDamage(int dmg) {
        health -= dmg;
        if (health <= 0 && !isDying) {
            isDying = true;
            anim.SetBool("isWalking", false);
            anim.SetBool("isAttacking", false);   
            anim.SetBool("isDying", true);
            Destroy(this.gameObject, 3);
            player.GetComponent<Shoot>().IncreaseScore(1);
        }
    }
}
