using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour
{
    public static TreeManager Instance;
    private bool isSelect = false;
    public float cooldown = 10f; // Khoảng thời gian hồi là 10 giây
    private float lastChopTime = -10f;// Đặt thời gian cuối cùng là 10 giây trước khi bắt đầu
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void EnableTreeSelection(bool enable)
    {
        if (enable)
        {
            // Kiểm tra xem thời gian hồi đã qua chưa
            if (Time.time - lastChopTime >= cooldown)
            {
                isSelect = true;
                lastChopTime = Time.time;
            }
            else
            {
                HighLightManager.Instance.HighlightTrees(false);
                Debug.Log("Chặt cây đang trong thời gian hồi. Vui lòng đợi.");
            }
        }
        else
        {
            isSelect = false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isSelect && Input.GetMouseButtonDown(0)) // Left mouse button
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject.GetComponent<Tree>() != null)
            {
                hit.collider.gameObject.GetComponent<Tree>().StartDestructionTimer();
                EnableTreeSelection(false); // Stop selection after choosing a tree
                HighLightManager.Instance.HighlightTrees(false);
            }
            else
            {
                EnableTreeSelection(false);
                HighLightManager.Instance.HighlightTrees(false);

            }
        }
    }

    public void OnChopTreeButtonClicked()
    {
        HighLightManager.Instance.HighlightTrees(true);
        TreeManager.Instance.EnableTreeSelection(true);
    }
}
