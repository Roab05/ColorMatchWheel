using UnityEngine;

public class PrivacyPolicyButton : MonoBehaviour
{
    public void OpenPrivatePolicy()
    {
        Application.OpenURL("https://www.google.com/");
    }
}
