using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelSwitch : MonoBehaviour
{
    [Header("Topics")]
    [SerializeField]
    private Button[] topicButtons;
    private int currentTopic;

    [Header("Categories")]
    [SerializeField]
    private GameObject categoryButtonPrefab;
    [SerializeField]
    private Transform categoryRoot;
    private List<GameObject> categoryButtons = new List<GameObject>();
    public int currentCategory { get; private set; }

    [Header("Answers")]
    [SerializeField]
    private GameObject answerButtonPrefab;
    [SerializeField]
    private Transform answerButtonRowUp;
    [SerializeField]
    private Transform answerButtonRowMiddle;
    [SerializeField]
    private Transform answerButtonRowDown;
    [SerializeField]
    private Text titleText;
    [SerializeField]
    private Text questionText;

    [SerializeField]
    private Button okButton;
    [SerializeField]
    private GameObject answerSlider;

    private List<GameObject> answerButtons = new List<GameObject>();
    private ScreenSaver screenSaver;

    private bool isActive = false;
    private TouchInput touchInput;

    private AnswerManager answerManager;
    private int currentAnswerIndex;
    private float answerSliderValue;

    void Start()
    {
        screenSaver = GetComponent<ScreenSaver>();
        touchInput = GetComponent<TouchInput>();
        answerManager = GetComponent<AnswerManager>();

        for (int i = 0; i < topicButtons.Length; i++)
        {
            int j = i;
            topicButtons[j].GetComponent<Button>().onClick.AddListener(delegate { SwitchTopic(j); });
            topicButtons[j].GetComponent<ButtonHighlighter>().Init();
        }
        currentTopic = -1;
        SwitchTopic(currentTopic);

        for (int i = 0; i < Enum.GetNames(typeof(Category)).Length; i++)
        {
            int j = i;
            Category category = (Category)j;
            var button = Instantiate(categoryButtonPrefab, categoryRoot);
            button.GetComponent<Button>().onClick.AddListener(delegate { SwitchCategory((int)category); });
            string name = "";
            switch (category)
            {
                case Category.Categorie1:
                    name = "Versicherungs- \n unternehmen";
                    break;
                case Category.Categorie2:
                    name = "Pensionskassen";
                    break;
                case Category.Categorie3:
                    name = "Betriebliche Vorsorge \n ";
                    break;
                case Category.Categorie4:
                    name = "Kreditinstitute \n ";
                    break;
                case Category.Categorie5:
                    name = "Wertpapier- \n dienstleister";
                    break;

                case Category.Categorie6:
                    name = "Fondsindustrie \n ";
                    break;

            }
            button.GetComponentInChildren<Text>().text = name;
            button.name = name;
            button.GetComponent<ButtonHighlighter>().Init();
            categoryButtons.Add(button);
        }
        currentCategory = -1;
        SwitchCategory(currentCategory);
        okButton.onClick.AddListener(delegate { OnOkButtonClicked(); });
        answerSlider.GetComponent<Slider>().onValueChanged.AddListener(OnAnswerSliderValueChanged);
        ActivateOKButton(false);
    }

    private void OnAnswerSliderValueChanged(float value)
    {
        answerSliderValue = value;
        ActivateOKButton(true);
    }

    private void CheckForScreensaverInput()
    {
        screenSaver.GetInput(currentCategory, currentTopic);
        if (!isActive && currentCategory >= 0 && currentTopic >= 0)
        {
            isActive = true;
            touchInput.Init();
        }
    }

    public void ActivateScreensaver()
    {
        currentTopic = -1;
        SwitchTopic(currentTopic);
        currentCategory = -1;
        SwitchCategory(currentCategory);
        isActive = false;
        answerManager.DeactivatePanel();
    }

    private void OnOkButtonClicked()
    {
        var currentQuestion = topicButtons[currentTopic].GetComponent<ButtonDisplayer>().GetCurrentQuestion();
        if (currentQuestion.hasSlider)
        {
            answerManager.SetupAnswerSingle(answerSliderValue, currentQuestion.answers[0].percents[currentCategory]);
        }
        else
        {
            answerManager.SetupAnswerMulti(currentQuestion, currentAnswerIndex, currentCategory);
        }
    }

    public void SwitchTopic(int topicIndex)
    {
        for (int i = 0; i < topicButtons.Length; i++)
        {
            topicButtons[i].GetComponent<ButtonHighlighter>().SelectButton(i == topicIndex);
        }
        if (topicIndex < 0)
        {
            return;
        }
        currentTopic = topicIndex;
        var question = topicButtons[currentTopic].GetComponent<ButtonDisplayer>().GetCurrentQuestion();
        OnQuestionChanged(question);
        touchInput.Init();
        //Debug.LogFormat("Switched to Topic {0}", topicIndex + 1);
        CheckForScreensaverInput();
        answerManager.DeactivatePanel();
    }

    public void SwitchCategory(int selectedCategorie)
    {
        for (int i = 0; i < categoryButtons.Count; i++)
        {
            categoryButtons[i].GetComponent<ButtonHighlighter>().SelectButton(i == selectedCategorie);
        }

        if (selectedCategorie < 0)
        {
            return;
        }

        if (currentCategory != selectedCategorie)
        {
            currentCategory = selectedCategorie;
            ActivateOKButton(false);

            //SwitchCategory(currentCategory);

            //Debug.LogFormat("Category switchted to {0}", selectedCategorie.ToString());
        }
        CheckForScreensaverInput();
        answerManager.DeactivatePanel();
    }

    private void SetupAnswers(QuestionContainer question)
    {
        //Clear Old Buttons and Remove from Panel
        foreach (var item in answerButtons)
        {
            Destroy(item);
        }
        answerButtons.Clear();

        if (question.hasSlider)
        {
            answerSlider.SetActive(true);
            return;
        }
        else
        {
            answerSlider.SetActive(false);
        }


        Transform selectedRow;

        int count = question.answers.Length;
        for (int i = 0; i < count; i++)
        {
            selectedRow = GetRow(question.answers.Length, i);

            var button = Instantiate(answerButtonPrefab, selectedRow);
            int index = i;
            button.GetComponent<Button>().onClick.AddListener(delegate { OnAnswerClicked(index); });
            button.GetComponentInChildren<Text>().text = question.answers[i].answer;
            button.GetComponent<ButtonHighlighter>().Init();
            button.GetComponent<ButtonHighlighter>().SelectButton(false);
            answerButtons.Add(button);
        }
    }

    private void OnQuestionChanged(QuestionContainer question)
    {
        answerManager.DeactivatePanel();
        SetupAnswers(question);
        titleText.text = question.title;
        questionText.text = question.question;
        ActivateOKButton(false);
    }

    private void OnAnswerClicked(int Index)
    {
        ActivateOKButton(true);
        currentAnswerIndex = Index;

        for (int i = 0; i < answerButtons.Count; i++)
        {
            answerButtons[i].GetComponent<ButtonHighlighter>().SelectButton(i == currentAnswerIndex);
        }

    }

    public void SwitchNextQuestion()
    {
        topicButtons[currentTopic].GetComponent<ButtonDisplayer>().NextQuestion();
        var question = topicButtons[currentTopic].GetComponent<ButtonDisplayer>().GetCurrentQuestion();
        OnQuestionChanged(question);
    }

    public void SwitchPreviousQuestion()
    {
        topicButtons[currentTopic].GetComponent<ButtonDisplayer>().PreviousQuestion();
        var question = topicButtons[currentTopic].GetComponent<ButtonDisplayer>().GetCurrentQuestion();
        OnQuestionChanged(question);
    }
    private Transform GetRow(int maxQuestions, int currentIteration)
    {
        Transform selected = null;

        switch (maxQuestions)
        {
            //all in one Row
            case 3:
            case 4:
                {
                    selected = answerButtonRowMiddle;
                    break;
                }
            case 6:
            case 7:
                {
                    //First 3 up, rest (up to total of 7) down
                    if (currentIteration < 3)
                    {
                        selected = answerButtonRowUp;
                    }
                    else
                    {
                        selected = answerButtonRowDown;
                    }
                    break;
                }
        }


        return selected;
    }

    private void ActivateOKButton(bool activate)
    {
        okButton.interactable = activate;
    }

    public Sprite GetNextQuestionIcon()
    {
        return topicButtons[currentTopic].GetComponent<ButtonDisplayer>().GetNextIcon();
    }

    public Sprite GetPreviousQuestionIcon()
    {
        return topicButtons[currentTopic].GetComponent<ButtonDisplayer>().GetPreviousIcon();
    }

    public Sprite GetCurrentQuestionIcon()
    {
        return topicButtons[currentTopic].GetComponent<ButtonDisplayer>().GetCurrentIcon();
    }
    public Sprite GetSecondQuestionIcon()
    {
        return topicButtons[currentTopic].GetComponent<ButtonDisplayer>().GetSecondIcon();
    }
    public Sprite GetLastQuestionIcon()
    {
        return topicButtons[currentTopic].GetComponent<ButtonDisplayer>().GetLastIcon();
    }

}
