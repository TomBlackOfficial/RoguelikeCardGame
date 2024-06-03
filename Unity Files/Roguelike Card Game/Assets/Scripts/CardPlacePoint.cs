using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CardPlacePoint : MonoBehaviour
{
    public Card activeCard;
    public bool isPlayerPoint;
    
    //Daniel's code
    private Card testCard;
    [SerializeField] private CinemachineVirtualCamera pairedCamera;
    [SerializeField] private float cameraFocusTime = 3.0f;

    private void Start()
    {
        Redo();
        pairedCamera.Priority = 0;
    }

    private void Update()
    {
        if (activeCard != testCard)
        {
            Redo();
            SwitchCamera();
        }
    }

    private void Redo()
    {
        testCard = activeCard;
    }
    
    private void SwitchCamera()
    {
        pairedCamera.Priority = 20;
        Invoke("CameraOff", cameraFocusTime); 

    }

    private void CameraOff()
    {
        pairedCamera.Priority = 0;
    }
}
