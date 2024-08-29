using UnityEngine;

[CreateAssetMenu(fileName = "New Sound", menuName = "Audio/Sound")]
public class Sound : ScriptableObject
{
    public string soundName;
    public AudioClip clip;
    public bool loop;
    [Range(0f, 1f)] public float volume = 1f;
}
