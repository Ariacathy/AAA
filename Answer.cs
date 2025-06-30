using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Answer : MonoBehaviour
{
    //读取文档
    string[][] ArrayX;//题目数据
    string[] lineArray;//读取到题目数据
    private int topicMax = 0;//最大题数
    private List<bool> isAnserList = new List<bool>();//存放是否答过题的状态

    //加载题目
    public GameObject tipsbtn;//提示按钮
    public Text tipsText;//提示信息
    public List<Toggle> toggleList;//答题Toggle
    public Text indexText;//当前第几题
    public Text TM_Text;//当前题目
    public List<Text> DA_TextList;//选项
    private int topicIndex = 0;//第几题
    public int randomQuestionsCount = 15; // 随机题目数量

    //按钮功能及提示信息
    public Transform TF_TopicShowParent;//完成按钮的父物体
    public Button BtnBack;//上一题
    public Button BtnNext;//下一题
    public Button BtnTip;//消息提醒
    public Button BtnJump;//跳转题目
    public Button BtnCompletePrefab;//已答题提示的预设
    public InputField jumpInput;//跳转题目
    public Text TextAccuracy;//正确率
    private int anserint = 0;//已经答过几题
    private int isRightNum = 0;//正确题数
    private int[] questionIndexes; // 存储随机题目索引的数组
    public QuizPopup quizPopup; // 引用 QuizPopup 脚本

    void Awake()
    {
        TextCsv();
        questionIndexes = new int[randomQuestionsCount];
        LoadAnswer();
    }
    public void LoadsGame()
    {
        SceneManager.LoadScene(1);
    }
    void Start()
    {
        toggleList[0].onValueChanged.AddListener((isOn) => AnswerRightRrongJudgment(isOn, 0));
        toggleList[1].onValueChanged.AddListener((isOn) => AnswerRightRrongJudgment(isOn, 1));
        toggleList[2].onValueChanged.AddListener((isOn) => AnswerRightRrongJudgment(isOn, 2));
        toggleList[3].onValueChanged.AddListener((isOn) => AnswerRightRrongJudgment(isOn, 3));

        BtnTip.onClick.AddListener(() => Select_Answer(0));
        BtnBack.onClick.AddListener(() => Select_Answer(1));
        BtnNext.onClick.AddListener(() => Select_Answer(2));
        BtnJump.onClick.AddListener(() => Select_Answer(3));

        //创建所有题目的Item 并且添加点击事件
        foreach (Transform child in TF_TopicShowParent)
        {
            Destroy(child.gameObject);
        }

        // 创建所有题目的Item 并且添加点击事件
        for (int i = 0; i < randomQuestionsCount; i++)
        {
            Button go = Instantiate(BtnCompletePrefab, TF_TopicShowParent);
            int currentIndex = i;
            go.name = currentIndex.ToString();
            go.transform.GetChild(0).GetComponent<Text>().text = (currentIndex + 1).ToString();
            go.onClick.AddListener(() =>
            {
                topicIndex = currentIndex;
                LoadAnswer();
            });
        }
    }


    /*****************读取txt数据******************/
    void TextCsv()
    {
        // 读取csv二进制文件
        TextAsset binAsset = Resources.Load("Data", typeof(TextAsset)) as TextAsset;
        // 读取每一行的内容
        lineArray = binAsset.text.Split('\r');
        // 创建二维数组，长度等于里面的内容
        ArrayX = new string[lineArray.Length][];

        // 把csv中的数据储存在二维数组中
        for (int i = 0; i < lineArray.Length; i++)
        {
            ArrayX[i] = lineArray[i].Split(':');
        }

        // 设置题目状态
        topicMax = lineArray.Length;
        for (int x = 0; x < topicMax + 1; x++)
        {
            isAnserList.Add(false);
        }

        // 创建一个列表来存储随机选择的题目索引
        List<int> randomIndexes = new List<int>(Enumerable.Range(0, topicMax));
        Shuffle(randomIndexes); // 打乱索引
        randomIndexes = randomIndexes.GetRange(0, Mathf.Min(randomQuestionsCount, randomIndexes.Count));
        questionIndexes = randomIndexes.ToArray();

        // 使用随机索引重新创建ArrayX，只包含随机题目
        ArrayX = randomIndexes.Select(index => ArrayX[index]).ToArray();
        topicMax = randomQuestionsCount; // 更新题目数量
    }

    void Shuffle(List<int> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            int value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    /*****************加载题目******************/
    void LoadAnswer()
    {
        for (int i = 0; i < toggleList.Count; i++)
        {
            toggleList[i].isOn = false;
        }
        for (int i = 0; i < toggleList.Count; i++)
        {
            toggleList[i].interactable = true;
        }

        tipsbtn.SetActive(false);
        tipsText.text = "";

        indexText.text ="共"+ randomQuestionsCount + "题,"+ "当前第" + (topicIndex + 1) + "题：";//第几题
        TM_Text.text = "";
        TM_Text.text = ArrayX[topicIndex][1].Replace("\n","").Replace("\r","").Replace("\t","");//题目,去掉其他不必要字符
        int idx = ArrayX[topicIndex].Length - 3;//有几个选项 
        for (int x = 0; x < idx; x++)
        {
            
            DA_TextList[x].text = ArrayX[topicIndex][x + 2];//选项,题目 
        }
    }

    /*****************按钮功能******************/
    void Select_Answer(int index)
    {
        switch (index)
        {
            case 0://提示
                int idx = ArrayX[topicIndex].Length - 1;
                int n = int.Parse(ArrayX[topicIndex][idx]);
                string nM = "";
                switch (n)
                {
                    case 1:
                        nM = "A";
                        break;
                    case 2:
                        nM = "B";
                        break;
                    case 3:
                        nM = "C";
                        break;
                    case 4:
                        nM = "D";
                        break;
                }
                tipsText.text = "<color=#FFAB08FF>" + "正确答案是：" + nM + "</color>";
                break;
            case 1://上一题
                topicIndex--;
                topicIndex = topicIndex < 0 ? topicMax - 1 : topicIndex;
                LoadAnswer();
                return;
                //不能循环
                if (topicIndex > 0)
                {
                    topicIndex--;
                    topicIndex = topicIndex < 0 ? lineArray.Length : topicIndex;
                    LoadAnswer();
                }
                else
                {
                    tipsText.text = "<color=#27FF02FF>" + "前面已经没有题目了！" + "</color>";
                }
                break;
            case 2://下一题
                topicIndex++;
                topicIndex = topicIndex > topicMax-1 ? 0 : topicIndex;
                LoadAnswer();
                return;
                //不能循环
                if (topicIndex < topicMax - 1)
                {
                    topicIndex++;
                    LoadAnswer();
                }
                else
                {
                    tipsText.text = "<color=#27FF02FF>" + "哎呀！已经是最后一题了。" + "</color>";
                }
                break;
            case 3://跳转
                int x = int.Parse(jumpInput.text) - 1;
                if (x >= 0 && x < topicMax)
                {
                    topicIndex = x;
                    jumpInput.text = "";
                    LoadAnswer();
                }
                else
                {
                    tipsText.text = "<color=#27FF02FF>" + "不在范围内！" + "</color>";
                }
                break;
        }
    }

    /*****************题目对错判断******************/
    void AnswerRightRrongJudgment(bool check, int index)
    {
        if (check)
        {
            bool isRight;
            int idx = ArrayX[topicIndex].Length - 1;
            int n = int.Parse(ArrayX[topicIndex][idx]) - 1;
            if (n == index)
            {
                tipsText.text = "<color=#27FF02FF>" + "恭喜你，答对了！" + "</color>";
                isRight = true;
                TF_TopicShowParent.GetChild(topicIndex).GetChild(0).GetComponent<Text>().color = new Color32(39, 255,2, 255);
                tipsbtn.SetActive(true);
            }
            else
            {
                tipsText.text = "<color=#FF0020FF>" + "对不起，答错了！" + "</color>";
                isRight = false;
                TF_TopicShowParent.GetChild(topicIndex).GetChild(0).GetComponent<Text>().color = new Color32(255, 0, 32, 255);
                tipsbtn.SetActive(true);
            }

            if (isAnserList[topicIndex])
            {
                tipsText.text = "<color=#FF0020FF>" + "这道题已答过！" + "</color>";
            }
            else
            {
                anserint++;
                if (isRight)
                {
                    isRightNum++;
                }
                isAnserList[topicIndex] = true;
                TextAccuracy.text = "正确率：" + ((float)isRightNum / anserint * 100).ToString("f2") + "%";
            }

            //禁用掉选项
            for (int i = 0; i < toggleList.Count; i++)
            {
                toggleList[i].interactable = false;
            }
        }
    }

    private void Update()
    {
        if (anserint >= randomQuestionsCount)
        {
            // 获取所有题目的答案
            string answers = GetAnswers();
            // 显示弹窗
            quizPopup.ShowPopup(answers);
            // 禁用答题选项
            foreach (Toggle toggle in toggleList)
            {
                toggle.interactable = false;
            }
        }
    }

    private string GetAnswers()
    {
        string answers = "";
        int count = 0; // 用于计数当前行的答案数量

        for (int i = 0; i < randomQuestionsCount; i++)
        {
            // 确保索引有效
            if (questionIndexes[i] >= 0 && questionIndexes[i] < ArrayX.Length)
            {
                int correctAnswerIndex = ArrayX[questionIndexes[i]].Length - 1;
                string answer = ArrayX[questionIndexes[i]][correctAnswerIndex];

                // 添加答案到字符串中
                answers += "Q" + (i + 1) + ": " + answer + "  "; // 添加两个空格作为分隔

                count++; // 增加计数

                // 每三个答案换行
                if (count % 3 == 0)
                {
                    answers += "\n"; // 换行
                }
            }
        }

        return answers;
    }
}