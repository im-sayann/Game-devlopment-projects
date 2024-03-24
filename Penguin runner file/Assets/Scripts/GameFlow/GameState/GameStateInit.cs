using TMPro;
using UnityEngine;                                          
                                                            
public class GameStateInit : GameState   
    
{
    public GameObject menuUI;
    [SerializeField] private TextMeshProUGUI highscoreText;
    [SerializeField] private TextMeshProUGUI fishcountText;
    [SerializeField] private AudioClip menuLoopMusic;


    public override void Construct()
    {
        GameManager.Instance.ChangeCamera(GameCamera.Init);

        highscoreText.text = "Highscore: " + SaveManager.Instance.save.Highscore.ToString();
        fishcountText.text = "Fish: " + SaveManager.Instance.save.Fish.ToString();

        menuUI.SetActive(true);

        AudioManagerReal.Instance.PlayMusicWthXFade(menuLoopMusic, 0.2f);
    }

    public override void Destruct()
    {
        menuUI.SetActive(false);
    }

    public void OnPlayClick()
    {
        brain.ChangeState(GetComponent<GameStateGame>());
        GameStats.Instance.ResetSession();
        
    }

    public void OnShopClick()
    {
        brain.ChangeState(GetComponent<GameStateShop>());
        
    }
}                                                           
                                                            