using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NavigationHandler : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private int currentPageIndex;
    [SerializeField] private int destinationPageIndex;

    [SerializeField] private ButtonHandlers buttonHandlers;

    void OnEnable()
    {
        button.onClick.AddListener(() => buttonHandlers.switchPage(currentPageIndex, destinationPageIndex));
    }

    void OnDisable()
    {
       button.onClick.RemoveAllListeners(); 
    }
}
