using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public GameObject loadingPopup;

    public void LoadDemoScene()
    {
        loadingPopup.GetComponent<LoadingPopup>().Show();

        SceneManager.LoadSceneAsync("Demo", LoadSceneMode.Single);
    }
}