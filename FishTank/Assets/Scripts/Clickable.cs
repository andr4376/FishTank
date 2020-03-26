using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Clickable : MonoBehaviour
{
    public AudioClip clickSound;
    public float volume;
    

    private void OnMouseDown()
    {
        //If there's a ui panel on top of the clickable object
        if (EventSystem.current.IsPointerOverGameObject())
            return;

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
