using System.Collections.Generic;
using UnityEngine;

public class AsteroidPool : MonoBehaviour
{
    public static AsteroidPool Instance { get; private set; }

    [SerializeField] private Asteroid asteroidPrefab;
    [SerializeField] private int poolSize = 10;

    private Dictionary<float, Queue<Asteroid>> asteroidPools = new Dictionary<float, Queue<Asteroid>>();
    private float[] asteroidSizes = { 1.65f, 1.0f, 0.75f, 0.35f }; // Четыре типа размеров

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (float size in asteroidSizes)
        {
            asteroidPools[size] = new Queue<Asteroid>();
            for (int i = 0; i < poolSize; i++)
            {
                AddAsteroidToPool(size);
            }
        }
    }

    private void AddAsteroidToPool(float size)
    {
        Asteroid asteroid = Instantiate(asteroidPrefab);
        asteroid.gameObject.SetActive(false);
        asteroid.size = size;
        asteroidPools[size].Enqueue(asteroid);
    }

    public Asteroid GetAsteroid(float size, Vector2 position, Quaternion rotation)
    {
        if (asteroidPools[size].Count == 0)
        {
            AddAsteroidToPool(size);
        }

        Asteroid asteroid = asteroidPools[size].Dequeue();
        asteroid.transform.position = position;
        asteroid.transform.rotation = rotation;
        asteroid.transform.localScale = Vector3.one * size;
        asteroid.size = size;
        asteroid.gameObject.SetActive(true);
        return asteroid;
    }

    public void ReturnAsteroid(Asteroid asteroid)
    {
        asteroid.gameObject.SetActive(false);
        asteroidPools[asteroid.size].Enqueue(asteroid);
    }
}