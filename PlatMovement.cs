using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5.0f; // 控制移动速度

    private void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal"); // 获取水平方向输入
        float moveVertical = Input.GetAxis("Vertical"); // 获取垂直方向输入

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical); // 创建移动向量
        transform.Translate(movement * speed * Time.deltaTime); // 应用移动
    }
}
