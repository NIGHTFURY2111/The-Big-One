using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebudingUI : MonoBehaviour
{
    [SerializeField]PlayerStateMachine psm;
    [SerializeField] Transform canvas;
    [SerializeField] GameObject editorFieldPrefab;
    [SerializeField] float editorHidingSpeed;

    List<TextMeshProUGUI> ui_elements = new();
    List<Transform> Editor_elements = new();
    
    Transform InfoFields, editorField;
    
    string currentState = null;
    string lastState = null;
    string lasterState = null;

    bool timerActive, isMoving;
    float currTime;
    float bestTime;
    float mult;

    float showPosi, HidePosi;
    int score;


    void CanvasValueAssignment()
    {
        InfoFields = canvas.transform.GetChild(0);
        editorField= canvas.transform.GetChild(1);
        foreach (Transform t in InfoFields) { ui_elements.Add(t.GetComponent<TextMeshProUGUI>()); }
        foreach (Transform t in editorField) { Editor_elements.Add(t.GetComponent<Transform>()); }
    }

    void EventSubscriber()
    {
        Timer.OnTimerStart += TimerStart;
        Timer.OnTimerStop += TimerStop;
        respawn.OnRespawn += TimerStopRespawn;


        Debug.LogWarning("only turn off when debugging");
        //turn on when building the game
        PlayerStateMachine.GamePaused += HideAndShowEditor;
    }

    void Start()
    {
        currTime = bestTime = 0f;
        showPosi = -355f;
        HidePosi = 100f;

        CanvasValueAssignment();

        EventSubscriber();

        //currentState = psm._currentState.ToString();
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

    private void Update()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //HandleInfoStates();

        HandleInfoSpeed();
        
        HandleInfoTimer();

        HandleInfoStates();
    }

    void HandleInfoTimer() { 

        if (timerActive)
        {
            currTime += Time.deltaTime;
        }
        score = Mathf.RoundToInt(currTime*mult);

        TimeSpan time = TimeSpan.FromSeconds(currTime);
        TimeSpan besttime = TimeSpan.FromSeconds(bestTime);
        ui_elements[1].text = time.ToString(@"mm\:ss\:fff");
        ui_elements[2].text = "Best time: " + besttime.ToString(@"mm\:ss\:fff");
    }


    void HandleInfoSpeed()
    {
        ui_elements[0].text = $@"H = {((int)psm._getPCC.GetCurrentHorizontal()).ToString()}     V = {((int)psm._getPCC.GetCurrentVertical()).ToString()}";

    }

    void HandleInfoStates()
    {
        if (currentState != psm._currentState.ToString())
        {
            lasterState = lastState;
            lastState = currentState;
            currentState = psm._currentState.ToString();
        }
        ui_elements[3].text = $@"{lasterState}  >>>  {lastState}  >>>  {currentState}";
        //TODO display this shit
    }

    public void HideAndShowEditor(bool show)
    { 
        editorField.gameObject.SetActive(show);
    }

    public void hideButtonAreas(GameObject obj)
    {
        foreach(Transform Element in Editor_elements)
        {
            Element.GetChild(1).gameObject.SetActive(false);
        }

        obj.transform.parent.transform.GetChild(1).gameObject.SetActive(true);

    }
}
