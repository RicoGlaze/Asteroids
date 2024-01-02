using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bullet bulletPrefab;
    public Transform shotPoint;

    public float moveSpeed;

    public float timeBetweenShots = 1f;

    private float shotTime;
    private float minTimeBetweenShots = .3f;

    public Rigidbody2D rb;
    private SpriteRenderer sr;
    public Animator anim;

    private AudioSource shot;
    private AudioSource engine;

    private Vector2 moveDirection;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        AudioSource[] allMyAudioSources = GetComponents<AudioSource>();
        shot = allMyAudioSources[0];
        engine = allMyAudioSources[1];
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInputs();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        anim.SetFloat("Horizontal", moveX);
        anim.SetFloat("Vertical", moveY);
        anim.SetFloat("Speed", moveDirection.sqrMagnitude);

        moveDirection = new Vector2 (moveX, moveY).normalized;

        if (Input.GetKey(KeyCode.Space))
        {
            if (Time.time >= shotTime)
            {
                shotPoint.GetComponent<SpriteRenderer>().enabled = true;
                Shoot();
                Invoke(nameof(DisableFlash), .2f);
                shot.Play();
                shotTime = Time.time + timeBetweenShots;
            }
        } else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)){
            engine.Stop();
            engine.Play();
        } else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)){
            engine.Stop();
            engine.Play();
        } else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            engine.Stop();
            engine.Play();
        } else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            engine.Stop();
            engine.Play();
        }
        else
        {
            engine.Stop();
        }
    }
    void DisableFlash()
    {
        shotPoint.GetComponent<SpriteRenderer>().enabled = false;
    }
    void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }

    private void Shoot()
    {
        bullet bullet = Instantiate(this.bulletPrefab, shotPoint.position, this.transform.rotation);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Asteroid")
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = 0.0f;

            this.gameObject.SetActive(false);

            timeBetweenShots = 1;
            FindObjectOfType<GameManager>().PlayerDied();
            FindObjectOfType<BackgroundScroller>().ResetSpeed();
            FindObjectOfType<AsteroidSpawner>().ResetDifficulty();
            FindObjectOfType<Asteroid>().ResetAsteroid();
        }
    }

    public void IncreaseFireRate()
    {
        if(timeBetweenShots > minTimeBetweenShots)
        {
            timeBetweenShots -= 0.1f;
        }
    }

    public void DecreaseFireRate()
    {
        if (timeBetweenShots > minTimeBetweenShots)
        {
            timeBetweenShots += 0.1f;
        }
    }
}
