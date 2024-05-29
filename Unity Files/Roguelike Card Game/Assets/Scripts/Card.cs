using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Card : MonoBehaviour
{
    private static string WARNING_MANA = "Not enough Mana to perform this action.";

    public CardScriptableObject cardSO;

    public int health { private set; get; }
    public int attack { private set; get; }
    public int manaCost { private set; get; }
    public CardScriptableObject.Type cardType { private set; get; }

    [Header("Settings")]
    public bool isPlayer;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 3f;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask placementMask;

    [Header("Assign")]
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text attackText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private TMP_Text cardTypeText;
    [SerializeField] private Transform creatureSpawnPoint;
    [SerializeField] private GameObject cardMesh;

    [HideInInspector] public CardPlacePoint assignedPlace;

    public bool inHand {  private set; get; }
    public int handPosition { private set; get; }
    public Animator anim { private set; get; }

    private HandController controller;
    private Collider col;
    private GameObject creatureObj;

    private Vector3 targetPos;
    private Quaternion targetRot;

    private bool isSelected;
    private bool justPressed;

    private void Start()
    {
        controller = HandController.instance;
        col = GetComponent<Collider>();

        if (targetPos == Vector3.zero)
        {
            targetPos = transform.position;
            targetRot = transform.rotation;
        }

        SetupCard();
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);

        if (isSelected)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f, groundMask))
            {
                MoveToPoint(hit.point + new Vector3(0f, 0.05f, 0f), Quaternion.identity);
            }

            if (cardType == CardScriptableObject.Type.Spell)
            {
                if (Input.GetMouseButtonDown(0) && justPressed == false && !BattleController.instance.battleEnded && BattleController.instance.CanPerformActions())
                    PlaySpell();
            }
            else if (cardType == CardScriptableObject.Type.Creature)
            {
                if (Input.GetMouseButtonDown(0) && justPressed == false && !BattleController.instance.battleEnded)
                {
                    if (Physics.Raycast(ray, out hit, 100f, placementMask) && BattleController.instance.CanPerformActions())
                    {
                        CardPlacePoint selectedPoint = hit.collider.GetComponent<CardPlacePoint>();

                        if (selectedPoint.activeCard == null && selectedPoint.isPlayerPoint)
                        {
                            if (BattleController.instance.playerMana >= manaCost)
                            {
                                // Successfully clicked on a valid placement point
                                PlaceCard(selectedPoint);
                            }
                            else
                            {
                                // Player doesn't have enough mana
                                ReturnToHand();

                                BattleUIController.instance.ShowWarning(WARNING_MANA);
                            }
                        }
                        else
                        {
                            // Selected placement point is full or isn't for the player
                            ReturnToHand();
                        }
                    }
                    else
                    {
                        // Player didn't click on any placement point or it's not their action turn
                        ReturnToHand();
                    }
                }
            }

            if (Input.GetMouseButtonDown(1) && !BattleController.instance.battleEnded)
            {
                ReturnToHand();
            }
        }

        if (justPressed)
            justPressed = false;
    }

    public void SetupCard()
    {
        cardType = cardSO.cardType;

        switch (cardType)
        {
            case CardScriptableObject.Type.Creature:
                health = cardSO.health;
                attack = cardSO.attack;
                break;
            case CardScriptableObject.Type.Spell:
                health = -1;
                attack = -1;
                break;
        }

        manaCost = cardSO.manaCost;

        UpdateCardDisplay();

        nameText.text = cardSO.cardName;
        descriptionText.text = cardSO.description;
    }

    public void UpdateCardDisplay()
    {
        switch (cardType)
        {
            case CardScriptableObject.Type.Creature:
                healthText.text = health.ToString();
                attackText.text = attack.ToString();
                cardTypeText.text = "Creature";
                break;
            case CardScriptableObject.Type.Spell:
                healthText.text = "";
                attackText.text = "";
                cardTypeText.text = "Spell";
                break;
        }

        costText.text = manaCost.ToString();
    }

    private void PlaceCard(CardPlacePoint placementPoint)
    {
        placementPoint.activeCard = this;
        assignedPlace = placementPoint;

        MoveToPoint(placementPoint.transform.position, Quaternion.identity);

        inHand = false;
        isSelected = false;

        controller.RemoveCardFromHand(this);

        BattleController.instance.SpendPlayerMana(manaCost);

        SpawnCreature();
    }

    public void SpawnCreature()
    {
        StartCoroutine(SpawnCreatureCoroutine());
    }

    IEnumerator SpawnCreatureCoroutine()
    {
        yield return new WaitForSeconds(0.2f);

        cardMesh.SetActive(false);
        creatureObj = Instantiate(cardSO.creatureModel, creatureSpawnPoint.position, creatureSpawnPoint.rotation);
        creatureObj.transform.parent = creatureSpawnPoint;
        creatureObj.transform.localPosition = Vector3.zero;
        anim = creatureObj.GetComponent<Animator>();
    }

    private void PlaySpell()
    {
        inHand = false;
        isSelected = false;

        controller.RemoveCardFromHand(this);

        BattleController.instance.SpendPlayerMana(manaCost);

        Destroy(gameObject);
    }

    public void DamageCard(int amount)
    {
        if (cardType == CardScriptableObject.Type.Spell)
            return;

        health -= amount;
        if (health <= 0)
        {
            health = 0;

            assignedPlace.activeCard = null;

            //MoveToPoint(BattleController.instance.discardPoint.position, BattleController.instance.discardPoint.rotation);

            SetAnimTrigger("Die");

            Destroy(gameObject, 1f);
        }

        SetAnimTrigger("Hurt");
        UpdateCardDisplay();
    }

    public void MoveToPoint(Vector3 newPos, Quaternion newRot)
    {
        targetPos = newPos;
        targetRot = newRot;
    }

    public void AddCardToHand(int newHandPosition)
    {
        inHand = true;
        handPosition = newHandPosition;
    }

    private void OnMouseOver()
    {
        if (inHand && isPlayer && !BattleController.instance.battleEnded)
        {
            MoveToPoint(controller.GetCardPositionX(handPosition) + new Vector3(0f, 0.2f, 0f), Quaternion.Euler(0, 0, 0));
        }
    }

    private void OnMouseExit()
    {
        if (inHand && isPlayer && !BattleController.instance.battleEnded)
        {
            MoveToPoint(controller.cardPositions[handPosition], controller.cardRotations[handPosition]);
        }
    }

    private void OnMouseDown()
    {
        if (inHand && isPlayer && !BattleController.instance.battleEnded && BattleController.instance.CanPerformActions())
        {
            justPressed = true;
            isSelected = true;
            col.enabled = false;
        }
    }
    
    public void ReturnToHand()
    {
        isSelected = false;
        col.enabled = true;

        MoveToPoint(controller.cardPositions[handPosition], controller.cardRotations[handPosition]);
    }

    public void SetAnimTrigger(string triggerName)
    {
        if (anim == null)
            return;

        anim.SetTrigger(triggerName);
    }
}
