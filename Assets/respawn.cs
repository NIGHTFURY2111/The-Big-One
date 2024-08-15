using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class respawn : MonoBehaviour
{
    [SerializeField] private float killzone;
    void Update()
    {
        if (transform.position.y < killzone)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
