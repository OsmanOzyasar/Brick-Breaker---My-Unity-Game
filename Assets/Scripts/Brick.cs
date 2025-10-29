using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public int brickHealth = 50;
    public bool isBreakible = true;
    private float interval = 7;
    private float timer;
    private BrickSpawner1 spawner;
    private GameManager gameManager;

    public static event Action<Brick> OnBrickDestroyed;

    private void Start()
    {
        spawner = FindFirstObjectByType<BrickSpawner1>();
        gameManager = FindFirstObjectByType<GameManager>();
    }
    private void Update()
    {
        timer += Time.deltaTime;

        if(timer >= interval)
        {
            timer = 0;
            MoveBricks();
        }
        
        if(brickHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if(spawner != null)
        {
            spawner.OnBrickDestroyed(this);
            OnBrickDestroyed?.Invoke(this);
        }
        
    }

    void MoveBricks()
    {
        float height = GetComponent<SpriteRenderer>().bounds.size.y;
        transform.position = new Vector2(transform.position.x, transform.position.y - height);
    }
}
