using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewerManager : MonoBehaviour
{
    //config params
    public enum InterestLevel { Low, Med, High, Ult };

    [SerializeField] int mouthAwakeThreshold = 20;
    [SerializeField] float maxSpawnDist = 40;
    [SerializeField] float minSpawnDist = 17;
    [SerializeField] Viewer viewerPrefab;
    [SerializeField] float snapIncrement = 0.05f;
    [SerializeField] Text viewerCountText;
    [SerializeField] Text viewersConsumedText;
    [SerializeField] int minLowInterest = 1;
    [SerializeField] int maxLowInterest = 3;
    [SerializeField] int minMedInterest = 4;
    [SerializeField] int maxMedInterest = 9;
    [SerializeField] int minHighInterest = 10;
    [SerializeField] int maxHighInterest = 24;
    [SerializeField] int minUltInterest = 25;
    [SerializeField] int maxUltInterest = 100;
    [SerializeField] float maxSpawnTimer = 3f;
    [SerializeField] float minSpawnTimer = 0.01f;
    [SerializeField] float spawnRateIncrease = 0.1f;


    //cached refs
    public float snapDistance;
    public List<Viewer> allViewers;
    public int viewersConsumed;
    public int hypeChannel;
    public float currentSpawnTimerMax;
    public float spawnTimer;
    Mouth mouth;
    ScreenChanger screen;
    ScoreManager scoreManager;



    // Start is called before the first frame update
    void Start()
    {
        allViewers = new List<Viewer>();
        viewersConsumed = 0;
        RandomizeHypeChannel();
        mouth = FindObjectOfType<Mouth>();
        screen = FindObjectOfType<ScreenChanger>();
        currentSpawnTimerMax = maxSpawnTimer;
        scoreManager = FindObjectOfType<ScoreManager>();
    }


    // Update is called once per frame
    void Update()
    {
        snapDistance = Mathf.Log(allViewers.Count) * snapIncrement;
        AssessHype();
        SpawnTimer();
        UpdateCounts();
    }

    private void AssessHype()
    {
        int distFromHype = Mathf.Abs(hypeChannel - screen.currentChannel);
        if (distFromHype > 50)
        {
            if (hypeChannel > screen.currentChannel)
            {
                distFromHype = (100 - hypeChannel) + (screen.currentChannel);
                Debug.Log("Hype is higher than current - " + distFromHype + " channels away.");
            }
            else if (hypeChannel < screen.currentChannel)
            {
                distFromHype = hypeChannel + (100 - screen.currentChannel);
                Debug.Log("Current is higher than hype - " + distFromHype + " channels away.");
            }
        }
        float newSpawnRate = maxSpawnTimer * ((float)distFromHype / 50);
        Debug.Log("new spawn rate should be " + newSpawnRate);
        if (newSpawnRate < minSpawnTimer)
        {
            newSpawnRate = minSpawnTimer;
        }
        currentSpawnTimerMax = newSpawnRate;
    }

    private void SpawnTimer()
    {
        if (!scoreManager.clickedStart || scoreManager.lossTriggered) { return; }
        if (spawnTimer <= currentSpawnTimerMax)
        {
            spawnTimer += Time.deltaTime * mouth.hungerSpeed;
        }
        else
        {
            SpawnViewer();
            spawnTimer = 0;
        }
    }
    public void RandomizeHypeChannel()
    {
        int newHypeChannel = UnityEngine.Random.Range(0,100);
        while (newHypeChannel == hypeChannel)
        {
            newHypeChannel = UnityEngine.Random.Range(0, 100);
        }
        hypeChannel = newHypeChannel;
    }

    private void UpdateCounts()
    {
        if (allViewers.Count >= mouthAwakeThreshold)
        {
            scoreManager.gameStarted = true;
        }
        viewerCountText.text = "Viewers: " + allViewers.Count.ToString();
        if (viewersConsumed == 0)
        {
            viewersConsumedText.text = "";
        }
        else
        {
            viewersConsumedText.text = "Consumed: " + viewersConsumed;
        }
    }

    public void SpawnViewer()
    {
        float randomZ = UnityEngine.Random.Range(-maxSpawnDist, -minSpawnDist);
        float randomX = UnityEngine.Random.Range(0, maxSpawnDist);
        int randomXDirection = Random.Range(0, 2);
        if (randomXDirection == 0)
        {
            randomX = -randomX;
        }
        Vector3 spawnPoint = new Vector3(randomX, transform.position.y, randomZ);
        var newViewer = Instantiate(viewerPrefab, spawnPoint, Quaternion.identity);
        newViewer.transform.parent = gameObject.transform;
        allViewers.Add(newViewer);
    }

    private int GenerateViewersInterested(InterestLevel interest)
    {
        int viewersToGenerate;
        switch(interest)
        {
            case InterestLevel.Low:
                viewersToGenerate = Random.Range(minLowInterest, maxLowInterest + 1);
                break;
            case InterestLevel.Med:
                viewersToGenerate = Random.Range(minMedInterest, maxMedInterest + 1);
                break;
            case InterestLevel.High:
                viewersToGenerate = Random.Range(minHighInterest, maxHighInterest + 1);
                break;
            case InterestLevel.Ult:
                viewersToGenerate = Random.Range(minUltInterest, maxUltInterest + 1);
                break;
            default:
                viewersToGenerate = 0;
                break;
        }
        return viewersToGenerate;
    }

}
