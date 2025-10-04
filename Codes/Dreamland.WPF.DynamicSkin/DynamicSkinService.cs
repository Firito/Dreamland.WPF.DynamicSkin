using System;
using System.Collections.Concurrent;

namespace Dreamland.WPF.DynamicSkin;

/// <summary>
///     换肤服务 / Dynamic Skin Service
/// </summary>
public static class DynamicSkinService
{
    static DynamicSkinService()
    {
        DynamicSkinModel = new DynamicSkinModel();
        ResourceDictionary = new ConcurrentDictionary<string, object>(StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    ///     动态皮肤模块 / Dynamic skin model
    /// </summary>
    internal static DynamicSkinModel DynamicSkinModel { get; }

    /// <summary>
    ///     当前皮肤名称 / Current skin name
    /// </summary>
    public static string? CurrentSkinName
    {
        get => DynamicSkinModel.CurrentSkinName;
        set => DynamicSkinModel.CurrentSkinName = value;
    }

    /// <summary>
    ///     资源字典（线程安全） / Resource dictionary (thread-safe)
    /// </summary>
    internal static ConcurrentDictionary<string, object> ResourceDictionary { get; }

    /// <summary>
    ///     错误输出事件 / Error output event
    /// </summary>
    public static event EventHandler<ErrorOutputEventArgs>? ErrorOutput;

    /// <summary>
    ///     触发错误输出事件 / Raise error output event
    /// </summary>
    /// <param name="errorMessage">错误消息 / Error message</param>
    internal static void RaiseErrorOutput(string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(errorMessage))
            return;

        ErrorOutput?.Invoke(null, new ErrorOutputEventArgs(errorMessage));
    }

    /// <summary>
    ///     清除资源字典缓存 / Clear resource cache
    /// </summary>
    public static void ClearResourceCache()
    {
        ResourceDictionary.Clear();
    }

    /// <summary>
    ///     添加或更新资源到缓存 / Add or update resource to cache
    /// </summary>
    /// <param name="key">资源键 / Resource key</param>
    /// <param name="resource">资源对象 / Resource object</param>
    /// <returns>是否成功添加或更新 / Whether the operation was successful</returns>
    public static bool AddOrUpdateResource(string key, object? resource)
    {
        if (string.IsNullOrWhiteSpace(key) || resource == null)
            return false;

        ResourceDictionary[key] = resource;
        return true;
    }

    /// <summary>
    ///     尝试获取资源 / Try to get resource
    /// </summary>
    /// <param name="key">资源键 / Resource key</param>
    /// <param name="resource">资源对象 / Resource object</param>
    /// <returns>是否找到资源 / Whether the resource was found</returns>
    public static bool TryGetResource(string key, out object? resource)
    {
        resource = null;
        if (string.IsNullOrWhiteSpace(key))
            return false;

        return ResourceDictionary.TryGetValue(key, out resource);
    }
}