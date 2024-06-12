using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CardPointsController : MonoBehaviour
{
    public static CardPointsController instance;

    public CardPlacePoint[] playerCardPoints, enemyCardPoints;
    public float timeBetweenAttacks = 1f;

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
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < playerCardPoints.Length; i++)
        {
            if (playerCardPoints[i].activeCard != null)
            {
                playerCardPoints[i].BattleCamSwitch();

                yield return new WaitForSeconds(timeBetweenAttacks);

                playerCardPoints[i].activeCard.SetAnimTrigger("Attack");

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
            }

            yield return new WaitForSeconds(1);

            playerCardPoints[i].CameraOff();
        }

        CheckAssignedCards();

        yield return new WaitForSeconds(timeBetweenAttacks);

        BattleController.instance.AdvanceTurn();
    }

    public void EnemyAttack()
    {
        StartCoroutine(EnemyAttackCoroutine());
    }

    IEnumerator EnemyAttackCoroutine()
    {
        for (int i = 0; i < enemyCardPoints.Length; i++)
        {
            if (enemyCardPoints[i].activeCard != null)
            {
                enemyCardPoints[i].BattleCamSwitch();

                yield return new WaitForSeconds(timeBetweenAttacks);

                enemyCardPoints[i].activeCard.SetAnimTrigger("Attack");

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
            }

            if (BattleController.instance.battleEnded)
            {
                i = enemyCardPoints.Length;
            }

            yield return new WaitForSeconds(1);
            enemyCardPoints[i].CameraOff();
        }

        CheckAssignedCards();

        yield return new WaitForSeconds(timeBetweenAttacks);

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

    public int PlayerCreatureCount()
    {
        int count = 0;

        foreach(CardPlacePoint point in playerCardPoints)
        {
            if (point.activeCard != null)
            {
                count++;
            }
        }

        return count;
    }

    public int EnemyCreatureCount()
    {
        int count = 0;

        foreach (CardPlacePoint point in enemyCardPoints)
        {
            if (point.activeCard != null)
            {
                count++;
            }
        }

        return count;
    }
}
