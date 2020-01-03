using System;

namespace Dreamland.WPF.DynamicSkin
{
    public class ErrorOutputEventArgs : EventArgs
    {
        public string Data { get; }

        public ErrorOutputEventArgs(string data)
        {
            Data = data;
        }
    }
}
