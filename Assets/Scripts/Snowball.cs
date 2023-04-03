using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowball : MonoBehaviour
{

    bool hasHit = false;

    [Header("Gun 2 snowballs")]

    [Tooltip("Particle system to generate where a snowball from Gun 2 lands")]
    [SerializeField]
    GameObject impactEffect2;

    [Tooltip("Damage per snowball of Gun 2")]
    [Range (3, 10)]
    [SerializeField]
    int damage2;

    void OnCollisionEnter(Collision other) {
        GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
        GameObject current = Instantiate(impactEffect2, transform.position, Quaternion.identity);
        Destroy(current, 8);
        if (other.gameObject.tag == "Enemy" && !hasHit) {
            other.gameObject.GetComponent<Enemy>().TakeDamage(damage2);
        }
        hasHit = true;
    }

}
