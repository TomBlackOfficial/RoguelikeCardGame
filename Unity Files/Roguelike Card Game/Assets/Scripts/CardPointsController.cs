using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPointsController : MonoBehaviour
{
    public static CardPointsController instance;

    [SerializeField] private CardPlacePoint[] playerCardPoints, enemyCardPoints;
    [SerializeField] private float timeBetweenAttacks = 0.25f;

    private void Awake()
    {
        instance = this;
    }

    public void PlayerAttack()
    {
        StartCoroutine(PlayerAttackCoroutine());
    }

    IEnumerator PlayerAttackCoroutine()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);

        for (int i = 0; i < playerCardPoints.Length; i++)
        {
            if (playerCardPoints[i].activeCard != null)
            {
                if (enemyCardPoints[i].activeCard != null)
                {
                    // Attack the enemy card
                    enemyCardPoints[i].activeCard.DamageCard(playerCardPoints[i].activeCard.attack);
                }
                else
                {
                    // Attack the enemy hero
                    BattleController.instance.DamageEnemy(playerCardPoints[i].activeCard.attack);
                }

                playerCardPoints[i].activeCard.anim.SetTrigger("Attack");

                yield return new WaitForSeconds(timeBetweenAttacks);
            }
        }

        CheckAssignedCards();

        BattleController.instance.AdvanceTurn();
    }

    public void EnemyAttack()
    {
        StartCoroutine(EnemyAttackCoroutine());
    }

    IEnumerator EnemyAttackCoroutine()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);

        for (int i = 0; i < enemyCardPoints.Length; i++)
        {
            if (enemyCardPoints[i].activeCard != null)
            {
                if (playerCardPoints[i].activeCard != null)
                {
                    // Attack the player card
                    playerCardPoints[i].activeCard.DamageCard(enemyCardPoints[i].activeCard.attack);
                }
                else
                {
                    // Attack the player hero
                    BattleController.instance.DamagePlayer(enemyCardPoints[i].activeCard.attack);
                }

                enemyCardPoints[i].activeCard.anim.SetTrigger("Attack");

                yield return new WaitForSeconds(timeBetweenAttacks);
            }
        }

        CheckAssignedCards();

        BattleController.instance.AdvanceTurn();
    }

    public void CheckAssignedCards()
    {
        foreach (CardPlacePoint point in enemyCardPoints)
        {
            if (point.activeCard != null)
            {
                if (point.activeCard.health <= 0)
                {
                    point.activeCard = null;
                }
            }
        }

        foreach (CardPlacePoint point in playerCardPoints)
        {
            if (point.activeCard != null)
            {
                if (point.activeCard.health <= 0)
                {
                    point.activeCard = null;
                }
            }
        }
    }
}
