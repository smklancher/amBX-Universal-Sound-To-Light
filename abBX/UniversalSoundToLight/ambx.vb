Imports System.Runtime.InteropServices
Imports System.Threading
Imports System.Threading.Tasks


'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
' amBXLibrary - http://www.vbfengshui.com
' Copyright (c) 2010 
' by Darin Higgins
' Portions of this code converted to VB.net from the original C amBX SDK files
' available at http://developer.ambx.com/
'
' Allows VB.net code to easily communicate with the amBX lighting and effects system
' by Phillips. You will need the amBX drivers installed. And it would be very beneficial 
' to have the amBX SDK installed. Both are readily available via www.ambx.com
'
' Original version available at www.vbfengshui.com
'
' This software is licensed under the Microsoft Reciprocal License (Ms-RL).
'
' Please see http://www.opensource.org/licenses/ms-rl.html for details
'
' If you wish to use this code in a commercial application, please let me hear about it!
' If you wish to modify it, improve it or correct bugs, please let me know as well.
'
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


Namespace amBXLibrary

#Region " Enumerations"
    ''' <summary>
    ''' Waveforms that are available for rumble devices
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum RumbleWaveForms
        amBX_boing
        amBX_crash
        amBX_engine
        amBX_explosion
        amBX_hit
        amBX_quake
        amBX_rattle
        amBX_road
        amBX_shot
        amBX_thud
        amBX_thunder
    End Enum


    'The location of a device, represented as compass points, where N is in front 
    'of the user and S is behind the user - see \ref page_locs_and_heights 
    Public Enum Locations
        Everywhere = 0     '< amBX Location considered to be all Locations
        North = 1          '< amBX Location North 
        NorthEast = 2      '< amBX Location North-East 
        East = 4           '< amBX Location East 
        SouthEast = 8      '< amBX Location South-East 
        South = 16         '< amBX Location South 
        SouthWest = 32     '< amBX Location South-West 
        West = 64          '< amBX Location West 
        NorthWest = 128    '< amBX Location North-West 
        Center = 256       '< amBX Location Center 
    End Enum


    ''' <summary>
    ''' The height of a device relative to the user's head - 
    ''' see \ref page_locs_and_heights. 
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum Heights
        AnyHeight = 0      '< amBX Height considered to be Any Height 
        EveryHeight = 1    '< amBX Height considered to be Every Height 
        Top = 2            '< amBX Height Top 
        Middle = 4         '< amBX Height Middle 
        Bottom = 8         '< amBX Height Bottom 
    End Enum


    ''' <summary>
    ''' The thread types available for use by amBX.
    ''' Currently, there's only one type.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum amBX_ThreadType
        amBX_Ambient_Update = 0
    End Enum


    ''' <summary>
    ''' This is the type returned from almost all amBX calls - 
    ''' amBX_OK means the call was successful, any other result is an error.
    ''' 
    ''' Generally speaking, User Code won't have to deal with these error codes
    ''' as matching exceptions are generated when one of these error codes are
    ''' detected.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum amBX_RESULT
        amBX_OK = 0                     'OK 
        amBX_NO_SPACE = 1               'out of buffer space 
        amBX_ERROR = 2                  'generic error 
        amBX_NOT_FOUND = 3              'no file/device etc. found 
        amBX_VERSION_NOT_FOUND = 4
        'The requested version of amBX API is not available

        amBX_BAD_ARGS = 5
        'arguments passed in bad (usually null pointers passed in) 

        amBX_OUT_OF_RANGE = 6
        'Value given is beyond usable range using default values if possible 

        amBX_OUT_OF_MEMORY = 7
        'couldn't allocate memory 

        amBX_NOT_INSTALLED = 8
        'amBX isn't installed on the PC 

        amBX_OLD_VERSION = 9
        'amBX installed, but need newer version 

        amBX_ENGINE_LOST = 10
        'connection to amBX Engine currently inoperative 

        amBX_SENDING_TIMEOUT = 11
        'request to send a script timed out 

        amBX_NOT_THREADED = 12
        'returned by a threaded function if threading is 
        'no being used for the purpose specified 

        amBX_BAD_THREADID = 13
        'returned by a function if the thread ID is 
        'incorrect or doesn't exist. The thread with the
        'ID could have been recently removed 

        amBX_THREAD_EXISTS = 14
        'returned by a threaded function if a thread is
        'currently being used for that function 

        amBX_UPDATE_TIMEOUT = 15
        'returned if waiting to perform an update
        'times out. This could be waiting for a thread
        'to finish or waiting for a critical section 

        amBX_THREAD_TIMEOUT = 16
        'returned if the request to run a thread times
        'out. This could be because another thread is
        'already being run or another thread is using the
        'resources needed to run the thread
    End Enum


    ''' <summary>
    ''' The state of a particular amBX object.
    ''' Objects can be enabled, disabled or in the process of being
    ''' enabled or disabled.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum amBX_EnabledStates
        ENABLED = 0         'amBX Currently Enabled
        DISABLED            'amBX Currently Disabled 
        ENABLING            'amBX in the process of becoming Enabled 
        DISABLING           'amBX in the process of becoming Disabled
    End Enum

#End Region


#Region " Structures"
    ''' <summary>
    ''' Floating point color structure. Simplifies working with amBX style
    ''' floating point color values.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class amBXColor
        ''' <summary>
        ''' The floating point Red value (between 0 and 1).
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property R() As Single
            Get
                Return _R
            End Get
            Set(ByVal value As Single)
                _R = Utility.ConstrainFloat(value)
            End Set
        End Property
        Private _R As Single


        ''' <summary>
        ''' The floating point Blue value (between 0 and 1).
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property B() As Single
            Get
                Return _B
            End Get
            Set(ByVal value As Single)
                _B = Utility.ConstrainFloat(value)
            End Set
        End Property
        Private _B As Single


        ''' <summary>
        ''' The floating point Green value (between 0 and 1).        
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property G() As Single
            Get
                Return _G
            End Get
            Set(ByVal value As Single)
                _G = Utility.ConstrainFloat(value)
            End Set
        End Property
        Private _G As Single


        ''' <summary>
        ''' Construct a new amBX Color object.
        ''' </summary>
        ''' <param name="Red"></param>
        ''' <param name="Green"></param>
        ''' <param name="Blue"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal Red As Single, ByVal Green As Single, ByVal Blue As Single)
            Me.R = Red
            Me.G = Green
            Me.B = Blue
        End Sub


        ''' <summary>
        ''' Convert an amBXColor into a normal Color object.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function toColor() As Color
            Return Color.FromArgb(Me.R * 255, Me.G * 255, Me.B * 255)
        End Function
    End Class
#End Region


    ''' <summary>
    ''' Core ambx Class. This class is static and cannot be instantiated.
    ''' It represents the core amBX device and as such, there can only ever be one
    ''' to a machine.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class amBX

        ''' <summary>
        ''' Class is not publicly creatable
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub New()
        End Sub


#Region " Declarations"
        ''' <summary>
        ''' The function exported from the amBX runtime dll to initiate all 
        ''' communication with the amBX drivers.
        ''' The amBXrt.dll file is included with the amBX SDK.
        ''' </summary>
        ''' <param name="IamBXPtr"></param>
        ''' <param name="Major"></param>
        ''' <param name="Minor"></param>
        ''' <param name="AppName"></param>
        ''' <param name="AppVer"></param>
        ''' <param name="Memptr"></param>
        ''' <param name="UsingThreads"></param>
        ''' <returns></returns>
        ''' <remarks>SMK 2011-11-27 Added CallingConvention:=CallingConvention.Cdecl to prevent pInvokeStackImbalance MDA</remarks>
        <DllImport("ambxrt.dll", ExactSpelling:=True, CharSet:=CharSet.Auto, CallingConvention:=CallingConvention.Cdecl)> _
        Private Shared Function amBXCreateInterface(ByRef IamBXPtr As IntPtr, ByVal Major As UInt32, ByVal Minor As UInt32, _
                                                    ByVal AppName As String, ByVal AppVer As String, ByVal Memptr As Integer, ByVal UsingThreads As Boolean) As Integer
        End Function
#End Region

#Region " Structures"
        ''' <summary>
        ''' This structure wraps the IamBX interface and its pointer
        ''' so it can be readily used when calling into the low level
        ''' amBX interface function pointers.
        ''' </summary>
        ''' <remarks></remarks>
        Private Structure IamBX
            Public IamBXPtr As IntPtr
            Public IamBXInterface As IamBXInterface
            Public IamBXDelegates As IamBXDelegates
        End Structure


        ''' <summary>
        ''' This structure matches the layout of the C amBX class
        ''' as an array of function pointers (the order is significant and must match!)
        ''' </summary>
        ''' <remarks></remarks>
        <StructLayout(LayoutKind.Sequential)> _
        Private Structure IamBXInterface
            Public ReleasePtr As IntPtr
            Public CreateLightPtr As IntPtr
            Public CreateFanPtr As IntPtr
            Public CreateRumblePtr As IntPtr
            Public CreateMoviePtr As IntPtr
            Public CreateEventPtr As IntPtr
            Public SetAllEnabledPtr As IntPtr
            Public UpdatePtr As IntPtr
            Public GetVersionInfoPtr As IntPtr
            Public RunThreadPtr As IntPtr
            Public StopThreadPtr As IntPtr
        End Structure


        ''' <summary>
        ''' Holds Delegates to all the functions pointed to by the pointers in
        ''' IamBXInterface.
        ''' This makes them easy to call via .net.
        ''' </summary>
        ''' <remarks></remarks>
        Private Structure IamBXDelegates
            Public Release As ReleaseDelegate
            Public CreateLight As CreateLightDelegate
            Public CreateFan As CreateFanDelegate
            Public CreateRumble As CreateRumbleDelegate
            Public CreateMovie As CreateMovieDelegate
            Public CreateEvent As CreateEventDelegate
            Public SetAllEnabled As SetAllEnabledDelegate
            Public Update As UpdateDelegate
            Public GetVersionInfo As GetVersionInfoDelegate
            Public RunThread As RunThreadDelegate
            Public StopThread As StopThreadDelegate


            Public Sub Generate(ByVal IambxInterface As IamBXInterface)
                Me.Release = Marshal.GetDelegateForFunctionPointer(IambxInterface.ReleasePtr, GetType(ReleaseDelegate))
                Me.CreateLight = Marshal.GetDelegateForFunctionPointer(IambxInterface.CreateLightPtr, GetType(CreateLightDelegate))
                Me.CreateFan = Marshal.GetDelegateForFunctionPointer(IambxInterface.CreateFanPtr, GetType(CreateFanDelegate))
                Me.CreateRumble = Marshal.GetDelegateForFunctionPointer(IambxInterface.CreateRumblePtr, GetType(CreateRumbleDelegate))
                Me.CreateMovie = Marshal.GetDelegateForFunctionPointer(IambxInterface.CreateMoviePtr, GetType(CreateMovieDelegate))
                Me.CreateEvent = Marshal.GetDelegateForFunctionPointer(IambxInterface.CreateEventPtr, GetType(CreateEventDelegate))
                Me.SetAllEnabled = Marshal.GetDelegateForFunctionPointer(IambxInterface.SetAllEnabledPtr, GetType(SetAllEnabledDelegate))
                Me.Update = Marshal.GetDelegateForFunctionPointer(IambxInterface.UpdatePtr, GetType(UpdateDelegate))
                Me.GetVersionInfo = Marshal.GetDelegateForFunctionPointer(IambxInterface.GetVersionInfoPtr, GetType(GetVersionInfoDelegate))
                Me.RunThread = Marshal.GetDelegateForFunctionPointer(IambxInterface.RunThreadPtr, GetType(RunThreadDelegate))
                Me.StopThread = Marshal.GetDelegateForFunctionPointer(IambxInterface.StopThreadPtr, GetType(StopThreadDelegate))
            End Sub


            <UnmanagedFunctionPointer(CallingConvention.Cdecl)> _
            Public Delegate Function ReleaseDelegate(ByVal IamBXPtr As IntPtr) As amBX_RESULT

            <UnmanagedFunctionPointer(CallingConvention.Cdecl)> _
            Public Delegate Function CreateLightDelegate(ByVal IamBXPtr As IntPtr, _
                                             ByVal Location As Locations, _
                                             ByVal Height As Heights, _
                                             ByRef IamBXLightPtr As IntPtr) As amBX_RESULT

            <UnmanagedFunctionPointer(CallingConvention.Cdecl)> _
            Public Delegate Function CreateFanDelegate(ByVal IamBXPtr As IntPtr, _
                             ByVal Location As Locations, _
                             ByVal Height As Heights, _
                             ByRef amBXFanPtr As IntPtr) As amBX_RESULT


            <UnmanagedFunctionPointer(CallingConvention.Cdecl)> _
            Public Delegate Function CreateRumbleDelegate(ByVal IamBXPtr As IntPtr, _
                             ByVal Location As Locations, _
                             ByVal Height As Heights, _
                             ByRef amBXRumble As IntPtr) As amBX_RESULT

            <UnmanagedFunctionPointer(CallingConvention.Cdecl)> _
            Public Delegate Function CreateMovieDelegate(ByVal IamBXPtr As IntPtr, _
                            <MarshalAs(UnmanagedType.LPArray)> ByVal File() As Byte, _
                            ByVal Size As Integer, _
                            ByRef MoviePtr As IntPtr) As amBX_RESULT

            <UnmanagedFunctionPointer(CallingConvention.Cdecl)> _
            Public Delegate Function CreateEventDelegate(ByVal IamBXPtr As IntPtr, _
                            <MarshalAs(UnmanagedType.LPArray)> ByVal File() As Byte, _
                            ByVal Size As Integer, _
                            ByRef EventPtr As IntPtr) As amBX_RESULT

            <UnmanagedFunctionPointer(CallingConvention.Cdecl)> _
            Public Delegate Function SetAllEnabledDelegate(ByVal IamBXPtr As IntPtr, _
                                                           <MarshalAs(UnmanagedType.I1)> ByVal Enabled As Boolean) As amBX_RESULT

            <UnmanagedFunctionPointer(CallingConvention.Cdecl)> _
            Public Delegate Function UpdateDelegate(ByVal IamBXPtr As IntPtr, _
                                                    ByVal MaxWaitInMilliseconds As Integer) As amBX_RESULT

            <UnmanagedFunctionPointer(CallingConvention.Cdecl)> _
            Public Delegate Function GetVersionInfoDelegate(ByVal IamBXPtr As IntPtr, _
                                                            ByRef Major As Integer, _
                                                            ByRef Minor As Integer, _
                                                            ByRef Revision As Integer, _
                                                            ByRef Build As Integer) As amBX_RESULT

            <UnmanagedFunctionPointer(CallingConvention.Cdecl)> _
            Public Delegate Function RunThreadDelegate(ByVal IamBXPtr As IntPtr, _
                                                       ByVal ThreadType As amBX_ThreadType, _
                                                       ByVal ThreadID As Integer) As amBX_RESULT

            <UnmanagedFunctionPointer(CallingConvention.Cdecl)> _
            Public Delegate Function StopThreadDelegate(ByVal IamBXPtr As IntPtr, _
                                                        ByVal ThreadID As Integer) As amBX_RESULT
        End Structure
#End Region


        ''' <summary>
        ''' Called when you want to open a channel of communication to 
        ''' the amBX drivers without "UsingThreading". Calling it more than once will have no effect.
        ''' Call DISCONNECT to shut down the connection
        ''' </summary>
        ''' <param name="MajorVersion">The Major version of the required amBX library.</param>
        ''' <param name="MinorVersion">The Minor version of the required amBX library.</param>
        ''' <param name="AppName">The name of your application.</param>
        ''' <param name="AppVersion">The version of your application.</param>
        ''' <remarks></remarks>
        Public Shared Sub Connect(ByVal MajorVersion As Integer, ByVal MinorVersion As Integer, ByVal AppName As String, ByVal AppVersion As String)
            '---- call into the ambx runtime to get a reference
            If Not amBX.IsConnected Then
                'SMK 2011-11-24 - Connect to driver with "UsingThreading" false
                'This still uses a separate thread, but updates happen automatically
                ConnectInternal(MajorVersion, MinorVersion, AppName, AppVersion, False)

                '---- default to 20 updates per second
                amBX.UpdatesPerSecond = 20  'this also starts the the update thread
            End If
        End Sub


        ''' <summary>
        ''' Called when you want to open a channel of communication to 
        ''' the amBX drivers "UsingThreading". Calling it more than once will have no effect.
        ''' Call DISCONNECT to shut down the connection
        ''' </summary>
        ''' <param name="MajorVersion">The Major version of the required amBX library.</param>
        ''' <param name="MinorVersion">The Minor version of the required amBX library.</param>
        ''' <param name="AppName">The name of your application.</param>
        ''' <param name="AppVersion">The version of your application.</param>
        ''' <remarks>
        ''' SMK 2011-11-24 - Separate function for threading so these changes can be a 
        ''' drop-in replacement for the original ambx.vb without breaking original functions.
        ''' </remarks>
        Public Shared Sub ConnectThreading(ByVal MajorVersion As Integer, ByVal MinorVersion As Integer, ByVal AppName As String, ByVal AppVersion As String)
            '---- call into the ambx runtime to get a reference
            If Not amBX.IsConnected Then
                'Connect using threading
                ConnectInternal(MajorVersion, MinorVersion, AppName, AppVersion, True)

                'start the task which will be the thread that responds to calls to update
                UpdateTask = Task.Factory.StartNew(AddressOf TaskFunc, TaskCreationOptions.LongRunning)
            End If
        End Sub

        Private Shared UpdateTask As Task
        Private Shared ThreadStopped As Boolean = False

        Private Shared Sub TaskFunc()
            Thread.CurrentThread.IsBackground = True
            Thread.CurrentThread.Name = "amBX RunThread"

            RunThread(amBX_ThreadType.amBX_Ambient_Update, Thread.CurrentThread.ManagedThreadId)

            Thread.VolatileWrite(ThreadStopped, True)
        End Sub




        ''' <summary>
        ''' Called when you want to open a channel of communication to 
        ''' the amBX drivers. Calling it more than once will have no effect.
        ''' Call DISCONNECT to shut down the connection
        ''' </summary>
        ''' <param name="MajorVersion">The Major version of the required amBX library.</param>
        ''' <param name="MinorVersion">The Minor version of the required amBX library.</param>
        ''' <param name="AppName">The name of your application.</param>
        ''' <param name="AppVersion">The version of your application.</param>
        ''' <remarks></remarks>
        Private Shared Sub ConnectInternal(ByVal MajorVersion As Integer, ByVal MinorVersion As Integer, ByVal AppName As String, ByVal AppVersion As String, ByVal UsingThreads As Boolean)
            '---- call into the ambx runtime to get a reference
            If Not amBX.IsConnected Then
                '---- first create an intptr, this is what the amBXCreateInterface will give us back
                '     ie a ptr to a block of memory that ambx is managing that contains
                '     an array of function pointers
                Try
                    amBXCreateInterface(_IamBX.IamBXPtr, MajorVersion, MinorVersion, AppName, AppVersion, Nothing, UsingThreads)

                Catch ex As DllNotFoundException
                    '---- throw a specific file not found error in this case
                    Throw New amBXExceptionamBXrtdllNotFound("Failed loading ambxrt.dll")

                    '---- any other errors will throw out of this sub
                End Try

                '---- now, copy those function pointers to out IamBX object
                '     since the layouts match, the pointers should end up in the right slots
                '     automatically
                _IamBX.IamBXInterface = Marshal.PtrToStructure(_IamBX.IamBXPtr, GetType(IamBXInterface))

                '---- now create all the delegates for this interface
                _IamBX.IamBXDelegates.Generate(_IamBX.IamBXInterface)

            End If
        End Sub





        Private Shared _IamBX As IamBX
        Private Shared _UpdateThread As Thread


        ''' <summary>
        ''' Returns whether we've successfully connected to the amBX drivers
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property IsConnected() As Boolean
            Get
                Return _IamBX.IamBXPtr <> 0
            End Get
        End Property


        ''' <summary>
        ''' Verify that we have a valid amBX connection before attempting
        ''' any operations.
        ''' </summary>
        ''' <remarks></remarks>
        Private Shared Sub CheckConnected()
            If Not amBX.IsConnected Then Throw New amBXExceptionNotConnected
        End Sub


        ''' <summary>
        ''' Private routine run on a worker thread to constantly update amBX
        ''' </summary>
        ''' <remarks></remarks>
        Private Shared Sub UpdateThreadRun()
            CheckConnected()
            Try
                Do
                    If amBX.UpdatesPerSecond > 0 Then
                        Dim start = DateTime.Now
                        amBX.Update(0)
                        Dim duration As TimeSpan = DateTime.Now - start
                        Thread.Sleep(Math.Max(0, (1000 / amBX.UpdatesPerSecond) - duration.Milliseconds))
                    Else
                        '---- bail out of this thread
                        Exit Do
                    End If
                Loop
            Catch ex As ThreadAbortException
                '---- terminate the thread
            End Try
        End Sub


        ''' <summary>
        ''' How many times per second should the library update amBX?
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Property UpdatesPerSecond() As Integer
            Get
                Return _UPS
            End Get
            Set(ByVal value As Integer)
                If value > 0 Then
                    If _UPS = 0 Then
                        _UPS = value
                        '---- need to startup up the background thread
                        If amBX.IsConnected Then
                            _UpdateThread = New Thread(AddressOf UpdateThreadRun)
                            _UpdateThread.Start()
                        End If
                    Else
                        '---- just update the property value
                        '     There should already be a thread running
                        _UPS = value
                    End If
                Else
                    '---- setting UPS to 0 will cause the current background thread to
                    '     terminate
                    _UPS = 0
                End If
            End Set
        End Property
        Private Shared _UPS As Integer


        ''' <summary>
        ''' Disconnect from the amBX drivers and release all created objects
        ''' and buffers.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub Disconnect()
            '---- if this pointer isn't initialized, nothing else will work
            CheckConnected()
            Try
                Try
                    '---- release the normal objects
                    Do While amBX.Lights.Count > 0
                        Dim l = amBX.Lights(0)
                        l.Dispose()
                        amBX.Lights.Remove(l)
                    Loop
                    Do While amBX.Fans.Count > 0
                        Dim f = amBX.Fans(0)
                        f.Dispose()
                        amBX.Fans.Remove(f)
                    Loop
                    Do While amBX.Rumbles.Count > 0
                        Dim r = amBX.Rumbles(0)
                        r.Dispose()
                        amBX.Rumbles.Remove(r)
                    Loop

                    '---- release and dispose of all events and movies
                    '     because amBX doesn't release them automatically.
                    Do While amBX.Events.Count > 0
                        Dim e = amBX.Events(0)
                        e.Dispose()
                        amBX.Events.Remove(e)
                    Loop
                    Do While amBX.Movies.Count > 0
                        Dim m = amBX.Movies(0)
                        m.Dispose()
                        amBX.Movies.Remove(m)
                    Loop

                Catch ex As Exception
                    '---- there's really nothing that can be done here
                    '     We just always want to make sure that the following
                    '     thread stop code is executed.
                End Try

                '---- kill the background updater thread
                If _UpdateThread IsNot Nothing Then
                    _UpdateThread.Abort()
                    _UpdateThread.Join()
                End If

                '---- release this object
                Exceptions.Check(_IamBX.IamBXDelegates.Release(_IamBX.IamBXPtr), "Unable to release amBX object")

            Finally
                '---- make sure we zero out the structures and pointers
                _IamBX = New IamBX
            End Try
        End Sub


        ''' <summary>
        ''' Enable or disable all devices currently associated with amBX.
        ''' </summary>
        ''' <value>A boolean, true to enable all devices, false to disable them.</value>
        ''' <returns>A boolean indicating where all devices are enabled or disabled.</returns>
        ''' <remarks></remarks>
        Public Shared Property Enabled() As Boolean
            Get
                Return _Enabled
            End Get
            Set(ByVal value As Boolean)
                CheckConnected()
                Exceptions.Check(_IamBX.IamBXDelegates.SetAllEnabled(_IamBX.IamBXPtr, value), "Unable to set enabled property")
                _Enabled = value
            End Set
        End Property
        Private Shared _Enabled As Boolean = True


        ''' <summary>
        ''' Checks all device interfaces for updates and sends
        ''' updated attributes to the amBX engine, if required.  
        ''' 
        ''' Under normal circumstances, this library will automatically call Update
        ''' via a background worker thread, so there should be no need for your
        ''' application to call this method explicitly.
        ''' </summary>
        ''' <param name="MaxWaitTimeInMilliseconds">
        ''' Under normal configuration, this code handles threading and updates
        ''' so this parameter will be ignored.
        ''' </param>
        ''' <remarks></remarks>
        Public Shared Sub Update(Optional ByVal MaxWaitTimeInMilliseconds As Integer = 0)
            CheckConnected()
            Exceptions.Check(_IamBX.IamBXDelegates.Update(_IamBX.IamBXPtr, MaxWaitTimeInMilliseconds), "Unable to update")
        End Sub


        ''' <summary>
        ''' Retrieves the version of the amBX drivers that are installed
        ''' </summary>
        ''' <param name="Major">The Major version of the installed amBX driver.</param>
        ''' <param name="Minor">The Minor version of the installed amBX driver.</param>
        ''' <param name="Revision">The revision number of the installed amBX driver.</param>
        ''' <param name="Build">The build number of the installed amBX driver.</param>
        ''' <remarks></remarks>
        Public Shared Sub GetVersionInfo(ByRef Major As Integer, ByRef Minor As Integer, ByRef Revision As Integer, ByRef Build As Integer)
            CheckConnected()
            Exceptions.Check(_IamBX.IamBXDelegates.GetVersionInfo(_IamBX.IamBXPtr, Major, Minor, Revision, Build), "Unable to retrieve version info")
        End Sub


        ''' <summary>
        ''' The entry point for a thread used by amBX to send updates from 
        ''' the device interfaces to the amBX engine.
        ''' This should be called inside your thread, and it will not exit until you
        ''' call stopThread() with the thread ID from outside the thread. 
        ''' See page_threading for more details.
        ''' </summary>
        ''' <param name="ThreadType">
        ''' An indicator of how the thread being run should
        ''' be used. Currently the only valid value is \b amBX_Ambient_Update.
        ''' </param>
        ''' <param name="ThreadID">
        ''' An identifier for the thread being run. This value is 
        ''' used to identify the thread ready for 'stopThread'. This value
        ''' must be unique otherwise the functionality of stopThread is
        ''' undetermined. Typically the operating system thread ID is used.
        ''' </param>
        ''' <remarks></remarks>
        Public Shared Sub RunThread(ByVal ThreadType As amBX_ThreadType, ByVal ThreadID As Integer)
            CheckConnected()
            Exceptions.Check(_IamBX.IamBXDelegates.RunThread(_IamBX.IamBXPtr, ThreadType, ThreadID), "Unable to run thread")
        End Sub


        ''' <summary>
        ''' Used to stop the execution of a thread started with runThread(). 
        ''' After the successful call of stopThread(), runThread() - which was called
        ''' inside the thread - will return. See page_threading.
        ''' </summary>
        ''' <param name="ThreadID">An identifier for the thread being stopped. This value
        '''	   should be the same as the ID passed to runThread().</param>
        ''' <remarks></remarks>
        Public Shared Sub StopThread(ByVal ThreadID As Integer)
            CheckConnected()
            Exceptions.Check(_IamBX.IamBXDelegates.StopThread(_IamBX.IamBXPtr, ThreadID), "Unable to stop thread")
        End Sub


        ''' <summary>
        ''' Retrieve the collection of all Lights objects
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property Lights() As LightCollection
            Get
                Return _LightCollection
            End Get
        End Property
        Private Shared _LightCollection As LightCollection = New LightCollection


        ''' <summary>
        ''' Private function to retrieve a Light interface pointer from ambx.
        ''' </summary>
        ''' <param name="Location"></param>
        ''' <param name="Height"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Shared Function CreateLight(ByVal Location As Locations, ByVal Height As Heights) As IntPtr
            CheckConnected()
            Dim r As IntPtr
            Exceptions.Check(_IamBX.IamBXDelegates.CreateLight(_IamBX.IamBXPtr, Location, Height, r), "Unable to create light object")
            Return r
        End Function



        ''' <summary>
        ''' Retrieve the collection of all Fans objects
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property Fans() As FanCollection
            Get
                Return _FanCollection
            End Get
        End Property
        Private Shared _FanCollection As FanCollection = New FanCollection


        ''' <summary>
        ''' Private function to retrieve a fan interface pointer from ambx.
        ''' </summary>
        ''' <param name="Location"></param>
        ''' <param name="Height"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Shared Function CreateFan(ByVal Location As Locations, ByVal Height As Heights) As IntPtr
            CheckConnected()
            Dim r As IntPtr
            Exceptions.Check(_IamBX.IamBXDelegates.CreateFan(_IamBX.IamBXPtr, Location, Height, r), "Unable to create fan object")
            Return r
        End Function


        ''' <summary>
        ''' Retrieve the collection of all Rumbles objects
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property Rumbles() As RumbleCollection
            Get
                Return _RumbleCollection
            End Get
        End Property
        Private Shared _RumbleCollection As RumbleCollection = New RumbleCollection


        ''' <summary>
        ''' Private function to retrieve a rumble interface pointer from ambx.
        ''' </summary>
        ''' <param name="Location"></param>
        ''' <param name="Height"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Shared Function CreateRumble(ByVal Location As Locations, ByVal Height As Heights) As IntPtr
            CheckConnected()
            Dim r As IntPtr
            Exceptions.Check(_IamBX.IamBXDelegates.CreateRumble(_IamBX.IamBXPtr, Location, Height, r), "Unable to create rumble object")
            Return r
        End Function


        ''' <summary>
        ''' Retrieve the collection of all Events objects
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property Events() As EventCollection
            Get
                Return _EventCollection
            End Get
        End Property
        Private Shared _EventCollection As EventCollection = New EventCollection


        ''' <summary>
        ''' Private function to retrieve an event interface pointer from ambx.
        ''' </summary>
        ''' <param name="FileBuffer"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Shared Function CreateEvent(ByVal FileBuffer As Byte()) As IntPtr
            CheckConnected()
            Dim r As IntPtr
            Exceptions.Check(_IamBX.IamBXDelegates.CreateEvent(_IamBX.IamBXPtr, FileBuffer, FileBuffer.Length, r), "Unable to create Event object")
            Return r
        End Function


        ''' <summary>
        ''' Retrieve the collection of all Movies objects
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property Movies() As MovieCollection
            Get
                Return _MovieCollection
            End Get
        End Property
        Private Shared _MovieCollection As MovieCollection = New MovieCollection


        ''' <summary>
        ''' Private function to retrieve a movie interface pointer from ambx.
        ''' </summary>
        ''' <param name="FileBuffer"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Shared Function CreateMovie(ByVal FileBuffer As Byte()) As IntPtr
            CheckConnected()
            Dim r As IntPtr
            Exceptions.Check(_IamBX.IamBXDelegates.CreateMovie(_IamBX.IamBXPtr, FileBuffer, FileBuffer.Length, r), "Unable to create movie object")
            Return r
        End Function


#Region " LightCollection"
        ''' <summary>
        ''' The Collection of light objects that have been created.
        ''' </summary>
        ''' <remarks></remarks>
        Public Class LightCollection
            Inherits List(Of Light)


            Public Overloads Function Add(ByVal Name As String, ByVal Location As Locations, ByVal Height As Heights) As Light
                Return New Light(Name, Location, Height)
            End Function


            Public Overloads Function Add(ByVal Location As Locations, ByVal Height As Heights) As Light
                Return New Light(Location, Height)
            End Function


            Public Overloads ReadOnly Property Item(ByVal Index As String) As Light
                Get
                    Return Me.FirstOrDefault(Function(L) StrComp(L.Name, Index, CompareMethod.Text) = 0)
                End Get
            End Property
        End Class
#End Region


#Region " Light"
        ''' <summary>
        ''' This object represents a Light object in amBX "space".
        ''' When you create it, you must indicate "where" the light is located.
        ''' The location cannot change once it's created.
        ''' </summary>
        ''' <remarks></remarks>
        Public Class Light
            Implements IDisposable


            '---- these variables track the interface pointers and delegates used to 
            '     actually communicate with the ambx drivers.
            Private _IamBXLightPtr As IntPtr
            Private _IamBXLightInterface As IamBXLightInterface
            Private _IamBXLightDelegates As IamBXLightDelegates


            <StructLayout(LayoutKind.Sequential)> _
            Private Structure IamBXLightInterface
                Public ReleasePtr As IntPtr
                Public SetColPtr As IntPtr
                Public GetColPtr As IntPtr
                Public SetFadeTimePtr As IntPtr
                Public GetFadeTimePtr As IntPtr
                Public GetLocationPtr As IntPtr
                Public SetEnabledPtr As IntPtr
                Public GetEnabledPtr As IntPtr
                Public SetUpdatePropertiesPtr As IntPtr
                Public GetUpdatePropertiesPtr As IntPtr
            End Structure


            Private Structure IamBXLightDelegates
                Public Release As ReleaseDelegate
                Public SetCol As SetColDelegate
                Public GetCol As GetColDelegate
                Public SetFadeTime As SetFadeTimeDelegate
                Public GetFadeTime As GetFadeTimeDelegate
                Public GetLocation As GetLocationDelegate
                Public SetEnabled As SetEnabledDelegate
                Public GetEnabled As GetEnabledDelegate
                Public SetUpdateProperties As SetUpdatePropertiesDelegate
                Public GetUpdateProperties As GetUpdatePropertiesDelegate


                Public Sub Generate(ByVal IamBXLightInterface As IamBXLightInterface)
                    Me.Release = Marshal.GetDelegateForFunctionPointer(IamBXLightInterface.ReleasePtr, GetType(ReleaseDelegate))
                    Me.GetCol = Marshal.GetDelegateForFunctionPointer(IamBXLightInterface.GetColPtr, GetType(GetColDelegate))
                    Me.SetCol = Marshal.GetDelegateForFunctionPointer(IamBXLightInterface.SetColPtr, GetType(SetColDelegate))
                    Me.GetFadeTime = Marshal.GetDelegateForFunctionPointer(IamBXLightInterface.GetFadeTimePtr, GetType(GetFadeTimeDelegate))
                    Me.SetFadeTime = Marshal.GetDelegateForFunctionPointer(IamBXLightInterface.SetFadeTimePtr, GetType(SetFadeTimeDelegate))
                    Me.GetLocation = Marshal.GetDelegateForFunctionPointer(IamBXLightInterface.GetLocationPtr, GetType(GetLocationDelegate))
                    Me.GetEnabled = Marshal.GetDelegateForFunctionPointer(IamBXLightInterface.GetEnabledPtr, GetType(GetEnabledDelegate))
                    Me.SetEnabled = Marshal.GetDelegateForFunctionPointer(IamBXLightInterface.SetEnabledPtr, GetType(SetEnabledDelegate))
                    Me.SetUpdateProperties = Marshal.GetDelegateForFunctionPointer(IamBXLightInterface.SetUpdatePropertiesPtr, GetType(SetUpdatePropertiesDelegate))
                    Me.GetUpdateProperties = Marshal.GetDelegateForFunctionPointer(IamBXLightInterface.GetUpdatePropertiesPtr, GetType(GetUpdatePropertiesDelegate))
                End Sub


                <UnmanagedFunctionPointer(CallingConvention.Cdecl)> _
                Public Delegate Function ReleaseDelegate(ByVal IamBXLightPtr As IntPtr) As amBX_RESULT

                <UnmanagedFunctionPointer(CallingConvention.Cdecl)> _
                Public Delegate Function SetColDelegate(ByVal IamBXLight As IntPtr, _
                                                        ByVal fRed As Single, _
                                                        ByVal fGreen As Single, _
                                                        ByVal fBlue As Single) As amBX_RESULT

                <UnmanagedFunctionPointer(CallingConvention.Cdecl)> _
                Public Delegate Function GetColDelegate(ByVal IamBXLight As IntPtr, _
                                                        ByRef fRed As Single, _
                                                        ByRef fGreen As Single, _
                                                        ByRef fBlue As Single) As amBX_RESULT

                <UnmanagedFunctionPointer(CallingConvention.Cdecl)> _
                Public Delegate Function SetFadeTimeDelegate(ByVal IamBXLightPtr As IntPtr, _
                                                        ByVal FadeTimeMS As Integer) As amBX_RESULT

                <UnmanagedFunctionPointer(CallingConvention.Cdecl)> _
                Public Delegate Function GetFadeTimeDelegate(ByVal IamBXLightPtr As IntPtr, _
                                                        ByRef FadeTimeMS As Integer) As amBX_RESULT

                <UnmanagedFunctionPointer(CallingConvention.Cdecl)> _
                Public Delegate Function GetLocationDelegate(ByVal IamBXLightPtr As IntPtr, _
                                                             ByRef Location As Locations) As amBX_RESULT

                <UnmanagedFunctionPointer(CallingConvention.Cdecl)> _
                Public Delegate Function SetEnabledDelegate(ByVal IamBXLightPtr As IntPtr, _
                                                             <MarshalAs(UnmanagedType.I1)> ByVal Enabled As Boolean) As amBX_RESULT

                <UnmanagedFunctionPointer(CallingConvention.Cdecl)> _
                Public Delegate Function GetEnabledDelegate(ByVal IamBXLightPtr As IntPtr, _
                                                             <MarshalAs(UnmanagedType.I1)> ByRef Enabled As Boolean) As amBX_RESULT

                <UnmanagedFunctionPointer(CallingConvention.Cdecl)> _
                Public Delegate Function SetUpdatePropertiesDelegate(ByVal IamBXLightPtr As IntPtr, _
                                                             ByVal LightUpdateIntervalMS As Int64, _
                                                             ByVal fLightDelta As Single) As amBX_RESULT

                <UnmanagedFunctionPointer(CallingConvention.Cdecl)> _
                Public Delegate Function GetUpdatePropertiesDelegate(ByVal IamBXLightPtr As IntPtr, _
                                                     ByRef lightUpdateIntervalMS As Int64, _
                                                     ByRef fLightDelta As Single) As amBX_RESULT
            End Structure


            ''' <summary>
            ''' Constructor for a new Light object. You must indicate where the light
            ''' is located at this point. The location of the object cannot be changed.
            ''' </summary>
            ''' <param name="Location"></param>
            ''' <param name="Height"></param>
            ''' <remarks></remarks>
            Public Sub New(ByVal Name As String, ByVal Location As Locations, ByVal Height As Heights)
                Initialize(Name, Location, Height)
            End Sub


            ''' <summary>
            ''' Constructor for a new Light object. You must indicate where the light
            ''' is located at this point. The location of the object cannot be changed.
            ''' </summary>
            ''' <param name="Location"></param>
            ''' <param name="Height"></param>
            ''' <remarks></remarks>
            Public Sub New(ByVal Location As Locations, ByVal Height As Heights)
                Initialize("", Location, Height)
            End Sub


            Private Sub Initialize(ByVal Name As String, ByVal Location As Locations, ByVal Height As Heights)
                Me.Name = Name
                _IamBXLightPtr = amBX.CreateLight(Location, Height)
                _IamBXLightInterface = Marshal.PtrToStructure(_IamBXLightPtr, GetType(IamBXLightInterface))
                _IamBXLightDelegates.Generate(_IamBXLightInterface)

                amBX.Lights.Add(Me)
            End Sub


            ''' <summary>
            ''' Name of this Light
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property Name() As String
                Get
                    Return _Name
                End Get
                Set(ByVal value As String)
                    _Name = value
                End Set
            End Property
            Private _Name As String


            Private Sub Release()
                Try
                    Exceptions.Check(_IamBXLightDelegates.Release(_IamBXLightPtr), "Unable to release light object")
                Finally
                    _IamBXLightPtr = 0
                    _IamBXLightInterface = New IamBXLightInterface
                    _IamBXLightDelegates = New IamBXLightDelegates
                End Try
            End Sub


            ''' <summary>
            ''' Set the target color (represented by a normal .net COLOR object)
            ''' of the light device interface, towards which the
            ''' actual color of the light should change at the fade rate set by 
            ''' FadeTime().
            ''' </summary>
            ''' <value>
            ''' A Color representing a Red Green Blue value for the light. 
            ''' The Alpha channel is ignored.
            ''' </value>
            ''' <returns>
            ''' A Color representing a Red Green Blue value of the light. 
            ''' The Alpha channel is left as 0.
            ''' </returns>
            ''' <remarks></remarks>
            Public Property Color() As Color
                Get
                    Return Me.amBXColor.toColor
                End Get
                Set(ByVal value As Color)
                    Me.amBXColor = New amBXColor(value.R / 255, value.G / 255, value.B / 255)
                End Set
            End Property


            ''' <summary>
            ''' Set the target color (represented by an amBXColor floating point color object)
            ''' of the light device interface, towards which the
            ''' actual color of the light should change at the fade rate set by 
            ''' FadeTime().
            ''' </summary>
            ''' <value>
            ''' An amBXColor representing a Red Green Blue value for the light. 
            ''' </value>
            ''' <returns>
            ''' An amBXColor representing a Red Green Blue value of the light. 
            ''' </returns>
            ''' <remarks></remarks>
            Public Property amBXColor() As amBXColor
                Get
                    Dim fRed, fGreen, fBlue As Single
                    Exceptions.Check(_IamBXLightDelegates.GetCol(_IamBXLightPtr, fRed, fGreen, fBlue), "Unable to get light color")
                    Return New amBXColor(fRed, fGreen, fBlue)
                End Get
                Set(ByVal value As amBXColor)
                    Exceptions.Check(_IamBXLightDelegates.SetCol(_IamBXLightPtr, value.R, value.G, value.B), "Unable to set light color")
                End Set
            End Property


            ''' <summary>
            ''' Sets the time taken to transition to the target color when the color 
            ''' is changed.
            ''' </summary>
            ''' <value>The fade time in milliseconds.</value>
            ''' <returns>The fade time in milliseconds.</returns>
            ''' <remarks></remarks>
            Public Property FadeTime() As Integer
                Get
                    Dim t As Integer
                    Exceptions.Check(_IamBXLightDelegates.GetFadeTime(_IamBXLightPtr, t), "Unable to get light fadetime")
                    Return t
                End Get
                Set(ByVal value As Integer)
                    Exceptions.Check(_IamBXLightDelegates.SetFadeTime(_IamBXLightPtr, value), "Unable to set light fadetime")
                End Set
            End Property


            ''' <summary>
            ''' Retrieve the target location of light effect.
            ''' This can be multiple locations OR'd together.
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public ReadOnly Property Location() As Locations
                Get
                    Dim l As Locations
                    Exceptions.Check(_IamBXLightDelegates.GetLocation(_IamBXLightPtr, l), "Unable to get light location")
                    Return l
                End Get
            End Property


            ''' <summary>
            ''' Retrieve the Enabled state of this light object.
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property Enabled() As Boolean
                Get
                    Dim e As Boolean
                    Exceptions.Check(_IamBXLightDelegates.GetEnabled(_IamBXLightPtr, e), "Unable to get light enabled state")
                    Return e
                End Get
                Set(ByVal value As Boolean)
                    Exceptions.Check(_IamBXLightDelegates.SetEnabled(_IamBXLightPtr, value), "Unable to set light enabled state")
                End Set
            End Property


            ''' <summary>
            ''' updates in this light interface will not be sent to the amBX engine 
            ''' before a time of LightUpdateIntervalMS has passed since the previous 
            ''' update of this interface that has been sent to the engine.
            ''' Default value is 100ms.
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property LightUpdateIntervalMS() As Int64
                Get
                    Dim i1 As Integer, f1 As Single
                    GetUpdateProperties(i1, f1)
                    Return i1
                End Get
                Set(ByVal value As Int64)
                    SetUpdateProperties(value, Me.LightDelta)
                End Set
            End Property


            ''' <summary>
            ''' Minimum required change per color component that
            ''' triggers sending an update. 0.0f &lt;= fLightDelta &lt;= 1.0f
            ''' Default value is .02.
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property LightDelta() As Single
                Get
                    Dim i1 As Int64, f1 As Single
                    GetUpdateProperties(i1, f1)
                    Return f1
                End Get
                Set(ByVal value As Single)
                    SetUpdateProperties(Me.LightUpdateIntervalMS, Utility.ConstrainFloat(value))
                End Set
            End Property


            Private Sub SetUpdateProperties(ByVal lightUpdateIntervalMS As Int64, ByVal fLightDelta As Single)
                Exceptions.Check(_IamBXLightDelegates.SetUpdateProperties(_IamBXLightPtr, lightUpdateIntervalMS, fLightDelta), "Unable to set light update properties")
            End Sub

            Private Sub GetUpdateProperties(ByRef lightUpdateIntervalMS As Int64, ByRef fLightDelta As Single)
                Exceptions.Check(_IamBXLightDelegates.GetUpdateProperties(_IamBXLightPtr, lightUpdateIntervalMS, fLightDelta), "Unable to get light update properties")
            End Sub


#Region " IDisposable Support "
            ' IDisposable
            Protected Overridable Sub Dispose(ByVal disposing As Boolean)
                If Not Me.disposedValue Then
                    If disposing Then
                        Me.Release()
                    End If
                End If
                Me.disposedValue = True
            End Sub
            Private disposedValue As Boolean = False        ' To detect redundant calls


            ' This code added by Visual Basic to correctly implement the disposable pattern.
            Public Sub Dispose() Implements IDisposable.Dispose
                ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
                Dispose(True)
                GC.SuppressFinalize(Me)
            End Sub
#End Region

        End Class
#End Region


#Region " FanCollection"
        ''' <summary>
        ''' The Collection of fan objects that have been created.
        ''' </summary>
        ''' <remarks></remarks>
        Public Class FanCollection
            Inherits List(Of Fan)


            Public Overloads Function Add(ByVal Name As String, ByVal Location As Locations, ByVal Height As Heights) As Fan
                Return New Fan(Name, Location, Height)
            End Function


            Public Overloads Function Add(ByVal Location As Locations, ByVal Height As Heights) As Fan
                Return New Fan(Location, Height)
            End Function


            Public Overloads ReadOnly Property Item(ByVal Index As String) As Fan
                Get
                    Return Me.FirstOrDefault(Function(L) StrComp(L.Name, Index, CompareMethod.Text) = 0)
                End Get
            End Property
        End Class
#End Region


#Region " Fan"
        ''' <summary>
        ''' This class represents the fan device interface - enabling the 
        ''' creation of air flow (wind) experiences. 
        ''' </summary>
        ''' <remarks></remarks>
        Public Class Fan
            Implements IDisposable


            '---- these variables track the interface pointers and delegates used to 
            '     actually communicate with the ambx drivers.
            Private _IamBXFanPtr As IntPtr
            Private _IamBXFanInterface As IamBXFanInterface
            Private _IamBXFanDelegates As IamBXFanDelegates


            <StructLayout(LayoutKind.Sequential)> _
            Private Structure IamBXFanInterface
                Public ReleasePtr As IntPtr
                Public SetIntensityPtr As IntPtr
                Public GetIntensityPtr As IntPtr
                Public GetLocationPtr As IntPtr
                Public GetEnabledPtr As IntPtr
                Public SetEnabledPtr As IntPtr
                Public SetUpdatePropertiesPtr As IntPtr
                Public GetUpdatePropertiesPtr As IntPtr
            End Structure


            Private Structure IamBXFanDelegates
                Public Release As ReleaseDelegate
                Public SetIntensity As SetIntensityDelegate
                Public GetIntensity As GetIntensityDelegate
                Public GetLocation As GetLocationDelegate
                Public GetEnabled As GetEnabledDelegate
                Public SetEnabled As SetEnabledDelegate
                Public SetUpdateProperties As SetUpdatePropertiesDelegate
                Public GetUpdateProperties As GetUpdatePropertiesDelegate


                Public Sub Generate(ByVal IamBXFanInterface As IamBXFanInterface)
                    Me.Release = Marshal.GetDelegateForFunctionPointer(IamBXFanInterface.ReleasePtr, GetType(ReleaseDelegate))
                    Me.GetIntensity = Marshal.GetDelegateForFunctionPointer(IamBXFanInterface.GetIntensityPtr, GetType(GetIntensityDelegate))
                    Me.SetIntensity = Marshal.GetDelegateForFunctionPointer(IamBXFanInterface.SetIntensityPtr, GetType(SetIntensityDelegate))
                    Me.GetLocation = Marshal.GetDelegateForFunctionPointer(IamBXFanInterface.GetLocationPtr, GetType(GetLocationDelegate))
                    Me.GetEnabled = Marshal.GetDelegateForFunctionPointer(IamBXFanInterface.GetEnabledPtr, GetType(GetEnabledDelegate))
                    Me.SetEnabled = Marshal.GetDelegateForFunctionPointer(IamBXFanInterface.SetEnabledPtr, GetType(SetEnabledDelegate))
                    Me.SetUpdateProperties = Marshal.GetDelegateForFunctionPointer(IamBXFanInterface.SetUpdatePropertiesPtr, GetType(SetUpdatePropertiesDelegate))
                    Me.GetUpdateProperties = Marshal.GetDelegateForFunctionPointer(IamBXFanInterface.GetUpdatePropertiesPtr, GetType(GetUpdatePropertiesDelegate))
                End Sub


                <UnmanagedFunctionPointer(CallingConvention.Cdecl)> _
                Public Delegate Function ReleaseDelegate(ByVal IamBXFanPtr As IntPtr) As amBX_RESULT

                <UnmanagedFunctionPointer(CallingConvention.Cdecl)> _
                Public Delegate Function SetIntensityDelegate(ByVal IamBXFanPtr As IntPtr, _
                                                              ByVal Intensity As Single) As amBX_RESULT

                <UnmanagedFunctionPointer(CallingConvention.Cdecl)> _
                Public Delegate Function GetIntensityDelegate(ByVal IamBXFanPtr As IntPtr, _
                                                              ByRef Intensity As Single) As amBX_RESULT

                <UnmanagedFunctionPointer(CallingConvention.Cdecl)> _
                Public Delegate Function GetLocationDelegate(ByVal IamBXFanPtr As IntPtr, _
                                                             ByRef Location As Locations) As amBX_RESULT

                <UnmanagedFunctionPointer(CallingConvention.Cdecl)> _
                Public Delegate Function SetEnabledDelegate(ByVal IamBXFanPtr As IntPtr, _
                                                             ByVal State As amBX_EnabledStates) As amBX_RESULT

                <UnmanagedFunctionPointer(CallingConvention.Cdecl)> _
                Public Delegate Function GetEnabledDelegate(ByVal IamBXFanPtr As IntPtr, _
                                                             ByRef State As amBX_EnabledStates) As amBX_RESULT

                <UnmanagedFunctionPointer(CallingConvention.Cdecl)> _
                Public Delegate Function SetUpdatePropertiesDelegate(ByVal IamBXFanPtr As IntPtr, _
                                                             ByVal FanUpdateIntervalMS As Int64, _
                                                             ByVal fFanDelta As Single) As amBX_RESULT

                <UnmanagedFunctionPointer(CallingConvention.Cdecl)> _
                Public Delegate Function GetUpdatePropertiesDelegate(ByVal IamBXFanPtr As IntPtr, _
                                                     ByRef FanUpdateIntervalMS As Int64, _
                                                     ByRef FanDelta As Single) As amBX_RESULT
            End Structure


            ''' <summary>
            ''' Constructor for the Fan class.
            ''' </summary>
            ''' <param name="Location"></param>
            ''' <param name="Height"></param>
            ''' <remarks></remarks>
            Public Sub New(ByVal Location As Locations, ByVal Height As Heights)
                Initialize("", Location, Height)
            End Sub


            ''' <summary>
            ''' Constructor for the Fan class.
            ''' </summary>
            ''' <param name="Location"></param>
            ''' <param name="Height"></param>
            ''' <remarks></remarks>
            Public Sub New(ByVal Name As String, ByVal Location As Locations, ByVal Height As Heights)
                Initialize(Name, Location, Height)
            End Sub


            ''' <summary>
            ''' Constructor for the Fan class.
            ''' </summary>
            ''' <param name="Location"></param>
            ''' <param name="Height"></param>
            ''' <remarks></remarks>
            Private Sub Initialize(ByVal Name As String, ByVal Location As Locations, ByVal Height As Heights)
                Me.Name = Name
                _IamBXFanPtr = amBX.CreateFan(Location, Height)
                _IamBXFanInterface = Marshal.PtrToStructure(_IamBXFanPtr, GetType(IamBXFanInterface))
                _IamBXFanDelegates.Generate(_IamBXFanInterface)

                amBX.Fans.Add(Me)
            End Sub


            ''' <summary>
            ''' Name of this Fan
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property Name() As String
                Get
                    Return _Name
                End Get
                Set(ByVal value As String)
                    _Name = value
                End Set
            End Property
            Private _Name As String


            Private Sub Release()
                Try
                    Exceptions.Check(_IamBXFanDelegates.Release(_IamBXFanPtr), "Unable to release fan object")
                Finally
                    _IamBXFanInterface = New IamBXFanInterface
                    _IamBXFanDelegates = New IamBXFanDelegates
                    _IamBXFanPtr = 0
                End Try
            End Sub


            ''' <summary>
            ''' Set or retrieve the intensity level of the fan effect.
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property Intensity() As Single
                Get
                    Dim i As Single
                    Exceptions.Check(_IamBXFanDelegates.GetIntensity(_IamBXFanPtr, i), "Unable to get intensity")
                    Return i
                End Get
                Set(ByVal value As Single)
                    Exceptions.Check(_IamBXFanDelegates.SetIntensity(_IamBXFanPtr, Utility.ConstrainFloat(value)), "Unable to set intensity")
                End Set
            End Property


            ''' <summary>
            ''' Retrieve the location of this Fan effect. Note that the location is set
            ''' when the object is created and can't be changed.
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public ReadOnly Property Location() As Locations
                Get
                    Dim l As Locations
                    Exceptions.Check(_IamBXFanDelegates.GetLocation(_IamBXFanPtr, l), "Unable to get fan location")
                    Return l
                End Get
            End Property


            ''' <summary>
            ''' Retrieve the enabled state of this fan object.
            ''' States are:
            '''   ENABLED if the fan is enabled
            '''   ENABLING if the fan is currently not enabled, but will 
            '''      be enabled after the next time the amBX.Update function is 
            '''      called.
            '''   DISABLED if the fan is disabled
            '''   DISABLING if the fan is currently enabled, but will disabled
            '''      after the next time the amBX.Update function is called.
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public ReadOnly Property Enabled() As amBX_EnabledStates
                Get
                    Dim e As amBX_EnabledStates
                    Exceptions.Check(_IamBXFanDelegates.GetEnabled(_IamBXFanPtr, e), "Unable to get fan enabled state")
                    Return e
                End Get
            End Property


            ''' <summary>
            ''' Enable this fan object
            ''' </summary>
            ''' <remarks></remarks>
            Public Sub Enable()
                Exceptions.Check(_IamBXFanDelegates.SetEnabled(_IamBXFanPtr, True), "Unable to enable fan")
            End Sub


            ''' <summary>
            ''' Disable this fan object
            ''' </summary>
            ''' <remarks></remarks>
            Public Sub Disable()
                Exceptions.Check(_IamBXFanDelegates.SetEnabled(_IamBXFanPtr, False), "Unable to disable fan")
            End Sub


            ''' <summary>
            ''' updates on this fan will not be sent to the amBX engine 
            ''' before a time of FanUpdateIntervalMS has passed since the previous 
            ''' update of this interface that has been sent to the engine.
            ''' Default value is 100ms
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property FanUpdateIntervalMS() As Int64
                Get
                    Dim i1 As Int64, f1 As Single
                    GetUpdateProperties(i1, f1)
                    Return i1
                End Get
                Set(ByVal value As Int64)
                    SetUpdateProperties(value, Me.FanDelta)
                End Set
            End Property


            ''' <summary>
            ''' Minimum required change per color component that
            ''' triggers sending an update. 0.0f &lt;= fLightDelta &lt;= 1.0f
            ''' Default value is .02
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property FanDelta() As Single
                Get
                    Dim i1 As Int64, f1 As Single
                    GetUpdateProperties(i1, f1)
                    Return f1
                End Get
                Set(ByVal value As Single)
                    SetUpdateProperties(Me.FanUpdateIntervalMS, Utility.ConstrainFloat(value))
                End Set
            End Property


            Private Sub SetUpdateProperties(ByVal FanUpdateIntervalMS As Int64, ByVal fanDelta As Single)
                Exceptions.Check(_IamBXFanDelegates.SetUpdateProperties(_IamBXFanPtr, FanUpdateIntervalMS, fanDelta), "Unable to set fan update properties")
            End Sub


            Private Sub GetUpdateProperties(ByRef FanUpdateIntervalMS As Int64, ByRef FanDelta As Single)
                Exceptions.Check(_IamBXFanDelegates.GetUpdateProperties(_IamBXFanPtr, FanUpdateIntervalMS, FanDelta), "Unable to get fan update properties")
            End Sub


#Region " IDisposable Support "
            ' IDisposable
            Protected Overridable Sub Dispose(ByVal disposing As Boolean)
                If Not Me.disposedValue Then
                    If disposing Then
                        Me.Release()
                    End If
                End If
                Me.disposedValue = True
            End Sub
            Private disposedValue As Boolean = False        ' To detect redundant calls

            ' This code added by Visual Basic to correctly implement the disposable pattern.
            Public Sub Dispose() Implements IDisposable.Dispose
                ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
                Dispose(True)
                GC.SuppressFinalize(Me)
            End Sub
#End Region

        End Class
#End Region


#Region " RumbleCollection"
        ''' <summary>
        ''' The Collection of rumble objects that have been created.
        ''' </summary>
        ''' <remarks></remarks>
        Public Class RumbleCollection
            Inherits List(Of Rumble)


            Public Overloads Function Add(ByVal Name As String, ByVal Location As Locations, ByVal Height As Heights) As Rumble
                Return New Rumble(Name, Location, Height)
            End Function


            Public Overloads Function Rumble(ByVal Location As Locations, ByVal Height As Heights) As Rumble
                Return New Rumble(Location, Height)
            End Function


            Public Overloads ReadOnly Property Item(ByVal Index As String) As Rumble
                Get
                    Return Me.FirstOrDefault(Function(L) StrComp(L.Name, Index, CompareMethod.Text) = 0)
                End Get
            End Property
        End Class
#End Region

#Region " Rumble"
        ''' <summary>
        ''' This class represents an amBX Rumble object, enabling the creation
        ''' of rumble experiences. 
        ''' </summary>
        ''' <remarks></remarks>
        Public Class Rumble
            Implements IDisposable

#Region " Waveform names"
            Public Const amBX_boing = "amBX_boing"
            Public Const amBX_crash = "amBX_crash"
            Public Const amBX_engine = "amBX_engine"
            Public Const amBX_explosion = "amBX_explosion"
            Public Const amBX_hit = "amBX_hit"
            Public Const amBX_quake = "amBX_quake"
            Public Const amBX_rattle = "amBX_rattle"
            Public Const amBX_road = "amBX_road"
            Public Const amBX_shot = "amBX_shot"
            Public Const amBX_thud = "amBX_thud"
            Public Const amBX_thunder = "amBX_thunder"
#End Region

            '---- these variables track the interface pointers and delegates used to 
            '     actually communicate with the ambx drivers.
            Private _IamBXRumblePtr As IntPtr
            Private _IamBXRumbleInterface As IamBXRumbleInterface
            Private _IamBXRumbleDelegates As IamBXRumbleDelegates

            ''' <summary>
            ''' The C interface structure of the rumble object
            ''' </summary>
            ''' <remarks></remarks>
            Private Structure IamBXRumbleInterface
                Public ReleasePtr As IntPtr
                Public SetRumblePtr As IntPtr
                Public GetRumblePtr As IntPtr
                Public GetLocationPtr As IntPtr
                Public GetEnabledPtr As IntPtr
                Public SetEnabledPtr As IntPtr
                Public SetUpdatePropertiesPtr As IntPtr
                Public GetUpdatePropertiesPtr As IntPtr
            End Structure


            Private Structure IamBXRumbleDelegates
                Public Release As ReleaseDelegate
                Public SetRumble As SetRumbleDelegate
                Public GetRumble As GetRumbleDelegate
                Public GetLocation As GetLocationDelegate
                Public GetEnabled As GetEnabledDelegate
                Public SetEnabled As SetEnabledDelegate
                Public SetUpdateProperties As SetUpdatePropertiesDelegate
                Public GetUpdateProperties As GetUpdatePropertiesDelegate


                Public Sub Generate(ByVal IamBXRumbleInterface As IamBXRumbleInterface)
                    Me.Release = Marshal.GetDelegateForFunctionPointer(IamBXRumbleInterface.ReleasePtr, GetType(ReleaseDelegate))
                    Me.SetRumble = Marshal.GetDelegateForFunctionPointer(IamBXRumbleInterface.SetRumblePtr, GetType(SetRumbleDelegate))
                    Me.GetRumble = Marshal.GetDelegateForFunctionPointer(IamBXRumbleInterface.GetRumblePtr, GetType(GetRumbleDelegate))
                    Me.GetLocation = Marshal.GetDelegateForFunctionPointer(IamBXRumbleInterface.GetLocationPtr, GetType(GetLocationDelegate))
                    Me.GetEnabled = Marshal.GetDelegateForFunctionPointer(IamBXRumbleInterface.GetEnabledPtr, GetType(GetEnabledDelegate))
                    Me.SetEnabled = Marshal.GetDelegateForFunctionPointer(IamBXRumbleInterface.SetEnabledPtr, GetType(SetEnabledDelegate))
                    Me.SetUpdateProperties = Marshal.GetDelegateForFunctionPointer(IamBXRumbleInterface.SetUpdatePropertiesPtr, GetType(SetUpdatePropertiesDelegate))
                    Me.GetUpdateProperties = Marshal.GetDelegateForFunctionPointer(IamBXRumbleInterface.GetUpdatePropertiesPtr, GetType(GetUpdatePropertiesDelegate))
                End Sub


                <UnmanagedFunctionPointer(CallingConvention.Cdecl)> _
                Public Delegate Function ReleaseDelegate(ByVal IamBXRumble As IntPtr) As amBX_RESULT

                <UnmanagedFunctionPointer(CallingConvention.Cdecl)> _
                Public Delegate Function SetRumbleDelegate(ByVal IamBXRumble As IntPtr, _
                                                             ByVal RumbleName As String, _
                                                             ByVal Speed As Single, _
                                                             ByVal Intensity As Single) As amBX_RESULT

                <UnmanagedFunctionPointer(CallingConvention.Cdecl)> _
                Public Delegate Function GetRumbleDelegate(ByVal IamBXRumblePtr As IntPtr, _
                                                             ByVal MaxLen As Integer, _
                                                             ByVal RumbleName As String, _
                                                             ByRef Length As Integer, _
                                                             ByRef Speed As Single, _
                                                             ByRef Intensity As Single) As amBX_RESULT

                <UnmanagedFunctionPointer(CallingConvention.Cdecl)> _
                Public Delegate Function GetLocationDelegate(ByVal IamBXRumblePtr As IntPtr, _
                                                             ByRef Location As Locations) As amBX_RESULT

                <UnmanagedFunctionPointer(CallingConvention.Cdecl)> _
                Public Delegate Function GetEnabledDelegate(ByVal IamBXRumblePtr As IntPtr, _
                                                     ByRef State As amBX_EnabledStates) As amBX_RESULT

                <UnmanagedFunctionPointer(CallingConvention.Cdecl)> _
                Public Delegate Function SetEnabledDelegate(ByVal IamBXRumblePtr As IntPtr, _
                                                     <MarshalAs(UnmanagedType.I1)> ByVal Enabled As Boolean) As amBX_RESULT

                <UnmanagedFunctionPointer(CallingConvention.Cdecl)> _
                Public Delegate Function SetUpdatePropertiesDelegate(ByVal IamBXRumblePtr As IntPtr, _
                                                     ByVal RumbleUpdateIntervalMS As Int64, _
                                                     ByVal fSpeedDelta As Single, _
                                                     ByVal IntensityDelta As Single) As amBX_RESULT

                <UnmanagedFunctionPointer(CallingConvention.Cdecl)> _
                Public Delegate Function GetUpdatePropertiesDelegate(ByVal IamBXRumblePtr As IntPtr, _
                                                     ByRef RumbleUpdateIntervalMS As Int64, _
                                                     ByRef SpeedDelta As Single, _
                                                     ByRef IntensityDelta As Single) As amBX_RESULT
            End Structure


            ''' <summary>
            ''' Constructor for a new Rumble object.
            ''' The location and Height are declared at creation time and cannot be changed.
            ''' </summary>
            ''' <param name="Location"></param>
            ''' <param name="Height"></param>
            ''' <remarks></remarks>
            Public Sub New(ByVal Location As Locations, ByVal Height As Heights)
                Initialize("", Location, Height)
            End Sub


            ''' <summary>
            ''' Constructor for a new Rumble object.
            ''' The location and Height are declared at creation time and cannot be changed.
            ''' </summary>
            ''' <param name="Location"></param>
            ''' <param name="Height"></param>
            ''' <remarks></remarks>
            Public Sub New(ByVal Name As String, ByVal Location As Locations, ByVal Height As Heights)
                Initialize(Name, Location, Height)
            End Sub


            ''' <summary>
            ''' Actually create the rumble object
            ''' </summary>
            ''' <param name="Location"></param>
            ''' <param name="Height"></param>
            ''' <remarks></remarks>
            Private Sub Initialize(ByVal Name As String, ByVal Location As Locations, ByVal Height As Heights)
                Me.Name = Name
                ReInitialize(Location, Height)

                amBX.Rumbles.Add(Me)
            End Sub


            ''' <summary>
            ''' Provides a way to reinitialize the underlying object so we can disable it.
            ''' </summary>
            ''' <param name="Location"></param>
            ''' <param name="Height"></param>
            ''' <remarks></remarks>
            Private Sub ReInitialize(ByVal Location As Locations, ByVal Height As Heights)
                _Height = Height
                _IamBXRumblePtr = amBX.CreateRumble(Location, Height)
                _IamBXRumbleInterface = Marshal.PtrToStructure(_IamBXRumblePtr, GetType(IamBXRumbleInterface))
                _IamBXRumbleDelegates.Generate(_IamBXRumbleInterface)
            End Sub


            ''' <summary>
            ''' Name of this Rumble
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property Name() As String
                Get
                    Return _Name
                End Get
                Set(ByVal value As String)
                    _Name = value
                End Set
            End Property
            Private _Name As String


            ''' <summary>
            ''' Retrieves the height of the rumble object as it was set during object 
            ''' creation.
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public ReadOnly Property Height() As Heights
                Get
                    Return _Height
                End Get
            End Property
            Private _Height As Heights


            ''' <summary>
            ''' Release the internal ambx interface for this object
            ''' This is normally automatically performed when the object is disposed of
            ''' or when the ambx object is released.
            ''' </summary>
            ''' <remarks></remarks>
            Private Sub Release()
                Try
                    Exceptions.Check(_IamBXRumbleDelegates.Release(_IamBXRumblePtr), "Unable to release rumble object")
                Finally
                    _IamBXRumbleInterface = New IamBXRumbleInterface
                    _IamBXRumbleDelegates = New IamBXRumbleDelegates
                    _IamBXRumblePtr = 0
                End Try
            End Sub


            Private Function MapEnumToName(ByVal WaveForm As RumbleWaveForms) As String
                Select Case WaveForm
                    Case RumbleWaveForms.amBX_boing : Return amBX_boing
                    Case RumbleWaveForms.amBX_crash : Return amBX_crash
                    Case RumbleWaveForms.amBX_engine : Return amBX_engine
                    Case RumbleWaveForms.amBX_explosion : Return amBX_explosion
                    Case RumbleWaveForms.amBX_hit : Return amBX_hit
                    Case RumbleWaveForms.amBX_quake : Return amBX_quake
                    Case RumbleWaveForms.amBX_rattle : Return amBX_rattle
                    Case RumbleWaveForms.amBX_road : Return amBX_road
                    Case RumbleWaveForms.amBX_shot : Return amBX_shot
                    Case RumbleWaveForms.amBX_thud : Return amBX_thud
                    Case RumbleWaveForms.amBX_thunder : Return amBX_thunder

                    Case Else
                        Return amBX_boing
                End Select
            End Function


            Private Function MapNameToEnum(ByVal WaveFormName As String) As RumbleWaveForms
                Select Case WaveFormName.ToLower
                    Case amBX_boing.ToLower : Return RumbleWaveForms.amBX_boing
                    Case amBX_crash.ToLower : Return RumbleWaveForms.amBX_crash
                    Case amBX_engine.ToLower : Return RumbleWaveForms.amBX_engine
                    Case amBX_explosion.ToLower : Return RumbleWaveForms.amBX_explosion
                    Case amBX_hit.ToLower : Return RumbleWaveForms.amBX_hit
                    Case amBX_quake.ToLower : Return RumbleWaveForms.amBX_quake
                    Case amBX_rattle.ToLower : Return RumbleWaveForms.amBX_rattle
                    Case amBX_road.ToLower : Return RumbleWaveForms.amBX_road
                    Case amBX_shot.ToLower : Return RumbleWaveForms.amBX_shot
                    Case amBX_thud.ToLower : Return RumbleWaveForms.amBX_thud
                    Case amBX_thunder.ToLower : Return RumbleWaveForms.amBX_thunder

                    Case Else
                        Return RumbleWaveForms.amBX_boing
                End Select
            End Function


            ''' <summary>
            ''' Sets the Rumble Waveform, speed and intensity for this rumble object.
            ''' </summary>
            ''' <param name="Name">The string name of the rumble waveform.</param>
            ''' <param name="Speed">The speed of playback, from 0.0 to 10.0</param>
            ''' <param name="Intensity">The intensity of the rumble effect, from 0.0 to 1.0</param>
            ''' <remarks></remarks>
            Public Sub SetRumble(ByVal Name As String, ByVal Speed As Single, ByVal Intensity As Single)
                Exceptions.Check(_IamBXRumbleDelegates.SetRumble(_IamBXRumblePtr, Name, Utility.ConstrainFloat(Speed), Utility.ConstrainFloat(Intensity)), "Unable to set rumble")
            End Sub


            ''' <summary>
            ''' Sets the Rumble Waveform, speed and intensity for this rumble object.
            ''' </summary>
            ''' <param name="RumbleWaveForm">The ID of the rumble waveform.</param>
            ''' <param name="Speed">The speed of playback, from 0.0 to 10.0</param>
            ''' <param name="Intensity">The intensity of the rumble effect, from 0.0 to 1.0</param>
            ''' <remarks></remarks>
            Public Sub SetRumble(ByVal RumbleWaveForm As RumbleWaveForms, ByVal Speed As Single, ByVal Intensity As Single)
                Me.SetRumble(MapEnumToName(RumbleWaveForm), Utility.ConstrainFloat(Speed), Utility.ConstrainFloat(Intensity))
            End Sub


            ''' <summary>
            ''' Retrieve the rumble waveform, speed and intensity as currently set
            ''' for this rumble object.
            ''' </summary>
            ''' <param name="Name"></param>
            ''' <param name="Speed"></param>
            ''' <param name="Intensity"></param>
            ''' <remarks></remarks>
            Public Sub GetRumble(ByRef Name As String, ByRef Speed As Single, ByRef Intensity As Single)
                Name = Space(255)
                Dim NameLen As Integer
                Exceptions.Check(_IamBXRumbleDelegates.GetRumble(_IamBXRumblePtr, 255, Name, NameLen, Speed, Intensity), "Unable to get rumble")
                Name = Left(Name, NameLen)
            End Sub


            ''' <summary>
            ''' Retrieve the rumble waveform, speed and intensity as currently set
            ''' for this rumble object.
            ''' </summary>
            ''' <param name="RumbleWaveForm"></param>
            ''' <param name="Speed"></param>
            ''' <param name="Intensity"></param>
            ''' <remarks></remarks>
            Public Sub GetRumble(ByRef RumbleWaveForm As RumbleWaveForms, ByRef Speed As Single, ByRef Intensity As Single)
                Dim Name = Space(255)
                Dim NameLen As Integer
                Exceptions.Check(_IamBXRumbleDelegates.GetRumble(_IamBXRumblePtr, 255, Name, NameLen, Speed, Intensity), "Unable to get rumble")
                RumbleWaveForm = MapNameToEnum(Left(Name, NameLen))
            End Sub


            ''' <summary>
            ''' Retrieve the location of this rumble object (set when the object 
            ''' was created).
            ''' Note that this could be a combination of location values.
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public ReadOnly Property Location() As Locations
                Get
                    Dim l As Locations
                    Exceptions.Check(_IamBXRumbleDelegates.GetLocation(_IamBXRumblePtr, l), "Unable to get rumble location")
                    Return l
                End Get
            End Property


            ''' <summary>
            ''' Retrieve the Enabled state of this object.
            ''' States can be:
            '''   ENABLED if the rumble is enabled
            '''   ENABLING if the rumble is currently not enabled, but will 
            '''      be enabled after the next time the IamBX.Update() is called.
            '''   DISABLED if the rumble is not disabled
            '''   DISABLING if the rumble is currently enabled, but will
            '''      be disabled after the next time the IamBX.Update() is called.
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public ReadOnly Property Enabled() As amBX_EnabledStates
                Get
                    Dim e As amBX_EnabledStates
                    Exceptions.Check(_IamBXRumbleDelegates.GetEnabled(_IamBXRumblePtr, e), "Unable to get rumble enabled state")
                    Return e
                End Get
            End Property


            ''' <summary>
            ''' Enable this rumble object.
            ''' </summary>
            ''' <remarks></remarks>
            Public Sub Enable()
                Exceptions.Check(_IamBXRumbleDelegates.SetEnabled(_IamBXRumblePtr, True), "Unable to enable rumble")
                amBX.Update()
            End Sub


            ''' <summary>
            ''' Disable this rumble object.
            ''' </summary>
            ''' <remarks></remarks>
            Public Sub Disable()
                '---- just call the setenable interface.
                '     NOTE that this DOES NOT appear to actualy stop the effect.
                '     Seems to be a bug with amBX, but I'm not sure.
                Exceptions.Check(_IamBXRumbleDelegates.SetEnabled(_IamBXRumblePtr, False), "Unable to disable rumble")
            End Sub


            ''' <summary>
            ''' updates in this rumble interface will not be sent to the amBX engine 
            ''' before a time of RumbleUpdateIntervalMS has passed since the previous 
            ''' update of this interface that has been sent to the engine.
            ''' Default value is 100ms
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property RumbleUpdateIntervalMS() As Int64
                Get
                    Dim i1 As Int64, f1 As Single, f2 As Single
                    GetUpdateProperties(i1, f1, f2)
                    Return i1
                End Get
                Set(ByVal value As Int64)
                    SetUpdateProperties(value, Me.SpeedDelta, Me.IntensityDelta)
                End Set
            End Property


            ''' <summary>
            ''' Minimum required change of the speed component that
            ''' triggers sending an update. 0.0f &lt;= SpeedDelta &lt;= 1.0f
            ''' Default value is .02
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property SpeedDelta() As Single
                Get
                    Dim i1 As Int64, f1 As Single, f2 As Single
                    GetUpdateProperties(i1, f1, f2)
                    Return f1
                End Get
                Set(ByVal value As Single)
                    SetUpdateProperties(Me.RumbleUpdateIntervalMS, Utility.ConstrainFloat(value), Me.IntensityDelta)
                End Set
            End Property


            ''' <summary>
            ''' Minimum required change Intensity component that
            ''' triggers sending an update. 0.0f &lt;= IntensityDelta &lt;= 1.0f
            ''' Default value is .02
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property IntensityDelta() As Single
                Get
                    Dim i1 As Int64, f1 As Single, f2 As Single
                    GetUpdateProperties(i1, f1, f2)
                    Return f2
                End Get
                Set(ByVal value As Single)
                    SetUpdateProperties(Me.RumbleUpdateIntervalMS, Me.SpeedDelta, Utility.ConstrainFloat(value))
                End Set
            End Property


            Private Sub SetUpdateProperties(ByVal RumbleUpdateIntervalMS As Int64, ByVal SpeedDelta As Single, ByVal IntensityDelta As Single)
                If SpeedDelta < 0 Then
                    SpeedDelta = 0
                ElseIf SpeedDelta > 10 Then
                    SpeedDelta = 10
                End If
                Exceptions.Check(_IamBXRumbleDelegates.SetUpdateProperties(_IamBXRumblePtr, RumbleUpdateIntervalMS, SpeedDelta, Utility.ConstrainFloat(IntensityDelta)), "Unable to set rumble update properties")
            End Sub


            Private Sub GetUpdateProperties(ByRef RumbleUpdateIntervalMS As Int64, ByRef SpeedDelta As Single, ByRef IntensityDelta As Single)
                Exceptions.Check(_IamBXRumbleDelegates.GetUpdateProperties(_IamBXRumblePtr, RumbleUpdateIntervalMS, SpeedDelta, IntensityDelta), "Unable to get rumble update properties")
            End Sub


#Region " IDisposable Support "
            ' IDisposable
            Protected Overridable Sub Dispose(ByVal disposing As Boolean)
                If Not Me.disposedValue Then
                    If disposing Then
                        Me.Release()
                    End If
                End If
                Me.disposedValue = True
            End Sub
            Private disposedValue As Boolean = False        ' To detect redundant calls

            ' This code added by Visual Basic to correctly implement the disposable pattern.
            Public Sub Dispose() Implements IDisposable.Dispose
                ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
                Dispose(True)
                GC.SuppressFinalize(Me)
            End Sub
#End Region

        End Class
#End Region


#Region " Events"
        ''' <summary>
        ''' The Collection of Event objects that have been created.
        ''' </summary>
        ''' <remarks></remarks>
        Public Class EventCollection
            Inherits List(Of [Event])


            Public Overloads Function Add(ByVal Name As String, ByVal Filename As String) As [Event]
                Return New [Event](Name, Filename)
            End Function


            Public Overloads Function Add(ByVal Filename As String) As [Event]
                Return New [Event](Filename)
            End Function


            Public Overloads Function Add(ByVal Name As String, ByVal FileBuffer() As Byte) As [Event]
                Return New [Event](Name, FileBuffer)
            End Function


            Public Overloads Function Add(ByVal FileBuffer() As Byte) As [Event]
                Return New [Event](FileBuffer)
            End Function


            Public Overloads ReadOnly Property Item(ByVal Index As String) As [Event]
                Get
                    Return Me.FirstOrDefault(Function(L) StrComp(L.Name, Index, CompareMethod.Text) = 0)
                End Get
            End Property
        End Class
#End Region


#Region " Event"
        ''' <summary>
        ''' Create an Event, which is usually a short sequence of effects that cannot
        ''' be paused or seeked into, or an ambience effect which can be simply turned on
        ''' or off.
        ''' </summary>
        ''' <remarks></remarks>
        Public Class [Event]
            Implements IDisposable

            '---- these variables track the interface pointers and delegates used to 
            '     actually communicate with the ambx drivers.
            Private _IamBXEventPtr As IntPtr
            Private _IamBXEventInterface As IamBXEventInterface
            Private _IamBXEventDelegates As IamBXEventDelegates


            <StructLayout(LayoutKind.Sequential)> _
            Private Structure IamBXEventInterface
                Public PlayPtr As IntPtr
                Public StopPtr As IntPtr
                Public ReleasePtr As IntPtr
            End Structure


            Private Structure IamBXEventDelegates
                Public Play As PlayDelegate
                Public [Stop] As StopDelegate
                Public Release As ReleaseDelegate

                Public Sub Generate(ByVal IamBXEventInterface As IamBXEventInterface)
                    Me.Play = Marshal.GetDelegateForFunctionPointer(IamBXEventInterface.PlayPtr, GetType(PlayDelegate))
                    Me.Stop = Marshal.GetDelegateForFunctionPointer(IamBXEventInterface.StopPtr, GetType(StopDelegate))
                    Me.Release = Marshal.GetDelegateForFunctionPointer(IamBXEventInterface.ReleasePtr, GetType(ReleaseDelegate))
                End Sub


                <UnmanagedFunctionPointer(CallingConvention.Cdecl)> _
                Public Delegate Function PlayDelegate(ByVal IamBXEventPtr As IntPtr) As amBX_RESULT

                <UnmanagedFunctionPointer(CallingConvention.Cdecl)> _
                Public Delegate Function StopDelegate(ByVal IamBXEventPtr As IntPtr) As amBX_RESULT

                <UnmanagedFunctionPointer(CallingConvention.Cdecl)> _
                Public Delegate Function ReleaseDelegate(ByVal IamBXEventPtr As IntPtr) As amBX_RESULT
            End Structure


            ''' <summary>
            ''' Create a new Event object based on the contents of a buffer.
            ''' </summary>
            ''' <param name="FileBuffer">
            ''' A byte array containing the event data, preferably read from 
            ''' a bn/ck file. Note, the buffer must contain valid data and cannot
            ''' be nothing or empty.
            ''' </param>
            ''' <remarks></remarks>
            Public Sub New(ByVal FileBuffer As Byte())
                Initialize("", FileBuffer)
            End Sub


            ''' <summary>
            ''' Create a new Event object based on the contents of a bn/ck file.
            ''' </summary>
            ''' <param name="FileName"></param>
            ''' <remarks></remarks>
            Public Sub New(ByVal FileName As String)
                Dim FileBuffer = System.IO.File.ReadAllBytes(FileName)
                Initialize("", FileBuffer)
            End Sub


            ''' <summary>
            ''' Create a new Event object based on the contents of a buffer.
            ''' </summary>
            ''' <param name="FileBuffer">
            ''' A byte array containing the event data, preferably read from 
            ''' a bn/ck file. Note, the buffer must contain valid data and cannot
            ''' be nothing or empty.
            ''' </param>
            ''' <remarks></remarks>
            Public Sub New(ByVal Name As String, ByVal FileBuffer As Byte())
                Initialize(Name, FileBuffer)
            End Sub


            ''' <summary>
            ''' Create a new Event object based on the contents of a bn/ck file.
            ''' </summary>
            ''' <param name="FileName"></param>
            ''' <remarks></remarks>
            Public Sub New(ByVal Name As String, ByVal FileName As String)
                Dim FileBuffer = System.IO.File.ReadAllBytes(FileName)
                Initialize(Name, FileBuffer)
            End Sub


            Private Sub Initialize(ByVal Name As String, ByVal FileBuffer As Byte())
                Me.Name = Name
                _IamBXEventPtr = amBX.CreateEvent(FileBuffer)
                _IamBXEventInterface = Marshal.PtrToStructure(_IamBXEventPtr, GetType(IamBXEventInterface))
                _IamBXEventDelegates.Generate(_IamBXEventInterface)

                amBX.Events.Add(Me)
            End Sub


            ''' <summary>
            ''' Name of this Event
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property Name() As String
                Get
                    Return _Name
                End Get
                Set(ByVal value As String)
                    _Name = value
                End Set
            End Property
            Private _Name As String


            ''' <summary>
            ''' Plays the amBX file loaded when the interface was created.
            ''' </summary>
            ''' <remarks></remarks>
            Public Sub Play()
                Exceptions.Check(_IamBXEventDelegates.Play(_IamBXEventPtr), "Unable to release event object")
            End Sub


            ''' <summary>
            ''' Stops the playback of the event.
            ''' </summary>
            ''' <remarks></remarks>
            Public Sub [Stop]()
                Exceptions.Check(_IamBXEventDelegates.Stop(_IamBXEventPtr), "Unable to stop event object")
            End Sub


            Private Sub Release()
                Try
                    Exceptions.Check(_IamBXEventDelegates.Release(_IamBXEventPtr), "Unable to release event object")
                Finally
                    _IamBXEventInterface = New IamBXEventInterface
                    _IamBXEventDelegates = New IamBXEventDelegates
                    _IamBXEventPtr = 0
                End Try
            End Sub


#Region " IDisposable Support "
            ' IDisposable
            Protected Overridable Sub Dispose(ByVal disposing As Boolean)
                If Not Me.disposedValue Then
                    If disposing Then
                        Me.Release()
                    End If
                End If
                Me.disposedValue = True
            End Sub
            Private disposedValue As Boolean = False        ' To detect redundant calls


            ' This code added by Visual Basic to correctly implement the disposable pattern.
            Public Sub Dispose() Implements IDisposable.Dispose
                ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
                Dispose(True)
                GC.SuppressFinalize(Me)
            End Sub
#End Region

        End Class
#End Region


#Region " MovieCollection"
        ''' <summary>
        ''' The Collection of Event objects that have been created.
        ''' </summary>
        ''' <remarks></remarks>
        Public Class MovieCollection
            Inherits List(Of Movie)


            Public Overloads Function Add(ByVal Name As String, ByVal Filename As String) As Movie
                Return New Movie(Name, Filename)
            End Function


            Public Overloads Function Add(ByVal Filename As String) As Movie
                Return New Movie(Filename)
            End Function


            Public Overloads Function Add(ByVal Name As String, ByVal FileBuffer() As Byte) As Movie
                Return New Movie(Name, FileBuffer)
            End Function


            Public Overloads Function Add(ByVal FileBuffer() As Byte) As Movie
                Return New Movie(FileBuffer)
            End Function


            Public Overloads ReadOnly Property Item(ByVal Index As String) As Movie
                Get
                    Return Me.FirstOrDefault(Function(L) StrComp(L.Name, Index, CompareMethod.Text) = 0)
                End Get
            End Property
        End Class
#End Region


#Region " Movie"
        ''' <summary>
        ''' Represents an amBX Movie object, which is a sequence of effects that can
        ''' be paused or seeked into, and restarted arbitrarily. Mainly used for 
        ''' cutscenes. You should only create movie objects as you need them, and release
        ''' them when done.
        ''' </summary>
        ''' <remarks></remarks>
        Public Class Movie
            Implements IDisposable

            '---- these variables track the interface pointers and delegates used to 
            '     actually communicate with the ambx drivers.
            Private _IamBXMoviePtr As IntPtr
            Private _IamBXMovieInterface As IamBXMovieInterface
            Private _IamBXMovieDelegates As IamBXMovieDelegates


            <StructLayout(LayoutKind.Sequential)> _
            Private Structure IamBXMovieInterface
                Public ReleasePtr As IntPtr
                Public SetFrozenPtr As IntPtr
                Public GetFrozenPtr As IntPtr
                Public SetSuspendedPtr As IntPtr
                Public GetSuspendedPtr As IntPtr
                Public SeekPtr As IntPtr
            End Structure


            Private Structure IamBXMovieDelegates
                Public Release As ReleaseDelegate
                Public SetFrozen As SetFrozenDelegate
                Public GetFrozen As GetFrozenDelegate
                Public SetSuspended As SetSuspendedDelegate
                Public GetSuspended As GetSuspendedDelegate
                Public Seek As SeekDelegate


                Public Sub Generate(ByVal IamBXMovieInterface As IamBXMovieInterface)
                    Me.Release = Marshal.GetDelegateForFunctionPointer(IamBXMovieInterface.ReleasePtr, GetType(ReleaseDelegate))
                    Me.SetFrozen = Marshal.GetDelegateForFunctionPointer(IamBXMovieInterface.SetFrozenPtr, GetType(SetFrozenDelegate))
                    Me.GetFrozen = Marshal.GetDelegateForFunctionPointer(IamBXMovieInterface.GetFrozenPtr, GetType(GetFrozenDelegate))
                    Me.SetSuspended = Marshal.GetDelegateForFunctionPointer(IamBXMovieInterface.SetSuspendedPtr, GetType(SetSuspendedDelegate))
                    Me.GetSuspended = Marshal.GetDelegateForFunctionPointer(IamBXMovieInterface.GetSuspendedPtr, GetType(GetSuspendedDelegate))
                    Me.Seek = Marshal.GetDelegateForFunctionPointer(IamBXMovieInterface.SeekPtr, GetType(SeekDelegate))
                End Sub


                <UnmanagedFunctionPointer(CallingConvention.Cdecl)> _
                Public Delegate Function ReleaseDelegate(ByVal IamBXMoviePtr As IntPtr) As amBX_RESULT

                <UnmanagedFunctionPointer(CallingConvention.Cdecl)> _
                Public Delegate Function SetFrozenDelegate(ByVal IamBXMoviePtr As IntPtr, _
                                                           <MarshalAs(UnmanagedType.I1)> ByVal Frozen As Boolean) As amBX_RESULT

                <UnmanagedFunctionPointer(CallingConvention.Cdecl)> _
                Public Delegate Function GetFrozenDelegate(ByVal IamBXMoviePtr As IntPtr, _
                                                           <MarshalAs(UnmanagedType.I1)> ByRef Frozen As Boolean) As amBX_RESULT

                <UnmanagedFunctionPointer(CallingConvention.Cdecl)> _
                Public Delegate Function SetSuspendedDelegate(ByVal IamBXMoviePtr As IntPtr, _
                                                           <MarshalAs(UnmanagedType.I1)> ByVal Suspended As Boolean) As amBX_RESULT

                <UnmanagedFunctionPointer(CallingConvention.Cdecl)> _
                Public Delegate Function GetSuspendedDelegate(ByVal IamBXMoviePtr As IntPtr, _
                                                           <MarshalAs(UnmanagedType.I1)> ByRef Suspended As Boolean) As amBX_RESULT

                <UnmanagedFunctionPointer(CallingConvention.Cdecl)> _
                Public Delegate Function SeekDelegate(ByVal IamBXMoviePtr As IntPtr, _
                                                       ByVal Seconds As Single) As amBX_RESULT
            End Structure


            ''' <summary>
            ''' Create a new Movie object based on the contents of a buffer.
            ''' </summary>
            ''' <param name="FileBuffer">
            ''' A byte array containing the event data, preferably read from 
            ''' a bn/ck file.
            ''' </param>
            ''' <remarks></remarks>
            Public Sub New(ByVal FileBuffer As Byte())
                Initialize("", FileBuffer)
            End Sub


            ''' <summary>
            ''' Create a new Movie object based on the contents of a buffer.
            ''' </summary>
            ''' <param name="FileBuffer">
            ''' A byte array containing the event data, preferably read from 
            ''' a bn/ck file.
            ''' </param>
            ''' <remarks></remarks>
            Public Sub New(ByVal Name As String, ByVal FileBuffer As Byte())
                Initialize(Name, FileBuffer)
            End Sub


            ''' <summary>
            ''' Create a new Movie object based on the contents of a bn/ck file.
            ''' </summary>
            ''' <param name="FileName"></param>
            ''' <remarks></remarks>
            Public Sub New(ByVal FileName As String)
                Dim FileBuffer = System.IO.File.ReadAllBytes(FileName)
                Initialize("", FileBuffer)
            End Sub


            ''' <summary>
            ''' Create a new Movie object based on the contents of a bn/ck file.
            ''' </summary>
            ''' <param name="FileName"></param>
            ''' <remarks></remarks>
            Public Sub New(ByVal Name As String, ByVal FileName As String)
                Dim FileBuffer = System.IO.File.ReadAllBytes(FileName)
                Initialize(Name, FileBuffer)
            End Sub


            ''' <summary>
            ''' Initialize the internal elements and create the Movie interface
            ''' </summary>
            ''' <param name="FileBuffer"></param>
            ''' <remarks></remarks>
            Private Sub Initialize(ByVal Name As String, ByVal FileBuffer As Byte())
                Me.Name = Name
                _IamBXMoviePtr = amBX.CreateMovie(FileBuffer)
                _IamBXMovieInterface = Marshal.PtrToStructure(_IamBXMoviePtr, GetType(IamBXMovieInterface))
                _IamBXMovieDelegates.Generate(_IamBXMovieInterface)

                amBX.Movies.Add(Me)
            End Sub


            ''' <summary>
            ''' Name of this Movie
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property Name() As String
                Get
                    Return _Name
                End Get
                Set(ByVal value As String)
                    _Name = value
                End Set
            End Property
            Private _Name As String


            Private Sub Release()
                Try
                    Exceptions.Check(_IamBXMovieDelegates.Release(_IamBXMoviePtr), "Unable to release movie object")
                Finally
                    _IamBXMovieInterface = New IamBXMovieInterface
                    _IamBXMovieDelegates = New IamBXMovieDelegates
                    _IamBXMoviePtr = 0
                End Try
            End Sub


            ''' <summary>
            ''' Method to simplify playing a loaded movie.
            ''' Basically, just Seek to the beginning and unsuspend.
            ''' </summary>
            ''' <remarks></remarks>
            Public Sub Play()
                Me.Seek(0)
                Me.Suspended = False
            End Sub


            ''' <summary>
            ''' Pause this movie's playback
            ''' </summary>
            ''' <remarks></remarks>
            Public Sub Pause()
                Me.Frozen = True
            End Sub


            ''' <summary>
            ''' Continue a paused movie
            ''' </summary>
            ''' <remarks></remarks>
            Public Sub [Continue]()
                Me.Frozen = False
            End Sub


            ''' <summary>
            ''' Stop playback and reset the movie to the beginning.
            ''' </summary>
            ''' <remarks></remarks>
            Public Sub [Stop]()
                Me.Pause()
                Me.Seek(0)
            End Sub


            ''' <summary>
            ''' Sets or retrieves the frozen state of this movie.
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property Frozen() As Boolean
                Get
                    Dim f As Boolean
                    Exceptions.Check(_IamBXMovieDelegates.GetFrozen(_IamBXMoviePtr, f), "Unable to get movie frozen property")
                    Return f
                End Get
                Set(ByVal value As Boolean)
                    Exceptions.Check(_IamBXMovieDelegates.SetFrozen(_IamBXMoviePtr, value), "Unable to set movie frozen property")
                End Set
            End Property


            ''' <summary>
            ''' Sets or retrieves the suspended state of this move.
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property Suspended() As Boolean
                Get
                    Dim s As Boolean
                    Exceptions.Check(_IamBXMovieDelegates.GetSuspended(_IamBXMoviePtr, s), "Unable to get movie suspended property")
                    Return s
                End Get
                Set(ByVal value As Boolean)
                    Exceptions.Check(_IamBXMovieDelegates.SetSuspended(_IamBXMoviePtr, value), "Unable to set movie suspended property")
                End Set
            End Property


            ''' <summary>
            ''' Jumps to the specified point in time within the movie. The seek will
            ''' on be made if the absolute difference of the current time 
            ''' and the last seek time is greater than the seek threshold
            ''' (Currently 0.2 seconds).
            ''' </summary>
            ''' <param name="Seconds"></param>
            ''' <remarks></remarks>
            Public Sub Seek(ByVal Seconds As Single)
                Exceptions.Check(_IamBXMovieDelegates.Seek(_IamBXMoviePtr, Seconds), "Unable to seek in movie object")
            End Sub


#Region " IDisposable Support "
            ' IDisposable
            Protected Overridable Sub Dispose(ByVal disposing As Boolean)
                If Not Me.disposedValue Then
                    If disposing Then
                        Me.Release()
                    End If
                End If
                Me.disposedValue = True
            End Sub
            Private disposedValue As Boolean = False        ' To detect redundant calls

            ' This code added by Visual Basic to correctly implement the disposable pattern.
            Public Sub Dispose() Implements IDisposable.Dispose
                ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
                Dispose(True)
                GC.SuppressFinalize(Me)
            End Sub
#End Region

        End Class
#End Region

    End Class


#Region " Exceptions"
    Friend Class Exceptions

        ''' <summary>
        ''' Eases exception handling and generation, Internal only, not a creatable class.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub New()
        End Sub


        ''' <summary>
        ''' Check an amBX error code and throw the applicable exception
        ''' </summary>
        ''' <param name="ErrCode"></param>
        ''' <param name="Message"></param>
        ''' <remarks></remarks>
        Public Shared Sub Check(ByVal ErrCode As amBX_RESULT, Optional ByVal Message As String = "")
            Select Case ErrCode
                Case amBX_RESULT.amBX_OK
                    '---- nothing to do in this case
                Case amBX_RESULT.amBX_NO_SPACE
                    Throw New amBXExceptionNoSpace(Message)
                Case amBX_RESULT.amBX_ERROR
                    Throw New amBXExceptionError(Message)
                Case amBX_RESULT.amBX_NOT_FOUND
                    Throw New amBXExceptionNotFound(Message)
                Case amBX_RESULT.amBX_VERSION_NOT_FOUND
                    Throw New amBXExceptionVersionNotFound(Message)
                Case amBX_RESULT.amBX_BAD_ARGS
                    Throw New amBXExceptionBadArgs(Message)
                Case amBX_RESULT.amBX_OUT_OF_RANGE
                    Throw New amBXExceptionOutofRange(Message)
                Case amBX_RESULT.amBX_OUT_OF_MEMORY
                    Throw New amBXExceptionOutofMemory(Message)
                Case amBX_RESULT.amBX_NOT_INSTALLED
                    Throw New amBXExceptionNotInstalled(Message)
                Case amBX_RESULT.amBX_OLD_VERSION
                    Throw New amBXExceptionOldVersion(Message)
                Case amBX_RESULT.amBX_ENGINE_LOST
                    Throw New amBXExceptionEngineLost(Message)
                Case amBX_RESULT.amBX_SENDING_TIMEOUT
                    Throw New amBXExceptionSendingTimeout(Message)
                Case amBX_RESULT.amBX_NOT_THREADED
                    Throw New amBXExceptionNotThreaded(Message)
                Case amBX_RESULT.amBX_BAD_THREADID
                    Throw New amBXExceptionBadThreadID(Message)
                Case amBX_RESULT.amBX_THREAD_EXISTS
                    Throw New amBXExceptionThreadExists(Message)
                Case amBX_RESULT.amBX_UPDATE_TIMEOUT
                    Throw New amBXExceptionUpdateTimeout(Message)
                Case amBX_RESULT.amBX_THREAD_TIMEOUT
                    Throw New amBXExceptionThreadTimeout(Message)
            End Select
        End Sub
    End Class


    ''' <summary>
    ''' The Generic amBX exception. All the specific exceptions inherit from
    ''' this one.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class amBXException
        Inherits Exception

        Public Sub New(ByVal Number As amBX_RESULT, ByVal Message As String)
            _Number = Number
            _Message = Message
        End Sub


        Public Overrides ReadOnly Property Message() As String
            Get
                Return _Message
            End Get
        End Property
        Private _Message As String


        Public ReadOnly Property Number() As amBX_RESULT
            Get
                Return _Number
            End Get
        End Property
        Private _Number As amBX_RESULT
    End Class


    Public Class amBXExceptionamBXrtdllNotFound
        Inherits amBXException

        Public Sub New()
            Me.New(String.Empty)
        End Sub

        Public Sub New(ByVal Message As String)
            MyBase.New(0, Message & ": Could not be found.")
        End Sub
    End Class


    Public Class amBXExceptionNotConnected
        Inherits amBXException

        Public Sub New()
            MyBase.New(0, "The amBXLibrary has not been connected. Please call CONNECT first.")
        End Sub
    End Class


    Public Class amBXExceptionNoSpace
        Inherits amBXException

        Public Sub New()
            Me.New(String.Empty)
        End Sub

        Public Sub New(ByVal Message As String)
            MyBase.New(amBX_RESULT.amBX_NO_SPACE, Message & ": Out of buffer space.")
        End Sub
    End Class


    Public Class amBXExceptionError
        Inherits amBXException

        Public Sub New()
            Me.New(String.Empty)
        End Sub

        Public Sub New(ByVal Message As String)
            MyBase.New(amBX_RESULT.amBX_ERROR, Message & ": General Failure.")
        End Sub
    End Class


    Public Class amBXExceptionNotFound
        Inherits amBXException

        Public Sub New()
            Me.New(String.Empty)
        End Sub

        Public Sub New(ByVal Message As String)
            MyBase.New(amBX_RESULT.amBX_NOT_FOUND, Message & ": File or device not found.")
        End Sub
    End Class


    Public Class amBXExceptionVersionNotFound
        Inherits amBXException

        Public Sub New()
            Me.New(String.Empty)
        End Sub

        Public Sub New(ByVal Message As String)
            MyBase.New(amBX_RESULT.amBX_VERSION_NOT_FOUND, Message & ": Expected version of amBX API not found.")
        End Sub
    End Class


    Public Class amBXExceptionBadArgs
        Inherits amBXException

        Public Sub New()
            Me.New(String.Empty)
        End Sub

        Public Sub New(ByVal Message As String)
            MyBase.New(amBX_RESULT.amBX_BAD_ARGS, Message & ": Bad argument detected (usually a null pointer).")
        End Sub
    End Class


    Public Class amBXExceptionOutofRange
        Inherits amBXException

        Public Sub New()
            Me.New(String.Empty)
        End Sub

        Public Sub New(ByVal Message As String)
            MyBase.New(amBX_RESULT.amBX_OUT_OF_RANGE, Message & ": Argument out of range.")
        End Sub
    End Class


    Public Class amBXExceptionOutofMemory
        Inherits amBXException

        Public Sub New()
            Me.New(String.Empty)
        End Sub

        Public Sub New(ByVal Message As String)
            MyBase.New(amBX_RESULT.amBX_OUT_OF_MEMORY, Message & ": Could not allocate memory.")
        End Sub
    End Class


    Public Class amBXExceptionNotInstalled
        Inherits amBXException

        Public Sub New()
            Me.New(String.Empty)
        End Sub

        Public Sub New(ByVal Message As String)
            MyBase.New(amBX_RESULT.amBX_NOT_INSTALLED, Message & ": amBX is not installed.")
        End Sub
    End Class


    Public Class amBXExceptionOldVersion
        Inherits amBXException

        Public Sub New()
            Me.New(String.Empty)
        End Sub

        Public Sub New(ByVal Message As String)
            MyBase.New(amBX_RESULT.amBX_OLD_VERSION, Message & ": amBX is installed, but is not the right version.")
        End Sub
    End Class


    Public Class amBXExceptionEngineLost
        Inherits amBXException

        Public Sub New()
            Me.New(String.Empty)
        End Sub

        Public Sub New(ByVal Message As String)
            MyBase.New(amBX_RESULT.amBX_ENGINE_LOST, Message & ": Connection to the amBX has been lost.")
        End Sub
    End Class


    Public Class amBXExceptionSendingTimeout
        Inherits amBXException

        Public Sub New()
            Me.New(String.Empty)
        End Sub

        Public Sub New(ByVal Message As String)
            MyBase.New(amBX_RESULT.amBX_SENDING_TIMEOUT, Message & ": Request to send script timed out.")
        End Sub
    End Class


    Public Class amBXExceptionNotThreaded
        Inherits amBXException

        Public Sub New()
            Me.New(String.Empty)
        End Sub

        Public Sub New(ByVal Message As String)
            MyBase.New(amBX_RESULT.amBX_NOT_THREADED, Message & ": A threaded function was called, but threading is not enabled.")
        End Sub
    End Class


    Public Class amBXExceptionBadThreadID
        Inherits amBXException

        Public Sub New()
            Me.New(String.Empty)
        End Sub

        Public Sub New(ByVal Message As String)
            MyBase.New(amBX_RESULT.amBX_BAD_THREADID, Message & ": A thread ID is incorrect or doesn't exist.")
        End Sub
    End Class


    Public Class amBXExceptionThreadExists
        Inherits amBXException

        Public Sub New()
            Me.New(String.Empty)
        End Sub

        Public Sub New(ByVal Message As String)
            MyBase.New(amBX_RESULT.amBX_THREAD_EXISTS, Message & ": A thread is currently being used for the requested function.")
        End Sub
    End Class


    Public Class amBXExceptionUpdateTimeout
        Inherits amBXException

        Public Sub New()
            Me.New(String.Empty)
        End Sub

        Public Sub New(ByVal Message As String)
            MyBase.New(amBX_RESULT.amBX_UPDATE_TIMEOUT, Message & ": Waiting to perform an update timed out.")
        End Sub
    End Class


    Public Class amBXExceptionThreadTimeout
        Inherits amBXException

        Public Sub New()
            Me.New(String.Empty)
        End Sub

        Public Sub New(ByVal Message As String)
            MyBase.New(amBX_RESULT.amBX_THREAD_TIMEOUT, Message & ": Request to run a thread timed out.")
        End Sub
    End Class
#End Region


    ''' <summary>
    ''' Various routines for use internally by the amBX library classes.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Utility

        ''' <summary>
        ''' Simple routine to contrains a single between 0 and 1.
        ''' </summary>
        ''' <param name="v"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ConstrainFloat(ByRef v As Single) As Single
            If v < 0 Then
                v = 0
            ElseIf v > 1 Then
                v = 1
            End If
            Return v
        End Function
    End Class
End Namespace
