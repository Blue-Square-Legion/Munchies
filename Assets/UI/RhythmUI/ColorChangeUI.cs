using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ColorTypes
{
    Red,
    Green, Blue,
    white
}


public class ColorChangeUI : MonoBehaviour
{
    [SerializeField] private Image m_image;

    public void ChangeColor(ColorTypes type)
    {

    }

    public void ChangeColor(string type)
    {
        switch (type)
        {
            case "Blue": ChangeColor(Color.blue); break;
            case "Green": ChangeColor(Color.green); break;
            case "Yellow": ChangeColor(Color.yellow); break;
            case "Gray": ChangeColor(Color.gray); break;
        }
    }


    public void ChangeColor(Color color)
    {

        m_image.color = color;
    }
}
