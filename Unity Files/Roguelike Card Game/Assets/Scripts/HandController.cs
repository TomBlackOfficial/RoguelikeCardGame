using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public static HandController instance;

    public Card[] heldCards;

    [SerializeField] private Vector3 offset = new Vector3(0.25f, 0.1f, -0.15f);
    [SerializeField] private float rotation = 20f;
    public Transform spawnPoint;

    public List<Vector3> cardPositions = new List<Vector3>();
    public List<Quaternion> cardRotations = new List<Quaternion>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SetCardPositionsInHand();
    }

    public void SetCardPositionsInHand()
    {
        cardPositions.Clear();

        for (int i = 0; i < heldCards.Length; i++)
        {
            float newXPos = spawnPoint.position.x + ((i * offset.x) - (heldCards.Length - 1) * (offset.x * 0.5f));
            float newYPos = spawnPoint.position.y + (i * offset.y);
            float newZPos = spawnPoint.position.z + (Mathf.Abs(spawnPoint.position.x - newXPos) * offset.z);

            Vector3 newPos = new Vector3(newXPos, newYPos, newZPos);
            Quaternion newRot = Quaternion.Euler(new Vector3(0, (spawnPoint.position.x - newXPos) * -rotation, 0));

            cardPositions.Add(newPos);
            cardRotations.Add(newRot);

            heldCards[i].MoveToPoint(newPos, newRot);
            heldCards[i].AddCardToHand(i);
        }
    }

    public Vector3 GetCardPositionX(int index)
    {
        return new Vector3(cardPositions[index].x, spawnPoint.position.y, spawnPoint.position.z);
    }
}
