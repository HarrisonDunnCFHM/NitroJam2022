using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewerManager : MonoBehaviour
{
    //config params
    [SerializeField] float maxSpawnDist = 40;
    [SerializeField] float minSpawnDist = 17;
    [SerializeField] Viewer viewerPrefab;
    [SerializeField] float snapIncrement = 0.05f;
    [SerializeField] Text viewerCountText;
    [SerializeField] Text viewersConsumedText;


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


}
