using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouth : MonoBehaviour
{
    //config params
    [SerializeField] Light screenGlow;
    [SerializeField] public float hungerSpeed = 1f;
    [SerializeField] List<AudioClip> chomps;
    
    //cached refs
    Animator myAnimator;
    SpriteRenderer myRenderer;
    ViewerManager viewerManager;
    ScoreManager scoreManager;
    AudioManager audioManager;


    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myRenderer = GetComponent<SpriteRenderer>();
        viewerManager = FindObjectOfType<ViewerManager>();
        scoreManager = FindObjectOfType<ScoreManager>();
        audioManager = FindObjectOfType<AudioManager>();
        audioManager.ResetSliders();
    }

    // Update is called once per frame
    void Update()
    {
        myRenderer.material.color = screenGlow.color;
        ProcessInput();
    }

    private void ProcessInput()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            EatHalf();
        }
    }

    public void Chomp()
    {
        myAnimator.Play("Mouth Chomp");
    }

    

    public void EatHalf()
    {
        if (!scoreManager.clickedStart || scoreManager.lossTriggered) { return; }
        StartCoroutine(RemoveViewers());
    }

    private void PlaySound(List<AudioClip> clipList)
    {
        int pickedSound = UnityEngine.Random.Range(0, clipList.Count);
        AudioSource.PlayClipAtPoint(clipList[pickedSound], Camera.main.transform.position, audioManager.masterVolume);
    }

    IEnumerator RemoveViewers()
    {
        float removeViewers = Mathf.Ceil((viewerManager.allViewers.Count + 1)/ 2);
        if (viewerManager.allViewers.Count < removeViewers || removeViewers == 0) { Debug.Log("not enough to eat"); yield return null; }
        else if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Mouth Chomp")) { Debug.Log("ALREADY EATING"); yield return null; }
        else
        {
            scoreManager.chomping = true;
            viewerManager.RandomizeHypeChannel();
            Chomp();
            yield return new WaitForSeconds(0.75f);
            PlaySound(chomps);
            scoreManager.gameStarted = true;
            scoreManager.angerMeter.value -= removeViewers * scoreManager.scoreMultiplier * ((scoreManager.chompCount + 1));
            scoreManager.chompCount++;
            for (int i = 0; i < removeViewers; i++)
            {
                Viewer viewerToRemove = viewerManager.allViewers[0];
                viewerManager.allViewers.Remove(viewerToRemove);
                StartCoroutine(viewerToRemove.Splat());
            }
            foreach(Viewer viewer in viewerManager.allViewers)
            {
                viewer.runningAway = true;
                //viewer.moving = true;
            }
            yield return new WaitForSeconds(2f);
            scoreManager.chomping = false;
        }
    }
}
