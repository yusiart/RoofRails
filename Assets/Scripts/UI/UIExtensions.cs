using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIExtensions : MonoBehaviour
{
    public void RestartGame()
    {
        Application.LoadLevel(Application.loadedLevel);
        Time.timeScale = 1;
    }
}
