﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonDisplayer : MonoBehaviour
{
    [SerializeField]
    private QuestionContainer[] questions;
    private int currentIndex = 0;

    public QuestionContainer GetCurrentQuestion()
    {
        return questions[currentIndex];
    }

    public void OnTopicSwitched()
    {
        currentIndex = 0;
    }

    public void NextQuestion()
    {
        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = questions.Length - 1;
        }
    }

    public void PreviousQuestion()
    {
        currentIndex++;
        if (currentIndex > questions.Length - 1)
        {
            currentIndex = 0;
        }
    }

    public Sprite GetPreviousIcon()
    {
        return questions[(int)Modulo(currentIndex + 2, questions.Length)].icon;
    }
    public Sprite GetNextIcon()
    {
        return questions[(int)Modulo(currentIndex - 2, questions.Length)].icon;

    }

    float Modulo(float a, float b)
    {
        return a - b * Mathf.Floor(a / b);
    }

    public Sprite GetCurrentIcon()
    {
        return questions[currentIndex].icon;

    }
    public Sprite GetSecondIcon()
    {
        return questions[1].icon;
    }

    public Sprite GetLastIcon()
    {
        return questions[questions.Length - 1].icon;
    }
}
