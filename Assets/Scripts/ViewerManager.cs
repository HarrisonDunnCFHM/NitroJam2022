using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewerManager : MonoBehaviour
{
    //config params
    public enum InterestLevel { Low, Med, High, Ult };
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


    //cached refs
    public float snapDistance;
    public List<Viewer> allViewers;
    public int viewersConsumed;



    // Start is called before the first frame update
    void Start()
    {
        allViewers = new List<Viewer>();
        viewersConsumed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        snapDistance = Mathf.Log(allViewers.Count) * snapIncrement;
        UpdateCounts();
    }

    private void UpdateCounts()
    {
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
