using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemoteControl : MonoBehaviour
{
    //config params
    //[SerializeField] Text remoteControlDisplay;
    [SerializeField] Text screenChannelDisplay;
    [SerializeField] int maxDigits = 2;

    //cachced refs
    ScreenChanger screen;

    // Start is called before the first frame update
    void Start()
    {
        //remoteControlDisplay.text = "";
        screen = FindObjectOfType<ScreenChanger>();
    }

    // Update is called once per frame
    void Update()
    {
        KeyBoardInput();
    }

    private void KeyBoardInput()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
        {
            EnterDigit(1);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
        {
            EnterDigit(2);
        }
        if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3))
        {
            EnterDigit(3);
        }
        if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4))
        {
            EnterDigit(4);
        }
        if (Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKeyDown(KeyCode.Alpha5))
        {
            EnterDigit(5);
        }
        if (Input.GetKeyDown(KeyCode.Keypad6) || Input.GetKeyDown(KeyCode.Alpha6))
        {
            EnterDigit(6);
        }
        if (Input.GetKeyDown(KeyCode.Keypad7) || Input.GetKeyDown(KeyCode.Alpha7))
        {
            EnterDigit(7);
        }
        if (Input.GetKeyDown(KeyCode.Keypad8) || Input.GetKeyDown(KeyCode.Alpha8))
        {
            EnterDigit(8);
        }
        if (Input.GetKeyDown(KeyCode.Keypad9) || Input.GetKeyDown(KeyCode.Alpha9))
        {
            EnterDigit(9);
        }
        if (Input.GetKeyDown(KeyCode.Keypad0) || Input.GetKeyDown(KeyCode.Alpha0))
        {
            EnterDigit(0);
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
            SubmitChannel();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            SurfChannel(1);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            SurfChannel(-1);
        }
    }

    public void EnterDigit(int digit)
    {
        if(screenChannelDisplay.text.Length >= maxDigits && Mathf.Abs(int.Parse(screenChannelDisplay.text)) > 9) { screenChannelDisplay.text = ""; }
        screen.ActivateChannelDisplay();
        if (screenChannelDisplay.text.Length <= 1)
        {
            screenChannelDisplay.text = "-" + digit.ToString();
        }
        else if (int.Parse(screenChannelDisplay.text) <= 9)
        {
            screenChannelDisplay.text = Mathf.Abs(int.Parse(screenChannelDisplay.text)) + digit.ToString();
        }

    }

    public void SubmitChannel()
    {
        if (screenChannelDisplay.text == "") { return; }
        int channelEntered = Mathf.Abs(int.Parse(screenChannelDisplay.text));
        screen.ChangeChannel(channelEntered);
        screenChannelDisplay.text = channelEntered.ToString();
        screen.ActivateChannelDisplay();
        if(Mathf.Abs(int.Parse(screenChannelDisplay.text)) <= 9)
        {
            screenChannelDisplay.text = Mathf.Abs(int.Parse(screenChannelDisplay.text)).ToString();
        }
    }

    public void SurfChannel(int upOrDown)
    {
        if (screen.currentChannel == 0 && upOrDown == -1)
        {
            screenChannelDisplay.text = "99";
        }
        else if (screen.currentChannel == 99 && upOrDown == 1)
        {
            screenChannelDisplay.text = "00";
        }
        else
        {
            screenChannelDisplay.text = (screen.currentChannel + upOrDown).ToString();
        }
        SubmitChannel();
    }
}
