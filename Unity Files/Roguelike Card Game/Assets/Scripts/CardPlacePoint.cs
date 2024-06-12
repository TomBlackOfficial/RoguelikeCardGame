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
    public CinemachineVirtualCamera battleCamera;
    private static float cameraFocusTime = 3f;
    List<string> playedCards = new List<string>();
   

    private void Start()
    {
        Redo();
        pairedCamera.Priority = 0;
        pairedCamera.LookAt = transform;
    }

    private void Update()
    {
        if (activeCard != null)
        {
            if (activeCard != testCard)
            {
                if (playedCards.Contains(activeCard.name))
                {
                    Redo();
                }
                else
                {
                    playedCards.Add(activeCard.name);
                    Redo();
                    SwitchCamera();
                }
            }
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

    public void CameraOff()
    {
        BattleController.instance.cameraMoving = false;
        pairedCamera.Priority = 0;
        battleCamera.Priority = 0;
    }

    public void BattleCamSwitch()
    {
        battleCamera.Priority = 20;
    }

}
