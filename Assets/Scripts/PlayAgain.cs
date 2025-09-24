using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayAgain : MonoBehaviour
{    public void PlayAgainButton()
    {
        SceneManager.LoadScene("GamePlay");
    }
}
