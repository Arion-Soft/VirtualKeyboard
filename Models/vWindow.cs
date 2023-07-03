using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace VirtualKeyboard.Models
{
    public abstract class vWindow
    {
        #region Constants

        public const int WsExTransparent = 0x00000020;
        public const int WsExNoActivate = 0x08000000;
        public const int GwlExStyle = -20;
        public const int WmNclButtonDown = 0x00A1;
        public const int HtCaption = 0x0002;

        public const int WmSysCommand = 0x112;
        public const int MfByPosition = 0x400;
        public const int MfSeparator = 0x800;

        public const uint TpmLeftAlign = 0x0000;
        public const uint TpmReturnCmd = 0x0100;

        #endregion

        #region DLL Imports

        [DllImport("user32", SetLastError = true)]
        public static extern int GetWindowLong(IntPtr hwnd, int nIndex);

        [DllImport("user32", SetLastError = true)]
        public static extern int SetWindowLong(IntPtr hwnd, int nIndex, int dwNewValue);

        //................................
        [DllImport("user32", SetLastError = true)]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32", SetLastError = true)]
        public static extern bool InsertMenu(IntPtr hMenu, Int32 wPosition, Int32 wFlags, Int32 wIdNewItem, string lpNewItem);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool AppendMenu(IntPtr hMenu, int uFlags, int uIdNewItem, string lpNewItem);

        [DllImport("user32.dll")]
        public static extern int TrackPopupMenuEx(IntPtr hmenu, uint fuFlags, int x, int y, IntPtr hwnd, IntPtr lptpm);

        //................................
        [DllImport("user32.dll")]
        public static extern IntPtr PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        #endregion
    }

    public class WindowController
    {
        private readonly WindowInteropHelper _mHelper;

        public WindowController(Window win)
        {
            _mHelper = new WindowInteropHelper(win);
        }

        #region Focus Control

        /*-----------------------------------------
        This allows the window to not gain focus when it is clicked on.
        -----------------------------------------*/
        private int _mStateFocus, _mStateUnfocus;

        public void LoadFocusControl()
        {
            _mStateFocus = vWindow.GetWindowLong(_mHelper.Handle, vWindow.GwlExStyle);
            _mStateUnfocus = _mStateFocus | vWindow.WsExNoActivate;
        }

        public void AllowFocus(bool isAllowed)
        {
            vWindow.SetWindowLong(_mHelper.Handle, vWindow.GwlExStyle, isAllowed ? _mStateFocus : _mStateUnfocus);
        } //func

        public void DisableFocus()
        {
            int style = vWindow.GetWindowLong(_mHelper.Handle, vWindow.GwlExStyle);
            vWindow.SetWindowLong(_mHelper.Handle, vWindow.GwlExStyle, style | vWindow.WsExNoActivate);
        }

        #endregion

        #region System Control Menu

        private IntPtr _mMenuHandle = IntPtr.Zero;

        public void LoadSystemMenu()
        {
            _mMenuHandle = vWindow.GetSystemMenu(_mHelper.Handle, false);
        }

        public void ShowSystemMenu(int x, int y)
        {
            int cmd = vWindow.TrackPopupMenuEx(_mMenuHandle, vWindow.TpmLeftAlign | vWindow.TpmReturnCmd, x, y,
                _mHelper.Handle, IntPtr.Zero);
            if (cmd == 0) return;
            vWindow.PostMessage(_mHelper.Handle, vWindow.WmSysCommand, new IntPtr(cmd), IntPtr.Zero);
        } //func

        public void InsertMenu(int pos, int itmId, string menuTitle)
        {
            vWindow.InsertMenu(_mMenuHandle, pos, vWindow.MfByPosition, itmId, menuTitle);
        } //func

        public void InsertMenuSep(int pos, int itmId)
        {
            vWindow.InsertMenu(_mMenuHandle, pos, vWindow.MfSeparator | vWindow.MfByPosition, itmId, null);
        } //func

        #endregion
    }
}