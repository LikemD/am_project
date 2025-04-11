using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHandlers : MonoBehaviour
{
    [SerializeField] GameObject StartScreen;
    [SerializeField] GameObject LOScreen;
    [SerializeField] GameObject[] pages;


    public void handleExplore3DPrinterButtonClick()
    {
        StartCoroutine(open3DPrinterScence());
    }

    static IEnumerator open3DPrinterScence()
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(1);
    }

    public void handleExit3DPrinterSceneClick()
    {
        StartCoroutine(exit3DPrinterScence());
    }

    static IEnumerator exit3DPrinterScence()
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(0);
    }

    public void switchPage(int from, int to)
    {
        if (from == 0)
        {
            StartScreen.SetActive(false);
            pages[0].SetActive(false);
            LOScreen.SetActive(true);
            pages[to].SetActive(true);
        }
        else if (to == 0)
        {
            LOScreen.SetActive(false);
            pages[from].SetActive(false);
            StartScreen.SetActive(true);
            pages[0].SetActive(true);
        }
        else
        {
            pages[from].SetActive(false);
            pages[to].SetActive(true);

        }
    }

}
