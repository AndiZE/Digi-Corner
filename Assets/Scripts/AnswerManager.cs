using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerManager : MonoBehaviour
{
    [SerializeField]
    private Transform answerBarRoot;

    private BarSlider [] sliders;
    private void Start()
    {
        sliders = GetComponents<BarSlider>();
    }

    private void SetupAnswer(QuestionContainer question, int userAnswerIndex)
    {
        for (int i = 0; i < sliders.Length; i++)
        {
            //Disable Leftover Sliders
            if (i > question.answers.Length-1)
            {
                sliders[i].gameObject.SetActive(false);
                continue;
            }
            //If needed Slider is Disabled, reenable
            if (!sliders[i].gameObject.activeSelf)
            {
                sliders[i].gameObject.SetActive(true);
            }
            //sliders[i].ActivateBar(,,userAnswerIndex, question.answers[i]);
        }
    }
}
