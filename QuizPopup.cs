using UnityEngine;
using UnityEngine.UI;

public class QuizPopup : MonoBehaviour
{
    public Text popupText; // 弹窗中文本的引用
    public Button confirmButton; // 确定按钮的引用

    void Start()
    {
        gameObject.SetActive(false); // 初始时隐藏弹窗
        confirmButton.onClick.AddListener(OnConfirmButtonClicked);
    }

    // 调用此方法来显示弹窗
    public void ShowPopup(string answers)
    {
        popupText.text = "你已经完成全部题目\n答案：" + answers;
        gameObject.SetActive(true);
    }

    // 调用此方法来隐藏弹窗
    public void HidePopup()
    {
        this.gameObject.SetActive(false);
    }

    // 确定按钮的点击事件
    public void OnConfirmButtonClicked()
    {
        HidePopup();
    }
}
