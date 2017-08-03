Imports System.Windows.Media.Animation
Class MainWindow
    Dim Ease
    Dim sd As Integer
    Dim x As Integer = 0
    Dim so As Boolean = False
    Dim Check As ClockGroup
    Dim CloseForm As Storyboard
    Dim sr, mr, hr As New RotateTransform
    Dim sa As New Animation.DoubleAnimation
    Dim Timer As System.Windows.Threading.DispatcherTimer
    Private Sub LayoutRoot_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)
        sd = 360 / 60 * Date.Now.Second
        Second.RenderTransform = New RotateTransform(sd)
        Minute.RenderTransform = New RotateTransform(360 / 60 * Date.Now.Minute)
        Hour.RenderTransform = New RotateTransform(360 / 12 * Date.Now.Hour)
        sa.Duration = TimeSpan.FromSeconds(1)
        Me.Left = System.Windows.SystemParameters.WorkArea.Width - Me.Width - 10
        Me.Top = System.Windows.SystemParameters.WorkArea.Height - Me.Height - 10
        Timer = New System.Windows.Threading.DispatcherTimer()
        CloseForm = CType(Me.Resources("Closing"), Storyboard)
        Check = CloseForm.CreateClock
        AddHandler CloseForm.Completed, AddressOf ExitApp
        AddHandler Timer.Tick, AddressOf Tick
        Timer.Interval = TimeSpan.FromSeconds(1)
        Timer.Start()
    End Sub
    Private Sub Tick(sender As Object, e As EventArgs)
        sa.From = sd
        sd += 6
        sa.To = sd
        sr.BeginAnimation(RotateTransform.AngleProperty, sa)
        Second.RenderTransform = sr
        If sd Mod 90 Then
            mr.Angle = 360 / 60 * Date.Now.Minute
            Minute.RenderTransform = mr
        ElseIf sd Mod 3600 Then
            hr.Angle = 360 / 12 * Date.Now.Hour
            Hour.RenderTransform = hr
        End If
        If so = True Then
            Time.Content = Date.Now.Date
        Else
            Time.Content = Date.Now.Hour & ":" & Date.Now.Minute & ":" & Date.Now.Second
        End If
    End Sub
    Private Sub MainWindow_MouseDoubleClick(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles Me.MouseDoubleClick
        If Digital.Visibility = Windows.Visibility.Hidden Then
            Digital.Visibility = Windows.Visibility.Visible
        Else
            Digital.Visibility = Windows.Visibility.Hidden
        End If
    End Sub
    Private Sub Window_MouseLeftButtonDown(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        Me.DragMove()
    End Sub
    Private Sub MainWindow_MouseWheel(sender As Object, e As System.Windows.Input.MouseWheelEventArgs) Handles Me.MouseWheel
        x += 1
        If x = 1 Then
            Ease = New ElasticEase
        ElseIf x = 2 Then
            Ease = New SineEase
        ElseIf x = 3 Then
            Second.Visibility = Windows.Visibility.Hidden
        Else
            Second.Visibility = Windows.Visibility.Visible
            sa.EasingFunction = Nothing
            x = 0
            Exit Sub
        End If
        sa.EasingFunction = Ease
    End Sub
    Private Sub Digital_MouseLeftButtonDown(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles Digital.MouseLeftButtonDown
        If so = False Then
            so = True
        Else
            so = False
        End If
        If so = True Then
            Time.Content = Date.Now.Date
        Else
            Time.Content = Date.Now.Hour & ":" & Date.Now.Minute & ":" & Date.Now.Second
        End If
    End Sub
    Private Sub MainWindow_MouseRightButtonDown(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles Me.MouseRightButtonDown
        If Check.CurrentProgress = 1 Then
            RemoveHandler Me.MouseLeftButtonDown, New MouseButtonEventHandler(AddressOf Window_MouseLeftButtonDown)
            RemoveHandler Me.MouseRightButtonDown, New MouseButtonEventHandler(AddressOf MainWindow_MouseRightButtonDown)
            RemoveHandler Me.MouseDoubleClick, New MouseButtonEventHandler(AddressOf MainWindow_MouseDoubleClick)
            RemoveHandler Me.MouseWheel, New MouseWheelEventHandler(AddressOf MainWindow_MouseWheel)
            RemoveHandler Digital.MouseLeftButtonDown, New MouseButtonEventHandler(AddressOf Digital_MouseLeftButtonDown)
            Timer.Stop()
            CloseForm.Begin(Me, True)
        End If
    End Sub
    Sub ExitApp()
        Application.Current.Shutdown()
    End Sub
End Class