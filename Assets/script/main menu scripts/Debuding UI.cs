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
    void Start()
    {
        foreach (Transform t in canvas)
        {
            ui_elements.Add(t.GetComponent<TextMeshProUGUI>());

        }
        psm = GetComponent<PlayerStateMachine>();
        currentState = psm._currentState.ToString();
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
        //ui_elements[0].text = (psm._getPCC.GetCurrentHorizontal()).ToString();


        ui_elements[0].text = $@"H = {((int)psm._getPCC.GetCurrentHorizontal()).ToString()}     V = {((int)psm._getPCC.GetCurrentVertical()).ToString()}";


        //ui_elements[1].text = (psm._getPCC.GetCurrentVertical()).ToString();
        ui_elements[1].text = lasterState + "  =>  " + lastState + "  =>  "+ currentState;
        ui_elements[2].text = psm._getPCC._TGTvelvocity.ToString() +"     "+ psm._getPCC._acceleration.ToString();
        
        //Debug.Log(ui_elements[0].name);
    }
}