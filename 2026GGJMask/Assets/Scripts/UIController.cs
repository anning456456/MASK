using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI控制器
/// </summary>
public class UIController : MonoBehaviour
{
    [Header("当前素材显示")]
    public GameObject currentMaterialDisplay;
    public Image currentMaterialImage;
    public Text currentMaterialText;

    [Header("按钮")]
    public Button placeButton;
    public Button cacheButton;
    public Button discardButton;
    public Button submitButton;
    public Button newGameButton;

    [Header("信息显示")]
    public Text materialCountText;
    public Text cacheSizeText;
    public Text messageText;

    [Header("设置")]
    public InputField cacheSizeInput;

    private MaterialLibrary materialLibrary;
    private MaterialData currentMaterial;
    private DraggableMaterial draggableMaterial;

    private void Start()
    {
        materialLibrary = FindObjectOfType<MaterialLibrary>();
        
        // 绑定按钮事件
        if (placeButton != null)
            placeButton.onClick.AddListener(OnPlaceButtonClicked);
        
        if (cacheButton != null)
            cacheButton.onClick.AddListener(OnCacheButtonClicked);
        
        if (discardButton != null)
            discardButton.onClick.AddListener(OnDiscardButtonClicked);
        
        if (submitButton != null)
            submitButton.onClick.AddListener(OnSubmitButtonClicked);
        
        if (newGameButton != null)
            newGameButton.onClick.AddListener(OnNewGameButtonClicked);

        if (cacheSizeInput != null)
        {
            cacheSizeInput.onEndEdit.AddListener(OnCacheSizeChanged);
        }

        // 初始化当前素材显示
        if (currentMaterialDisplay != null)
        {
            currentMaterialDisplay.SetActive(false);
        }
    }

    /// <summary>
    /// 显示当前素材
    /// </summary>
    public void ShowCurrentMaterial(MaterialData material)
    {
        currentMaterial = material;

        if (currentMaterialDisplay != null)
        {
            currentMaterialDisplay.SetActive(material != null);
        }

        if (material != null && materialLibrary != null)
        {
            // 更新图像
            if (currentMaterialImage != null)
            {
                currentMaterialImage.color = materialLibrary.GetColor(material.color);
                currentMaterialImage.enabled = true;
            }

            // 更新文本
            if (currentMaterialText != null)
            {
                string shapeName = materialLibrary.GetShapeName(material.shape);
                string colorName = materialLibrary.GetColorName(material.color);
                currentMaterialText.text = $"{colorName}色 {shapeName}";
            }

            // 添加拖拽功能
            if (draggableMaterial == null)
            {
                draggableMaterial = currentMaterialDisplay.GetComponent<DraggableMaterial>();
                if (draggableMaterial == null)
                {
                    draggableMaterial = currentMaterialDisplay.AddComponent<DraggableMaterial>();
                }
            }
            draggableMaterial.SetMaterialData(material);
        }
        else
        {
            if (currentMaterialImage != null)
            {
                currentMaterialImage.enabled = false;
            }
            if (currentMaterialText != null)
            {
                currentMaterialText.text = "";
            }
        }
    }

    /// <summary>
    /// 更新素材数量显示
    /// </summary>
    public void UpdateMaterialCount(int count)
    {
        if (materialCountText != null)
        {
            materialCountText.text = $"剩余素材：{count}";
        }
    }

    /// <summary>
    /// 更新缓存槽大小显示
    /// </summary>
    public void UpdateCacheSize(int size)
    {
        if (cacheSizeText != null)
        {
            cacheSizeText.text = $"缓存槽大小：{size}";
        }
        if (cacheSizeInput != null)
        {
            cacheSizeInput.text = size.ToString();
        }
    }

    /// <summary>
    /// 启用/禁用提交按钮
    /// </summary>
    public void EnableSubmitButton(bool enable)
    {
        if (submitButton != null)
        {
            submitButton.interactable = enable;
        }
    }

    /// <summary>
    /// 显示消息
    /// </summary>
    public void ShowSubmitMessage(string message)
    {
        if (messageText != null)
        {
            messageText.text = message;
            messageText.gameObject.SetActive(true);
            
            // 3秒后隐藏消息
            Invoke(nameof(HideMessage), 3f);
        }
    }

    private void HideMessage()
    {
        if (messageText != null)
        {
            messageText.gameObject.SetActive(false);
        }
    }

    // 按钮事件处理
    private void OnPlaceButtonClicked()
    {
        // 放置功能通过拖拽实现，这里可以显示提示
        ShowMessage("请拖拽素材到面具槽相应位置");
    }

    private void OnCacheButtonClicked()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.CacheMaterial();
        }
    }

    private void OnDiscardButtonClicked()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.DiscardMaterial();
        }
    }

    private void OnSubmitButtonClicked()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SubmitMask();
        }
    }

    private void OnNewGameButtonClicked()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.NewGame();
        }
    }

    private void OnCacheSizeChanged(string value)
    {
        if (int.TryParse(value, out int size) && GameManager.Instance != null)
        {
            GameManager.Instance.SetCacheSize(size);
        }
    }

    private void ShowMessage(string message)
    {
        if (messageText != null)
        {
            messageText.text = message;
            messageText.gameObject.SetActive(true);
            Invoke(nameof(HideMessage), 2f);
        }
    }
}

