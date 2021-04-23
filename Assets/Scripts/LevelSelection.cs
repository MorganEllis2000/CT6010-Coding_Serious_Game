using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    public Button[] lvlButtons;
    private ColorBlock buttonColours;

    void Update()
    {
        int levelAt = PlayerPrefs.GetInt("levelAt", 1);
        Debug.Log(levelAt);

        for (int i = 0; i < lvlButtons.Length; i++) {
            buttonColours = lvlButtons[i].GetComponent<Button>().colors;
            if (i + 1 > levelAt) {
                lvlButtons[i].interactable = false;
                buttonColours.disabledColor = Color.red;
                lvlButtons[i].GetComponent<Button>().colors = buttonColours;
            } else if (i + 1 < levelAt) {
                buttonColours.normalColor = Color.green;
                lvlButtons[i].GetComponent<Button>().colors = buttonColours;
            } else {
                buttonColours.normalColor = Color.yellow;
                lvlButtons[i].GetComponent<Button>().colors = buttonColours;
            }
		}
    }
}
