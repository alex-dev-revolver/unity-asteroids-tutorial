using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public float spawnDistance = 12f;
    public float spawnRate = 1f;
    public int amountPerSpawn = 1;
    [Range(0f, 45f)]
    public float trajectoryVariance = 15f;

    private float[] asteroidSizes = { 1.65f, 1.0f, 0.75f, 0.35f };

    private void Start()
    {
        InvokeRepeating(nameof(Spawn), spawnRate, spawnRate);
    }

    public void Spawn()
    {
        for (int i = 0; i < amountPerSpawn; i++)
        {
            Vector3 spawnDirection = Random.insideUnitCircle.normalized;
            Vector3 spawnPoint = transform.position + (spawnDirection * spawnDistance);
            float variance = Random.Range(-trajectoryVariance, trajectoryVariance);
            Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward);

            // Выбираем случайный размер астероида из 4 возможных
            float size = asteroidSizes[Random.Range(0, asteroidSizes.Length)];

            Asteroid asteroid = AsteroidPool.Instance.GetAsteroid(size, spawnPoint, rotation);

            Vector2 trajectory = rotation * -spawnDirection;
            asteroid.SetTrajectory(trajectory);
        }
    }
}