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
    public Category currentCategory { get; private set; }

    [Header("Answers")]
    [SerializeField]
    private GameObject answerButtonPrefab;
    [SerializeField]
    private Transform answerButtonRow;
    [SerializeField]
    private Text titleText;
    [SerializeField]
    private Text questionText;

    private List<GameObject> answerButtons = new List<GameObject>();


    void Start()
    {
        for (int i = 0; i < topicButtons.Length; i++)
        {
            int j = i;
            topicButtons[j].GetComponent<Button>().onClick.AddListener(delegate { SwitchTopic(j); });
            topicButtons[j].GetComponent<ButtonHighlighter>().Init();
        }
        currentTopic = 0;
        SwitchTopic(currentTopic);

        for (int i = 0; i < Enum.GetNames(typeof(Category)).Length; i++)
        {
            int j = i;
            Category category = (Category)j;
            var button = Instantiate(categoryButtonPrefab, categoryRoot);
            button.GetComponent<Button>().onClick.AddListener(delegate { SwitchCategory(category); });
            button.GetComponentInChildren<Text>().text = category.ToString();
            button.name = category.ToString();
            button.GetComponent<ButtonHighlighter>().Init();
            categoryButtons.Add(button);
        }
        currentCategory = 0;
        SwitchCategory(currentCategory);

    }

    public void SwitchTopic(int topicIndex)
    {
        for (int i = 0; i < topicButtons.Length; i++)
        {
            topicButtons[i].GetComponent<ButtonHighlighter>().SelectButton(i == topicIndex);
        }
        currentTopic = topicIndex;
        var question = topicButtons[currentTopic].GetComponent<ButtonDisplayer>().GetCurrentQuestion();
        OnQuestionChanged(question);

        //Debug.LogFormat("Switched to Topic {0}", topicIndex + 1);
    }

    public void SwitchCategory(Category selectedCategorie)
    {
        for (int i = 0; i < categoryButtons.Count; i++)
        {
            categoryButtons[i].GetComponent<ButtonHighlighter>().SelectButton(i == (int)selectedCategorie);
        }

        if (currentCategory != selectedCategorie)
        {
            currentCategory = selectedCategorie;
            //Debug.LogFormat("Category switchted to {0}", selectedCategorie.ToString());
        }
    }

    private void SetupAnswers(QuestionContainer question)
    {
        //Clear Old Buttons and Remove from Panel
        foreach (var item in answerButtons)
        {
            Destroy(item);
        }
        answerButtons.Clear();

        int count = question.answers.Length;
        for (int i = 0; i < count; i++)
        {
            var button = Instantiate(answerButtonPrefab, answerButtonRow);
            int index = i;
            button.GetComponent<Button>().onClick.AddListener(delegate { OnAnswerClicked(index); });
            button.GetComponentInChildren<Text>().text = question.answers[i];
            answerButtons.Add(button);
        }
    }

    private void OnQuestionChanged(QuestionContainer question)
    {
        SetupAnswers(question);
        titleText.text = question.title;
        questionText.text = question.question;
    }

    private void OnAnswerClicked(int Index)
    {
        //Debug.LogFormat("Answer {0} clicked", Index + 1);
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



}
