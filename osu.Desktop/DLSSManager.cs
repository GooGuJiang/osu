using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using osu.Framework.Logging;

namespace osu.Desktop
{
    [SupportedOSPlatform("windows")]
    internal static class DLSSManager
    {
        public static bool Available { get; }

        private static bool enabled;

        static DLSSManager()
        {
            if (!OperatingSystem.IsWindows())
                return;

            try
            {
                IntPtr handle;
                Available = NativeLibrary.TryLoad("nvngx_dlss.dll", out handle);
                if (handle != IntPtr.Zero)
                    NativeLibrary.Free(handle);
            }
            catch
            {
                Available = false;
            }
        }

        public static bool Enabled
        {
            get => enabled;
            set
            {
                if (!Available)
                    return;

                enabled = value;
                Logger.Log($"[DLSS] {(enabled ? "Enabled" : "Disabled")}");
                // TODO: call into DLSS APIs once available
            }
        }
    }
}
