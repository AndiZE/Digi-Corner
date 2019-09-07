﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour
{
    //private Wheel wheel;

    private bool isActive;

    [SerializeField]
    private Transform inputArea;
    private float xInput = 0;
    private bool isInputActive;
    [SerializeField]
    private SplineMover[] splineIcons;
    [SerializeField]
    private float[] splinePositions;

    private PanelSwitch panelSwitch;
    private void Start()
    {
        Input.simulateMouseWithTouches = true;
        panelSwitch = GetComponent<PanelSwitch>();
    }

    public void Init()
    {
        splineIcons[0].Init(splinePositions[0], panelSwitch.GetCurrentQuestionIcon());
        splineIcons[1].Init(splinePositions[1], panelSwitch.GetSecondQuestionIcon());
        splineIcons[2].Init(splinePositions[2], panelSwitch.GetLastQuestionIcon());
    }

    public void ActivateInput(bool active)
    {
        isInputActive = active;

        if (!active)
        {
            if (xInput > 0)
            {
                SwapRight();
            }
            else
            {
                SwapLeft();
            }

            xInput = 0;
        }
    }

    private void Update()
    {
        if (isInputActive && Input.touchCount == 1)
        {
            xInput += Input.GetTouch(0).deltaPosition.x;
        }

        if (Input.GetMouseButtonDown(2))
        {
            SwapLeft();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            SwapRight();
        }

    }

    private void SwapLeft()
    {
        for (int i = 0; i < splineIcons.Length; i++)
        {
            splineIcons[i].Rotate(GetNextPosition(splineIcons[i].currentPosition), panelSwitch.GetNextQuestionIcon());
        }
        panelSwitch.SwitchNextQuestion();
    }

    private void SwapRight()
    {
        for (int i = 0; i < splineIcons.Length; i++)
        {
            splineIcons[i].Rotate(GetPreviousPosition(splineIcons[i].currentPosition), panelSwitch.GetPreviousQuestionIcon());
        }
        panelSwitch.SwitchPreviousQuestion();
    }

    private float GetNextPosition(float position)
    {
        float temp = 0f;

        if (position + 0.2f > 7)
        {
            position = 0f;
        }

        for (int i = 0; i < splinePositions.Length; i++)
        {
            if (position + 0.2f > splinePositions[splinePositions.Length - 1])
            {
                temp = 7;
                break;
            }

            if (position + 0.2f > splinePositions[i] &&
                position + 0.2f < splinePositions[i + 1])
            {
                //Last Index moves to first position
                temp = splinePositions[i + 1];
                break;
            }
        }
        return temp;
    }

    private float GetPreviousPosition(float position)
    {

        float temp = 0f;
        for (int i = 0; i < splinePositions.Length-1; i++)
        {
            if (position - 0.2f < splinePositions[0] || position - 0.2f > 6)
            {
                temp = splinePositions[splinePositions.Length - 1];
                break;
            }

            if (position - 0.2f > splinePositions[i ] &&
                position - 0.2f < splinePositions[i +1])
            {
                //Last Index moves to first position
                temp = splinePositions[i];
                break;
            }
        }
        return temp;
    }
}
