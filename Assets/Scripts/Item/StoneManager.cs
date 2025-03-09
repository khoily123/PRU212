using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneManager : MonoBehaviour
{
    public static StoneManager Instance;
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

    public void EnableStoneSelection(bool enable)
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
                HighLightManager.Instance.HighlightStones(false);
                Debug.Log("phá đá đang trong thời gian hồi. Vui lòng đợi.");
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
            if (hit.collider != null && hit.collider.gameObject.GetComponent<Stone>() != null)
            {
                hit.collider.gameObject.GetComponent<Stone>().StartDestructionTimer();
                EnableStoneSelection(false); // Stop selection after choosing a tree
                HighLightManager.Instance.HighlightStones(false);
            }
            else
            {
                EnableStoneSelection(false);
                HighLightManager.Instance.HighlightStones(false);
            }
        }
    }

    public void OnChopStoneButtonClicked()
    {
        HighLightManager.Instance.HighlightStones(true);
        StoneManager.Instance.EnableStoneSelection(true);
    }
}
