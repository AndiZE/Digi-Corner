using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class FadeObject : MonoBehaviour
{
    private List<Image> images = new List<Image>();
    private List<Outline> outlines = new List<Outline>();
    private List<Text> texts = new List<Text>();

    public void Init()
    {
        var obj = GetComponentsInChildren<Image>();

        foreach (var item in obj)
        {
            if (item.GetComponent<Outline>() != null)
            {
                outlines.Add(item.GetComponent<Outline>());
                continue;
            }
            images.Add(item);
        }

        texts.AddRange(GetComponentsInChildren<Text>());
    }

    public void SetOpacity(float alpha)
    {
        Color color;
        //Images
        foreach (var image in images)
        {
            color = image.color;
            color.a = alpha;
            image.color = color;
        }

        //Outlines
        foreach (var outline in outlines)
        {
            color = outline.effectColor;
            color.a = alpha;
            outline.effectColor = color;
        }

        //UI Texts
        foreach (var text in texts)
        {
            color = text.color;
            color.a = alpha;
            text.color = color;
        }
    }

}
