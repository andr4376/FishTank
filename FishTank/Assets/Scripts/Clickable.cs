using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clickable : MonoBehaviour
{
    public AudioClip clickSound;
    public float volume;
    

    private void OnMouseDown()
    {
        OnClick();

        
    }

    protected virtual void OnClick()
    {
        if (clickSound != null)
        {
            SoundManager.PlayAudio(clickSound, volume);
        }
    }
}
