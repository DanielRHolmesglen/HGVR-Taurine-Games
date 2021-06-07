using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class Timer : MonoBehaviour
{
    public double timeRemaining;
    public bool active;
    private Text text;

    private void Start()
    {
        text = GetComponent<Text>();
    }

    private void Update()
    {
        if (!active) return;
        if (timeRemaining <= 0f)
        {
            // Todo
            active = false;
        }
        
        timeRemaining -= Time.deltaTime;
        text.text = $"{TimeSpan.FromSeconds(timeRemaining):hh\\:mm\\:ss}";
    }
}