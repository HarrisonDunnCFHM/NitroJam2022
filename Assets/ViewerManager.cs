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


    //cached refs
    public float snapDistance;
    public List<Viewer> allViewers;



    // Start is called before the first frame update
    void Start()
    {
        allViewers = new List<Viewer>();   
    }

    // Update is called once per frame
    void Update()
    {
        snapDistance = Mathf.Log(allViewers.Count) * snapIncrement;
        viewerCountText.text = "Viewers: " + allViewers.Count.ToString();
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
