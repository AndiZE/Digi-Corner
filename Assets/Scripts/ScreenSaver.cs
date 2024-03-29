﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSaver : MonoBehaviour
{
    [SerializeField]
    private GameObject screenSaver;
    [SerializeField]
    private GameObject infoPanel;
    [SerializeField]
    private GameObject startPanel;
    [SerializeField]
    private float inactiveTime = 120f;
    [SerializeField]
    private float timer;
    private bool isActive;
    void Update()
    {

        if (!isActive && timer > inactiveTime)
        {
            UpdateScreenSaver(true);
            isActive = true;
        }
        else if (timer < inactiveTime)
        {
            timer += Time.deltaTime;
        }
    }

    public void GetInput(int categorie, int topic)
    {
        if (categorie >= 0 && topic >= 0)
        {
            UpdateScreenSaver(false);
            startPanel.SetActive(false);
        }
        timer = 0;
    }

    private void UpdateScreenSaver(bool state)
    {
        screenSaver.SetActive(state);
        isActive = state;
        if (state)
        {
            GetComponent<PanelSwitch>().ActivateScreensaver();
            infoPanel.SetActive(false);
            startPanel.SetActive(false);
        }
    }
    public void ResetTimer()
    {
        timer = 0f;
    }

}
