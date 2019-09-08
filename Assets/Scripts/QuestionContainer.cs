using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Container", menuName = "New Question")]
public class QuestionContainer : ScriptableObject
{
    [TextArea]
    public string title;
    [TextArea]
    public string question;
    public Sprite icon;
    public AnswerContainer[] answers;
    public bool hasSlider;
}

[Serializable]
public struct AnswerContainer
{
    [TextArea]
    public string answer;
    public float [] percents;
    public bool [] isRightAnswer;
}
