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

    [SerializeField] private TMP_Text nameText, descriptionText, healthText, attackText, costText;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 3f;

    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask placementMask;

    public bool inHand {  private set; get; }
    public int handPosition { private set; get; }
    public CardPlacePoint assignedPlace { private set; get; }

    private HandController controller;
    private Collider col;

    private Vector3 targetPos;
    private Quaternion targetRot;

    private bool isSelected;
    private bool justPressed;

    private void Start()
    {
        controller = HandController.instance;
        col = GetComponent<Collider>();

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

            if (Input.GetMouseButtonDown(1))
            {
                ReturnToHand();
            }
            else if (Input.GetMouseButtonDown(0) && justPressed == false)
            {
                if (Physics.Raycast(ray, out hit, 100f, placementMask))
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
                    // Player didn't click on any placement point
                    ReturnToHand();
                }
            }
        }

        if (justPressed)
            justPressed = false;
    }

    public void SetupCard()
    {
        health = cardSO.health;
        attack = cardSO.attack;
        manaCost = cardSO.manaCost;

        healthText.text = health.ToString();
        attackText.text = attack.ToString();
        costText.text = manaCost.ToString();

        nameText.text = cardSO.cardName;
        descriptionText.text = cardSO.description;
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
        if (inHand)
        {
            MoveToPoint(controller.GetCardPositionX(handPosition) + new Vector3(0f, 0.2f, 0f), Quaternion.Euler(0, 0, 0));
        }
    }

    private void OnMouseExit()
    {
        if (inHand)
        {
            MoveToPoint(controller.cardPositions[handPosition], controller.cardRotations[handPosition]);
        }
    }

    private void OnMouseDown()
    {
        if (inHand)
        {
            isSelected = true;
            col.enabled = false;
            justPressed = true;
        }
    }
    
    public void ReturnToHand()
    {
        isSelected = false;
        col.enabled = true;

        MoveToPoint(controller.cardPositions[handPosition], controller.cardRotations[handPosition]);
    }
}
