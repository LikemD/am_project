using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputRelaySink : MonoBehaviour
{
    [SerializeField] RectTransform CanvasTransform;

    GraphicRaycaster Raycaster;
    // Start is called before the first frame update
    void Start()
    {
        Raycaster = GetComponent<GraphicRaycaster>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnCursorInput(Vector2 normalizedPosition)
    {
        // calculate the position of the cursor relative to the canvas space
        Vector3 mousePosition = new Vector3(CanvasTransform.sizeDelta.x * normalizedPosition.x, CanvasTransform.sizeDelta.y * normalizedPosition.y, 0.0f);

        // construct pointer event and assign the relative position to it
        PointerEventData mouseEvent = new PointerEventData(EventSystem.current);
        mouseEvent.position = mousePosition;

        // perform raycast using graphics raycaster
        List<RaycastResult> results = new List<RaycastResult>();
        Raycaster.Raycast(mouseEvent, results);

        bool sendMouseDown = Input.GetMouseButtonDown(0);
        bool sendMouseUp = Input.GetMouseButtonUp(0);

        // process the raycast results
        foreach (var result in results)
        {
            if (sendMouseDown) ExecuteEvents.Execute(result.gameObject, mouseEvent, ExecuteEvents.pointerDownHandler);
            else if (sendMouseUp)
            {
                ExecuteEvents.Execute(result.gameObject, mouseEvent, ExecuteEvents.pointerUpHandler);
                ExecuteEvents.Execute(result.gameObject, mouseEvent, ExecuteEvents.pointerClickHandler);
            }
        }
    }
}
