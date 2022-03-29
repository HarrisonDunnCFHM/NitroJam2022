using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenChanger : MonoBehaviour
{
    //config params
    [SerializeField] Light screenGlow;
    [SerializeField] Light myDirectional;
    [SerializeField] Mouth mouth;
    [SerializeField] Text channelDisplay;
    [SerializeField] float channelDisplayTimeOut = 2f;
    [SerializeField] [Range(0, 1)] float minFlicker = 0.7f;
    [SerializeField] float brightnessAdjustSpeed = 1f;
    [SerializeField] float minFlickerCooldown = 0.01f;
    [SerializeField] float maxFlickerCooldown = 0.1f;
    [SerializeField] List<GameObject> channelSprites;
    [SerializeField] Canvas spriteCanvas;

    //cached references
    Material myMaterial;
    SpriteRenderer mouthRenderer;
    public bool channelDisplayActive;
    float channelDisplayTimer;
    public int currentChannel;
    int currentChannelSpriteIndex;
    GameObject currentChannelSprite;
    
    // Start is called before the first frame update
    void Start()
    {
        myMaterial = GetComponent<Renderer>().material;
        mouthRenderer = mouth.GetComponent<SpriteRenderer>();
        channelDisplayActive = false;
        channelDisplay.text = "";
        channelDisplayTimer = 0f;
        int currentChannelSpriteIndex = -1;
        //currentChannelSprite = null;
    }

    // Update is called once per frame
    void Update()
    {
        ChannelDisplayTimeOut();
        AdjustBrightness(); //adjusts brightness after flicker
        ManageChannelSprite();
    }

    private void ManageChannelSprite()
    {
        //do I need this ???
        //if(currentChannelSpriteIndex == -1)
        //{
        //  currentChannelSprite.SetActive(false);
        //}
        //else
        //{
        //    currentChannelSprite.SetActive(true);
        //} 
    }

    private void ChannelDisplayTimeOut()
    {
        if (channelDisplayActive)
        {
            channelDisplayTimer -= Time.deltaTime;
            if (channelDisplayTimer <= 0)
            {
                channelDisplay.text = "";
                channelDisplayActive = false;
            }
        }
    }

    public void ActivateChannelDisplay()
    {
        channelDisplayActive = true;
        channelDisplayTimer = channelDisplayTimeOut;
    }

    private void AdjustBrightness()
    {
        myDirectional.intensity = screenGlow.intensity;
        if (screenGlow.intensity < 1)
        {
            screenGlow.intensity += Time.deltaTime * brightnessAdjustSpeed;
        }
        else { screenGlow.intensity = 1f; }
    }

    private IEnumerator ScreenFlicker()
    {
        screenGlow.intensity = minFlicker;
        float randomSeconds1 = UnityEngine.Random.Range(minFlickerCooldown, maxFlickerCooldown);
        yield return new WaitForSeconds(randomSeconds1);
        screenGlow.intensity = minFlicker;
        float randomSeconds2 = UnityEngine.Random.Range(minFlickerCooldown, maxFlickerCooldown);
        yield return new WaitForSeconds(randomSeconds2);
        screenGlow.intensity = minFlicker;
    }

    public void ChangeChannel(int channel)
    {
        currentChannel = channel;
        StartCoroutine(ScreenFlicker());
        RandomChannelSprite();
        if (channel < 10)
        {
            ChangeToRed();
        }
        else if (channel % 2 == 0)
        {
            ChangeToBlue();
        }
        else if (channel == 69)
        {
            ChangeToGreen();
        }
        else
        {
            ClearColor();
        }
    }

    public void ChangeToRed()
    {
        myMaterial.color = Color.red;
        screenGlow.color = Color.red;
    }
    public void ChangeToBlue()
    {
        myMaterial.color = Color.blue;
        screenGlow.color = Color.blue;
    }
    public void ChangeToGreen()
    {
        myMaterial.color = Color.green;
        screenGlow.color = Color.green;
    }

    public void ClearColor()
    {
        myMaterial.color = Color.white;
        screenGlow.color = Color.white;
    }

    public void RandomChannelSprite()
    {
        int newChannelSpriteIndex = UnityEngine.Random.Range(0, channelSprites.Count);
        while (newChannelSpriteIndex == currentChannelSpriteIndex)
        {
            newChannelSpriteIndex = UnityEngine.Random.Range(0, channelSprites.Count);
        }
        if(currentChannelSprite != null)
        {
            Destroy(currentChannelSprite);
        }
        currentChannelSprite = Instantiate(channelSprites[newChannelSpriteIndex], transform.position, Quaternion.identity);
        currentChannelSprite.transform.SetParent(spriteCanvas.transform, false);
        //currentChannelSprite.transform.parent = spriteCanvas.transform;
        currentChannelSprite.transform.localPosition = new Vector3(0f, 0f, -0.5f);
        //currentChannelSprite.transform.localScale = new Vector3(1f, 1f, 1f);
        currentChannelSpriteIndex = newChannelSpriteIndex;
    }
    
}
