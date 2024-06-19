using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Card : MonoBehaviour
{
    public static string WARNING_MANA = "Not enough Mana to perform this action.";
    public static string WARNING_OCCUPIED = "This land is occupied by another creature.";

    public CardScriptableObject cardSO;

    private CreatureAudioManager creatureAudioManager;

    public int health { private set; get; }
    public int maxHealth { private set; get; }
    public int attack { private set; get; }
    public int manaCost { private set; get; }
    public int originalManaCost { private set; get; }
    public CardScriptableObject.Type cardType { private set; get; }

    [Header("Settings")]
    public bool isPlayer;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 3f;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask placementMask;

    [Header("Materials")]
    [SerializeField] private Material creatureCardMat;
    [SerializeField] private Material spellCardMat;

    [Header("Assign")]
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private GameObject healthIcon;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private GameObject attackIcon;
    [SerializeField] private TMP_Text attackText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private TMP_Text cardTypeText;
    [SerializeField] private Transform creatureSpawnPoint;
    [SerializeField] private GameObject cardMesh;
    [SerializeField] private GameObject cardMeshCreature;
    [SerializeField] private GameObject cardMeshSpell;
    [SerializeField] private GameObject creatureCanvas;
    [SerializeField] private TMP_Text creatureAttackText;
    [SerializeField] private TMP_Text creatureHealthText;
    [SerializeField] private TMP_Text creatureMaxHealthText;
    [SerializeField] private Slider creatureHealthSlider;
    [SerializeField] private MeshRenderer cardModel;
    [SerializeField] private MeshRenderer cardModelSpell;
    
    [SerializeField] private Transform modelOnCardTransform;
    private GameObject creatureObj2;

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
        creatureAudioManager = CreatureAudioManager.instance;
        controller = HandController.instance;
        col = GetComponent<Collider>();

        if (targetPos == Vector3.zero)
        {
            targetPos = transform.position;
            targetRot = transform.rotation;
        }

        if (creatureCanvas.activeInHierarchy)
            creatureCanvas.SetActive(false);

        SetupCard();
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);

        if (isSelected)
        {
            if (!BattleController.instance.CanPerformActions())
            {
                ReturnToHand();
                isSelected = false;
            }
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100f, groundMask))
                {
                    MoveToPoint(hit.point + new Vector3(0f, 0.05f, 0f), Quaternion.identity);
                }

                if (Input.GetMouseButtonDown(0) && justPressed == false && !BattleController.instance.battleEnded)
                {
                    if (Physics.Raycast(ray, out hit, 100f, placementMask) && BattleController.instance.CanPerformActions())
                    {
                        CardPlacePoint selectedPoint = hit.collider.GetComponent<CardPlacePoint>();

                        if (BattleController.instance.playerMana >= manaCost)
                        {
                            if (cardType == CardScriptableObject.Type.Spell)
                            {
                                if (PlaySpell(selectedPoint))
                                {
                                    Destroy(gameObject);
                                }
                                else
                                {
                                    ReturnToHand();
                                }
                            }
                            else
                            {
                                if (selectedPoint.activeCard == null && selectedPoint.isPlayerPoint)
                                {
                                    PlaceCard(selectedPoint);
                                }
                                else
                                {
                                    ReturnToHand();
                                    BattleUIController.instance.ShowWarning(WARNING_OCCUPIED);
                                }
                            }
                        }
                        else
                        {
                            ReturnToHand();
                            BattleUIController.instance.ShowWarning(WARNING_MANA);
                        }
                    }
                    else
                    {
                        // Player didn't click on any placement point or it's not their action turn
                        ReturnToHand();
                    }
                }

                if (Input.GetMouseButtonDown(1) && !BattleController.instance.battleEnded)
                {
                    ReturnToHand();
                }
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
                maxHealth = cardSO.health;
                attack = cardSO.attack;
                break;
            case CardScriptableObject.Type.Spell:
                health = -1;
                attack = -1;
                break;
        }

        manaCost = cardSO.manaCost;
        originalManaCost = cardSO.manaCost;

        UpdateCardDisplay();

        nameText.text = cardSO.cardName;
        descriptionText.text = cardSO.description;
    }
    
    public void CreatureSpawnOnCard()
    {
        creatureObj2 = Instantiate(cardSO.creatureOnCardModel, modelOnCardTransform.transform.position, modelOnCardTransform.transform.rotation);
        creatureObj2.transform.parent = modelOnCardTransform.transform;
        creatureObj2.transform.localPosition = Vector3.zero;
        creatureObj2.transform.localScale = modelOnCardTransform.transform.localScale;
        if (modelOnCardTransform.transform.childCount > 0)
        {
            modelOnCardTransform.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void UpdateCardDisplay()
    {
        Material[] newMats;

        switch (cardType)
        {
                    
            case CardScriptableObject.Type.Creature:
                newMats = cardModel.materials;
                cardMeshCreature.SetActive(true);
                cardMeshSpell.SetActive(false);
                healthIcon.SetActive(true);
                healthText.text = health.ToString();
                attackIcon.SetActive(true);
                attackText.text = attack.ToString();
                cardTypeText.text = "Creature";
                newMats[1] = creatureCardMat;
                CreatureSpawnOnCard();
                cardModel.materials = newMats;
                break;
            case CardScriptableObject.Type.Spell:
                 newMats = cardModelSpell.materials;
                cardMeshCreature.SetActive(false);
                cardMeshSpell.SetActive(true);
                healthIcon.SetActive(false);
                healthText.text = "";
                attackIcon.SetActive(false);
                attackText.text = "";
                cardTypeText.text = "Spell";
                newMats[1] = spellCardMat;
                cardModelSpell.materials = newMats;
                break;
        }

        

        costText.text = manaCost.ToString();
    }

    public void UpdateCreatureCanvas()
    {
        creatureAttackText.text = attack.ToString();
        creatureHealthText.text = health.ToString();
        creatureMaxHealthText.text = maxHealth.ToString();
        creatureHealthSlider.value = ((float)health / (float)maxHealth);

        if (!creatureCanvas.activeInHierarchy)
        {
            creatureCanvas.SetActive(true);
        }
    }

    private void PlaceCard(CardPlacePoint placementPoint)
    {
        BattleController.instance.cardSelected = false;

        placementPoint.activeCard = this;
        assignedPlace = placementPoint;

        MoveToPoint(placementPoint.transform.position, Quaternion.identity);

        inHand = false;
        isSelected = false;

        controller.RemoveCardFromHand(this);

        BattleController.instance.SpendPlayerMana(manaCost);

        SpawnCreature();
    }

    private bool PlaySpell(CardPlacePoint placementPoint)
    {
        foreach (Effect effect in cardSO.effects)
        {
            if (effect.OnUse(true, placementPoint, this))
            {

            }
            else
            {
                return false;
            }
        }

        BattleController.instance.cardSelected = false;

        inHand = false;
        isSelected = false;

        controller.RemoveCardFromHand(this);

        BattleController.instance.SpendPlayerMana(manaCost);

        return true;
    }

    public void SpawnCreature()
    {
        StartCoroutine(SpawnCreatureCoroutine());
    }

    IEnumerator SpawnCreatureCoroutine()
    {
        // yield return new WaitForSeconds(0.2f);
        //
        // cardMesh.SetActive(false);
        // creatureObj = Instantiate(cardSO.creatureModel, creatureSpawnPoint.position, creatureSpawnPoint.rotation);
        // Renderer creatureRenderer = creatureObj.GetComponentInChildren<Renderer>();
        // Material creatureMaterial = creatureRenderer.sharedMaterial;
        // Texture2D creatureTexture2D = creatureMaterial.mainTexture as Texture2D;
        // HologramDissolve.mainTexture = creatureTexture2D;
        // creatureObj.GetComponentInChildren<Renderer>().material = HologramDissolve;
        // creatureObj.transform.parent = creatureSpawnPoint;
        // creatureObj.transform.localPosition = Vector3.zero;
        // anim = creatureObj.GetComponent<Animator>();
        //
        // UpdateCreatureCanvas();
        yield return new WaitForSeconds(0.2f);
        creatureAudioManager.PlayCreatureSound(cardSO, "Spawn");

        cardMesh.SetActive(false);
        creatureObj = Instantiate(cardSO.creatureModel, creatureSpawnPoint.position, creatureSpawnPoint.rotation);
        Renderer creatureRenderer = creatureObj.GetComponentInChildren<Renderer>();
        Material creatureMaterial = creatureRenderer.sharedMaterial;
        Texture2D creatureTexture2D = creatureMaterial.mainTexture as Texture2D;
        //creatureMaterial.SetFloat("_CuttOffHeight", 0.2f);

        creatureObj.transform.parent = creatureSpawnPoint;
        creatureObj.transform.localPosition = Vector3.zero;
        anim = creatureObj.GetComponent<Animator>();

        float startValue = -0.35f;
        float endValue = 1.0f;
        float duration = 3.0f;
        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            float lerpValue = Mathf.Lerp(startValue, endValue, t);
            creatureMaterial.SetFloat("_CuttOffHeight", lerpValue);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        creatureMaterial.SetFloat("_CuttOffHeight", endValue);

        UpdateCreatureCanvas();
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
            creatureAudioManager.PlayCreatureSound(cardSO, "Death");
            SetAnimTrigger("Die");

            Destroy(gameObject, 1f);
        }
        
        SetAnimTrigger("Hurt");
        creatureAudioManager.PlayCreatureSound(cardSO, "Damage");
        
        
        UpdateCreatureCanvas();
    }

    public void HealCard(int amount)
    {
        if (cardType == CardScriptableObject.Type.Spell)
            return;

        health = Mathf.Clamp(health + amount, 0, maxHealth);

        UpdateCreatureCanvas();
    }

    public void BuffCard(int amountAttack, int amountHealth)
    {
        if (cardType == CardScriptableObject.Type.Spell)
            return;

        attack += amountAttack;
        maxHealth += amountHealth;
        health = Mathf.Clamp(health + amountHealth, 0, maxHealth);

        UpdateCreatureCanvas();
    }

    public void DebuffCard(int amountAttack, int amountHealth)
    {
        BuffCard(-amountAttack, -amountHealth);
    }

    public void ReduceManaCost(int amount)
    {
        manaCost -= amount;
    }

    public void ResetManaCost()
    {
        manaCost = originalManaCost;
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
        if (inHand && isPlayer && !BattleController.instance.battleEnded && BattleController.instance.CanPerformActions() && !BattleController.instance.cardSelected)
        {
            MoveToPoint(controller.GetCardPositionX(handPosition) + new Vector3(0f, 0.2f, 0f), Quaternion.Euler(0, 0, 0));
        }
    }

    private void OnMouseExit()
    {
        if (inHand && isPlayer && !BattleController.instance.battleEnded && !BattleController.instance.cardSelected)
        {
            MoveToPoint(controller.cardPositions[handPosition], controller.cardRotations[handPosition]);
        }
    }

    private void OnMouseDown()
    {
        if (inHand && isPlayer && !BattleController.instance.battleEnded && BattleController.instance.CanPerformActions() && !BattleController.instance.cardSelected)
        {
            justPressed = true;
            isSelected = true;
            col.enabled = false;
            BattleController.instance.cardSelected = true;
        }
    }
    
    public void ReturnToHand()
    {
        BattleController.instance.cardSelected = false;
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
