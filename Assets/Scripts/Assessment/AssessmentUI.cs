using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using System.Collections;


[Serializable()]
public struct UIParameters
{
    [Header("Answer Options")]
    [SerializeField] private float margins; // value of the margin between each answer card
    public float Margins { get { return margins; } }


    [Header("Colors")]
    [SerializeField] private Color correctAnswerBG;
    public Color CorrectAnswerBG { get { return correctAnswerBG; } }
    [SerializeField] private Color incorrectAnswerBG;
    public Color IncorrectAnswerBG { get { return incorrectAnswerBG; } }
    [SerializeField] private Color resultsBG;
    public Color ResultsBG { get { return resultsBG; } }
}


[Serializable()]
public struct UiElements
{
    [SerializeField] RectTransform answersContentArea;
    public RectTransform AnswersContentArea { get { return answersContentArea; } }


    [SerializeField] TextMeshProUGUI questionText;
    public TextMeshProUGUI QuestionText { get { return questionText; } }


    [SerializeField] TextMeshProUGUI scoreText;
    public TextMeshProUGUI ScoreText { get { return scoreText; } }


    [SerializeField] Animator answerResolutionAnimator;
    public Animator AnswerResolutionAnimator { get { return answerResolutionAnimator; } }


    [SerializeField] Image answerResolutionBG;
    public Image AnswerResolutionBG { get { return answerResolutionBG; } }


    [SerializeField] TextMeshProUGUI answerResolutionText;
    public TextMeshProUGUI AnswerResolutionText { get { return answerResolutionText; } }

    [Space]
    [Header("Assessment Pages")]
    [SerializeField] GameObject startPage;
    public GameObject StartPage { get { return startPage; } }
    [SerializeField] GameObject qnaPage;
    public GameObject QnAPage { get { return qnaPage; } }



    [Space]
    [Header("Results UI Elements")]
    [SerializeField] GameObject resultsUI;
    public GameObject ResultsUI { get { return resultsUI; } }
    [SerializeField] TextMeshProUGUI scoreDescriptionText;
    public TextMeshProUGUI ScoreDescriptionText { get { return scoreDescriptionText; } }
    [SerializeField] Button retryButton;
    public Button RetryButton { get { return retryButton; } }
    [SerializeField] Button mainMenuButton;
    public Button MainMenuButton { get { return mainMenuButton; } }
}

public class AssessmentUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] AssessmentEvent evnt;


    [Header("UI Elements")]
    [SerializeField] AnswerData answerPrefab;
    [SerializeField] UiElements uiElements;
    [SerializeField] UIParameters parameters;


    [Space]
    List<AnswerData> currentAnswers = new List<AnswerData>();
    private int answerResolutionStateParameterHash = 0;
    public int AnswerResolutionStateParameterHash { get { return answerResolutionStateParameterHash; } }
    private IEnumerator IE_DisplayTimedResolution = null;


    void Start()
    {
        answerResolutionStateParameterHash = Animator.StringToHash("ScreenState");
        uiElements.RetryButton.onClick.AddListener(() =>
        {
            evnt.ResetGame();
        });

        uiElements.MainMenuButton.onClick.AddListener(() =>
        {
            evnt.ResetGame();
        });
    }

    void OnEnable()
    {
        // subscribe to questions
        evnt.UpdateQuestionUI += UpdateQuestionUI;

        // subscribe to update answer event
        evnt.UpdateQuestionAnswer += UpdateQuestionAnswer;

        // subscribe to answer resolution display event
        evnt.DisplayAnswerResolutionScreen += DisplayAnswerResolutionScreen;

        evnt.ResetGame += ResetGame;
    }

    void OnDisable()
    {
        // unsubscribe from questions
        evnt.UpdateQuestionUI -= UpdateQuestionUI;

        // unsubscribe from answer events
        evnt.UpdateQuestionAnswer -= UpdateQuestionAnswer;

        // unsubscribe from answer resolution display event
        evnt.DisplayAnswerResolutionScreen -= DisplayAnswerResolutionScreen;

        evnt.ResetGame -= ResetGame;
    }

    void DisplayAnswerResolutionScreen(AssessmentUtils.AnswerResolutionType answerResolutionType, int score, int questionsLength)
    {
        UpdateAnswerResolutionUI(answerResolutionType, score, questionsLength);
        uiElements.AnswerResolutionAnimator.SetInteger(answerResolutionStateParameterHash, 2);

        if (answerResolutionType != AssessmentUtils.AnswerResolutionType.Finished)
        {
            if (IE_DisplayTimedResolution != null) StartCoroutine(IE_DisplayTimedResolution);

            IE_DisplayTimedResolution = DisplayTimedResolution();
            StartCoroutine(IE_DisplayTimedResolution);

        }
    }

    IEnumerator DisplayTimedResolution()
    {
        yield return new WaitForSeconds(AssessmentUtils.AppDelayTime);
        uiElements.AnswerResolutionAnimator.SetInteger(answerResolutionStateParameterHash, 1);
    }

    void UpdateAnswerResolutionUI(AssessmentUtils.AnswerResolutionType answerResolutionType, int score, int questionsLength)
    {
        switch (answerResolutionType)
        {
            case AssessmentUtils.AnswerResolutionType.Correct:
                uiElements.AnswerResolutionBG.color = parameters.CorrectAnswerBG;
                uiElements.AnswerResolutionText.text = "Correct!";
                break;

            case AssessmentUtils.AnswerResolutionType.Incorrect:
                uiElements.AnswerResolutionBG.color = parameters.IncorrectAnswerBG;
                uiElements.AnswerResolutionText.text = "Wrong!";
                break;

            case AssessmentUtils.AnswerResolutionType.Finished:
                int percentage = CalculateScorePercentage(score, questionsLength);
                uiElements.AnswerResolutionBG.color = parameters.ResultsBG;
                uiElements.AnswerResolutionText.text = $"{percentage}%";
                uiElements.ScoreDescriptionText.text = $"You answered {score} out of {questionsLength} questions right";

                uiElements.ResultsUI.gameObject.SetActive(true);
                break;
        }
    }

    static int CalculateScorePercentage(int score, int questionsLength)
    {
        return (score * 100) / questionsLength;
    }

    void UpdateQuestionUI(QuestionSO question)
    {
        uiElements.QuestionText.text = question.Question;
        CreateAnswers(question);
    }

    void UpdateQuestionAnswer(AnswerData pickedAnswer)
    {
        // toggle the ui
        for (int i = 0; i < currentAnswers.ToArray().Length; i++)
        {
            if (currentAnswers[i].AnswerIndex != pickedAnswer.AnswerIndex) currentAnswers[i].Reset();
        }

    }

    void CreateAnswers(QuestionSO question)
    {
        EraseAnswers();
        float offset = 0 - parameters.Margins;

        for (int i = 0; i < question.Answers.Length; i++)
        {
            AnswerData newAnswer = (AnswerData)Instantiate(answerPrefab, uiElements.AnswersContentArea);
            newAnswer.UpdateData(question.Answers[i].Text, i);
            newAnswer.UpdateUI();

            newAnswer.Rect.anchoredPosition = new Vector2(0, (offset * i) - (newAnswer.Rect.rect.size.y * i));

            // offset -= (newAnswer.Rect.sizeDelta.y + parameters.Margins);
            // uiElements.AnswersContentArea.sizeDelta = new Vector2(uiElements.AnswersContentArea.sizeDelta.x, offset * -1);

            currentAnswers.Add(newAnswer);
        }
    }

    void EraseAnswers()
    {
        foreach (var answer in currentAnswers)
        {
            // destroy answer gameobject
            Destroy(answer.gameObject);
        }

        currentAnswers.Clear();
    }

    void ResetGame()
    {
        uiElements.ResultsUI.gameObject.SetActive(false);
        uiElements.QnAPage.gameObject.SetActive(false);
        uiElements.StartPage.gameObject.SetActive(true);
    }
}


