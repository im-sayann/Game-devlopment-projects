using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameStateDeath : GameState
{
    public GameObject deathUI;
    [SerializeField] private TextMeshProUGUI highScore;
    [SerializeField] private TextMeshProUGUI currentScore;
    [SerializeField] private TextMeshProUGUI fishTotal;
    [SerializeField] private TextMeshProUGUI currentFish;

    public PlayerMotor PLAYERMOTOR;

    // Completion circle fields
    [SerializeField] private Image completionCircle;
    public float timeToDecision = 9.5f;
    private float deathTime;

    public override void Construct()
    {
        base.Construct();
        GameManager.Instance.motor.PausePlayer();

        deathTime = Time.time;
        deathUI.SetActive(true);
        completionCircle.gameObject.SetActive(true);


        //Prior to saving, set the highscore if needed
        if (SaveManager.Instance.save.Highscore < (int)GameStats.Instance.score)
        {
            SaveManager.Instance.save.Highscore = (int)GameStats.Instance.score;
            currentScore.color = Color.green;
        }
        else
            currentScore.color = Color.white;


        SaveManager.Instance.save.Fish += GameStats.Instance.fishCollectedThisSession;
        SaveManager.Instance.Save();

        highScore.text = "HighScore :" + SaveManager.Instance.save.Highscore;
        currentScore.text = GameStats.Instance.ScoreToText();
        fishTotal.text = "Total fish :" + SaveManager.Instance.save.Fish;
        currentFish.text = GameStats.Instance.FishToText();

    }
    public override void Destruct()
    {

        deathUI.SetActive(false);
    }

    public override void UpdateState()
    {
        float ratio = (Time.time - deathTime) / timeToDecision;
        completionCircle.color = Color.Lerp(Color.green, Color.red, ratio);
        completionCircle.fillAmount = 1 - ratio;

        if (ratio > 1)
        {
            completionCircle.gameObject.SetActive(false);

        }
    }

    public void ResumeGame()
    {
        brain.ChangeState(GetComponent<GameStateGame>());
        GameManager.Instance.motor.RespawnPlayer();
        



    }
    public void ToMenu()
    {


        brain.ChangeState(GetComponent<GameStateInit>());

        GameManager.Instance.motor.ResetPlayer();
        GameManager.Instance.motor.baseRunSpeed = 7.7f;
        GameManager.Instance.worldGeneration.ResetWorld();
        GameManager.Instance.sceneChunkGeneration.ResetWorld();



       /* PLAYERMOTOR = FindAnyObjectByType<PlayerMotor>();

        PLAYERMOTOR.baseRunSpeed = 7.7f;*/

    }



}

