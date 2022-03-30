using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    //config params
    [SerializeField] Text timerText;
    [SerializeField] Slider angerMeter;
    [SerializeField] float mouthScale = 2f;
    [SerializeField] Image blackScreen;
    [SerializeField] float fadeSpeed = 1f;
    [SerializeField] Text gameOverText;
    [SerializeField] Text consumedCount;
    [SerializeField] Button start;

    //cached refs
    float currentTimer;
    public bool gameStarted;
    Mouth mouth;
    bool lossTriggered;
    bool clickedStart;
    bool angerActive;

    private void Awake()
    {
        int numberOfManagers = FindObjectsOfType<ScoreManager>().Length;
        if (numberOfManagers > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentTimer = 0f;
        mouth = FindObjectOfType<Mouth>();
        lossTriggered = false;
        gameOverText.enabled = false;
        timerText.text = "";
        clickedStart = false;
        angerMeter.gameObject.SetActive(false);
        angerActive = false;
    }


    // Update is called once per frame
    void Update()
    {
        FadeInScreen();
        if (gameStarted && !lossTriggered)
        {
            if (!angerActive)
            {
                angerMeter.gameObject.SetActive(true);
                angerActive = true;
            }
            currentTimer += Time.deltaTime;
            var timerAsText = FormatTime(currentTimer);
            timerText.text = timerAsText;
            angerMeter.value += Time.deltaTime; 
        }
        CheckForLoss();
    }
    private void FadeInScreen()
    {
        if (lossTriggered || !clickedStart) { return; }
        if(blackScreen.color.a >= 0)
        {
            blackScreen.color = new Color(0f, 0f, 0f, blackScreen.color.a - (Time.deltaTime * fadeSpeed));
        }
    }

    public void StartGame()
    {
        clickedStart = true;
        start.gameObject.SetActive(false);
    }

    private void CheckForLoss()
    {
        if (angerMeter.value == angerMeter.maxValue && !lossTriggered)
        {
            lossTriggered = true;
            mouth.gameObject.transform.localScale = new Vector3(mouth.gameObject.transform.localScale.x * mouthScale,
                                                                mouth.gameObject.transform.localScale.y * mouthScale,
                                                                mouth.gameObject.transform.localScale.z * mouthScale);
            mouth.gameObject.transform.localPosition = new Vector3(mouth.gameObject.transform.localPosition.x,
                                                                    mouth.gameObject.transform.localPosition.y,
                                                                    -2f);
            mouth.Chomp();
            StartCoroutine(GameOver());
        }
    }

    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(0.8f);
        blackScreen.color = Color.black;
        yield return new WaitForSeconds(1f);
        gameOverText.enabled = true;
        yield return new WaitForSeconds(1f);
        consumedCount.gameObject.transform.SetParent(blackScreen.gameObject.transform);
        yield return new WaitForSeconds(1f);
        timerText.gameObject.transform.SetParent(blackScreen.gameObject.transform);
    }

    private string FormatTime(float time)
    {
        int minutes = (int)time / 60;
        int seconds = (int)time - 60 * minutes;
        int milliseconds = (int)(10 * (time - minutes * 60 - seconds));
        return string.Format("{0:00}:{1:00}.{2:0}", minutes, seconds, milliseconds);
    }
}
