using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public Card[] heldCards;

    [SerializeField] private float xOffset = 0.26f;
    [SerializeField] private float yDrop = 0.1f;
    [SerializeField] private Transform spawnPoint;

    public List<Vector3> cardPositions = new List<Vector3>();

    private void Start()
    {
        SetCardPositionsInHand();
    }

    public void SetCardPositionsInHand()
    {
        cardPositions.Clear();

        for (int i = 0; i < heldCards.Length; i++)
        {
            float newXPos = spawnPoint.position.x + ((i * xOffset) - (heldCards.Length - 1) * (xOffset * 0.5f));
            heldCards[i].transform.position = new Vector3(newXPos, spawnPoint.position.y, spawnPoint.position.z);
            heldCards[i].transform.localEulerAngles = new Vector3(0, (spawnPoint.position.x - newXPos) * -15, 0);
        }
    }
}
