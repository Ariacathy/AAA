using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // 目标物体的Transform组件
    public Vector3 offset; // 摄像机相对于目标物体的偏移量
    public float smoothSpeed = 0.125f; // 平滑移动的速度

    // 更新摄像机的位置，使其平滑地跟随目标物体
    void LateUpdate()
    {
        if (target)
        {
            // 计算摄像机的目标位置
            Vector3 desiredPosition = target.position + offset;
            // 使用Vector3.Lerp进行平滑插值
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
        }
    }
}
