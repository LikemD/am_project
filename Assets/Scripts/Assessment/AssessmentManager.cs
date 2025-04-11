using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssessmentManager : MonoBehaviour
{
    QuestionSO[] _questions = null;
    public QuestionSO[] Questions { get { return _questions; } }
    [SerializeField] AssessmentEvent evnt = null;

    private AnswerData PickedAnswer;
    private List<int> FinishedQuestions = new List<int>();
    private int currentQuestionIndex = 0;
    private int score;
    private IEnumerator IE_DelayMoveToNextQuestion = null;

    private bool gameOver
    {
        get
        {
            return Questions.Length <= currentQuestionIndex + 1;
        }
    }

    void OnEnable()
    {
        evnt.UpdateQuestionAnswer += UpdateAnswer;
        evnt.ResetGame += ResetGame;
    }

    void OnDisable()
    {
        evnt.UpdateQuestionAnswer -= UpdateAnswer;
        evnt.ResetGame -= ResetGame;
    }

    void Start()
    {
        LoadQuestions();
        DisplayQuestion();
    }


    public void HandleAnswerSubmission()
    {
        if (PickedAnswer != null)
        {

            bool isCorrect = MarkAnswer();
            if (isCorrect) UpdateScore();

            FinishedQuestions.Add(currentQuestionIndex);

            var answerResolutionType = gameOver ?
                    AssessmentUtils.AnswerResolutionType.Finished : isCorrect
                    ? AssessmentUtils.AnswerResolutionType.Correct : AssessmentUtils.AnswerResolutionType.Incorrect;

            if (evnt.DisplayAnswerResolutionScreen != null)
            {
                evnt.DisplayAnswerResolutionScreen(answerResolutionType, score, Questions.Length);
            }

            if (answerResolutionType != AssessmentUtils.AnswerResolutionType.Finished)
            {
                currentQuestionIndex++; // set question index to the next one
                if (IE_DelayMoveToNextQuestion != null)
                {
                    StartCoroutine(IE_DelayMoveToNextQuestion);
                }
                IE_DelayMoveToNextQuestion = DelayMoveToNextQuestion();
                StartCoroutine(IE_DelayMoveToNextQuestion);
            }
        }
    }


    public void UpdateAnswer(AnswerData newAnswer)
    {
        PickedAnswer = newAnswer;
    }

    private void UpdateScore()
    {
        score++;
    }


    public void EraseAnswers()
    {
        PickedAnswer = null;
    }

    void DisplayQuestion()
    {
        EraseAnswers();
        if (evnt.UpdateQuestionUI != null)
        {
            evnt.UpdateQuestionUI(Questions[FinishedQuestions.Count]);
        }
        else { Debug.Log("Fix assessment manager display method."); }
    }

    void LoadQuestions()
    {
        Object[] objs = Resources.LoadAll("Questions", typeof(QuestionSO));
        _questions = new QuestionSO[objs.Length];

        for (var i = 0; i < objs.Length; i++)
        {
            _questions[i] = (QuestionSO)objs[i];
        }

    }

    public bool MarkAnswer()
    {
        return PickedAnswer.AnswerIndex == Questions[currentQuestionIndex].GetCorrectAnswer();
    }

    void ResetGame()
    {
        UpdateAnswer(null);
        FinishedQuestions.Clear();
        currentQuestionIndex = 0;
        score = 0;
        LoadQuestions();
        DisplayQuestion();
    }

    private IEnumerator DelayMoveToNextQuestion()
    {
        yield return new WaitForSeconds(AssessmentUtils.AppDelayTime);
        DisplayQuestion();
    }
}
