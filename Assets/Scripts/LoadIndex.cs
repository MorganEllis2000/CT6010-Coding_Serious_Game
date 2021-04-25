using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadIndex : MonoBehaviour
{
    /// <summary>
    /// loads the scene based on the index assigned in the inspector
    /// </summary>
    public void LoadByIndex(int sceneIndex) {
        SceneManager.LoadScene(sceneIndex);
    }


    public void LoadPreviousScene() {
        int iCurrentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(iCurrentSceneIndex - 1);
    }

    public void LoadNextScene() {
        int iCurrentSceneIndex = SceneManager.GetActiveScene().buildIndex;       
        SceneManager.LoadScene(iCurrentSceneIndex + 1);
    }

	private void Update() {
        GameObject nextButton = GameObject.Find("Next");
        int iCurrentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int levelAt = PlayerPrefs.GetInt("levelAt", 1);
        if (iCurrentSceneIndex < levelAt) {
            nextButton.SetActive(true);
        } 
    }
}
