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

    [TextArea]
    public string[] answers;

    public bool hasSlider;
}
