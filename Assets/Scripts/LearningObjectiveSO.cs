using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;

[CreateAssetMenu(fileName = "LearningObjectiveSO", menuName = "LearningObjectiveSO")]
public class LearningObjectiveSO : ScriptableObject
{
    public string title;
    public string description;

    public Sprite icon;
}
