using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelThemeClick : MonoBehaviour
{
    public void OnClick()
    {
        PixelationEffect p;
        p = Camera.main.GetComponent<PixelationEffect>();

        p.enabled = !p.enabled;
    }
}
