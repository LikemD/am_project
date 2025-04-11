using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class NavClickedEvent: UnityEvent<LearningObjectiveSO>{}
public class LearningObjectiveNavigation : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] TextMeshProUGUI Title;
    [SerializeField] TextMeshProUGUI Description;
    [SerializeField] SVGImage IconBase;

    LearningObjectiveSO LearningObjective;

    NavClickedEvent onNavClicked = new NavClickedEvent();

    public void Bind(LearningObjectiveSO learningObjective) {
        LearningObjective = learningObjective;
        Title.text = learningObjective.title;
        Description.text = learningObjective.description;
        IconBase.sprite = learningObjective.icon;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // public void OnSelected(string direction) {
    //     // if (direction == NavDirection.Left) {}
    //     // else {}

    //     onNavClicked.Invoke(LearningObjective);
    // }
}
