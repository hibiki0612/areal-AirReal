using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using  UnityEngine.UI;

public class TextAndColorSave : MonoBehaviour
{
    
    [SerializeField] private AcquisitionColorController acquisitionColorController;
    [SerializeField] private Text text;
    
    private Dictionary<string, Color> dictionary = new Dictionary<string, Color>();

    private string _text;
    
    public string word_str;
    public string color_str;
    
    
    public void SaveButton()
    {
        dictionary = acquisitionColorController.word_List;

        foreach (var word in dictionary)
        {
            word_str = word_str + ',' + word.Key;
            color_str = color_str + ',' + word.Value;
        }
        
        Debug.Log(word_str);
        Debug.Log((color_str));
    }

    public void TextSaveButton()
    {
        _text = text.text;
        Debug.Log(_text);
    }
}
