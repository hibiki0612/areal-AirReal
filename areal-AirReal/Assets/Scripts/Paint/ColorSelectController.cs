using System.Collections;
using System.Collections.Generic;
using Es.InkPainter.Sample;
using UnityEngine;

public class ColorSelectController : MonoBehaviour
{
    [SerializeField] private Color _color;
    [SerializeField] private MousePainter mousePainter;
    public void ColorSelectButton()
    {
        mousePainter.erase = false;
        mousePainter.color = _color;
    }

    public void PaintButton()
    {
        mousePainter.erase = false;
    }

    public void EraseButton()
    {
        mousePainter.erase = true;
    }
}
