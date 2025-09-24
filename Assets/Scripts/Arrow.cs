using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Arrow : MonoBehaviour
{
    // Start is called before the first frame update
    
    private SpriteRenderer colorbarSpriteRenderer;
    [SerializeField]
    private Color[] colors;
    private readonly string[] colors_name = { "Red", "Orange", "Yellow", "Green", "Blue", "Purple" };
    private string _name;
    private int idx, new_idx, plus_point;

    private float previousTap, fastTapThreshhold1, fastTapThreshhold2, colorbarTime, const_colorbarTime;

    public Transform arrowHead;
    public GameObject popUpPointPrefab, popUpCoinPrefab, canvasObject, colorbarObject;
    public AudioClip tapSFX1, tapSFX2;
    private AudioSource tapSFX;
    void Start()
    {
        colorbarSpriteRenderer = colorbarObject.GetComponent<SpriteRenderer>();
        tapSFX = GetComponent<AudioSource>();
        new_idx = Random.Range(1, colors.Length);
        colorbarSpriteRenderer.color = colors[new_idx];
        previousTap = -10f;
        fastTapThreshhold1 = 0.5f;
        fastTapThreshhold2 = 0.3f;
        const_colorbarTime = 5f;
        colorbarTime = const_colorbarTime;
    }

    private void Update()
    {
        if (!GameManager.instance.gameOver)
        {
            GetTap();
            colorbarObject.transform.Translate(new Vector2(0f, (2.14f - 0.6f) / const_colorbarTime) * Time.deltaTime, Space.Self);
            colorbarTime -= Time.deltaTime;
            if (colorbarTime <= 0f) GameManager.instance.gameOver = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        _name = collision.gameObject.name;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        _name = collision.gameObject.name;
        if (!GameManager.instance.gameOver && _name == colors_name[new_idx])
        {
            GameManager.instance.score -= GameManager.instance.level;
            //Handheld.Vibrate();
            ShowPopupPoint2();
        }
    }

    private void GetTap()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_name == colors_name[new_idx])
            {
                idx = new_idx;
                while (new_idx == idx)
                    new_idx = Random.Range(0, colors.Length);
                ColorbarManagement();
                plus_point = 1;
                if (x2PointManager.instance.isX2Active(idx))
                {
                    x2PointManager.instance.x2Disable(idx);
                    plus_point *= 2;
                }
                if (GameManager.instance.score + plus_point > GameManager.instance.nxtlvl_score)
                {
                    GameManager.instance.level++;
                    plus_point = GameManager.instance.level;
                    GameManager.instance._rotateSpeed += 10f;
                    GameManager.instance.nxtlvl_score += 5 + GameManager.instance.level;
                    GameManager.instance.gameSFX.PlayOneShot(GameManager.instance.levelupSFX);
                    const_colorbarTime -= 0.2f;
                    Instantiate(GameManager.instance.popupLevelPrefab);
                }
                else
                {
                    tapSFX.PlayOneShot(tapSFX1);
                }
                if (FastTap2())
                {
                    GameManager.instance.coins += 2;
                    popUpCoinPrefab.GetComponent<TextMeshProUGUI>().text = "Great! +2";
                    GameManager.instance.gameSFX.PlayOneShot(GameManager.instance.fasttap2SFX);
                    ShowPopupCoin();
                }
                else if (FastTap1())
                {
                    GameManager.instance.coins += 1;
                    popUpCoinPrefab.GetComponent<TextMeshProUGUI>().text = "Good! +1";
                    GameManager.instance.gameSFX.PlayOneShot(GameManager.instance.fasttap1SFX);
                    ShowPopupCoin();
                }
                ShowPopupPoint1();
                previousTap = Time.time;
                GameManager.instance.score += plus_point;
            }
            else
            {
                GameManager.instance.gameOver = true;
            }
            x2PointManager.instance.x2Generate();
        }
    }

    private void ShowPopupPoint1()
    {
        if (!popUpPointPrefab)
            return;
        var obj = Instantiate(popUpPointPrefab, arrowHead.position + new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f), 0), Quaternion.identity);
        obj.GetComponent<TextMesh>().text = "+" + (plus_point).ToString();
        //if (plus_point > 0)
            //obj.GetComponent<TextMesh>().color = colors[idx];
    }
    private void ShowPopupPoint2()
    {
        if (!popUpPointPrefab)
            return;
        var obj = Instantiate(popUpPointPrefab, arrowHead.position + new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f), 0), Quaternion.identity);
        obj.GetComponent<TextMesh>().text = "-" + GameManager.instance.level;
        obj.GetComponent<Rigidbody2D>().gravityScale *= -1;

    }
    private bool FastTap1()
    {
        if (Time.time - previousTap <= fastTapThreshhold1)
            return true;
        return false;
    }
    private bool FastTap2()
    {
        if (Time.time - previousTap <= fastTapThreshhold2)
            return true;
        return false;
    }
    private void ShowPopupCoin()
    {
        var coin = Instantiate(popUpCoinPrefab);
        coin.transform.SetParent(canvasObject.transform, false);
    }

    private void ColorbarManagement()
    {
        colorbarSpriteRenderer.color = colors[new_idx];
        colorbarObject.transform.localPosition = new Vector2(0f, 0.6f);
        colorbarTime = const_colorbarTime;
    }
}
