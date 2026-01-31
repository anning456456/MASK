using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 可拖拽的素材组件
/// </summary>
public class DraggableMaterial : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private MaterialData materialData;
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 originalPosition;
    private Transform originalParent;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        // 查找Canvas
        canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            canvas = FindObjectOfType<Canvas>();
        }
    }

    /// <summary>
    /// 设置素材数据
    /// </summary>
    public void SetMaterialData(MaterialData data)
    {
        materialData = data;
    }

    /// <summary>
    /// 获取素材数据
    /// </summary>
    public MaterialData GetMaterialData()
    {
        return materialData;
    }

    /// <summary>
    /// 开始拖拽
    /// </summary>
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = rectTransform.anchoredPosition;
        originalParent = rectTransform.parent;
        
        // 设置为可穿透，以便检测下方的目标
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        
        // 移到最上层
        rectTransform.SetAsLastSibling();
    }

    /// <summary>
    /// 拖拽中
    /// </summary>
    public void OnDrag(PointerEventData eventData)
    {
        if (canvas != null)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
        else
        {
            rectTransform.position = eventData.position;
        }
    }

    /// <summary>
    /// 结束拖拽
    /// </summary>
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        // 检查是否放置到有效目标
        GameObject dropTarget = eventData.pointerCurrentRaycast.gameObject;
        if (dropTarget != null)
        {
            MaskSlot maskSlot = dropTarget.GetComponent<MaskSlot>();
            if (maskSlot == null)
            {
                // 检查父对象
                maskSlot = dropTarget.GetComponentInParent<MaskSlot>();
            }

            if (maskSlot != null && maskSlot.IsEmpty())
            {
                // 放置到面具槽
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.PlaceMaterial(maskSlot.GetPosition());
                }
                // 素材已被使用，可以销毁或隐藏
                gameObject.SetActive(false);
                return;
            }
        }

        // 如果没有放置到有效目标，返回原位置
        rectTransform.anchoredPosition = originalPosition;
        rectTransform.SetParent(originalParent);
    }
}

