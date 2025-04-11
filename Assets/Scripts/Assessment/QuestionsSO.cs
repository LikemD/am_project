using UnityEditor;
using UnityEngine;

[System.Serializable]
public struct Answer
{

    [SerializeField] private string _text;
    public string Text { get { return _text; } }

    [SerializeField] private bool _isCorrect;
    public bool IsCorrect { get { return _isCorrect; } set { _isCorrect = value; } }


    public Answer(string text, bool isCorrect)
    {
        _text = text;
        _isCorrect = isCorrect;
    }
}

[CreateAssetMenu(fileName = "QuestionSO", menuName = "Assessment/Question")]
public class QuestionSO : ScriptableObject
{
    [SerializeField] private string _question = string.Empty;
    public string Question { get { return _question; } }

    [SerializeField] private Answer[] _answers = null;
    public Answer[] Answers { get { return _answers; } }

    public int GetCorrectAnswer()
    {
        int correctAnswerIndex = -1;

        for (int i = 0; i < Answers.Length; i++)
        {
            if (Answers[i].IsCorrect) correctAnswerIndex = i;
        }

        return correctAnswerIndex;
    }
}