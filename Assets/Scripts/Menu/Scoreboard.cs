using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Scoreboard : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [Space] 
    [SerializeField] private List<TMP_Text> resultTexts;
    
    private const string resKey = "Result";
    
    // Start is called before the first frame update
    void Start()
    {
        LoadResults();
    }

    public void BackToMainMenu()
    {
        mainMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    private void LoadResults()
    {
        for (int i = 0; i < resultTexts.Count; i++)
        {
            int resVal = PlayerPrefs.GetInt(resKey + i, 0);
            resultTexts[i].text = resVal.ToString();
        }
    }
}
