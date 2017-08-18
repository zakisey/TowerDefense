using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberSprite : MonoBehaviour
{
    public Texture2D[] textures;
    public int Number
    {
        set
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = Sprite.Create(textures[value], new Rect(0, 0, 64, 64), Vector2.zero);
        }
    }
}
