using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 500f;
    public float maxLifetime = 10f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Shoot(Vector2 direction)
    {
        gameObject.SetActive(true);
        rb.linearVelocity = Vector2.zero; // Сбрасываем скорость перед повторным использованием
        rb.angularVelocity = 0f;   // Убираем вращение
        rb.AddForce(direction * speed);

        // Возвращаем пулю в пул через maxLifetime
        Invoke(nameof(ReturnToPool), maxLifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ReturnToPool();
    }

    private void ReturnToPool()
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        gameObject.SetActive(false);
        BulletPool.Instance.ReturnBullet(this);
    }
}