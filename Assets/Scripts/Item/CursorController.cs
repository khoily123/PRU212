using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    public Sprite cursorAxeSprite;  // Assign the sprite in the inspector
    public Sprite cursorDefaultSprite;  // Assign the sprite in the inspector
    public Texture2D cursorAxe;
    public Texture2D cursorDefault;

    // Start is called before the first frame update
    void Start()
    {
        cursorAxe = SpriteToTexture(cursorAxeSprite);
        cursorDefault = SpriteToTexture(cursorDefaultSprite);
        SetDefaultCursor();
    }

    public void SetAxeCursor()
    {
        Cursor.SetCursor(cursorAxe, Vector2.zero, CursorMode.Auto);
    }

    // Call this method to reset the cursor to default
    public void SetDefaultCursor()
    {
        Cursor.SetCursor(cursorDefault, Vector2.zero, CursorMode.Auto);
    }

    Texture2D SpriteToTexture(Sprite sprite)
    {
        if (sprite.rect.width != sprite.texture.width || sprite.rect.height != sprite.texture.height)
        {
            Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
            Color[] newColors = sprite.texture.GetPixels((int)sprite.rect.x,
                                                         (int)sprite.rect.y,
                                                         (int)sprite.rect.width,
                                                         (int)sprite.rect.height);
            newText.SetPixels(newColors);
            newText.Apply();
            return newText;
        }
        else
            return sprite.texture;
    }
}
