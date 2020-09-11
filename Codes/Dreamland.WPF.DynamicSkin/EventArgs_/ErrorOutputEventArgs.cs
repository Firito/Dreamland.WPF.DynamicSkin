using System;

namespace Dreamland.WPF.DynamicSkin
{
    public class ErrorOutputEventArgs : EventArgs
    {
        public ErrorOutputEventArgs(string data)
        {
            Data = data;
        }

        public string Data { get; }
    }
}