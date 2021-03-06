using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viewer : MonoBehaviour
{
    //config params
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float defaultY = 0.8f;
    [SerializeField] ParticleSystem mySplat;
    [SerializeField] GameObject myHead;
    [SerializeField] List<AudioClip> oohs;
    [SerializeField] List<AudioClip> ohNos;
    
    //cached references
    public float targetX;
    public float targetZ;
    Vector3 startPos;
    Vector3 targetPoint;
    Vector3 targetDirection;
    public bool moving;
    public bool runningAway;
    ViewerManager viewerManager;
    Renderer myRenderer;
    AudioManager audioManager;
    bool oohed;
    bool ohNoed;
    
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        myRenderer = GetComponent<Renderer>();
        viewerManager = FindObjectOfType<ViewerManager>();
        moving = true;
        transform.localPosition = new Vector3 (transform.localPosition.x, defaultY, transform.localPosition.z);
        targetPoint = new Vector3(targetX, defaultY, targetZ);
        audioManager = FindObjectOfType<AudioManager>();
        oohed = false;
    }

    // Update is called once per frame
    void Update()
    {
        MoveToTarget();
        RunAway();
    }

    private void MoveToTarget()
    {
        if(moving)
        {
            targetDirection = targetPoint - transform.localPosition;
            transform.Translate(targetDirection * moveSpeed * Time.deltaTime);
            //viewerBody.velocity = targetDirection * moveSpeed * Time.deltaTime;
            float distFromTarget = Vector3.Magnitude(transform.localPosition - targetPoint);
            if (distFromTarget <= viewerManager.snapDistance)
            {
                //transform.localPosition = new Vector3(targetPoint.x, targetPoint.y, targetPoint.z);
                if(!oohed)
                {
                    PlaySound(oohs);
                    oohed = true;
                }
                moving = false;
            }
        }
    }
    private void PlaySound(List<AudioClip> clipList)
    {
        int pickedSound = UnityEngine.Random.Range(0, clipList.Count);
        AudioSource.PlayClipAtPoint(clipList[pickedSound], Camera.main.transform.position, audioManager.masterVolume);
    }

    private void RunAway()
    {
        if (runningAway)
        {
            if(!ohNoed)
            {
                PlaySound(ohNos);
                ohNoed = true;
            }
            moving = false;
            targetDirection = startPos - transform.localPosition;
            transform.Translate(targetDirection * moveSpeed * Time.deltaTime);
            //viewerBody.velocity = targetDirection * moveSpeed * Time.deltaTime;
            float distFromHome = Vector3.Magnitude(transform.localPosition - startPos);
            if (distFromHome <= 2f)
            {
                //transform.localPosition = new Vector3(targetPoint.x, targetPoint.y, targetPoint.z);
                viewerManager.allViewers.Remove(this);
                Destroy(gameObject);
            }
        }
    }

    public IEnumerator Splat()
    {
        ParticleSystem newSplat = Instantiate(mySplat, transform.position, Quaternion.identity);
        myHead.GetComponent<Collider>().enabled = false;
        myHead.GetComponent<Renderer>().enabled = false;
        myRenderer.enabled = false;
        GetComponent<Collider>().enabled = false;
        viewerManager.viewersConsumed++;
        yield return new WaitForSeconds(3f);
        Destroy(newSplat);
        Destroy(gameObject);
    }
}
