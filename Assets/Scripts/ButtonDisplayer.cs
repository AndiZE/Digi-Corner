using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonDisplayer : MonoBehaviour
{
    [SerializeField]
    private QuestionContainer [] questions;
    private int currentIndex = 0;

    public QuestionContainer GetCurrentQuestion()
    {
        print(currentIndex);
        return questions[currentIndex];
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
}
