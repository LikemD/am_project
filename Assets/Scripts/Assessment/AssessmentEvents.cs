using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AssessmentEvent", menuName = "Assessment/Event")]
public class AssessmentEvent : ScriptableObject
{
    public delegate void UpdateQuestionUICallback(QuestionSO question);
    public UpdateQuestionUICallback UpdateQuestionUI;

    public delegate void UpdateQuestionAnswerCallback(AnswerData pickedAnswer);
    public UpdateQuestionAnswerCallback UpdateQuestionAnswer;

    public delegate void DisplayAnswerResolutionScreenCallback(AssessmentUtils.AnswerResolutionType answerResolutionType, int score, int questionsLength);
    public DisplayAnswerResolutionScreenCallback DisplayAnswerResolutionScreen;

    public delegate void ResetGameCallback();
    public ResetGameCallback ResetGame;

    public delegate void ScoreUpdatedCallback();
    public ScoreUpdatedCallback ScoreUpdated;

    public int FinalScore;
}
