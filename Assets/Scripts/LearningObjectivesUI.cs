using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;


public enum NavDirection { Left, Right }

public class LearningObjectivesUI : MonoBehaviour
{
    [SerializeField] List<LearningObjectiveSO> LearningObjectives;
    [SerializeField] GameObject LearningObjectivePrefab;
    [SerializeField] Transform LearningObjectivesUIRoot;

    [SerializeField] TextMeshProUGUI ProgressIndicator;
    [SerializeField] GameObject LeftNav;
    [SerializeField] GameObject RightNav;
    [SerializeField] Sprite NavEnabledSprite;
    [SerializeField] Sprite NavDisabledSprite;


    int LOIndex;
    int LOsLength;
    GameObject curGO;

    Button LeftNavButton;
    Button RightNavButton;
    Image LeftNavImage;
    Image RightNavImage;

    // Start is called before the first frame update
    void Start()
    {
        LOIndex = 0;
        LOsLength = LearningObjectives.ToArray().Length;

        LeftNavButton = LeftNav.GetComponent<Button>();
        RightNavButton = RightNav.GetComponent<Button>();

        LeftNavImage = LeftNav.GetComponent<Image>();
        RightNavImage = RightNav.GetComponent<Image>();

        // spawn a learning objective card instance
        curGO = Instantiate(LearningObjectivePrefab, Vector3.zero, Quaternion.identity, LearningObjectivesUIRoot);

        // set learning objective card instance with data from the first item in the list
        var navScript = curGO.GetComponent<LearningObjectiveNavigation>();
        navScript.Bind(LearningObjectives[LOIndex]);

        // set the position of the learning objective card
        RectTransform rt = curGO.GetComponent<RectTransform>();
        rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, rt.rect.width);
        rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, rt.rect.height);

        setProgressIndicator();

        // disable left nav btn
        disableNavButton(LeftNavButton, LeftNavImage);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void setLOData()
    {
        // set learning objective data
        var navScript = curGO.GetComponent<LearningObjectiveNavigation>();
        navScript.Bind(LearningObjectives[LOIndex]);

    }

    void setProgressIndicator()
    {
        // set progress indicator text
        ProgressIndicator.text = $"{LOIndex + 1} / {LOsLength}";
    }

    void disableNavButton(Button button, Image image)
    {
        button.interactable = false;
        image.sprite = NavDisabledSprite;
    }
    void enableNavButton(Button button, Image image)
    {
        button.interactable = true;
        image.sprite = NavEnabledSprite;
    }

    public void resetLOScreen() {
        LOIndex = 0;

        // set learning objective card instance with data from the first item in the list
        var navScript = curGO.GetComponent<LearningObjectiveNavigation>();
        navScript.Bind(LearningObjectives[LOIndex]);

        disableNavButton(LeftNavButton, LeftNavImage);
        enableNavButton(RightNavButton, RightNavImage);

        setProgressIndicator();
    }

    public void handleNoteSelection(string direction)
    {
        if (direction == "left")
        {
            if (LOIndex > 0)
            {
                LOIndex -= 1;
                setLOData();
                setProgressIndicator();

                // disable left nav btn on first slide
                if (LOIndex == 0) disableNavButton(LeftNavButton, LeftNavImage);

                // enable left nav btn on second slide
                if (LOIndex < LOsLength - 1 && !RightNavButton.interactable) enableNavButton(RightNavButton, RightNavImage);
            }
        }
        else
        {
            if (LOIndex < LOsLength - 1)
            {
                LOIndex += 1;
                setLOData();
                setProgressIndicator();

                // disable right nav btn on last slide
                if (LOIndex == LOsLength - 1) disableNavButton(RightNavButton, RightNavImage);

                // enable left nav btn on second slide
                if (LOIndex > 0 && !LeftNavButton.interactable) enableNavButton(LeftNavButton, LeftNavImage);
            }
        }

    }
}
