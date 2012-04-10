' Translated to vb.net from http://www.xoc.net/works/tips/folderbrowserdialog.asp
Imports System.Reflection
Friend Class clsFBExtension
    Protected Friend NotInheritable Class FolderBrowserDialogEx
        <Flags()> _
        Friend Enum CsIdl
            Desktop = &H0
            Internet = &H1
            Programs = &H2
            Controls = &H3
            Printers = &H4
            Personal = &H5
            Favorites = &H6
            Startup = &H7
            Recent = &H8
            SendTo = &H9
            BitBucket = &HA
            StartMenu = &HB
            MyDocuments = &HC
            MyMusic = &HD
            MyVideo = &HE
            DesktopDirectory = &H10
            Drives = &H11
            Network = &H12
            Nethood = &H13
            Fonts = &H14
            Templates = &H15
            CommonStartMenu = &H16
            CommonPrograms = &H17
            CommonStartup = &H18
            CommonDesktopDirectory = &H19
            AppData = &H1A
            PrintHood = &H1B
            LocalAppData = &H1C
            AltStartup = &H1D
            CommonAltStartup = &H1E
            CommonFavorites = &H1F
            InternetCache = &H20
            Cookies = &H21
            History = &H22
            CommonAppdata = &H23
            Windows = &H24
            System = &H25
            ProgramFiles = &H26
            MyPictures = &H27
            Profile = &H28
            SystemX86 = &H29
            ProgramFilesX86 = &H2A
            ProgramFilesCommon = &H2B
            ProgramFilesCommonx86 = &H2C
            CommonTemplates = &H2D
            CommonDocuments = &H2E
            CommonAdminTools = &H2F
            AdminTools = &H30
            Connections = &H31
            CommonMusic = &H35
            CommonPictures = &H36
            CommonVideo = &H37
            Resources = &H38
            ResourcesLocalized = &H39
            CommonOemLinks = &H3A
            CdBurnArea = &H3B
            ComputersNearMe = &H3D
            FlagCreate = &H8000
            FlagDontVerify = &H4000
            FlagNoAlias = &H1000
            FlagPerUserInit = &H800
            FlagMask = &HFF00
        End Enum

        Private Sub New()
            MyBase.New()
        End Sub

        Friend Shared Sub SetRootFolder(ByVal fbd As System.Windows.Forms.FolderBrowserDialog, ByVal csidl As CsIdl)
            Dim t As Type = fbd.GetType
            Dim fi As FieldInfo = t.GetField("rootFolder", (BindingFlags.Instance Or BindingFlags.NonPublic))
            fi.SetValue(fbd, CType(csidl, System.Environment.SpecialFolder))
        End Sub
    End Class
End Class
