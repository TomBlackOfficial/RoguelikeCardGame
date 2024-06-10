using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public static EnemyController instance;
    
    public DeckScriptableObject enemyDeck;

    public enum AIType
    {
        placeFromDeck,
        handRandomPlace,
        handDefensive,
        handAttacking
    }
    public AIType enemyAIType;

    [SerializeField] private List<CardScriptableObject> deckToUse = new List<CardScriptableObject>();
    private List<CardScriptableObject> activeCards = new List<CardScriptableObject>();

    [SerializeField] private Card cardToSpawn;
    [SerializeField] private Transform cardSpawnPoint;

    public List<CardScriptableObject> cardsInHand = new List<CardScriptableObject>();

    private void Awake()
    {
        instance = this;

        RandomizeAIType();
    }

    private void Start()
    {
        SetupDeck();

        if (enemyAIType != AIType.placeFromDeck)
        {
            SetupHand();
        }
    }

    private void SetupDeck()
    {
        activeCards.Clear();

        List<CardScriptableObject> tempDeck = new List<CardScriptableObject>();
        tempDeck.AddRange(deckToUse);

        int iterations = 0;
        while (tempDeck.Count > 0 && iterations < 500)
        {
            int selected = Random.Range(0, tempDeck.Count);
            activeCards.Add(tempDeck[selected]);
            tempDeck.RemoveAt(selected);

            iterations++;
        }
    }

    public void StartAction()
    {
        StartCoroutine(EnemyActionCoroutine());
    }

    IEnumerator EnemyActionCoroutine()
    {
        if (activeCards.Count == 0)
        {
            SetupDeck();
        }

        yield return new WaitForSeconds(0.5f);

        if (enemyAIType != AIType.placeFromDeck)
        {
            for (int i = 0; i < BattleController.instance.cardsPerTurn; i++)
            {
                DrawMultipleCards(BattleController.instance.cardsPerTurn);
            }
        }

        List<CardPlacePoint> cardPoints = new List<CardPlacePoint>();
        cardPoints.AddRange(CardPointsController.instance.enemyCardPoints);

        int randomPoint = Random.Range(0, cardPoints.Count);
        CardPlacePoint selectedPoint = cardPoints[randomPoint];

        if (enemyAIType == AIType.placeFromDeck || enemyAIType == AIType.handRandomPlace)
        {
            cardPoints.Remove(selectedPoint);

            while (selectedPoint.activeCard != null && cardPoints.Count > 0)
            {
                randomPoint = Random.Range(0, cardPoints.Count);
                selectedPoint = cardPoints[randomPoint];
                cardPoints.RemoveAt(randomPoint);
            }
        }

        CardScriptableObject selectedCard = null;
        int iterations = 0;
        List<CardPlacePoint> preferredPoints = new List<CardPlacePoint>();
        List<CardPlacePoint> secondaryPoints = new List<CardPlacePoint>();

        switch (enemyAIType)
        {
            case AIType.placeFromDeck:

                if (selectedPoint.activeCard == null)
                {
                    Card newCard = Instantiate(cardToSpawn, cardSpawnPoint.position, cardSpawnPoint.rotation);
                    newCard.cardSO = activeCards[0];
                    activeCards.RemoveAt(0);
                    newCard.SetupCard();
                    newCard.MoveToPoint(selectedPoint.transform.position, selectedPoint.transform.rotation);

                    selectedPoint.activeCard = newCard;
                    newCard.assignedPlace = selectedPoint;
                }

                break;

            case AIType.handRandomPlace:

                selectedCard = SelectedCardToPlay();

                iterations = 0;
                while(selectedCard != null && iterations < 100 && selectedPoint.activeCard == null)
                {
                    PlayCard(selectedCard, selectedPoint);

                    selectedCard = SelectedCardToPlay();

                    iterations++;

                    yield return new WaitForSeconds(CardPointsController.instance.timeBetweenAttacks);

                    while (selectedPoint.activeCard != null && cardPoints.Count > 0)
                    {
                        randomPoint = Random.Range(0, cardPoints.Count);
                        selectedPoint = cardPoints[randomPoint];
                        cardPoints.RemoveAt(randomPoint);
                    }
                }

                break;

            case AIType.handDefensive:

                selectedCard = SelectedCardToPlay();

                preferredPoints.Clear();
                secondaryPoints.Clear();

                for (int i = 0; i < cardPoints.Count; i++)
                {
                    if (cardPoints[i].activeCard == null)
                    {
                        if (CardPointsController.instance.playerCardPoints[i].activeCard != null)
                        {
                            preferredPoints.Add(cardPoints[i]);
                        }
                        else
                        {
                            secondaryPoints.Add(cardPoints[i]);
                        }
                    }
                }

                iterations = 0;
                while (selectedCard != null && iterations < 50 && preferredPoints.Count + secondaryPoints.Count > 0)
                {
                    if (preferredPoints.Count > 0)
                    {
                        int selectPoint = Random.Range(0, preferredPoints.Count);
                        selectedPoint = preferredPoints[selectPoint];

                        preferredPoints.RemoveAt(selectPoint);
                    }
                    else
                    {
                        int selectPoint = Random.Range(0, secondaryPoints.Count);
                        selectedPoint = secondaryPoints[selectPoint];

                        secondaryPoints.RemoveAt(selectPoint);
                    }

                    PlayCard(selectedCard, selectedPoint);

                    selectedCard = SelectedCardToPlay();

                    iterations++;

                    yield return new WaitForSeconds(CardPointsController.instance.timeBetweenAttacks);
                }

                break;

            case AIType.handAttacking:

                selectedCard = SelectedCardToPlay();

                preferredPoints.Clear();
                secondaryPoints.Clear();

                for (int i = 0; i < cardPoints.Count; i++)
                {
                    if (cardPoints[i].activeCard == null)
                    {
                        if (CardPointsController.instance.playerCardPoints[i].activeCard == null)
                        {
                            preferredPoints.Add(cardPoints[i]);
                        }
                        else
                        {
                            secondaryPoints.Add(cardPoints[i]);
                        }
                    }
                }

                iterations = 0;
                while (selectedCard != null && iterations < 50 && preferredPoints.Count + secondaryPoints.Count > 0)
                {
                    if (preferredPoints.Count > 0)
                    {
                        int selectPoint = Random.Range(0, preferredPoints.Count);
                        selectedPoint = preferredPoints[selectPoint];

                        preferredPoints.RemoveAt(selectPoint);
                    }
                    else
                    {
                        int selectPoint = Random.Range(0, secondaryPoints.Count);
                        selectedPoint = secondaryPoints[selectPoint];

                        secondaryPoints.RemoveAt(selectPoint);
                    }

                    PlayCard(selectedCard, selectedPoint);

                    selectedCard = SelectedCardToPlay();

                    iterations++;

                    yield return new WaitForSeconds(CardPointsController.instance.timeBetweenAttacks);
                }

                break;
        }

        if (BattleController.instance.enemyHealth <= 0)
        {
            // Transfer a random card from enemy's deck to player's deck
            if (enemyDeck.cards.Count > 0)
            {
                int randomIndex = Random.Range(0, enemyDeck.cards.Count);
                CardScriptableObject transferredCard = enemyDeck.cards[randomIndex];
                enemyDeck.cards.RemoveAt(randomIndex);
                DeckController.instance.AddCardToDeck(transferredCard);
            }
        }

        yield return new WaitForSeconds(0.5f);

        BattleController.instance.AdvanceTurn();
    }

    public void DrawCardToHand()
    {
        if (activeCards.Count == 0)
        {
            SetupDeck();
        }

        cardsInHand.Add(activeCards[0]);
        activeCards.RemoveAt(0);
    }

    public void DrawMultipleCards(int amountToDraw)
    {
        if (amountToDraw < 1)
            return;

        for (int i = 0; i < amountToDraw; i++)
        {
            DrawCardToHand();
        }
    }

    private void SetupHand()
    {
        for (int i = 0; i < BattleController.instance.startingCards; i++)
        {
            if (activeCards.Count == 0)
            {
                SetupDeck();
            }

            cardsInHand.Add(activeCards[0]);
            activeCards.RemoveAt(0);
        }
    }

    public void PlayCard(CardScriptableObject cardSO, CardPlacePoint placePoint)
    {
        Card newCard = Instantiate(cardToSpawn, cardSpawnPoint.position, cardSpawnPoint.rotation);
        newCard.cardSO = cardSO;
        newCard.SetupCard();
        newCard.MoveToPoint(placePoint.transform.position, placePoint.transform.rotation);
        newCard.SpawnCreature();

        placePoint.activeCard = newCard;
        newCard.assignedPlace = placePoint;

        cardsInHand.Remove(cardSO);

        BattleController.instance.SpendEnemyMana(cardSO.manaCost);
    }

    CardScriptableObject SelectedCardToPlay()
    {
        CardScriptableObject cardToPlay = null;

        List<CardScriptableObject> cardsToPlay = new List<CardScriptableObject>();
        foreach (CardScriptableObject card in cardsInHand)
        {
            if (card.manaCost <= BattleController.instance.enemyMana)
            {
                cardsToPlay.Add(card);
            }
        }

        if (cardsToPlay.Count > 0)
        {
            int selected = Random.Range(0, Random.Range(0, cardsToPlay.Count));

            cardToPlay = cardsToPlay[selected];
        }

        return cardToPlay;
    }

    private void RandomizeAIType()
    {
        var values = System.Enum.GetValues(typeof(AIType));
        int random = Random.Range(1, values.Length);
        enemyAIType = (AIType)values.GetValue(random);
    }
}
