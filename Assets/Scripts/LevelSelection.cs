using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    public Button[] lvlButtons;
    private Text buttonText;
    private ColorBlock buttonColours;

    void Update()
    {
        int levelAt = PlayerPrefs.GetInt("levelAt", 1);
        
        for (int i = 0; i < lvlButtons.Length; i++) {
            buttonColours = lvlButtons[i].GetComponent<Button>().colors;
            buttonText = lvlButtons[i].GetComponentInChildren<Text>();
            if (i + 1 > levelAt) {
                lvlButtons[i].interactable = false;
                buttonColours.disabledColor = Color.red;
                buttonText.color = Color.red;
                lvlButtons[i].GetComponent<Button>().colors = buttonColours;
            } else if (i + 1 < levelAt) {
                buttonColours.normalColor = Color.green;
                buttonText.color = Color.green;
                lvlButtons[i].GetComponent<Button>().colors = buttonColours;
            } else {
                buttonColours.normalColor = Color.yellow;
                buttonText.color = Color.yellow;
                lvlButtons[i].GetComponent<Button>().colors = buttonColours;
            }
		}
    }
}
