using System.Collections.Generic;
using UnityEngine;

public class CreatureAudioManager : MonoBehaviour
{
    public static CreatureAudioManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayCreatureSound(CardScriptableObject cardSO, string state)
    {
        if (cardSO == null || cardSO.cardType != CardScriptableObject.Type.Creature)
            return;

        AudioClip clipToPlay = null;
        switch (state)
        {
            case "Spawn":
                clipToPlay = cardSO.SpawnClip;
                break;
            case "Attack":
                clipToPlay = cardSO.AttackClip;
                break;
            case "Damage":
                clipToPlay = cardSO.TakingDamageClip;
                break;
            case "Death":
                clipToPlay = cardSO.DeathClip;
                break;
        }

        if (clipToPlay != null)
        {
            AudioSource source = Instantiate(new GameObject($"AudioSource_{state}_{cardSO.cardName}"), Vector3.zero, Quaternion.identity).AddComponent<AudioSource>();
            source.transform.parent = transform;

            source.playOnAwake = true;
            source.clip = clipToPlay;
            source.volume = 1.0f;  // Adjust volume as needed
            source.Play();

            Destroy(source.gameObject, clipToPlay.length);
        }
    }
}