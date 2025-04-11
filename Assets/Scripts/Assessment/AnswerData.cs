using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnswerData : MonoBehaviour
{

    [Header("UI Elements")]
    [SerializeField] TextMeshProUGUI answer;
    [SerializeField] Image toggle;
    [SerializeField] Image cardBackground;

    [Header("Sprites")]
    [SerializeField] Sprite checkedToggle;
    [SerializeField] Sprite uncheckedToggle;
    [SerializeField] Sprite checkedBG;
    [SerializeField] Sprite uncheckedBG;

    [Header("References")]
    [SerializeField] AssessmentEvent evnt;

    private RectTransform _rectTransform;
    public RectTransform Rect
    {
        get
        {
            if (_rectTransform == null)
            {
                _rectTransform = GetComponent<RectTransform>() ?? gameObject.AddComponent<RectTransform>();
            }
            return _rectTransform;
        }
    }

    private int _answerIndex;
    public int AnswerIndex { get { return _answerIndex; } }
    private bool Checked = false;

    public void UpdateData(string text, int index)
    {
        answer.text = text;
        _answerIndex = index;
    }

    public void Reset()
    {
        Checked = false;
        UpdateUI();
    }

    public void SwitchState()
    {
        Checked = !Checked;
        UpdateUI();

        if (evnt.UpdateQuestionAnswer != null)
        {
            evnt.UpdateQuestionAnswer(this);
        }
    }

    public void UpdateUI()
    {
        if (Checked) {
        toggle.sprite = checkedToggle;
        cardBackground.sprite = checkedBG;
        } else {
            toggle.sprite = uncheckedToggle;
            cardBackground.sprite = uncheckedBG;
        }
    }
}
