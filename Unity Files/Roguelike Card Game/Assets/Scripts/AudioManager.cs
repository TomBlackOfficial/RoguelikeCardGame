using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class Sound
{
    public string name = "New Sound";
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume = 0.5f;
    [Range(-3f, 3f)]
    public float pitch = 1;
    public bool randomize = false;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public List<Sound> sounds = new List<Sound>();

    void Reset()
    {
        sounds = new List<Sound>()
        {
           new Sound()
        };
    }

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);

        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void PlaySound(string soundName)
    {
        Sound newSound = sounds.FirstOrDefault(item => item.name == soundName);
        AudioSource source = Instantiate(new GameObject($"AudioSource_{newSound.name}"), Vector3.zero, Quaternion.identity).AddComponent<AudioSource>();
        source.transform.parent = transform;

        source.playOnAwake = true;
        source.clip = newSound.clip;
        source.volume = newSound.volume;

        if (newSound.randomize)
            source.pitch = Mathf.Clamp(Random.Range(newSound.pitch - 0.2f, newSound.pitch + 0.2f), -3, 3);
        else
            source.pitch = Mathf.Clamp(newSound.pitch, -3, 3);
    }
}
