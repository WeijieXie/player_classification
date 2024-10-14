using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform playerTransform; // �ƶ�������
    public Vector3 deviation; // ƫ����

    void Start()
    {
        deviation = transform.position - playerTransform.position; // ��ʼ�����������ƫ����=�����λ�� - �ƶ������ƫ����
    }

    void LateUpdate()
    {
        transform.position = playerTransform.position + deviation; // �����λ�� = �ƶ������λ�� + ƫ����

    }
}
