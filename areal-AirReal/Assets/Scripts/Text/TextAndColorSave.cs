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
    
    public Dictionary<string, Color32> _Color_dictionary = new Dictionary<string, Color32>();
     
    public string _sentence;
    
    public string word_str;
    public string color_str;
    private string _after_color;
    private List<string> _wordList;

    
    public void SaveButton()
    {
        _Color_dictionary = acquisitionColorController.word_List;
        _wordList = acquisitionColorController.word;

        foreach (var word in _Color_dictionary)
        {
            _after_color = word.Value.ToString();
            _after_color = _after_color.Replace("RGBA", "");
            Debug.Log(word.Key);
            //word_str = word_str + ',' + word.Key;
            color_str = color_str + ',' + _after_color;
        }

        foreach (var str in _wordList )
        {
            word_str = word_str + ',' + str;
        }
        
    }

}
