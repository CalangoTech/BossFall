using UnityEngine;

public class CharacterSoundPlayer : MonoBehaviour
{
    [Header("Audio Clips")]
    [Tooltip("Arraste o AudioClip para cada som aqui.")]
    public AudioClip sound1;
    public AudioClip sound2;
    public AudioClip sound3;

    [Header("Audio Settings")]
    [Tooltip("Componente AudioSource usado para tocar os sons.")]
    public AudioSource audioSource;

    public void PlaySound1()
    {
        PlaySound(sound1);
    }


    public void PlaySound2()
    {
        PlaySound(sound2);
    }


    public void PlaySound3()
    {
        PlaySound(sound3);
    }


    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
