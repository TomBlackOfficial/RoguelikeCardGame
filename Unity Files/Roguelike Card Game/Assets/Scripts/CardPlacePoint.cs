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
    public CinemachineVirtualCamera pairedCamera;
    private static float cameraFocusTime = 2f;
   

    private void Start()
    {
        Redo();
        pairedCamera.Priority = 0;
        pairedCamera.LookAt = transform;
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
    
    public void SwitchCamera()
    {
        pairedCamera.Priority = 20;
        BattleController.instance.cameraMoving = true;
        Invoke("CameraOff", cameraFocusTime); 

    }

    private void CameraOff()
    {
        BattleController.instance.cameraMoving = false;
        pairedCamera.Priority = 0;
    }

}
