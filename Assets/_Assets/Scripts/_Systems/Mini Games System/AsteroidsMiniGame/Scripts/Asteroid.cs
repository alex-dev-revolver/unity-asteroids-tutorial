using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Asteroid : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Sprite[] sprites;

    public float size = 1f;
    public float minSize = 0.35f;
    public float maxSize = 1.65f;
    public float movementSpeed = 50f;
    public float maxLifetime = 30f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        transform.eulerAngles = new Vector3(0f, 0f, Random.value * 360f);
        transform.localScale = Vector3.one * size;
        rb.mass = size;

        Invoke(nameof(ReturnToPool), maxLifetime);
    }

    public void SetTrajectory(Vector2 direction)
    {
        // rb.linearVelocity = direction * movementSpeed;
        rb.AddForce(direction * movementSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            if ((size * 0.5f) >= minSize)
            {
                CreateSplit();
                CreateSplit();
            }

            AsteroidsGameManager.Instance.OnAsteroidDestroyed(this);
            ReturnToPool();
        }
    }

    private Asteroid CreateSplit()
    {
        Vector2 position = (Vector2)transform.position + (Random.insideUnitCircle * 0.5f);

        // Приводим размер к ближайшему из доступных
        float newSize = GetClosestSize(size * 0.5f);

        Asteroid half = AsteroidPool.Instance.GetAsteroid(newSize, position, transform.rotation);
        half.SetTrajectory(Random.insideUnitCircle.normalized);

        return half;
    }
    
    private float GetClosestSize(float targetSize)
    {
        float[] availableSizes = { 1.65f, 1.0f, 0.75f, 0.35f };
        float closestSize = availableSizes[0];
        float minDifference = Mathf.Abs(targetSize - closestSize);

        foreach (float size in availableSizes)
        {
            float difference = Mathf.Abs(targetSize - size);
            if (difference < minDifference)
            {
                closestSize = size;
                minDifference = difference;
            }
        }

        return closestSize;
    }


    private void ReturnToPool()
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        gameObject.SetActive(false);
        AsteroidPool.Instance.ReturnAsteroid(this);
    }
}
