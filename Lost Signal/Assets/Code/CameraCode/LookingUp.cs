using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;

public class LookingUp : MonoBehaviour
{
    CinemachineVirtualCamera cam;
    KeyCode key = KeyCode.W;
    bool lookingUp;
    [SerializeField] float lookingUpDis;
    float standardY;
    Vector3 pos;
    CinemachineFramingTransposer transposer;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
        transposer = cam.GetCinemachineComponent<CinemachineFramingTransposer>();
        standardY = transposer.m_ScreenY;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(key))
        {
            Debug.Log("move");
            transposer.m_ScreenY += lookingUpDis;
        }
        else if(Input.GetKeyUp(key))
        {
            transposer.m_ScreenY -= lookingUpDis;
            //Invoke("returnToReg",1);
        }

    }
    void returnToReg()
    {
        transposer.m_ScreenY =standardY;
    }
}
