using UnityEngine;

public class RotateWithMouse : MonoBehaviour
{
    public float rotationSpeed = 100.0f; // 控制旋转速度

    private void Update()
    {
        // 检查鼠标左键是否被按下
        if (Input.GetMouseButton(0)) // 0代表鼠标左键
        {
            // 获取鼠标在水平方向上的移动量
            float horizontalRotation = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;

            // 只围绕Y轴旋转
            transform.Rotate(0f, horizontalRotation, 0f);
        }
    }
}