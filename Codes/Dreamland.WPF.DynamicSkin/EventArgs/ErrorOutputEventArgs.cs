using System;

namespace Dreamland.WPF.DynamicSkin;

/// <summary>
///     错误输出事件参数 / Error output event arguments
/// </summary>
public class ErrorOutputEventArgs : EventArgs
{
    /// <summary>
    ///     初始化 <see cref="ErrorOutputEventArgs" /> 类的新实例
    ///     Initializes a new instance of the <see cref="ErrorOutputEventArgs" /> class
    /// </summary>
    /// <param name="data">错误数据 / Error data</param>
    public ErrorOutputEventArgs(string data)
    {
        Data = data ?? string.Empty;
        Timestamp = DateTime.Now;
    }

    /// <summary>
    ///     错误数据 / Error data
    /// </summary>
    public string Data { get; }

    /// <summary>
    ///     错误发生时间戳 / Error timestamp
    /// </summary>
    public DateTime Timestamp { get; }

    /// <summary>
    ///     返回错误信息的字符串表示 / Returns a string representation of the error information
    /// </summary>
    public override string ToString()
    {
        return $"[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}] {Data}";
    }
}