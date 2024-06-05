using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public static HandController instance;

    public List<Card> heldCards = new List<Card>();

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
        cardRotations.Clear();

        for (int i = 0; i < heldCards.Count; i++)
        {
            float newXPos = spawnPoint.position.x + ((i * (offset.x - (heldCards.Count * 0.008f))) - (heldCards.Count - 1) * ((offset.x - (heldCards.Count * 0.008f)) * 0.5f));
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

    public void RemoveCardFromHand(Card cardToRemove)
    {
        if (heldCards[cardToRemove.handPosition] == cardToRemove)
        {
            heldCards.RemoveAt(cardToRemove.handPosition);
        } 
        else
        {
            Debug.LogError("Card at position " + cardToRemove.handPosition + " is not the card being removed from hand.");
        }

        SetCardPositionsInHand();
    }

    public void AddCardToHand(Card cardToAdd)
    {
        heldCards.Add(cardToAdd);
        SetCardPositionsInHand();
    }

    public void ReduceHandManaCost(int amount, Card cardToIgnore)
    {
        if (heldCards == null || heldCards.Count <= 0)
            return;

        foreach (Card card in heldCards)
        {
            if (card != cardToIgnore)
            {
                card.ReduceManaCost(amount);
                card.UpdateCardDisplay();
            }
        }
    }

    public Vector3 GetCardPositionX(int index)
    {
        return new Vector3(cardPositions[index].x, spawnPoint.position.y, spawnPoint.position.z);
    }
}
