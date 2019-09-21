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
    private AnimationCurve alphaTextCurve;
    [SerializeField]
    private AnimationCurve alphaTextAnswerBlendCurve;


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
    private Coroutine fadeRoutine;

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
                    name = "Versicherungs-\nunternehmen";
                    break;
                case Category.Categorie2:
                    name = "Pensionskassen\n";
                    break;
                case Category.Categorie3:
                    name = "Betriebliche\nVorsorgekassen  ";
                    break;
                case Category.Categorie4:
                    name = "Kreditinstitute\n ";
                    break;
                case Category.Categorie5:
                    name = "Wertpapier-\ndienstleister";
                    break;

                case Category.Categorie6:
                    name = "Fondsindustrie\n ";
                    break;

            }
            button.GetComponentInChildren<Text>().text = name;
            button.name = name;
            button.GetComponent<ButtonHighlighter>().Init();
            categoryButtons.Add(button);
        }
        currentCategory = -1;
        SwitchCategory(currentCategory);
        okButton.GetComponent<ButtonAnimator>().onAnimationFinished.AddListener(delegate { OnOkButtonClicked(); });
        answerSlider.GetComponent<Slider>().onValueChanged.AddListener(OnAnswerSliderValueChanged);
        answerSlider.GetComponent<FadeObject>().Init();
        ActivateOKButton(false);
    }

    #region Screen Saver
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
        screenSaver.GetInput(currentCategory, currentTopic);
    }

    #endregion

    #region Input
    private void OnAnswerSliderValueChanged(float value)
    {
        answerSliderValue = value;
        ActivateOKButton(true);
        CheckForScreensaverInput();
    }

    private void OnOkButtonClicked(bool categorySwitched = false)
    {
        CheckForScreensaverInput();
        var currentQuestion = topicButtons[currentTopic].GetComponent<ButtonDisplayer>().GetCurrentQuestion();
        if (currentQuestion.hasSlider)
        {
            answerManager.SetupAnswerSingle(currentQuestion, answerSliderValue, currentQuestion.answers[0].percents[currentCategory], categorySwitched);
        }
        else
        {
            answerManager.SetupAnswerMulti(currentQuestion, currentAnswerIndex, currentCategory, categorySwitched);
        }
        if (!categorySwitched && currentAnswerIndex >= 0)
        {
            answerButtons[currentAnswerIndex].GetComponent<ButtonHighlighter>().SelectButton(false);
        }
        currentAnswerIndex = -1;
        answerSliderValue = 0;
        answerSlider.GetComponent<Slider>().value = 0f;
    }

    private void OnAnswerClicked(int Index)
    {
        CheckForScreensaverInput();
        ActivateOKButton(true);
        currentAnswerIndex = Index;

        for (int i = 0; i < answerButtons.Count; i++)
        {
            answerButtons[i].GetComponent<ButtonHighlighter>().SelectButton(i == currentAnswerIndex);
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
        topicButtons[currentTopic].GetComponent<ButtonDisplayer>().OnTopicSwitched();
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
            //ActivateOKButton(false);

            //SwitchCategory(currentCategory);

            //Debug.LogFormat("Category switchted to {0}", selectedCategorie.ToString());
        }
        CheckForScreensaverInput();

        if (answerManager.isActive)
        {
            answerSliderValue = 0;
            OnOkButtonClicked(true);
        }
        else
        {
            answerManager.DeactivatePanel();
        }
    }

    #endregion

    #region Questions
    private IEnumerator SetupAnswers(float fadeDuration, QuestionContainer question, AnimationCurve usedCurve)
    {
        float time = 0f;
        bool halfReached = false;
        bool onSecondHalfenter = true;
        float value = 0f;
        FadeObject answerSliderFade = answerSlider.GetComponent<FadeObject>();
        List<FadeObject> answerFade = new List<FadeObject>();
        foreach (var button in answerButtons)
        {
            answerFade.Add(button.GetComponent<FadeObject>());
        }

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            value = usedCurve.Evaluate(time / fadeDuration);
            //Switch on Halftime
            if (!halfReached && time > fadeDuration / 2f)
            {
                halfReached = true;
            }

            //First Half
            if (!halfReached)
            {
                //Slider
                if (answerSlider.activeSelf)
                {
                    answerSliderFade.SetOpacity(value);
                }
                else
                {
                    foreach (var answer in answerFade)
                    {
                        answer.SetOpacity(value);
                    }
                }
            }
            //Second Half
            else
            {
                //End First Half Deactivation
                if (onSecondHalfenter)
                {
                    //Remove old Buttons
                    foreach (var item in answerButtons)
                    {
                        Destroy(item);
                    }
                    answerButtons.Clear();
                    answerFade.Clear();

                    //Check for Sliders
                    if (answerSlider.activeSelf && !question.hasSlider)
                    {
                        answerSliderFade.SetOpacity(0);
                        answerSlider.SetActive(false);
                    }

                    //Add New Buttons
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
                        button.GetComponent<FadeObject>().Init();
                        button.GetComponent<FadeObject>().SetOpacity(0f);
                        answerFade.Add(button.GetComponent<FadeObject>());
                        answerButtons.Add(button);
                    }
                    onSecondHalfenter = false;
                }

                //Slider
                if (question.hasSlider)
                {
                    if (!answerSlider.activeSelf)
                    {
                        answerSlider.SetActive(true);
                    }
                    answerSliderFade.SetOpacity(value);
                }

                //Multi Choice
                else
                {
                    foreach (var answer in answerFade)
                    {
                        answer.SetOpacity(value);
                    }
                }
            }
            yield return null;
        }
    }

    private IEnumerator FadeSlider(float duration)
    {
        float time = 0f;

        while (time < duration)
        {
            yield return null;
        }
    }

    private void OnQuestionChanged(QuestionContainer question)
    {
        CheckForScreensaverInput();
        AnimationCurve usedCurve = answerManager.isActive ? alphaTextAnswerBlendCurve : alphaTextCurve;
        answerManager.DeactivatePanel();
        ActivateOKButton(false);

        if (fadeRoutine == null)
        {
            fadeRoutine = StartCoroutine(FadeQuestion(1f, question, usedCurve));
            StartCoroutine(SetupAnswers(1, question, usedCurve));
        }
    }

    private IEnumerator FadeQuestion(float duration,  QuestionContainer question, AnimationCurve alphaCurve)
    {
        float time = 0f;
        bool switched = false;
        Color titleColor = titleText.color;
        Color questionColor = questionText.color;

        while (time < duration)
        {
            time += Time.deltaTime;
            float value = alphaCurve.Evaluate(time / duration);
            questionColor.a = titleColor.a = value;
            titleText.color = titleColor;
            questionText.color = questionColor;

            if (!switched && time > duration / 2f)
            {
                switched = true;
                titleText.text = question.title;
                questionText.text = question.question;
            }

            yield return null;
        }

        questionColor.a = titleColor.a = alphaCurve.Evaluate(1f);
        titleText.color = titleColor;
        questionText.color = questionColor;
        fadeRoutine = null;
    }

    public void SwitchNextQuestion()
    {
        CheckForScreensaverInput();
        topicButtons[currentTopic].GetComponent<ButtonDisplayer>().NextQuestion();
        var question = topicButtons[currentTopic].GetComponent<ButtonDisplayer>().GetCurrentQuestion();
        OnQuestionChanged(question);
    }

    public void SwitchPreviousQuestion()
    {
        CheckForScreensaverInput();
        topicButtons[currentTopic].GetComponent<ButtonDisplayer>().PreviousQuestion();
        var question = topicButtons[currentTopic].GetComponent<ButtonDisplayer>().GetCurrentQuestion();
        OnQuestionChanged(question);
    }

    #endregion
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
    #region Icons
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
    #endregion

    #region Question Texts
    public string GetNextQuestionText()
    {
        return topicButtons[currentTopic].GetComponent<ButtonDisplayer>().GetNextQuestionText();
    }

    public string GetPreviousQuestionText()
    {
        return topicButtons[currentTopic].GetComponent<ButtonDisplayer>().GetPreviousQuestionText();
    }

    public string GetCurrentQuestionText()
    {
        return topicButtons[currentTopic].GetComponent<ButtonDisplayer>().GetCurrentQuestionText();
    }
    public string GetSecondQuestionText()
    {
        return topicButtons[currentTopic].GetComponent<ButtonDisplayer>().GetSecondQuestionText();
    }
    public string GetLastQuestionText()
    {
        return topicButtons[currentTopic].GetComponent<ButtonDisplayer>().GetLastQuestionText();
    }
    #endregion
}
