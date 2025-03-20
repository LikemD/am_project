using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHandlers : MonoBehaviour
{
    [SerializeField] GameObject StartScreen;
    [SerializeField] GameObject LOScreen;
    [SerializeField] LearningObjectivesUI LOUI;
    public void sayHello(string message) {
        Debug.Log($"Hello from button {message}");
    }

    public void handleExplore3DPrinterButtonClick()
    {
        StartCoroutine(open3DPrinterScence());
    }

    IEnumerator open3DPrinterScence()
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(1);
    }

    public void handleExit3DPrinterSceneClick()
    {
        StartCoroutine(exit3DPrinterScence());
    }

    IEnumerator exit3DPrinterScence()
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(0);
    }

    public void loadLOScreen()
    {
        StartScreen.SetActive(false);
        LOScreen.SetActive(true);
    }

    public void exitLOScreen()
    {
        // reset learning objectives screen data
        LOUI.resetLOScreen();

        LOScreen.SetActive(false);
        StartScreen.SetActive(true);
    }
}
