using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject color_wheel, center, newbest_textobj, level_textobj,
    bestscore_textobj, nextlevelwhenpass_textobj, coins_textobj,
    taptoplayagain_textobj, popupLevelPrefab;
    public TextMeshProUGUI score_text, level_text;
    private int _color_wheel_rotateDir, _center_rotateDir;
    [HideInInspector] public float _rotateSpeed;
    private bool gameOverTrigger;
    [HideInInspector]
    public bool gameOver;
    [HideInInspector]
    public int score, level, nxtlvl_score, coins;
    private bool isNewBest;

    public AudioClip startSFX, gameoverSFX1, gameoverSFX2, levelupSFX, newbestSFX, fasttap1SFX, fasttap2SFX;
    [HideInInspector]
    public AudioSource gameSFX;
    private int sfxRand;
    void Awake()
    {
        gameSFX = GetComponent<AudioSource>();
        if (instance == null) instance = this;
        else Destroy(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        gameSFX.PlayOneShot(startSFX);
        SpinDirectionManaging();
        _rotateSpeed = 50f;
        gameOver = false;
        gameOverTrigger = true;
        level = 1;
        nxtlvl_score = 5;
        score = 0;
        coins = PlayerPrefs.GetInt("Coins", 0);

        //FPS
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        if (score < 0) gameOver = true;
        if (gameOver)
        {
            if (gameOverTrigger)
            {
                isNewBest = SetBestScoreandCoins();
                if (isNewBest)
                    NewBestSFXandText();
                else
                    PlayGameOverSFX();
                GameOverUISetup();
                gameOverTrigger = false;
            }
            TapToPlayAgain();
        }
        else if (!gameOver && score >= 0)
        {
            level_text.text = "Level " + level.ToString();
            nextlevelwhenpass_textobj.GetComponent<TextMeshProUGUI>().text = "Next level when pass: " + (nxtlvl_score).ToString();
            coins_textobj.GetComponent<TextMeshProUGUI>().text = coins.ToString();
            score_text.text = score.ToString();
            color_wheel.transform.Rotate(new Vector3(0f, 0f, _rotateSpeed * _color_wheel_rotateDir) * Time.deltaTime, Space.Self);
            center.transform.Rotate(new Vector3(0f, 0f, _rotateSpeed * _center_rotateDir) * Time.deltaTime, Space.Self);
            if (Input.GetMouseButtonDown(0))
            {
                SpinDirectionManaging();
                if (level > PlayerPrefs.GetInt("BestLevel", 1))
                    PlayerPrefs.SetInt("BestLevel", level);
            }
        }
    }
    private void SpinDirectionManaging()
    {
        do
        {
            _center_rotateDir = Random.Range(-1, 2);
            _color_wheel_rotateDir = Random.Range(-1, 2);
        }
        while (_center_rotateDir == _color_wheel_rotateDir);
        if (_center_rotateDir == 0)
            _color_wheel_rotateDir *= 2;
        if (_color_wheel_rotateDir == 0)
            _center_rotateDir *= 2;
    }

    private bool SetBestScoreandCoins()
    {
        PlayerPrefs.SetInt("Coins", coins);
        if (score > PlayerPrefs.GetInt("BestScore", 0))
        {
            PlayerPrefs.SetInt("BestScore", score);
            return true;
        }
        return false;
    }

    private void NewBestSFXandText()
    {
        gameSFX.PlayOneShot(newbestSFX);
        newbest_textobj.SetActive(true);
    }
    private void PlayGameOverSFX()
    {
        sfxRand = Random.Range(0, 2);
        if (sfxRand == 0) gameSFX.PlayOneShot(gameoverSFX1);
        else gameSFX.PlayOneShot(gameoverSFX2);
    }

    private void GameOverUISetup()
    {
        if (!isNewBest)
        {
            bestscore_textobj.SetActive(true);
            bestscore_textobj.GetComponent<TextMeshProUGUI>().text = "Best: " + PlayerPrefs.GetInt("BestScore", 0)
            + " (Level " + PlayerPrefs.GetInt("BestLevel", 1) + ")";
        }
        nextlevelwhenpass_textobj.SetActive(false);
        if (score < 0)
            score = 0;
        score_text.text = "Score: " + score.ToString();
        score_text.fontSize = 100;
        taptoplayagain_textobj.SetActive(true);
    }
    private void TapToPlayAgain()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene("GamePlay");
        }
    }
}
