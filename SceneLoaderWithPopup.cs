using UnityEngine;
using UnityEngine.UI;

public class LoadingPopup : MonoBehaviour
{
    public Text loadingText; // 弹窗中文本的引用

    // 显示弹窗
    public void Show()
    {
        loadingText.text = "工厂加载中";
        gameObject.SetActive(true);
    }

    // 隐藏弹窗
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
