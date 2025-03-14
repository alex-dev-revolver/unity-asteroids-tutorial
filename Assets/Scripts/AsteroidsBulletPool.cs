using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance { get; private set; }

    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private int initialPoolSize = 10;

    private Queue<Bullet> bulletPool = new Queue<Bullet>();

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

        // Заполняем пул при старте
        for (int i = 0; i < initialPoolSize; i++)
        {
            AddBulletToPool();
        }
    }

    private void AddBulletToPool()
    {
        Bullet bullet = Instantiate(bulletPrefab);
        bullet.gameObject.SetActive(false);
        bulletPool.Enqueue(bullet);
    }

    public Bullet GetBullet()
    {
        if (bulletPool.Count == 0)
        {
            AddBulletToPool();
        }

        Bullet bullet = bulletPool.Dequeue();
        return bullet;
    }

    public void ReturnBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
        bulletPool.Enqueue(bullet);
    }
}