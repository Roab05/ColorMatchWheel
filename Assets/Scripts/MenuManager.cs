using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public TextMeshProUGUI coins_text;
    public Button clearDataButton, playButton;
    public GameObject cleardata_textprefab, canvasObject;
    public AudioClip enterMainScreenSFX, dataClearSFX;
    private AudioSource menuAudioSource;
    private void Start()
    {
        menuAudioSource = GetComponent<AudioSource>();
        menuAudioSource.PlayOneShot(enterMainScreenSFX);
        coins_text.text = PlayerPrefs.GetInt("Coins", 0).ToString();
        clearDataButton.onClick.AddListener(ClearData);
        playButton.onClick.AddListener(PlayGame);
    }

    private void ClearData()
    {
        PlayerPrefs.DeleteKey("Coins");
        PlayerPrefs.DeleteKey("BestLevel");
        PlayerPrefs.DeleteKey("BestScore");
        coins_text.text = PlayerPrefs.GetInt("Coins", 0).ToString();
        var dataClear = Instantiate(cleardata_textprefab);
        dataClear.transform.SetParent(canvasObject.transform, false);
        Destroy(dataClear, 1f);
        menuAudioSource.PlayOneShot(dataClearSFX);
    }

    private void PlayGame()
    {
        SceneManager.LoadScene("GamePlay");
    }

}
