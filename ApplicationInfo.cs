using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Herbs
{
    public enum WindowState
    {
        Top,
        Normal,
    }
    public class ApplicationInfo
    {
        public Process Process { get; }

        public ImageSource IconImage { get; }

        public string Header => Process.MainWindowTitle;

        private WindowState _windowState;

        public WindowState WindowState
        {
            get => _windowState;
            set
            {
                _windowState = value;
                switch (_windowState)
                {
                    case WindowState.Top:
                        SetWindowTop(Process.MainWindowHandle);
                        break;
                    case WindowState.Normal:
                        SetWindowNormal(Process.MainWindowHandle);
                        break;
                    default:
                        break;
                }
            }
        }

        public ApplicationInfo(Process process)
        {
            Process = process;

            var shinfo = new SHFILEINFO();
            var success = SHGetFileInfo(Process.MainModule.FileName, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), SHGFI_ICON | SHGFI_LARGEICON);
            if (success != IntPtr.Zero)
            {
                var img = Icon.FromHandle(shinfo.hIcon);

                var bitmap = img.ToBitmap();
                var hBitmap = bitmap.GetHbitmap();

                IconImage = Imaging.CreateBitmapSourceFromHBitmap(
                                                hBitmap, IntPtr.Zero, Int32Rect.Empty,
                                                BitmapSizeOptions.FromEmptyOptions());
            }
        }

        // SHGetFileInfo関数
        [DllImport("shell32.dll")]
        private static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);

        // SHGetFileInfo関数で使用するフラグ
        private const uint SHGFI_ICON = 0x100; // アイコン・リソースの取得
        private const uint SHGFI_LARGEICON = 0x0; // 大きいアイコン
        private const uint SHGFI_SMALLICON = 0x1; // 小さいアイコン

        // SHGetFileInfo関数で使用する構造体
        private struct SHFILEINFO
        {
            public IntPtr hIcon;
            public IntPtr iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        };


        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cx, int cy, int uFlags);

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        public static void SetWindowTop(IntPtr hwnd)
        {
            const int SWP_NOSIZE = 0x0001;
            const int SWP_NOMOVE = 0x0002;

            const int HWND_TOPMOST = -1;

            SetWindowPos(hwnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
        }

        public static void SetWindowNormal(IntPtr hwnd)
        {
            const int SWP_NOSIZE = 0x0001;
            const int SWP_NOMOVE = 0x0002;

            const int HWND_NORMAL = -2;

            GetWindowRect(hwnd, out var rect);
            SetWindowPos(hwnd, HWND_NORMAL, rect.left, rect.top, rect.right, rect.bottom, SWP_NOMOVE | SWP_NOSIZE);
        }
    }
}
