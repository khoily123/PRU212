using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighLightManager : MonoBehaviour
{
    public static HighLightManager Instance;
    private Color highlightColor = Color.red; // Màu sắc để làm nổi bật
    private Color originalColor; // Màu sắc gốc của Sprite
    // Start is called before the first frame update

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    public void HighlightTrees(bool highlight)
    {
        GameObject[] trees = GameObject.FindGameObjectsWithTag("Tree");
        foreach (GameObject tree in trees)
        {
            SpriteRenderer spriteRenderer = tree.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                if (highlight)
                {
                    // Lưu lại màu gốc và đổi sang màu làm nổi bật
                    originalColor = spriteRenderer.color;
                    spriteRenderer.color = highlightColor;
                }
                else
                {
                    // Phục hồi màu gốc
                    spriteRenderer.color = originalColor;
                }
            }
        }
    }

        public void HighlightStones(bool highlight)
    {
        GameObject[] stones = GameObject.FindGameObjectsWithTag("Stone");
        foreach (GameObject stone in stones)
        {
            SpriteRenderer spriteRenderer = stone.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                if (highlight)
                {
                    // Lưu lại màu gốc và đổi sang màu làm nổi bật
                    originalColor = spriteRenderer.color;
                    spriteRenderer.color = highlightColor;
                }
                else
                {
                    // Phục hồi màu gốc
                    spriteRenderer.color = originalColor;
                }
            }
        }
    }
}
