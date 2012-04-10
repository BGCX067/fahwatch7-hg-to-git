Imports FAHWatch7.NetworkInformation
Imports System.Runtime.InteropServices
Imports System.Text

Friend Class NativeMethods


    ''' <summary>
    ''' Changes an attribute of the specified window. The function also sets the 32-bit (long) value at the specified offset into the extra window memory.
    ''' </summary>
    ''' <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs..</param>
    ''' <param name="nIndex">The zero-based offset to the value to be set. Valid values are in the range zero through the number of bytes of extra window memory, minus the size of an integer. To set any other value, specify one of the following values: GWL_EXSTYLE, GWL_HINSTANCE, GWL_ID, GWL_STYLE, GWL_USERDATA, GWL_WNDPROC </param>
    ''' <param name="dwNewLong">The replacement value.</param>
    ''' <returns>If the function succeeds, the return value is the previous value of the specified 32-bit integer. 
    ''' If the function fails, the return value is zero. To get extended error information, call GetLastError. </returns>
    <System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint:="SetWindowLong")> _
    Private Shared Function SetWindowLong32(ByVal hWnd As IntPtr, <MarshalAs(UnmanagedType.I4)> nIndex As WindowLongFlags, ByVal dwNewLong As Integer) As Integer
    End Function

    <System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint:="SetWindowLongPtr")> _
    Protected Friend Shared Function SetWindowLongPtr64(ByVal hWnd As IntPtr, <MarshalAs(UnmanagedType.I4)> nIndex As WindowLongFlags, ByVal dwNewLong As IntPtr) As IntPtr
    End Function
    <System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint:="SetWindowLongPtr")> _
    Protected Friend Shared Function SetWindowLongPtr32(ByVal hWnd As IntPtr, <MarshalAs(UnmanagedType.I4)> nIndex As WindowLongFlags, ByVal dwNewLong As IntPtr) As IntPtr
    End Function

    <DllImport("user32.dll", SetLastError:=True)> _
    Protected Friend Shared Function GetForegroundWindow() As IntPtr
    End Function

    <DllImport("netapi32.dll", CharSet:=CharSet.Unicode, SetLastError:=True)> _
    Protected Friend Shared Function NetGetJoinInformation(ByVal computerName As String, ByRef buffer As IntPtr, ByRef status As JoinStatus) As Integer
    End Function

    <DllImport("netapi32.dll", SetLastError:=True)> _
    Protected Friend Shared Function NetApiBufferFree(ByVal buffer As IntPtr) As Integer
    End Function

    <DllImport("netapi32.dll", CharSet:=CharSet.Unicode, setlasterror:=True)> _
    Protected Friend Shared Function NetShareEnum(ByVal ServerName As StringBuilder, ByVal level As Integer, ByRef BufPtr As IntPtr, ByVal prefmaxbufferlen As Integer, ByRef entriesread As Integer, ByRef totalentries As Integer, ByRef resume_handle As Integer) As Integer
    End Function

    <DllImport("netapi32.dll", CharSet:=CharSet.Unicode, setlasterror:=True)> _
    Protected Friend Shared Function NetShareGetInfo(ByVal ServerName As String, ByVal ShareName As String, ByVal Level As Integer, ByRef BufPtr As IntPtr) As Integer
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Ansi, SetLastError:=True, ExactSpelling:=True)> _
    Protected Friend Shared Function SetForegroundWindow(ByVal hwnd As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
    Protected Friend Shared Function ShowWindow(ByVal hwnd As IntPtr, ByVal nCmdShow As ShowWindowCommands) As Boolean
    End Function

    <DllImport("user32.dll", EntryPoint:="SetWindowPos")> _
    Protected Friend Shared Function SetWindowPos(hWnd As IntPtr, hWndInsertAfter As IntPtr, X As Integer, Y As Integer, cx As Integer, cy As Integer, uFlags As UInteger) As Boolean
    End Function

    <DllImport("user32.dll")> _
    Protected Friend Shared Function AnimateWindow(ByVal hwnd As IntPtr, ByVal time As Integer, ByVal flags As AnimateWindowFlags) As Boolean
    End Function

    <DllImport("user32.dll")> _
    Private Shared Function GetWindowPlacement(ByVal hWnd As IntPtr, ByRef lpwndpl As WINDOWPLACEMENT) As Boolean
    End Function

    <DllImport("user32.dll", SetLastError:=True)> _
    Protected Friend Shared Function SetActiveWindow(ByVal hWnd As IntPtr) As IntPtr
    End Function

    <DllImport("user32.dll")> _
    Protected Friend Shared Function EnableWindow(ByVal hWnd As IntPtr, ByVal bEnable As Boolean) As Boolean
    End Function

    Public Enum WindowLongFlags As Integer
        GWL_EXSTYLE = -20
        GWLP_HINSTANCE = -6
        GWLP_HWNDPARENT = -8
        GWL_ID = -12
        GWL_STYLE = -16
        GWL_USERDATA = -21
        GWL_WNDPROC = -4
        DWLP_USER = &H8
        DWLP_MSGRESULT = &H0
        DWLP_DLGPROC = &H4
    End Enum

    Enum ShowWindowCommands As Integer
        SW_FORCEMINIMIZE = 11
        SW_HIDE = 0
        SW_MAXIMIZE = 3
        SW_MINIMIZE = 6
        SW_RESTORE = 9
        SW_SHOW = 5
        SW_SHOWDEFAULT = 10
        SW_SHOWMAXIMIZED = 3
        SW_SHOWMINIMIZED = 2
        SW_SHOWMINNOACTIVE = 7
        SW_SHOWNA = 8
        SW_SHOWNOACTIVATE = 4
        SW_SHOWNORMAL = 1
    End Enum

    <Flags()>
    Public Enum SetWindowPosFlags As UInteger
        SWP_ASYNCWINDOWPOS = &H4000
        SWP_DEFERERASE = &H2000
        SWP_DRAWFRAME = SWP_FRAMECHANGED
        SWP_FRAMECHANGED = &H20
        SWP_HIDEWINDOW = &H80
        SWP_NOACTIVATE = &H10
        SWP_NOCOPYBITS = &H100
        SWP_NOMOVE = &H2
        SWP_NOOWNERZORDER = &H200
        SWP_NOREDRAW = &H8
        SWP_NOREPOSITION = SWP_NOOWNERZORDER
        SWP_NOSENDCHANGING = &H400
        SWP_NOSIZE = &H1
        SWP_NOZORDER = &H4
        SWP_SHOWWINDOW = &H40
    End Enum


    Friend Class ShowWindowHandles
        Friend Shared HWND_TOP As New IntPtr(0)
        Friend Shared HWND_BOTTOM As New IntPtr(1)
        Friend Shared HWND_TOPMOST As New IntPtr(-1)
        Friend Shared HWND_NOTOPMOST As New IntPtr(-2)
    End Class

    <Flags()> _
    Enum AnimateWindowFlags
        AW_HOR_POSITIVE = &H1
        AW_HOR_NEGATIVE = &H2
        AW_VER_POSITIVE = &H4
        AW_VER_NEGATIVE = &H8
        AW_CENTER = &H10
        AW_HIDE = &H10000
        AW_ACTIVATE = &H20000
        AW_SLIDE = &H40000
        AW_BLEND = &H80000
    End Enum

    <StructLayout(LayoutKind.Sequential)> _
    Private Structure WINDOWPLACEMENT
        Public length As Integer
        Public flags As Integer
        Public showCmd As ShowWindowCommands
        Public minPosition As Point
        Public maxPosition As Point
        Public normalPosition As Rectangle
    End Structure


    Friend Const WM_SYSCOMMAND As Int32 = &H112

    Friend Class WM_SYSCOMMANDS
        Friend Const SC_MAXIMIZE As Int32 = &HF030
        Friend Const SC_MINIMIZE As Int32 = &HF020
        Friend Const SC_RESTORE As Int32 = &HF120
        Friend Const SC_CLOSE As Int32 = &HF060
        Friend Const SC_SIZE As Int32 = &HF000
        Friend Const SC_MOVE As Int32 = &HF010
        Friend Const SC_NEXTWINDOW As Int32 = &HF040
        Friend Const SC_PREVWINDOW As Int32 = &HF050
        Friend Const SC_VSCROLL As Int32 = &HF070
        Friend Const SC_HSCROLL As Int32 = &HF080
        Friend Const SC_MOUSEMENU As Int32 = &HF090
        Friend Const SC_KEYMENU As Int32 = &HF100
        Friend Const SC_ARRANGE As Int32 = &HF110
        Friend Const SC_TASKLIST As Int32 = &HF130
        Friend Const SC_SCREENSAVE As Int32 = &HF140
        Friend Const SC_HOTKEY As Int32 = &HF150
#If (WINVER >= &H400) Then 'Win95
        Friend const SC_DEFAULT as int32 = &HF160
        Friend const SC_MONITORPOWER as int32 = &HF170
        Friend const SC_CONTEXTHELP as int32 = &HF180
        Friend const SC_SEPARATOR as int32 = &HF00F
#End If 'WINVER >= &h0400
#If (WINVER >= &H600) Then 'Vista
        Friend const SCF_ISSECURE as int32 = &H1
#End If 'WINVER >= &h0600
    End Class





    Public Enum WindowsMessages As UInteger
        ''' <summary>
        '''The WM_ACTIVATE message is sent when a window is being activated or deactivated. This message is sent first to the window procedure of the top-level window being deactivated; it is then sent to the window procedure of the top-level window being activated.
        ''' </summary>
        ''' <remarks></remarks>
        WM_ACTIVATE = &H6

        ''' <summary>
        '''The WM_ACTIVATEAPP message is sent when a window belonging to a different application than the active window is about to be activated. The message is sent to the application whose window is being activated and to the application whose window is being deactivated.
        ''' </summary>
        ''' <remarks></remarks>
        WM_ACTIVATEAPP = &H1C

        ''' <summary>
        '''Definition Needed
        ''' </summary>
        ''' <remarks></remarks>
        WM_AFXFIRST = &H360

        ''' <summary>
        '''Definition Needed
        ''' </summary>
        ''' <remarks></remarks>
        WM_AFXLAST = &H37F

        ''' <summary>
        '''The WM_APP constant is used by applications to help define private messages usually of the form WM_APP+X where X is an integer value.
        ''' </summary>
        ''' <remarks></remarks>
        WM_APP = &H8000

        ''' <summary>
        '''The WM_ASKCBFORMATNAME message is sent to the clipboard owner by a clipboard viewer window to request the name of a CF_OWNERDISPLAY clipboard format.
        ''' </summary>
        ''' <remarks></remarks>
        WM_ASKCBFORMATNAME = &H30C

        ''' <summary>
        '''The WM_CANCELJOURNAL message is posted to an application when a user cancels the application's journaling activities. The message is posted with a NULL window handle.
        ''' </summary>
        ''' <remarks></remarks>
        WM_CANCELJOURNAL = &H4B

        ''' <summary>
        '''The WM_CANCELMODE message is sent to cancel certain modes such as mouse capture. For example the system sends this message to the active window when a dialog box or message box is displayed. Certain functions also send this message explicitly to the specified window regardless of whether it is the active window. For example the EnableWindow function sends this message when disabling the specified window.
        ''' </summary>
        ''' <remarks></remarks>
        WM_CANCELMODE = &H1F

        ''' <summary>
        '''The WM_CAPTURECHANGED message is sent to the window that is losing the mouse capture.
        ''' </summary>
        ''' <remarks></remarks>
        WM_CAPTURECHANGED = &H215

        ''' <summary>
        '''The WM_CHANGECBCHAIN message is sent to the first window in the clipboard viewer chain when a window is being removed from the chain.
        ''' </summary>
        ''' <remarks></remarks>
        WM_CHANGECBCHAIN = &H30D

        ''' <summary>
        '''An application sends the WM_CHANGEUISTATE message to indicate that the user interface (UI) state should be changed.
        ''' </summary>
        ''' <remarks></remarks>
        WM_CHANGEUISTATE = &H127

        ''' <summary>
        '''The WM_CHAR message is posted to the window with the keyboard focus when a WM_KEYDOWN message is translated by the TranslateMessage function. The WM_CHAR message contains the character code of the key that was pressed.
        ''' </summary>
        ''' <remarks></remarks>
        WM_CHAR = &H102

        ''' <summary>
        '''Sent by a list box with the LBS_WANTKEYBOARDINPUT style to its owner in response to a WM_CHAR message.
        ''' </summary>
        ''' <remarks></remarks>
        WM_CHARTOITEM = &H2F

        ''' <summary>
        '''The WM_CHILDACTIVATE message is sent to a child window when the user clicks the window's title bar or when the window is activated moved or sized.
        ''' </summary>
        ''' <remarks></remarks>
        WM_CHILDACTIVATE = &H22

        ''' <summary>
        '''An application sends a WM_CLEAR message to an edit control or combo box to delete (clear) the current selection if any from the edit control.
        ''' </summary>
        ''' <remarks></remarks>
        WM_CLEAR = &H303

        ''' <summary>
        '''The WM_CLOSE message is sent as a signal that a window or an application should terminate.
        ''' </summary>
        ''' <remarks></remarks>
        WM_CLOSE = &H10

        ''' <summary>
        '''The WM_COMMAND message is sent when the user selects a command item from a menu when a control sends a notification message to its parent window or when an accelerator keystroke is translated.
        ''' </summary>
        ''' <remarks></remarks>
        WM_COMMAND = &H111

        ''' <summary>
        '''The WM_COMPACTING message is sent to all top-level windows when the system detects more than 12.5 percent of system time over a 30- to 60-second interval is being spent compacting memory. This indicates that system memory is low.
        ''' </summary>
        ''' <remarks></remarks>
        WM_COMPACTING = &H41

        ''' <summary>
        '''The system sends the WM_COMPAREITEM message to determine the relative position of a new item in the sorted list of an owner-drawn combo box or list box. Whenever the application adds a new item the system sends this message to the owner of a combo box or list box created with the CBS_SORT or LBS_SORT style.
        ''' </summary>
        ''' <remarks></remarks>
        WM_COMPAREITEM = &H39

        ''' <summary>
        '''The WM_CONTEXTMENU message notifies a window that the user clicked the right mouse button (right-clicked) in the window.
        ''' </summary>
        ''' <remarks></remarks>
        WM_CONTEXTMENU = &H7B

        ''' <summary>
        '''An application sends the WM_COPY message to an edit control or combo box to copy the current selection to the clipboard in CF_TEXT format.
        ''' </summary>
        ''' <remarks></remarks>
        WM_COPY = &H301

        ''' <summary>
        '''An application sends the WM_COPYDATA message to pass data to another application.
        ''' </summary>
        ''' <remarks></remarks>
        WM_COPYDATA = &H4A

        ''' <summary>
        '''The WM_CREATE message is sent when an application requests that a window be created by calling the CreateWindowEx or CreateWindow function. (The message is sent before the function returns.) The window procedure of the new window receives this message after the window is created but before the window becomes visible.
        ''' </summary>
        ''' <remarks></remarks>
        WM_CREATE = &H1

        ''' <summary>
        '''The WM_CTLCOLORBTN message is sent to the parent window of a button before drawing the button. The parent window can change the button's text and background colors. However only owner-drawn buttons respond to the parent window processing this message.
        ''' </summary>
        ''' <remarks></remarks>
        WM_CTLCOLORBTN = &H135

        ''' <summary>
        '''The WM_CTLCOLORDLG message is sent to a dialog box before the system draws the dialog box. By responding to this message the dialog box can set its text and background colors using the specified display device context handle.
        ''' </summary>
        ''' <remarks></remarks>
        WM_CTLCOLORDLG = &H136

        ''' <summary>
        '''An edit control that is not read-only or disabled sends the WM_CTLCOLOREDIT message to its parent window when the control is about to be drawn. By responding to this message the parent window can use the specified device context handle to set the text and background colors of the edit control.
        ''' </summary>
        ''' <remarks></remarks>
        WM_CTLCOLOREDIT = &H133

        ''' <summary>
        '''Sent to the parent window of a list box before the system draws the list box. By responding to this message the parent window can set the text and background colors of the list box by using the specified display device context handle.
        ''' </summary>
        ''' <remarks></remarks>
        WM_CTLCOLORLISTBOX = &H134

        ''' <summary>
        '''The WM_CTLCOLORMSGBOX message is sent to the owner window of a message box before Windows draws the message box. By responding to this message the owner window can set the text and background colors of the message box by using the given display device context handle.
        ''' </summary>
        ''' <remarks></remarks>
        WM_CTLCOLORMSGBOX = &H132

        ''' <summary>
        '''The WM_CTLCOLORSCROLLBAR message is sent to the parent window of a scroll bar control when the control is about to be drawn. By responding to this message the parent window can use the display context handle to set the background color of the scroll bar control.
        ''' </summary>
        ''' <remarks></remarks>
        WM_CTLCOLORSCROLLBAR = &H137

        ''' <summary>
        '''A static control or an edit control that is read-only or disabled sends the WM_CTLCOLORSTATIC message to its parent window when the control is about to be drawn. By responding to this message the parent window can use the specified device context handle to set the text and background colors of the static control.
        ''' </summary>
        ''' <remarks></remarks>
        WM_CTLCOLORSTATIC = &H138

        ''' <summary>
        '''An application sends a WM_CUT message to an edit control or combo box to delete (cut) the current selection if any in the edit control and copy the deleted text to the clipboard in CF_TEXT format.
        ''' </summary>
        ''' <remarks></remarks>
        WM_CUT = &H300

        ''' <summary>
        '''The WM_DEADCHAR message is posted to the window with the keyboard focus when a WM_KEYUP message is translated by the TranslateMessage function. WM_DEADCHAR specifies a character code generated by a dead key. A dead key is a key that generates a character such as the umlaut (double-dot) that is combined with another character to form a composite character. For example the umlaut-O character (?) is generated by typing the dead key for the umlaut character and then typing the O key.
        ''' </summary>
        ''' <remarks></remarks>
        WM_DEADCHAR = &H103

        ''' <summary>
        '''Sent to the owner of a list box or combo box when the list box or combo box is destroyed or when items are removed by the LB_DELETESTRING LB_RESETCONTENT CB_DELETESTRING or CB_RESETCONTENT message. The system sends a WM_DELETEITEM message for each deleted item. The system sends the WM_DELETEITEM message for any deleted list box or combo box item with nonzero item data.
        ''' </summary>
        ''' <remarks></remarks>
        WM_DELETEITEM = &H2D

        ''' <summary>
        '''The WM_DESTROY message is sent when a window is being destroyed. It is sent to the window procedure of the window being destroyed after the window is removed from the screen. This message is sent first to the window being destroyed and then to the child windows (if any) as they are destroyed. During the processing of the message it can be assumed that all child windows still exist.
        ''' </summary>
        ''' <remarks></remarks>
        WM_DESTROY = &H2

        ''' <summary>
        '''The WM_DESTROYCLIPBOARD message is sent to the clipboard owner when a call to the EmptyClipboard function empties the clipboard.
        ''' </summary>
        ''' <remarks></remarks>
        WM_DESTROYCLIPBOARD = &H307

        ''' <summary>
        '''Notifies an application of a change to the hardware configuration of a device or the computer.
        ''' </summary>
        ''' <remarks></remarks>
        WM_DEVICECHANGE = &H219

        ''' <summary>
        '''The WM_DEVMODECHANGE message is sent to all top-level windows whenever the user changes device-mode settings.
        ''' </summary>
        ''' <remarks></remarks>
        WM_DEVMODECHANGE = &H1B

        ''' <summary>
        '''The WM_DISPLAYCHANGE message is sent to all windows when the display resolution has changed.
        ''' </summary>
        ''' <remarks></remarks>
        WM_DISPLAYCHANGE = &H7E

        ''' <summary>
        '''The WM_DRAWCLIPBOARD message is sent to the first window in the clipboard viewer chain when the content of the clipboard changes. This enables a clipboard viewer window to display the new content of the clipboard.
        ''' </summary>
        ''' <remarks></remarks>
        WM_DRAWCLIPBOARD = &H308

        ''' <summary>
        '''The WM_DRAWITEM message is sent to the parent window of an owner-drawn button combo box list box or menu when a visual aspect of the button combo box list box or menu has changed.
        ''' </summary>
        ''' <remarks></remarks>
        WM_DRAWITEM = &H2B

        ''' <summary>
        '''Sent when the user drops a file on the window of an application that has registered itself as a recipient of dropped files.
        ''' </summary>
        ''' <remarks></remarks>
        WM_DROPFILES = &H233

        ''' <summary>
        '''The WM_ENABLE message is sent when an application changes the enabled state of a window. It is sent to the window whose enabled state is changing. This message is sent before the EnableWindow function returns but after the enabled state (WS_DISABLED style bit) of the window has changed.
        ''' </summary>
        ''' <remarks></remarks>
        WM_ENABLE = &HA

        ''' <summary>
        '''The WM_ENDSESSION message is sent to an application after the system processes the results of the WM_QUERYENDSESSION message. The WM_ENDSESSION message informs the application whether the session is ending.
        ''' </summary>
        ''' <remarks></remarks>
        WM_ENDSESSION = &H16

        ''' <summary>
        '''The WM_ENTERIDLE message is sent to the owner window of a modal dialog box or menu that is entering an idle state. A modal dialog box or menu enters an idle state when no messages are waiting in its queue after it has processed one or more previous messages.
        ''' </summary>
        ''' <remarks></remarks>
        WM_ENTERIDLE = &H121

        ''' <summary>
        '''The WM_ENTERMENULOOP message informs an application's main window procedure that a menu modal loop has been entered.
        ''' </summary>
        ''' <remarks></remarks>
        WM_ENTERMENULOOP = &H211

        ''' <summary>
        '''The WM_ENTERSIZEMOVE message is sent one time to a window after it enters the moving or sizing modal loop. The window enters the moving or sizing modal loop when the user clicks the window's title bar or sizing border or when the window passes the WM_SYSCOMMAND message to the DefWindowProc function and the wParam parameter of the message specifies the SC_MOVE or SC_SIZE value. The operation is complete when DefWindowProc returns.
        ''' </summary>
        ''' <remarks></remarks>
        WM_ENTERSIZEMOVE = &H231

        ''' <summary>
        '''The WM_ERASEBKGND message is sent when the window background must be erased (for example when a window is resized). The message is sent to prepare an invalidated portion of a window for painting.
        ''' </summary>
        ''' <remarks></remarks>
        WM_ERASEBKGND = &H14

        ''' <summary>
        '''The WM_EXITMENULOOP message informs an application's main window procedure that a menu modal loop has been exited.
        ''' </summary>
        ''' <remarks></remarks>
        WM_EXITMENULOOP = &H212

        ''' <summary>
        '''The WM_EXITSIZEMOVE message is sent one time to a window after it has exited the moving or sizing modal loop. The window enters the moving or sizing modal loop when the user clicks the window's title bar or sizing border or when the window passes the WM_SYSCOMMAND message to the DefWindowProc function and the wParam parameter of the message specifies the SC_MOVE or SC_SIZE value. The operation is complete when DefWindowProc returns.
        ''' </summary>
        ''' <remarks></remarks>
        WM_EXITSIZEMOVE = &H232

        ''' <summary>
        '''An application sends the WM_FONTCHANGE message to all top-level windows in the system after changing the pool of font resources.
        ''' </summary>
        ''' <remarks></remarks>
        WM_FONTCHANGE = &H1D

        ''' <summary>
        '''The WM_GETDLGCODE message is sent to the window procedure associated with a control. By default the system handles all keyboard input to the control; the system interprets certain types of keyboard input as dialog box navigation keys. To override this default behavior the control can respond to the WM_GETDLGCODE message to indicate the types of input it wants to process itself.
        ''' </summary>
        ''' <remarks></remarks>
        WM_GETDLGCODE = &H87

        ''' <summary>
        '''An application sends a WM_GETFONT message to a control to retrieve the font with which the control is currently drawing its text.
        ''' </summary>
        ''' <remarks></remarks>
        WM_GETFONT = &H31

        ''' <summary>
        '''An application sends a WM_GETHOTKEY message to determine the hot key associated with a window.
        ''' </summary>
        ''' <remarks></remarks>
        WM_GETHOTKEY = &H33

        ''' <summary>
        '''The WM_GETICON message is sent to a window to retrieve a handle to the large or small icon associated with a window. The system displays the large icon in the ALT+TAB dialog and the small icon in the window caption.
        ''' </summary>
        ''' <remarks></remarks>
        WM_GETICON = &H7F

        ''' <summary>
        '''The WM_GETMINMAXINFO message is sent to a window when the size or position of the window is about to change. An application can use this message to override the window's default maximized size and position or its default minimum or maximum tracking size.
        ''' </summary>
        ''' <remarks></remarks>
        WM_GETMINMAXINFO = &H24

        ''' <summary>
        '''Active Accessibility sends the WM_GETOBJECT message to obtain information about an accessible object contained in a server application. Applications never send this message directly. It is sent only by Active Accessibility in response to calls to AccessibleObjectFromPoint AccessibleObjectFromEvent or AccessibleObjectFromWindow. However server applications handle this message.
        ''' </summary>
        ''' <remarks></remarks>
        WM_GETOBJECT = &H3D

        ''' <summary>
        '''An application sends a WM_GETTEXT message to copy the text that corresponds to a window into a buffer provided by the caller.
        ''' </summary>
        ''' <remarks></remarks>
        WM_GETTEXT = &HD

        ''' <summary>
        '''An application sends a WM_GETTEXTLENGTH message to determine the length in characters of the text associated with a window.
        ''' </summary>
        ''' <remarks></remarks>
        WM_GETTEXTLENGTH = &HE

        ''' <summary>
        '''Definition Needed
        ''' </summary>
        ''' <remarks></remarks>
        WM_HANDHELDFIRST = &H358

        ''' <summary>
        '''Definition Needed
        ''' </summary>
        ''' <remarks></remarks>
        WM_HANDHELDLAST = &H35F

        ''' <summary>
        '''Indicates that the user pressed the F1 key. If a menu is active when F1 is pressed WM_HELP is sent to the window associated with the menu; otherwise WM_HELP is sent to the window that has the keyboard focus. If no window has the keyboard focus WM_HELP is sent to the currently active window.
        ''' </summary>
        ''' <remarks></remarks>
        WM_HELP = &H53

        ''' <summary>
        '''The WM_HOTKEY message is posted when the user presses a hot key registered by the RegisterHotKey function. The message is placed at the top of the message queue associated with the thread that registered the hot key.
        ''' </summary>
        ''' <remarks></remarks>
        WM_HOTKEY = &H312

        ''' <summary>
        '''This message is sent to a window when a scroll event occurs in the window's standard horizontal scroll bar. This message is also sent to the owner of a horizontal scroll bar control when a scroll event occurs in the control.
        ''' </summary>
        ''' <remarks></remarks>
        WM_HSCROLL = &H114

        ''' <summary>
        '''The WM_HSCROLLCLIPBOARD message is sent to the clipboard owner by a clipboard viewer window. This occurs when the clipboard contains data in the CF_OWNERDISPLAY format and an event occurs in the clipboard viewer's horizontal scroll bar. The owner should scroll the clipboard image and update the scroll bar values.
        ''' </summary>
        ''' <remarks></remarks>
        WM_HSCROLLCLIPBOARD = &H30E

        ''' <summary>
        '''Windows NT 3.51 and earlier: The WM_ICONERASEBKGND message is sent to a minimized window when the background of the icon must be filled before painting the icon. A window receives this message only if a class icon is defined for the window; otherwise WM_ERASEBKGND is sent. This message is not sent by newer versions of Windows.
        ''' </summary>
        ''' <remarks></remarks>
        WM_ICONERASEBKGND = &H27

        ''' <summary>
        '''Sent to an application when the IME gets a character of the conversion result. A window receives this message through its WindowProc function.
        ''' </summary>
        ''' <remarks></remarks>
        WM_IME_CHAR = &H286

        ''' <summary>
        '''Sent to an application when the IME changes composition status as a result of a keystroke. A window receives this message through its WindowProc function.
        ''' </summary>
        ''' <remarks></remarks>
        WM_IME_COMPOSITION = &H10F

        ''' <summary>
        '''Sent to an application when the IME window finds no space to extend the area for the composition window. A window receives this message through its WindowProc function.
        ''' </summary>
        ''' <remarks></remarks>
        WM_IME_COMPOSITIONFULL = &H284

        ''' <summary>
        '''Sent by an application to direct the IME window to carry out the requested command. The application uses this message to control the IME window that it has created. To send this message the application calls the SendMessage function with the following parameters.
        ''' </summary>
        ''' <remarks></remarks>
        WM_IME_CONTROL = &H283

        ''' <summary>
        '''Sent to an application when the IME ends composition. A window receives this message through its WindowProc function.
        ''' </summary>
        ''' <remarks></remarks>
        WM_IME_ENDCOMPOSITION = &H10E

        ''' <summary>
        '''Sent to an application by the IME to notify the application of a key press and to keep message order. A window receives this message through its WindowProc function.
        ''' </summary>
        ''' <remarks></remarks>
        WM_IME_KEYDOWN = &H290

        ''' <summary>
        '''Definition Needed
        ''' </summary>
        ''' <remarks></remarks>
        WM_IME_KEYLAST = &H10F

        ''' <summary>
        '''Sent to an application by the IME to notify the application of a key release and to keep message order. A window receives this message through its WindowProc function.
        ''' </summary>
        ''' <remarks></remarks>
        WM_IME_KEYUP = &H291

        ''' <summary>
        '''Sent to an application to notify it of changes to the IME window. A window receives this message through its WindowProc function.
        ''' </summary>
        ''' <remarks></remarks>
        WM_IME_NOTIFY = &H282

        ''' <summary>
        '''Sent to an application to provide commands and request information. A window receives this message through its WindowProc function.
        ''' </summary>
        ''' <remarks></remarks>
        WM_IME_REQUEST = &H288

        ''' <summary>
        '''Sent to an application when the operating system is about to change the current IME. A window receives this message through its WindowProc function.
        ''' </summary>
        ''' <remarks></remarks>
        WM_IME_SELECT = &H285

        ''' <summary>
        '''Sent to an application when a window is activated. A window receives this message through its WindowProc function.
        ''' </summary>
        ''' <remarks></remarks>
        WM_IME_SETCONTEXT = &H281

        ''' <summary>
        '''Sent immediately before the IME generates the composition string as a result of a keystroke. A window receives this message through its WindowProc function.
        ''' </summary>
        ''' <remarks></remarks>
        WM_IME_STARTCOMPOSITION = &H10D

        ''' <summary>
        '''The WM_INITDIALOG message is sent to the dialog box procedure immediately before a dialog box is displayed. Dialog box procedures typically use this message to initialize controls and carry out any other initialization tasks that affect the appearance of the dialog box.
        ''' </summary>
        ''' <remarks></remarks>
        WM_INITDIALOG = &H110

        ''' <summary>
        '''The WM_INITMENU message is sent when a menu is about to become active. It occurs when the user clicks an item on the menu bar or presses a menu key. This allows the application to modify the menu before it is displayed.
        ''' </summary>
        ''' <remarks></remarks>
        WM_INITMENU = &H116

        ''' <summary>
        '''The WM_INITMENUPOPUP message is sent when a drop-down menu or submenu is about to become active. This allows an application to modify the menu before it is displayed without changing the entire menu.
        ''' </summary>
        ''' <remarks></remarks>
        WM_INITMENUPOPUP = &H117

        ''' <summary>
        '''The WM_INPUTLANGCHANGE message is sent to the topmost affected window after an application's input language has been changed. You should make any application-specific settings and pass the message to the DefWindowProc function which passes the message to all first-level child windows. These child windows can pass the message to DefWindowProc to have it pass the message to their child windows and so on.
        ''' </summary>
        ''' <remarks></remarks>
        WM_INPUTLANGCHANGE = &H51

        ''' <summary>
        '''The WM_INPUTLANGCHANGEREQUEST message is posted to the window with the focus when the user chooses a new input language either with the hotkey (specified in the Keyboard control panel application) or from the indicator on the system taskbar. An application can accept the change by passing the message to the DefWindowProc function or reject the change (and prevent it from taking place) by returning immediately.
        ''' </summary>
        ''' <remarks></remarks>
        WM_INPUTLANGCHANGEREQUEST = &H50

        ''' <summary>
        '''The WM_KEYDOWN message is posted to the window with the keyboard focus when a nonsystem key is pressed. A nonsystem key is a key that is pressed when the ALT key is not pressed.
        ''' </summary>
        ''' <remarks></remarks>
        WM_KEYDOWN = &H100

        ''' <summary>
        '''This message filters for keyboard messages.
        ''' </summary>
        ''' <remarks></remarks>
        WM_KEYFIRST = &H100

        ''' <summary>
        '''This message filters for keyboard messages.
        ''' </summary>
        ''' <remarks></remarks>
        WM_KEYLAST = &H108

        ''' <summary>
        '''The WM_KEYUP message is posted to the window with the keyboard focus when a nonsystem key is released. A nonsystem key is a key that is pressed when the ALT key is not pressed or a keyboard key that is pressed when a window has the keyboard focus.
        ''' </summary>
        ''' <remarks></remarks>
        WM_KEYUP = &H101

        ''' <summary>
        '''The WM_KILLFOCUS message is sent to a window immediately before it loses the keyboard focus.
        ''' </summary>
        ''' <remarks></remarks>
        WM_KILLFOCUS = &H8

        ''' <summary>
        '''The WM_LBUTTONDBLCLK message is posted when the user double-clicks the left mouse button while the cursor is in the client area of a window. If the mouse is not captured the message is posted to the window beneath the cursor. Otherwise the message is posted to the window that has captured the mouse.
        ''' </summary>
        ''' <remarks></remarks>
        WM_LBUTTONDBLCLK = &H203

        ''' <summary>
        '''The WM_LBUTTONDOWN message is posted when the user presses the left mouse button while the cursor is in the client area of a window. If the mouse is not captured the message is posted to the window beneath the cursor. Otherwise the message is posted to the window that has captured the mouse.
        ''' </summary>
        ''' <remarks></remarks>
        WM_LBUTTONDOWN = &H201

        ''' <summary>
        '''The WM_LBUTTONUP message is posted when the user releases the left mouse button while the cursor is in the client area of a window. If the mouse is not captured the message is posted to the window beneath the cursor. Otherwise the message is posted to the window that has captured the mouse.
        ''' </summary>
        ''' <remarks></remarks>
        WM_LBUTTONUP = &H202

        ''' <summary>
        '''The WM_MBUTTONDBLCLK message is posted when the user double-clicks the middle mouse button while the cursor is in the client area of a window. If the mouse is not captured the message is posted to the window beneath the cursor. Otherwise the message is posted to the window that has captured the mouse.
        ''' </summary>
        ''' <remarks></remarks>
        WM_MBUTTONDBLCLK = &H209

        ''' <summary>
        '''The WM_MBUTTONDOWN message is posted when the user presses the middle mouse button while the cursor is in the client area of a window. If the mouse is not captured the message is posted to the window beneath the cursor. Otherwise the message is posted to the window that has captured the mouse.
        ''' </summary>
        ''' <remarks></remarks>
        WM_MBUTTONDOWN = &H207

        ''' <summary>
        '''The WM_MBUTTONUP message is posted when the user releases the middle mouse button while the cursor is in the client area of a window. If the mouse is not captured the message is posted to the window beneath the cursor. Otherwise the message is posted to the window that has captured the mouse.
        ''' </summary>
        ''' <remarks></remarks>
        WM_MBUTTONUP = &H208

        ''' <summary>
        '''An application sends the WM_MDIACTIVATE message to a multiple-document interface (MDI) client window to instruct the client window to activate a different MDI child window.
        ''' </summary>
        ''' <remarks></remarks>
        WM_MDIACTIVATE = &H222

        ''' <summary>
        '''An application sends the WM_MDICASCADE message to a multiple-document interface (MDI) client window to arrange all its child windows in a cascade format.
        ''' </summary>
        ''' <remarks></remarks>
        WM_MDICASCADE = &H227

        ''' <summary>
        '''An application sends the WM_MDICREATE message to a multiple-document interface (MDI) client window to create an MDI child window.
        ''' </summary>
        ''' <remarks></remarks>
        WM_MDICREATE = &H220

        ''' <summary>
        '''An application sends the WM_MDIDESTROY message to a multiple-document interface (MDI) client window to close an MDI child window.
        ''' </summary>
        ''' <remarks></remarks>
        WM_MDIDESTROY = &H221

        ''' <summary>
        '''An application sends the WM_MDIGETACTIVE message to a multiple-document interface (MDI) client window to retrieve the handle to the active MDI child window.
        ''' </summary>
        ''' <remarks></remarks>
        WM_MDIGETACTIVE = &H229

        ''' <summary>
        '''An application sends the WM_MDIICONARRANGE message to a multiple-document interface (MDI) client window to arrange all minimized MDI child windows. It does not affect child windows that are not minimized.
        ''' </summary>
        ''' <remarks></remarks>
        WM_MDIICONARRANGE = &H228

        ''' <summary>
        '''An application sends the WM_MDIMAXIMIZE message to a multiple-document interface (MDI) client window to maximize an MDI child window. The system resizes the child window to make its client area fill the client window. The system places the child window's window menu icon in the rightmost position of the frame window's menu bar and places the child window's restore icon in the leftmost position. The system also appends the title bar text of the child window to that of the frame window.
        ''' </summary>
        ''' <remarks></remarks>
        WM_MDIMAXIMIZE = &H225

        ''' <summary>
        '''An application sends the WM_MDINEXT message to a multiple-document interface (MDI) client window to activate the next or previous child window.
        ''' </summary>
        ''' <remarks></remarks>
        WM_MDINEXT = &H224

        ''' <summary>
        '''An application sends the WM_MDIREFRESHMENU message to a multiple-document interface (MDI) client window to refresh the window menu of the MDI frame window.
        ''' </summary>
        ''' <remarks></remarks>
        WM_MDIREFRESHMENU = &H234

        ''' <summary>
        '''An application sends the WM_MDIRESTORE message to a multiple-document interface (MDI) client window to restore an MDI child window from maximized or minimized size.
        ''' </summary>
        ''' <remarks></remarks>
        WM_MDIRESTORE = &H223

        ''' <summary>
        '''An application sends the WM_MDISETMENU message to a multiple-document interface (MDI) client window to replace the entire menu of an MDI frame window to replace the window menu of the frame window or both.
        ''' </summary>
        ''' <remarks></remarks>
        WM_MDISETMENU = &H230

        ''' <summary>
        '''An application sends the WM_MDITILE message to a multiple-document interface (MDI) client window to arrange all of its MDI child windows in a tile format.
        ''' </summary>
        ''' <remarks></remarks>
        WM_MDITILE = &H226

        ''' <summary>
        '''The WM_MEASUREITEM message is sent to the owner window of a combo box list box list view control or menu item when the control or menu is created.
        ''' </summary>
        ''' <remarks></remarks>
        WM_MEASUREITEM = &H2C

        ''' <summary>
        '''The WM_MENUCHAR message is sent when a menu is active and the user presses a key that does not correspond to any mnemonic or accelerator key. This message is sent to the window that owns the menu.
        ''' </summary>
        ''' <remarks></remarks>
        WM_MENUCHAR = &H120

        ''' <summary>
        '''The WM_MENUCOMMAND message is sent when the user makes a selection from a menu.
        ''' </summary>
        ''' <remarks></remarks>
        WM_MENUCOMMAND = &H126

        ''' <summary>
        '''The WM_MENUDRAG message is sent to the owner of a drag-and-drop menu when the user drags a menu item.
        ''' </summary>
        ''' <remarks></remarks>
        WM_MENUDRAG = &H123

        ''' <summary>
        '''The WM_MENUGETOBJECT message is sent to the owner of a drag-and-drop menu when the mouse cursor enters a menu item or moves from the center of the item to the top or bottom of the item.
        ''' </summary>
        ''' <remarks></remarks>
        WM_MENUGETOBJECT = &H124

        ''' <summary>
        '''The WM_MENURBUTTONUP message is sent when the user releases the right mouse button while the cursor is on a menu item.
        ''' </summary>
        ''' <remarks></remarks>
        WM_MENURBUTTONUP = &H122

        ''' <summary>
        '''The WM_MENUSELECT message is sent to a menu's owner window when the user selects a menu item.
        ''' </summary>
        ''' <remarks></remarks>
        WM_MENUSELECT = &H11F

        ''' <summary>
        '''The WM_MOUSEACTIVATE message is sent when the cursor is in an inactive window and the user presses a mouse button. The parent window receives this message only if the child window passes it to the DefWindowProc function.
        ''' </summary>
        ''' <remarks></remarks>
        WM_MOUSEACTIVATE = &H21

        ''' <summary>
        '''Use WM_MOUSEFIRST to specify the first mouse message. Use the PeekMessage() Function.
        ''' </summary>
        ''' <remarks></remarks>
        WM_MOUSEFIRST = &H200

        ''' <summary>
        '''The WM_MOUSEHOVER message is posted to a window when the cursor hovers over the client area of the window for the period of time specified in a prior call to TrackMouseEvent.
        ''' </summary>
        ''' <remarks></remarks>
        WM_MOUSEHOVER = &H2A1

        ''' <summary>
        '''Definition Needed
        ''' </summary>
        ''' <remarks></remarks>
        WM_MOUSELAST = &H20D

        ''' <summary>
        '''The WM_MOUSELEAVE message is posted to a window when the cursor leaves the client area of the window specified in a prior call to TrackMouseEvent.
        ''' </summary>
        ''' <remarks></remarks>
        WM_MOUSELEAVE = &H2A3

        ''' <summary>
        '''The WM_MOUSEMOVE message is posted to a window when the cursor moves. If the mouse is not captured the message is posted to the window that contains the cursor. Otherwise the message is posted to the window that has captured the mouse.
        ''' </summary>
        ''' <remarks></remarks>
        WM_MOUSEMOVE = &H200

        ''' <summary>
        '''The WM_MOUSEWHEEL message is sent to the focus window when the mouse wheel is rotated. The DefWindowProc function propagates the message to the window's parent. There should be no internal forwarding of the message since DefWindowProc propagates it up the parent chain until it finds a window that processes it.
        ''' </summary>
        ''' <remarks></remarks>
        WM_MOUSEWHEEL = &H20A

        ''' <summary>
        '''The WM_MOUSEHWHEEL message is sent to the focus window when the mouse's horizontal scroll wheel is tilted or rotated. The DefWindowProc function propagates the message to the window's parent. There should be no internal forwarding of the message since DefWindowProc propagates it up the parent chain until it finds a window that processes it.
        ''' </summary>
        ''' <remarks></remarks>
        WM_MOUSEHWHEEL = &H20E

        ''' <summary>
        '''The WM_MOVE message is sent after a window has been moved.
        ''' </summary>
        ''' <remarks></remarks>
        WM_MOVE = &H3

        ''' <summary>
        '''The WM_MOVING message is sent to a window that the user is moving. By processing this message an application can monitor the position of the drag rectangle and if needed change its position.
        ''' </summary>
        ''' <remarks></remarks>
        WM_MOVING = &H216

        ''' <summary>
        '''Non Client Area Activated Caption(Title) of the Form
        ''' </summary>
        ''' <remarks></remarks>
        WM_NCACTIVATE = &H86

        ''' <summary>
        '''The WM_NCCALCSIZE message is sent when the size and position of a window's client area must be calculated. By processing this message an application can control the content of the window's client area when the size or position of the window changes.
        ''' </summary>
        ''' <remarks></remarks>
        WM_NCCALCSIZE = &H83

        ''' <summary>
        '''The WM_NCCREATE message is sent prior to the WM_CREATE message when a window is first created.
        ''' </summary>
        ''' <remarks></remarks>
        WM_NCCREATE = &H81

        ''' <summary>
        '''The WM_NCDESTROY message informs a window that its nonclient area is being destroyed. The DestroyWindow function sends the WM_NCDESTROY message to the window following the WM_DESTROY message. WM_DESTROY is used to free the allocated memory object associated with the window.
        ''' </summary>
        ''' <remarks></remarks>
        WM_NCDESTROY = &H82

        ''' <summary>
        '''The WM_NCHITTEST message is sent to a window when the cursor moves or when a mouse button is pressed or released. If the mouse is not captured the message is sent to the window beneath the cursor. Otherwise the message is sent to the window that has captured the mouse.
        ''' </summary>
        ''' <remarks></remarks>
        WM_NCHITTEST = &H84

        ''' <summary>
        '''The WM_NCLBUTTONDBLCLK message is posted when the user double-clicks the left mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse this message is not posted.
        ''' </summary>
        ''' <remarks></remarks>
        WM_NCLBUTTONDBLCLK = &HA3

        ''' <summary>
        '''The WM_NCLBUTTONDOWN message is posted when the user presses the left mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse this message is not posted.
        ''' </summary>
        ''' <remarks></remarks>
        WM_NCLBUTTONDOWN = &HA1

        ''' <summary>
        '''The WM_NCLBUTTONUP message is posted when the user releases the left mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse this message is not posted.
        ''' </summary>
        ''' <remarks></remarks>
        WM_NCLBUTTONUP = &HA2

        ''' <summary>
        '''The WM_NCMBUTTONDBLCLK message is posted when the user double-clicks the middle mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse this message is not posted.
        ''' </summary>
        ''' <remarks></remarks>
        WM_NCMBUTTONDBLCLK = &HA9

        ''' <summary>
        '''The WM_NCMBUTTONDOWN message is posted when the user presses the middle mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse this message is not posted.
        ''' </summary>
        ''' <remarks></remarks>
        WM_NCMBUTTONDOWN = &HA7

        ''' <summary>
        '''The WM_NCMBUTTONUP message is posted when the user releases the middle mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse this message is not posted.
        ''' </summary>
        ''' <remarks></remarks>
        WM_NCMBUTTONUP = &HA8

        ''' <summary>
        ''' The WM_NCMOUSELEAVE message is posted to a window when the cursor leaves the nonclient area of the window specified in a prior call to TrackMouseEvent.
        ''' </summary>
        WM_NCMOUSELEAVE = &H2A2

        ''' <summary>
        '''The WM_NCMOUSEMOVE message is posted to a window when the cursor is moved within the nonclient area of the window. This message is posted to the window that contains the cursor. If a window has captured the mouse this message is not posted.
        ''' </summary>
        ''' <remarks></remarks>
        WM_NCMOUSEMOVE = &HA0

        ''' <summary>
        '''The WM_NCPAINT message is sent to a window when its frame must be painted.
        ''' </summary>
        ''' <remarks></remarks>
        WM_NCPAINT = &H85

        ''' <summary>
        '''The WM_NCRBUTTONDBLCLK message is posted when the user double-clicks the right mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse this message is not posted.
        ''' </summary>
        ''' <remarks></remarks>
        WM_NCRBUTTONDBLCLK = &HA6

        ''' <summary>
        '''The WM_NCRBUTTONDOWN message is posted when the user presses the right mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse this message is not posted.
        ''' </summary>
        ''' <remarks></remarks>
        WM_NCRBUTTONDOWN = &HA4

        ''' <summary>
        '''The WM_NCRBUTTONUP message is posted when the user releases the right mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse this message is not posted.
        ''' </summary>
        ''' <remarks></remarks>
        WM_NCRBUTTONUP = &HA5

        ''' <summary>
        '''The WM_NEXTDLGCTL message is sent to a dialog box procedure to set the keyboard focus to a different control in the dialog box
        ''' </summary>
        ''' <remarks></remarks>
        WM_NEXTDLGCTL = &H28

        ''' <summary>
        '''The WM_NEXTMENU message is sent to an application when the right or left arrow key is used to switch between the menu bar and the system menu.
        ''' </summary>
        ''' <remarks></remarks>
        WM_NEXTMENU = &H213

        ''' <summary>
        '''Sent by a common control to its parent window when an event has occurred or the control requires some information.
        ''' </summary>
        ''' <remarks></remarks>
        WM_NOTIFY = &H4E

        ''' <summary>
        '''Determines if a window accepts ANSI or Unicode structures in the WM_NOTIFY notification message. WM_NOTIFYFORMAT messages are sent from a common control to its parent window and from the parent window to the common control.
        ''' </summary>
        ''' <remarks></remarks>
        WM_NOTIFYFORMAT = &H55

        ''' <summary>
        '''The WM_NULL message performs no operation. An application sends the WM_NULL message if it wants to post a message that the recipient window will ignore.
        ''' </summary>
        ''' <remarks></remarks>
        WM_NULL = &H0

        ''' <summary>
        '''Occurs when the control needs repainting
        ''' </summary>
        ''' <remarks></remarks>
        WM_PAINT = &HF

        ''' <summary>
        '''The WM_PAINTCLIPBOARD message is sent to the clipboard owner by a clipboard viewer window when the clipboard contains data in the CF_OWNERDISPLAY format and the clipboard viewer's client area needs repainting.
        ''' </summary>
        ''' <remarks></remarks>
        WM_PAINTCLIPBOARD = &H309

        ''' <summary>
        '''Windows NT 3.51 and earlier: The WM_PAINTICON message is sent to a minimized window when the icon is to be painted. This message is not sent by newer versions of Microsoft Windows except in unusual circumstances explained in the Remarks.
        ''' </summary>
        ''' <remarks></remarks>
        WM_PAINTICON = &H26

        ''' <summary>
        '''This message is sent by the OS to all top-level and overlapped windows after the window with the keyboard focus realizes its logical palette. This message enables windows that do not have the keyboard focus to realize their logical palettes and update their client areas.
        ''' </summary>
        ''' <remarks></remarks>
        WM_PALETTECHANGED = &H311

        ''' <summary>
        '''The WM_PALETTEISCHANGING message informs applications that an application is going to realize its logical palette.
        ''' </summary>
        ''' <remarks></remarks>
        WM_PALETTEISCHANGING = &H310

        ''' <summary>
        '''The WM_PARENTNOTIFY message is sent to the parent of a child window when the child window is created or destroyed or when the user clicks a mouse button while the cursor is over the child window. When the child window is being created the system sends WM_PARENTNOTIFY just before the CreateWindow or CreateWindowEx function that creates the window returns. When the child window is being destroyed the system sends the message before any processing to destroy the window takes place.
        ''' </summary>
        ''' <remarks></remarks>
        WM_PARENTNOTIFY = &H210

        ''' <summary>
        '''An application sends a WM_PASTE message to an edit control or combo box to copy the current content of the clipboard to the edit control at the current caret position. Data is inserted only if the clipboard contains data in CF_TEXT format.
        ''' </summary>
        ''' <remarks></remarks>
        WM_PASTE = &H302

        ''' <summary>
        '''Definition Needed
        ''' </summary>
        ''' <remarks></remarks>
        WM_PENWINFIRST = &H380

        ''' <summary>
        '''Definition Needed
        ''' </summary>
        ''' <remarks></remarks>
        WM_PENWINLAST = &H38F

        ''' <summary>
        '''Notifies applications that the system typically a battery-powered personal computer is about to enter a suspended mode. Obsolete : use POWERBROADCAST instead
        ''' </summary>
        ''' <remarks></remarks>
        WM_POWER = &H48

        ''' <summary>
        '''Notifies applications that a power-management event has occurred.
        ''' </summary>
        ''' <remarks></remarks>
        WM_POWERBROADCAST = &H218

        ''' <summary>
        '''The WM_PRINT message is sent to a window to request that it draw itself in the specified device context most commonly in a printer device context.
        ''' </summary>
        ''' <remarks></remarks>
        WM_PRINT = &H317

        ''' <summary>
        '''The WM_PRINTCLIENT message is sent to a window to request that it draw its client area in the specified device context most commonly in a printer device context.
        ''' </summary>
        ''' <remarks></remarks>
        WM_PRINTCLIENT = &H318

        ''' <summary>
        '''The WM_QUERYDRAGICON message is sent to a minimized (iconic) window. The window is about to be dragged by the user but does not have an icon defined for its class. An application can return a handle to an icon or cursor. The system displays this cursor or icon while the user drags the icon.
        ''' </summary>
        ''' <remarks></remarks>
        WM_QUERYDRAGICON = &H37

        ''' <summary>
        '''The WM_QUERYENDSESSION message is sent when the user chooses to end the session or when an application calls one of the system shutdown functions. If any application returns zero the session is not ended. The system stops sending WM_QUERYENDSESSION messages as soon as one application returns zero. After processing this message the system sends the WM_ENDSESSION message with the wParam parameter set to the results of the WM_QUERYENDSESSION message.
        ''' </summary>
        ''' <remarks></remarks>
        WM_QUERYENDSESSION = &H11

        ''' <summary>
        '''This message informs a window that it is about to receive the keyboard focus giving the window the opportunity to realize its logical palette when it receives the focus.
        ''' </summary>
        ''' <remarks></remarks>
        WM_QUERYNEWPALETTE = &H30F

        ''' <summary>
        '''The WM_QUERYOPEN message is sent to an icon when the user requests that the window be restored to its previous size and position.
        ''' </summary>
        ''' <remarks></remarks>
        WM_QUERYOPEN = &H13

        ''' <summary>
        '''The WM_QUEUESYNC message is sent by a computer-based training (CBT) application to separate user-input messages from other messages sent through the WH_JOURNALPLAYBACK Hook procedure.
        ''' </summary>
        ''' <remarks></remarks>
        WM_QUEUESYNC = &H23

        ''' <summary>
        '''Once received it ends the application's Message Loop signaling the application to end. It can be sent by pressing Alt+F4 Clicking the X in the upper right-hand of the program or going to File->Exit.
        ''' </summary>
        ''' <remarks></remarks>
        WM_QUIT = &H12

        ''' <summary>
        '''he WM_RBUTTONDBLCLK message is posted when the user double-clicks the right mouse button while the cursor is in the client area of a window. If the mouse is not captured the message is posted to the window beneath the cursor. Otherwise the message is posted to the window that has captured the mouse.
        ''' </summary>
        ''' <remarks></remarks>
        WM_RBUTTONDBLCLK = &H206

        ''' <summary>
        '''The WM_RBUTTONDOWN message is posted when the user presses the right mouse button while the cursor is in the client area of a window. If the mouse is not captured the message is posted to the window beneath the cursor. Otherwise the message is posted to the window that has captured the mouse.
        ''' </summary>
        ''' <remarks></remarks>
        WM_RBUTTONDOWN = &H204

        ''' <summary>
        '''The WM_RBUTTONUP message is posted when the user releases the right mouse button while the cursor is in the client area of a window. If the mouse is not captured the message is posted to the window beneath the cursor. Otherwise the message is posted to the window that has captured the mouse.
        ''' </summary>
        ''' <remarks></remarks>
        WM_RBUTTONUP = &H205

        ''' <summary>
        '''The WM_RENDERALLFORMATS message is sent to the clipboard owner before it is destroyed if the clipboard owner has delayed rendering one or more clipboard formats. For the content of the clipboard to remain available to other applications the clipboard owner must render data in all the formats it is capable of generating and place the data on the clipboard by calling the SetClipboardData function.
        ''' </summary>
        ''' <remarks></remarks>
        WM_RENDERALLFORMATS = &H306

        ''' <summary>
        '''The WM_RENDERFORMAT message is sent to the clipboard owner if it has delayed rendering a specific clipboard format and if an application has requested data in that format. The clipboard owner must render data in the specified format and place it on the clipboard by calling the SetClipboardData function.
        ''' </summary>
        ''' <remarks></remarks>
        WM_RENDERFORMAT = &H305

        ''' <summary>
        '''The WM_SETCURSOR message is sent to a window if the mouse causes the cursor to move within a window and mouse input is not captured.
        ''' </summary>
        ''' <remarks></remarks>
        WM_SETCURSOR = &H20

        ''' <summary>
        '''When the controll got the focus
        ''' </summary>
        ''' <remarks></remarks>
        WM_SETFOCUS = &H7

        ''' <summary>
        '''An application sends a WM_SETFONT message to specify the font that a control is to use when drawing text.
        ''' </summary>
        ''' <remarks></remarks>
        WM_SETFONT = &H30

        ''' <summary>
        '''An application sends a WM_SETHOTKEY message to a window to associate a hot key with the window. When the user presses the hot key the system activates the window.
        ''' </summary>
        ''' <remarks></remarks>
        WM_SETHOTKEY = &H32

        ''' <summary>
        '''An application sends the WM_SETICON message to associate a new large or small icon with a window. The system displays the large icon in the ALT+TAB dialog box and the small icon in the window caption.
        ''' </summary>
        ''' <remarks></remarks>
        WM_SETICON = &H80

        ''' <summary>
        '''An application sends the WM_SETREDRAW message to a window to allow changes in that window to be redrawn or to prevent changes in that window from being redrawn.
        ''' </summary>
        ''' <remarks></remarks>
        WM_SETREDRAW = &HB

        ''' <summary>
        '''Text / Caption changed on the control. An application sends a WM_SETTEXT message to set the text of a window.
        ''' </summary>
        ''' <remarks></remarks>
        WM_SETTEXT = &HC

        ''' <summary>
        '''An application sends the WM_SETTINGCHANGE message to all top-level windows after making a change to the WIN.INI file. The SystemParametersInfo function sends this message after an application uses the function to change a setting in WIN.INI.
        ''' </summary>
        ''' <remarks></remarks>
        WM_SETTINGCHANGE = &H1A

        ''' <summary>
        '''The WM_SHOWWINDOW message is sent to a window when the window is about to be hidden or shown
        ''' </summary>
        ''' <remarks></remarks>
        WM_SHOWWINDOW = &H18

        ''' <summary>
        '''The WM_SIZE message is sent to a window after its size has changed.
        ''' </summary>
        ''' <remarks></remarks>
        WM_SIZE = &H5

        ''' <summary>
        '''The WM_SIZECLIPBOARD message is sent to the clipboard owner by a clipboard viewer window when the clipboard contains data in the CF_OWNERDISPLAY format and the clipboard viewer's client area has changed size.
        ''' </summary>
        ''' <remarks></remarks>
        WM_SIZECLIPBOARD = &H30B

        ''' <summary>
        '''The WM_SIZING message is sent to a window that the user is resizing. By processing this message an application can monitor the size and position of the drag rectangle and if needed change its size or position.
        ''' </summary>
        ''' <remarks></remarks>
        WM_SIZING = &H214

        ''' <summary>
        '''The WM_SPOOLERSTATUS message is sent from Print Manager whenever a job is added to or removed from the Print Manager queue.
        ''' </summary>
        ''' <remarks></remarks>
        WM_SPOOLERSTATUS = &H2A

        ''' <summary>
        '''The WM_STYLECHANGED message is sent to a window after the SetWindowLong function has changed one or more of the window's styles.
        ''' </summary>
        ''' <remarks></remarks>
        WM_STYLECHANGED = &H7D

        ''' <summary>
        '''The WM_STYLECHANGING message is sent to a window when the SetWindowLong function is about to change one or more of the window's styles.
        ''' </summary>
        ''' <remarks></remarks>
        WM_STYLECHANGING = &H7C

        ''' <summary>
        '''The WM_SYNCPAINT message is used to synchronize painting while avoiding linking independent GUI threads.
        ''' </summary>
        ''' <remarks></remarks>
        WM_SYNCPAINT = &H88

        ''' <summary>
        '''The WM_SYSCHAR message is posted to the window with the keyboard focus when a WM_SYSKEYDOWN message is translated by the TranslateMessage function. It specifies the character code of a system character key — that is a character key that is pressed while the ALT key is down.
        ''' </summary>
        ''' <remarks></remarks>
        WM_SYSCHAR = &H106

        ''' <summary>
        '''This message is sent to all top-level windows when a change is made to a system color setting.
        ''' </summary>
        ''' <remarks></remarks>
        WM_SYSCOLORCHANGE = &H15

        ''' <summary>
        '''A window receives this message when the user chooses a command from the Window menu (formerly known as the system or control menu) or when the user chooses the maximize button minimize button restore button or close button.
        ''' </summary>
        ''' <remarks></remarks>
        WM_SYSCOMMAND = &H112

        ''' <summary>
        '''The WM_SYSDEADCHAR message is sent to the window with the keyboard focus when a WM_SYSKEYDOWN message is translated by the TranslateMessage function. WM_SYSDEADCHAR specifies the character code of a system dead key — that is a dead key that is pressed while holding down the ALT key.
        ''' </summary>
        ''' <remarks></remarks>
        WM_SYSDEADCHAR = &H107

        ''' <summary>
        '''The WM_SYSKEYDOWN message is posted to the window with the keyboard focus when the user presses the F10 key (which activates the menu bar) or holds down the ALT key and then presses another key. It also occurs when no window currently has the keyboard focus; in this case the WM_SYSKEYDOWN message is sent to the active window. The window that receives the message can distinguish between these two contexts by checking the context code in the lParam parameter.
        ''' </summary>
        ''' <remarks></remarks>
        WM_SYSKEYDOWN = &H104

        ''' <summary>
        '''The WM_SYSKEYUP message is posted to the window with the keyboard focus when the user releases a key that was pressed while the ALT key was held down. It also occurs when no window currently has the keyboard focus; in this case the WM_SYSKEYUP message is sent to the active window. The window that receives the message can distinguish between these two contexts by checking the context code in the lParam parameter.
        ''' </summary>
        ''' <remarks></remarks>
        WM_SYSKEYUP = &H105

        ''' <summary>
        '''Sent to an application that has initiated a training card with Microsoft Windows Help. The message informs the application when the user clicks an authorable button. An application initiates a training card by specifying the HELP_TCARD command in a call to the WinHelp function.
        ''' </summary>
        ''' <remarks></remarks>
        WM_TCARD = &H52

        ''' <summary>
        '''A message that is sent whenever there is a change in the system time.
        ''' </summary>
        ''' <remarks></remarks>
        WM_TIMECHANGE = &H1E

        ''' <summary>
        '''The WM_TIMER message is posted to the installing thread's message queue when a timer expires. The message is posted by the GetMessage or PeekMessage function.
        ''' </summary>
        ''' <remarks></remarks>
        WM_TIMER = &H113

        ''' <summary>
        '''An application sends a WM_UNDO message to an edit control to undo the last operation. When this message is sent to an edit control the previously deleted text is restored or the previously added text is deleted.
        ''' </summary>
        ''' <remarks></remarks>
        WM_UNDO = &H304

        ''' <summary>
        '''The WM_UNINITMENUPOPUP message is sent when a drop-down menu or submenu has been destroyed.
        ''' </summary>
        ''' <remarks></remarks>
        WM_UNINITMENUPOPUP = &H125

        ''' <summary>
        '''The WM_USER constant is used by applications to help define private messages for use by private window classes usually of the form WM_USER+X where X is an integer value.
        ''' </summary>
        ''' <remarks></remarks>
        WM_USER = &H400

        ''' <summary>
        '''The WM_USERCHANGED message is sent to all windows after the user has logged on or off. When the user logs on or off the system updates the user-specific settings. The system sends this message immediately after updating the settings.
        ''' </summary>
        ''' <remarks></remarks>
        WM_USERCHANGED = &H54

        ''' <summary>
        '''Sent by a list box with the LBS_WANTKEYBOARDINPUT style to its owner in response to a WM_KEYDOWN message.
        ''' </summary>
        ''' <remarks></remarks>
        WM_VKEYTOITEM = &H2E

        ''' <summary>
        '''The WM_VSCROLL message is sent to a window when a scroll event occurs in the window's standard vertical scroll bar. This message is also sent to the owner of a vertical scroll bar control when a scroll event occurs in the control.
        ''' </summary>
        ''' <remarks></remarks>
        WM_VSCROLL = &H115

        ''' <summary>
        '''The WM_VSCROLLCLIPBOARD message is sent to the clipboard owner by a clipboard viewer window when the clipboard contains data in the CF_OWNERDISPLAY format and an event occurs in the clipboard viewer's vertical scroll bar. The owner should scroll the clipboard image and update the scroll bar values.
        ''' </summary>
        ''' <remarks></remarks>
        WM_VSCROLLCLIPBOARD = &H30A

        ''' <summary>
        '''The WM_WINDOWPOSCHANGED message is sent to a window whose size position or place in the Z order has changed as a result of a call to the SetWindowPos function or another window-management function.
        ''' </summary>
        ''' <remarks></remarks>
        WM_WINDOWPOSCHANGED = &H47

        ''' <summary>
        '''The WM_WINDOWPOSCHANGING message is sent to a window whose size position or place in the Z order is about to change as a result of a call to the SetWindowPos function or another window-management function.
        ''' </summary>
        ''' <remarks></remarks>
        WM_WINDOWPOSCHANGING = &H46

        ''' <summary>
        '''An application sends the WM_WININICHANGE message to all top-level windows after making a change to the WIN.INI file. The SystemParametersInfo function sends this message after an application uses the function to change a setting in WIN.INI. Note The WM_WININICHANGE message is provided only for compatibility with earlier versions of the system. Applications should use the WM_SETTINGCHANGE message.
        ''' </summary>
        ''' <remarks></remarks>
        WM_WININICHANGE = &H1A

        ''' <summary>
        '''Definition Needed
        ''' </summary>
        ''' <remarks></remarks>
        WM_XBUTTONDBLCLK = &H20D

        ''' <summary>
        '''Definition Needed
        ''' </summary>
        ''' <remarks></remarks>
        WM_XBUTTONDOWN = &H20B

        ''' <summary>
        '''Definition Needed
        ''' </summary>
        ''' <remarks></remarks>
        WM_XBUTTONUP = &H20C
    End Enum

End Class
