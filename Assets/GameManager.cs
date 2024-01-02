using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public PlayerMovement player;
    public ParticleSystem explosion;
    private SceneTransitions sceneTransitions;

    private AudioSource music;
    private AudioSource hit;

    public int lives = 3;
    public float respawnTime = 1.0f;
    public float spawnInvulnerability = 1.5f;

    public Image[] healthNodes;
    public Sprite fullHealthNode;
    public Sprite emptyHealthNode;

    public TMP_Text scoreValue;
    public TMP_Text highScore;

    public int score = 0;

    private int asteroidsDestroyed = 0;
    private int asteroidsDestroyedTotal = 0;

    public void Start()
    {
        AudioSource[] allMyAudioSources = GetComponents<AudioSource>();
        sceneTransitions = FindObjectOfType<SceneTransitions>();
        UpdateHighScoreText();
        music = allMyAudioSources[0];
        hit = allMyAudioSources[1];
    }

    void UpdateScoreUI(int score)
    {
        scoreValue.text = "" + score;
    }

    void UpdateHighScoreText()
    {
        highScore.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
    }

    void UpdateHealthUI(int currentHealth)
    {
        for(int i = 0; i < healthNodes.Length; i++)
        {
            if(i < currentHealth)
            {
                healthNodes[i].sprite = fullHealthNode;
            }
            else{
                healthNodes[i].sprite = emptyHealthNode;
            }
        }
    }
    public void AsteroidDestroyed(Asteroid asteroid)
    {
        this.explosion.transform.position = asteroid.transform.position;
        this.explosion.Play();
        hit.Play();

        if(asteroid.size < 1.0f)
        {
            this.score += 100;
        }else if(asteroid.size < 2.0f)
        {
            this.score += 50;
        }
        else
        {
            this.score += 25;
        }

        CheckHighScore();

        UpdateScoreUI(score);
        asteroidsDestroyed++;
        asteroidsDestroyedTotal++;
        asteroidsDestroyed = FindObjectOfType<AsteroidSpawner>().IncreaseDifficulty(asteroidsDestroyed);
        FindObjectOfType<BackgroundScroller>().IncreaseSpeed();
        FindObjectOfType<Asteroid>().IncreaseAsteroidSpeed();
    }

    public void CheckHighScore()
    {
        if(score > PlayerPrefs.GetInt("HighScore", 0)){
            PlayerPrefs.SetInt("HighScore", score);
            UpdateHighScoreText();
        }
    }

    public void AddHealth()
    {
        this.lives++;
        UpdateHealthUI(lives);
    }

    public void PlayerDied()
    {
        this.explosion.transform.position = this.player.transform.position;
        this.explosion.Play();
        hit.Play();

        this.lives--;
        UpdateHealthUI(lives);

        if(this.lives <= 0)
        {
            //GameOver();
            sceneTransitions.LoadScene("Lose");
        }
        else
        {
            Invoke(nameof(Respawn), this.respawnTime);
        }
    }

    private void Respawn()
    {
        this.player.transform.position = new Vector3(0,0,-1);
        this.player.gameObject.layer = LayerMask.NameToLayer("IgnoreCollisions");
        this.player.gameObject.SetActive(true);

        Invoke(nameof(TurnOnCollisions), this.spawnInvulnerability);
    }

    private void TurnOnCollisions()
    {
        this.player.gameObject.layer = LayerMask.NameToLayer("Player");
    }

    private void GameOver()
    {
        this.lives = 3;
        this.score = 0;

        UpdateHealthUI(lives);
        UpdateScoreUI(score);

        Invoke(nameof(Respawn), this.respawnTime);
    }
}
