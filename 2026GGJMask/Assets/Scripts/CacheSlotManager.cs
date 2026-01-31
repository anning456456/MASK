using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 缓存槽管理器
/// </summary>
public class CacheSlotManager : MonoBehaviour
{
    [Header("缓存槽UI容器")]
    public Transform cacheSlotContainer;

    [Header("缓存槽预制体")]
    public GameObject cacheSlotPrefab;

    private List<CacheSlot> cacheSlots;
    private List<MaterialData> cachedMaterials;
    private int maxSize;

    /// <summary>
    /// 初始化缓存槽
    /// </summary>
    public void Initialize(int size)
    {
        maxSize = size;
        cachedMaterials = new List<MaterialData>();
        cacheSlots = new List<CacheSlot>();

        if (cacheSlotContainer == null)
        {
            Debug.LogError("缓存槽容器未设置！");
            return;
        }

        // 创建缓存槽UI
        CreateCacheSlots();
    }

    /// <summary>
    /// 创建缓存槽UI
    /// </summary>
    private void CreateCacheSlots()
    {
        // 清除现有槽位
        foreach (Transform child in cacheSlotContainer)
        {
            Destroy(child.gameObject);
        }
        cacheSlots.Clear();

        // 创建新槽位
        for (int i = 0; i < maxSize; i++)
        {
            GameObject slotObj;
            if (cacheSlotPrefab != null)
            {
                slotObj = Instantiate(cacheSlotPrefab, cacheSlotContainer);
            }
            else
            {
                slotObj = new GameObject($"CacheSlot_{i}");
                slotObj.transform.SetParent(cacheSlotContainer);
                slotObj.AddComponent<RectTransform>();
                slotObj.AddComponent<Image>();
            }

            CacheSlot slot = slotObj.GetComponent<CacheSlot>();
            if (slot == null)
            {
                slot = slotObj.AddComponent<CacheSlot>();
            }

            slot.Initialize(i);
            cacheSlots.Add(slot);
        }
    }

    /// <summary>
    /// 添加素材到缓存槽
    /// </summary>
    public bool AddMaterial(MaterialData material)
    {
        if (cachedMaterials.Count >= maxSize)
        {
            return false; // 缓存槽已满
        }

        cachedMaterials.Add(material);
        UpdateCacheSlots();
        return true;
    }

    /// <summary>
    /// 从缓存槽移除素材
    /// </summary>
    public bool RemoveMaterial(MaterialData material)
    {
        if (cachedMaterials.Remove(material))
        {
            UpdateCacheSlots();
            return true;
        }
        return false;
    }

    /// <summary>
    /// 更新缓存槽显示
    /// </summary>
    private void UpdateCacheSlots()
    {
        for (int i = 0; i < cacheSlots.Count; i++)
        {
            if (i < cachedMaterials.Count)
            {
                cacheSlots[i].SetMaterial(cachedMaterials[i]);
            }
            else
            {
                cacheSlots[i].ClearMaterial();
            }
        }
    }

    /// <summary>
    /// 重置缓存槽
    /// </summary>
    public void Reset()
    {
        cachedMaterials.Clear();
        UpdateCacheSlots();
    }

    /// <summary>
    /// 设置缓存槽大小
    /// </summary>
    public void SetSize(int size)
    {
        maxSize = Mathf.Clamp(size, 3, 10);
        
        // 如果新大小小于当前素材数量，移除多余的素材
        while (cachedMaterials.Count > maxSize)
        {
            cachedMaterials.RemoveAt(cachedMaterials.Count - 1);
        }

        CreateCacheSlots();
        UpdateCacheSlots();
    }

    /// <summary>
    /// 获取缓存槽中的素材列表
    /// </summary>
    public List<MaterialData> GetCachedMaterials()
    {
        return new List<MaterialData>(cachedMaterials);
    }

    /// <summary>
    /// 检查缓存槽是否已满
    /// </summary>
    public bool IsFull()
    {
        return cachedMaterials.Count >= maxSize;
    }
}

