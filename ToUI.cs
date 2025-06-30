using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToUIScene : MonoBehaviour
{
    public void OnReturnButtonClicked()
    {
        SceneManager.LoadScene("UI");
    }
}
