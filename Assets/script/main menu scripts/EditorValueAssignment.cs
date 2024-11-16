using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorValueAssignment : MonoBehaviour
{
    List<Transform> ChildElements = new();
    List<Transform> sliders = new();
    [SerializeField]PlayerStateMachine psm;
    private void Start()
    {
        foreach (Transform child in transform)
        {
            Debug.Log(child.transform.GetChild(1).name);
            ChildElements.Add(child.transform.GetChild(1));
        }

        foreach(Transform element in ChildElements)
        {
            Debug.Log(element.GetChild(0).transform.GetChild(0).name);
            sliders.Add(element.GetChild(0).transform.GetChild(0));
        }
        
        setupSliders();
    }

    void setupSliders()
    {
        foreach(Transform element in sliders)
        {
            ContentSliderSetup elem = element.GetComponent<ContentSliderSetup>();

            elem.SetupBlock(nameof(psm._dashSpeed),psm._dashSpeed, 0f, 5f, psm._DashModifier);
        }
    }


}

public class FloatValue
{
    public float value = 1f;
}
