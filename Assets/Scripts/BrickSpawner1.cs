using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BrickSpawner1 : MonoBehaviour
{
    [SerializeField] GameObject brick;
    [SerializeField] Transform spawnPoint;
    [SerializeField] Transform parentObject;
    [SerializeField] int spawnTimer;
    public int brokenBrickCount;
    [SerializeField] int rowBrickCount;
    private bool isTimerFinish;
    private bool isTimerRunning;
    public List<GameObject> bricks = new List<GameObject>();

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTimerRunning)
        {
            StartCoroutine(nameof(WaitForSpawn));
        }
        
        if (bricks.Count == 0 || isTimerFinish == true)
        {
            int rowCount = rowBrickCount; 
            //float spacing = brick.transform.localScale.x;
            float spacing = brick.GetComponent<SpriteRenderer>().bounds.size.x;

            for (int i = 0; i < rowCount; i++)
            {
                Vector2 spawnPos = new Vector2(
                    spawnPoint.position.x + (i * spacing) + brick.transform.localScale.x / 2,
                    spawnPoint.position.y + brick.GetComponent<SpriteRenderer>().bounds.size.y / -2
                );

                var clone = Instantiate(brick, spawnPos, Quaternion.identity, parentObject);
                bricks.Add(clone);

            }
            
            isTimerFinish = false;
        }

       
    }

    public void OnBrickDestroyed(Brick brick)
    {
        bricks.Remove(brick.gameObject);
        brokenBrickCount++;
    }

    IEnumerator WaitForSpawn()
    {
        isTimerRunning = true;
        yield return new WaitForSeconds(spawnTimer);
        isTimerFinish = true;
        isTimerRunning = false;
    }
}
