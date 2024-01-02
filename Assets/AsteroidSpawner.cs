using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public float spawnRate = 2.0f;
    public int spawnAmount = 1;
    public float spawnDistance = 15.0f;
    public float trajectoryVariance = 15.0f;

    private float minSpawnRate = 0.5f;

    public Asteroid asteroidPrefab;
    // Start is called before the first frame update
    private void Start()
    {
        InvokeRepeating(nameof(Spawn), this.spawnRate, this.spawnRate);
    }

    private void Spawn()
    {
        for(int i = 0; i < this.spawnAmount; i++)
        {
            Vector3 spawnDirection = Random.insideUnitCircle.normalized * this.spawnDistance;
            Vector3 spawnPoint = this.transform.position + spawnDirection;

            float variance = Random.Range(-this.trajectoryVariance, this.trajectoryVariance);
            Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward);

            Asteroid asteroid = Instantiate(this.asteroidPrefab, spawnPoint, rotation);
            asteroid.size = Random.Range(asteroid.minSize, asteroid.maxSize);
            asteroid.SetTrajectory(rotation * -spawnDirection);
        }
    }

    public int IncreaseDifficulty(int asteroidsDestroyed)
    {
        if (spawnAmount != 3) {
            if (asteroidsDestroyed == 5) {
                spawnAmount++;
                return asteroidsDestroyed = 0;
            }
        }
        else if(asteroidsDestroyed == 5){
            if(minSpawnRate < spawnRate)
            {
                spawnRate -= 0.1f;
            }
            return asteroidsDestroyed = 0;
        }
        return asteroidsDestroyed;
    }

    public void ResetDifficulty()
    {
        spawnAmount = 1;
        spawnRate = 2.0f;
    }
}
