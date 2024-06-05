using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPlacePoint : MonoBehaviour
{
    public Card activeCard;
    public bool isPlayerPoint;
<<<<<<< Updated upstream
=======
    
    //Daniel's code
    private Card testCard;
    public CinemachineVirtualCamera pairedCamera;
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
    
    public void SwitchCamera()
    {
        pairedCamera.Priority = 20;
        Invoke("CameraOff", cameraFocusTime); 

    }

    private void CameraOff()
    {
        pairedCamera.Priority = 0;
    }
>>>>>>> Stashed changes
}
