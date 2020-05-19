using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] float movespeed = 10f;
    [SerializeField] int health = 200;
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 0.7f;
    [SerializeField] GameObject deathVFX;
    [SerializeField] float durationOfExplosion;

    [SerializeField] AudioClip shootSound;
    [SerializeField] [Range(0, 1)] float shootSoundVolume = 0.3f;

    [Header("Projectile")]
    [SerializeField] float firespeed = 1f;
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileFiringPeriod = 0.1f;

     float xMax = 3.46f;
     float xMin = -3.43f;
     float yMax = 0f;
     float yMin = -6.45f;
    
    bool check = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    }

    private void Fire()
    {
        
            if (Input.GetButtonDown("Fire1"))
            {
                StartCoroutine(FireContinuous());
            }
        if (Input.GetButtonUp("Fire1"))
        {
            StopAllCoroutines();
        }



    }
    IEnumerator FireContinuous()
    {
        while(true)
        {
            GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, firespeed);
            AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
        
    }
      




    private void Move()
    {
       
        float deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * movespeed;
        var newXpos = transform.position.x + deltaX;
        
        
        float deltaY = Input.GetAxis("Vertical") * Time.deltaTime * movespeed;
        var newYpos = transform.position.y + deltaY; 
        Vector2 pos = new Vector2(newXpos, newYpos);
        pos.x = Mathf.Clamp(newXpos, xMin, xMax);
        pos.y = Mathf.Clamp(newYpos, yMin, yMax);
        
        transform.position = pos;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        FindObjectOfType<Level>().LoadGameOver();
        Destroy(gameObject);
        GameObject explosion = Instantiate(deathVFX, transform.position, Quaternion.identity);
        Destroy(explosion, durationOfExplosion);
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
    }

    public int GetHealth()
    {
        return health;
    }








}
