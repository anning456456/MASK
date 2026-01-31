using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("游戏设置")]
    public int cacheSlotSize = 5; // 缓存槽长度

    [Header("引用")]
    public MaterialLibrary materialLibrary;
    public MaskSlotManager maskSlotManager;
    public CacheSlotManager cacheSlotManager;
    public UIController uiController;

    private MaterialData currentMaterial; // 当前抽取的素材
    private List<MaterialData> materialPool; // 素材池

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializeGame();
    }

    /// <summary>
    /// 初始化游戏
    /// </summary>
    public void InitializeGame()
    {
        // 生成所有素材（42个）
        materialPool = GenerateAllMaterials();
        
        // 初始化各个管理器
        maskSlotManager.Initialize();
        cacheSlotManager.Initialize(cacheSlotSize);
        uiController.UpdateMaterialCount(materialPool.Count);
        uiController.UpdateCacheSize(cacheSlotSize);
        
        // 抽取第一个素材
        DrawNextMaterial();
    }

    /// <summary>
    /// 生成所有素材（3种形状 × 7种颜色 × 2张 = 42个）
    /// </summary>
    private List<MaterialData> GenerateAllMaterials()
    {
        List<MaterialData> materials = new List<MaterialData>();
        
        foreach (MaterialShape shape in System.Enum.GetValues(typeof(MaterialShape)))
        {
            foreach (MaterialColor color in System.Enum.GetValues(typeof(MaterialColor)))
            {
                // 每种组合生成2张
                materials.Add(new MaterialData(shape, color, 0));
                materials.Add(new MaterialData(shape, color, 1));
            }
        }
        
        return materials;
    }

    /// <summary>
    /// 从素材池中随机抽取一个素材
    /// </summary>
    public void DrawNextMaterial()
    {
        if (materialPool.Count == 0)
        {
            Debug.Log("素材库已空！");
            uiController.ShowCurrentMaterial(null);
            return;
        }

        int randomIndex = Random.Range(0, materialPool.Count);
        currentMaterial = materialPool[randomIndex];
        materialPool.RemoveAt(randomIndex);
        
        uiController.ShowCurrentMaterial(currentMaterial);
        uiController.UpdateMaterialCount(materialPool.Count);
    }

    /// <summary>
    /// 放置素材到面具槽
    /// </summary>
    public void PlaceMaterial(MaskPosition position)
    {
        if (currentMaterial == null)
        {
            Debug.LogWarning("没有当前素材可放置！");
            return;
        }

        if (maskSlotManager.PlaceMaterial(position, currentMaterial))
        {
            currentMaterial = null;
            uiController.ShowCurrentMaterial(null);
            CheckMaskComplete();
            DrawNextMaterial();
        }
        else
        {
            Debug.LogWarning($"无法放置到位置：{position}");
        }
    }

    /// <summary>
    /// 暂存当前素材到缓存槽
    /// </summary>
    public void CacheMaterial()
    {
        if (currentMaterial == null)
        {
            Debug.LogWarning("没有当前素材可暂存！");
            return;
        }

        if (cacheSlotManager.AddMaterial(currentMaterial))
        {
            currentMaterial = null;
            uiController.ShowCurrentMaterial(null);
            DrawNextMaterial();
        }
        else
        {
            Debug.LogWarning("缓存槽已满！");
        }
    }

    /// <summary>
    /// 丢弃当前素材
    /// </summary>
    public void DiscardMaterial()
    {
        if (currentMaterial == null)
        {
            Debug.LogWarning("没有当前素材可丢弃！");
            return;
        }

        currentMaterial = null;
        uiController.ShowCurrentMaterial(null);
        DrawNextMaterial();
    }

    /// <summary>
    /// 从缓存槽取回素材
    /// </summary>
    public void RetrieveFromCache(MaterialData material)
    {
        if (currentMaterial != null)
        {
            Debug.LogWarning("已有当前素材，请先处理！");
            return;
        }

        if (cacheSlotManager.RemoveMaterial(material))
        {
            currentMaterial = material;
            uiController.ShowCurrentMaterial(currentMaterial);
        }
    }

    /// <summary>
    /// 检查面具是否填充完成
    /// </summary>
    private void CheckMaskComplete()
    {
        if (maskSlotManager.IsMaskComplete())
        {
            uiController.EnableSubmitButton(true);
        }
    }

    /// <summary>
    /// 提交面具
    /// </summary>
    public void SubmitMask()
    {
        if (!maskSlotManager.IsMaskComplete())
        {
            Debug.LogWarning("面具未完成，无法提交！");
            return;
        }

        Debug.Log("面具提交成功！");
        uiController.ShowSubmitMessage("面具提交成功！");
        // 这里可以添加提交后的逻辑，比如评分、保存等
    }

    /// <summary>
    /// 开始新游戏
    /// </summary>
    public void NewGame()
    {
        // 重置所有状态
        materialPool = GenerateAllMaterials();
        maskSlotManager.Reset();
        cacheSlotManager.Reset();
        currentMaterial = null;
        
        // 更新UI
        uiController.ShowCurrentMaterial(null);
        uiController.UpdateMaterialCount(materialPool.Count);
        uiController.EnableSubmitButton(false);
        
        // 抽取第一个素材
        DrawNextMaterial();
    }

    /// <summary>
    /// 设置缓存槽大小
    /// </summary>
    public void SetCacheSize(int size)
    {
        cacheSlotSize = Mathf.Clamp(size, 3, 10);
        cacheSlotManager.SetSize(cacheSlotSize);
        uiController.UpdateCacheSize(cacheSlotSize);
    }

    /// <summary>
    /// 获取当前素材（用于拖拽）
    /// </summary>
    public MaterialData GetCurrentMaterial()
    {
        return currentMaterial;
    }
}

