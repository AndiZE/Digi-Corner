using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerManager : MonoBehaviour
{
    [Header("Multi Answer")]
    [SerializeField]
    private Transform answerBarRoot;
    [SerializeField]
    private GameObject multiPanel;

    private BarSlider [] sliders;

    [Header("Single Answer")]
    [SerializeField]
    private Slider userSlider;
    [SerializeField]
    private Slider answerSlider;
    [SerializeField]
    private float tollerance = 3f;
    [SerializeField]
    private GameObject richtigImage;
    [SerializeField]
    private GameObject singlePanel;
    public bool isActive { get; private set; }
    [SerializeField]
    private GameObject schätzungImage;

    private void Start()
    {
        sliders = answerBarRoot.GetComponentsInChildren<BarSlider>();
        singlePanel.SetActive(false);
        multiPanel.SetActive(false);
    }

    public void SetupAnswerMulti(QuestionContainer question, int userAnswerIndex, int categoryIndex, bool categorySwitched)
    {
        isActive = true;
        bool isUserAnswer = false;
        multiPanel.SetActive(true);
        singlePanel.SetActive(false);
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

            isUserAnswer = i == userAnswerIndex ? true : false;


            sliders[i].ActivateBar(
                question.answers[i].percents[categoryIndex], 
                question.answers[i].isRightAnswer[categoryIndex],
                isUserAnswer,
                question.answers[i].answer,
                categorySwitched);
        }
    }

    public void SetupAnswerSingle(float userValue, float questionValue, bool categorySwitched)
    {
        isActive = true;
        multiPanel.SetActive(false);
        singlePanel.SetActive(true);
        schätzungImage.SetActive(!categorySwitched);
        ActivateBar(userValue, questionValue,categorySwitched);
    }

    public void DeactivatePanel()
    {
        multiPanel.SetActive(false);
        singlePanel.SetActive(false);
        isActive = false;
    }

    private void ActivateBar(float userInput, float questionInput, bool categorySwitched)
    {
        userSlider.GetComponent<BarSlider>().SimpleActivateBar(userInput);
        answerSlider.GetComponent<BarSlider>().SimpleActivateBar(questionInput);

        richtigImage.SetActive(Mathf.Abs(userInput - questionInput) <= tollerance ? true : false);
        if (categorySwitched)
        {
            richtigImage.SetActive(false);
        }
    }


}
