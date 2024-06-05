using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeckController : MonoBehaviour
{
    public static DeckController instance;

    [SerializeField] private List<CardScriptableObject> deckToUse = new List<CardScriptableObject>();
    private List<CardScriptableObject> activeCards = new List<CardScriptableObject>();

    [SerializeField] private Card cardPrefab;
    private float waitBetweenDrawingCards = 0.25f;

    // Ensure that the DeckController persists across scenes or life cycles where it is needed.
    private void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }


    private void Awake()
    {
        instance = this;
        SetupDeck(); // Ensure deck is set up at the start
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DrawCardToHand();
        }
    }

    private void SetupDeck()
    {
        activeCards.Clear();

        List<CardScriptableObject> tempDeck = new List<CardScriptableObject>();
        tempDeck.AddRange(deckToUse);

        int iterations = 0;
        while(tempDeck.Count > 0 && iterations < 500)
        {
            int selected = Random.Range(0, tempDeck.Count);
            activeCards.Add(tempDeck[selected]);
            tempDeck.RemoveAt(selected);

            iterations++;
        }
    }

    public void DrawCardToHand()
    {
        if (activeCards.Count == 0)
        {
            SetupDeck();
        }

        if (activeCards.Count > 0)
        {
            Card newCard = Instantiate(cardPrefab, transform.position, transform.rotation);
            newCard.cardSO = activeCards[0];
            newCard.SetupCard();

            activeCards.RemoveAt(0);

            HandController.instance.AddCardToHand(newCard);
        }

        else
        {
            Debug.LogWarning("No cards available to draw.");
        }

    }

    public void DrawMultipleCards(int amountToDraw)
    {
        if (amountToDraw < 1)
            return;
        else if (amountToDraw == 1)
        {
            DrawCardToHand();
            return;
        }
            

        StartCoroutine(DrawMultipleCoroutine(amountToDraw));
    }

    IEnumerator DrawMultipleCoroutine(int amountToDraw) 
    {
        for (int i = 0; i < amountToDraw; i++)
        {
            DrawCardToHand();

            yield return new WaitForSeconds(waitBetweenDrawingCards);
        }
    }

    public void AddCardToDeck(CardScriptableObject card)
    {
        deckToUse.Add(card);
    }

    public void ClearDeck()
    {
        deckToUse.Clear();
    }
    
    public void UpdateActiveCards()
    {
        activeCards.Clear();
        activeCards.AddRange(deckToUse);
    }
}
