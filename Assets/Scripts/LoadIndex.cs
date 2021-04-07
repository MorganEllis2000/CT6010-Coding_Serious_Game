using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadIndex : MonoBehaviour
{
    /// <summary>
    /// loads the scene based on the index assigned in the inspector
    /// </summary>
    public void LoadByIndex(int sceneIndex) {
        SceneManager.LoadScene(sceneIndex);
    }
}
