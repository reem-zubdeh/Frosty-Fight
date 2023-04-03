using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shoot : MonoBehaviour
{

    int activeGun;
    int score;
    float nextFireTime1;
    float nextFireTime2;

    [Header("Firing rate (shots per second)")]

    [Tooltip("Firing rate for Gun 1 (Full auto)")]
    [Range (4, 9)]
    [SerializeField]
    int fireRate1;

    [Tooltip("Firing rate for Gun 1 (Semi auto)")]
    [Range (1, 3)]
    [SerializeField]
    int fireRate2;

    [Header("Current ammo")]
    
    [Tooltip("Ammo for Gun 1")]
    [Range (5, 50)]
    [SerializeField]
    int ammo1;

    [Tooltip("Ammo for Gun 2")]
    [Range (3, 30)]
    [SerializeField]
    int ammo2;

    [Header("Gun 1 bullets")]

    [Tooltip("Particle system to generate where Gun 1 bullets hit")]
    [SerializeField]
    GameObject impactEffect1;

    [Tooltip("Damage per bullet of Gun 1")]
    [Range (1, 5)]
    [SerializeField]
    int damage1;

    [Header("Gun 2 snowballs")]

    [Tooltip("Snowball projectile prefab")]
    [SerializeField]
    GameObject snowball;

    [Tooltip("Speed of the snowball projectile when shot from Gun 2")]
    [Range (5, 50)]
    [SerializeField]
    float snowballSpeed;

    MeshRenderer gun1, gun2;
    ParticleSystem shootEffect1, shootEffect2;
    AudioSource shootAudio1, shootAudio2;
    Image crosshairImage, activeImage, inactiveImage, ammoImage;
    Sprite crosshair1, crosshair2, gun1Sprite, gun2Sprite, ammo1Sprite, ammo2Sprite;
    TMP_Text activeNumber,inactiveNumber, ammoText, scoreText;

    void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;

        activeGun = 1;
        score = 0;
        nextFireTime1 = 0f;
        nextFireTime2 = 0f;

        gun1 = GameObject.Find("Gun1").GetComponent<MeshRenderer>();
        gun2 = GameObject.Find("Gun2").GetComponent<MeshRenderer>();

        shootAudio1 = GameObject.Find("ShootEffect1").GetComponent<AudioSource>();
        shootEffect1 = GameObject.Find("ShootEffect1").GetComponent<ParticleSystem>();

        shootAudio2 = GameObject.Find("ShootEffect2").GetComponent<AudioSource>();
        shootEffect2 = GameObject.Find("ShootEffect2").GetComponent<ParticleSystem>();

        crosshairImage = GameObject.Find("Crosshair").GetComponent<Image>();
        activeImage = GameObject.Find("ActiveGun").GetComponent<Image>();
        inactiveImage = GameObject.Find("InactiveGun").GetComponent<Image>();
        ammoImage = GameObject.Find("AmmoSymbol").GetComponent<Image>();

        crosshair1 = Resources.Load<Sprite>("Kenney - Crosshair Pack/crosshair007");
        crosshair2 = Resources.Load<Sprite>("Kenney - Crosshair Pack/crosshair121");
        gun1Sprite = Resources.Load<Sprite>("Kenney - Blaster Kit/blasterB");
        gun2Sprite = Resources.Load<Sprite>("Kenney - Blaster Kit/blasterH");
        ammo1Sprite = Resources.Load<Sprite>("ammo1");
        ammo2Sprite = Resources.Load<Sprite>("ammo2");

        activeNumber = GameObject.Find("ActiveNum").GetComponent<TMP_Text>();
        inactiveNumber = GameObject.Find("InactiveNum").GetComponent<TMP_Text>();
        ammoText = GameObject.Find("AmmoNum").GetComponent<TMP_Text>();
        scoreText = GameObject.Find("ScoreNum").GetComponent<TMP_Text>();

    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            SwitchGun(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            SwitchGun(2);
        }

        if (Input.GetKey(KeyCode.Mouse0)) {

            if (activeGun == 1 && ammo1 > 0 && Time.time >= nextFireTime1) {

                nextFireTime1 = Time.time + 1f/fireRate1;
                ammo1--;
                ammoText.text = ammo1.ToString();
                shootAudio1.Play();
                shootEffect1.Play();
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit)) {
                    GameObject current = Instantiate(impactEffect1, Vector3.Lerp(hit.point, transform.position, 0.1f), Quaternion.identity);
                    Destroy(current, 8);
                    if (hit.transform.gameObject.tag == "Enemy") {
                        hit.transform.gameObject.GetComponent<Enemy>().TakeDamage(damage1);
                    }
                }

            }

        }

        if (Input.GetKeyDown(KeyCode.Mouse0)) {

            if (activeGun == 2 && ammo2 > 0 && Time.time >= nextFireTime2) {

                nextFireTime2 = Time.time + 1f/fireRate2;
                ammo2--;
                ammoText.text = ammo2.ToString();
                shootAudio2.Play();
                shootEffect2.Play();
                Vector3 shootfrom = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
                GameObject currentSnowball = Instantiate(snowball, shootfrom, transform.rotation) as GameObject;
                currentSnowball.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * snowballSpeed, ForceMode.Impulse);
                Destroy(currentSnowball, 10);

            }

        }

    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Ammo1") PickupAmmo(1);
        else if (other.gameObject.tag == "Ammo2") PickupAmmo(2);
        Destroy(other.gameObject);
    }

    void PickupAmmo(int type) {
        if (type == 1) {
            ammo1 += 5;
            if (activeGun == 1) ammoText.text = ammo1.ToString();
        }
        else if (type == 2) {
            ammo2 += 3;
            if (activeGun == 2) ammoText.text = ammo2.ToString();
        }
    }

    void SwitchGun(int on) {
        activeGun = on;
        if (on == 1) {

            gun1.enabled = true;
            gun2.enabled = false;

            crosshairImage.sprite = crosshair1;

            activeImage.sprite = gun1Sprite;
            inactiveImage.sprite = gun2Sprite;
            activeNumber.text = "1";
            inactiveNumber.text = "2";

            ammoImage.sprite = ammo1Sprite;
            ammoText.text = ammo1.ToString();

        }
        else if (on == 2) {

            gun1.enabled = false;
            gun2.enabled = true;

            crosshairImage.sprite = crosshair2;

            activeImage.sprite = gun2Sprite;
            inactiveImage.sprite = gun1Sprite;
            activeNumber.text = "2";
            inactiveNumber.text = "1";

            ammoImage.sprite = ammo2Sprite;
            ammoText.text = ammo2.ToString();

        }
    }

    public int IncreaseScore(int points) {
        score += points;
        scoreText.text = $"SCORE: {score.ToString()}/9";
        if (score == 9) Invoke("Win", 3f);
        return score;
    }

    void Win() {
        GameManager.GameOver(true, score);
    }

}
