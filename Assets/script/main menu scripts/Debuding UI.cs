using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DebudingUI : MonoBehaviour
{
    [SerializeField] Transform canvas;
    List<TextMeshProUGUI> ui_elements = new();
    PlayerStateMachine psm;
    string currentState = null;
    string lastState = null;
    string lasterState = null;

    bool timerActive;
    float currTime;
    float bestTime;

    int score;
    float mult;
  

    void Start()
    {
        currTime = 0f;
        bestTime = 0f;

        Timer.OnTimerStart += TimerStart;
        Timer.OnTimerStop += TimerStop;
        respawn.OnRespawn += TimerStopRespawn;


        foreach (Transform t in canvas)
        {
            ui_elements.Add(t.GetComponent<TextMeshProUGUI>());

        }
        psm = GetComponent<PlayerStateMachine>();
        currentState = psm._currentState.ToString();
    }


    void TimerStart()
    {
        currTime = 0f;
        timerActive = true;
    }

    void TimerStop()
    {
        if (bestTime != 0f)
        {
            if (currTime < bestTime)
            {
                bestTime = currTime;
            }
        }
        else 
        {
            bestTime = currTime;
        
        }
        timerActive = false;
    }

    void TimerStopRespawn()
    {
        currTime = 0f;
        timerActive = false;
    }
    
    // Update is called once per frame
    void LateUpdate()
    {
        if (currentState != psm._currentState.ToString())
        {
            lasterState = lastState;
            lastState = currentState;
            currentState = psm._currentState.ToString();
        }

        if (timerActive)
        {
            currTime += Time.deltaTime;
        }
        score = Mathf.RoundToInt(currTime*mult);
        TimeSpan time = TimeSpan.FromSeconds(currTime);
        TimeSpan besttime = TimeSpan.FromSeconds(bestTime);
        


        //ui_elements[0].text = (psm._getPCC.GetCurrentHorizontal()).ToString();


        ui_elements[0].text = $@"H = {((int)psm._getPCC.GetCurrentHorizontal()).ToString()}     V = {((int)psm._getPCC.GetCurrentVertical()).ToString()}";


        //ui_elements[1].text = (psm._getPCC.GetCurrentVertical()).ToString();
        ui_elements[1].text = time.ToString(@"mm\:ss\:fff");
        ui_elements[2].text = "Best time: " + besttime.ToString(@"mm\:ss\:fff");
        //ui_elements[2].text = psm._getPCC._TGTvelvocity.ToString() +"     "+ psm._getPCC._acceleration.ToString();

        //Debug.Log(ui_elements[0].name);
    }
}
