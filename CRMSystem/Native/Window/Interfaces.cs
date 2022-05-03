using System;

namespace CRMSystem.Native.Window
{
    interface INativelyRestorableWindow
    {
        public bool DuringRestoreToMaximized { get; }
    }
}
