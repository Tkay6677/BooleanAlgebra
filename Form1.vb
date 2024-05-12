Public Class Form1
    Dim expression As String = "B'+C+B+C+C'"
    Dim input As String
    Dim Delimiter As String
    Dim newStr As String
    Dim ExpressionArry() As String
    Dim ORLaws As New Dictionary(Of String, String) From {{"A+1", "1"}, {"A+A", "A"}, {"A+A'", "1"}, {"A+0", "A"},
                                                          {"A*1", "A"}, {"A*A", "A"}, {"A*A'", "0"}, {"A*0", "0"}}
    Dim Variable As String
    Dim HasValue As Boolean
    Dim Operators As String
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TextBox1.Text = expression

       


    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
      
        SmartSplit()
        For Each item In ListBox1.Items
            TextBox2.Text = TextBox2.Text & item
        Next
        TextBox2.Text = TextBox2.Text & vbCrLf
        Do Until "a" = "b"
            Grouping()
            ' MsgBox(ListBox2.Items.Count)
            If ListBox2.Items.Count = 0 Then
                '    MsgBox("END")
                Exit Do
            End If
            For i As Integer = 0 To ListBox2.Items.Count - 1
                Translate(i)
                PerformOperation(0, i)
                If HasValue Then
                    Exit For
                End If
            Next
        Loop

    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        SmartSplit()
        For Each item In ListBox1.Items
            TextBox2.Text = TextBox2.Text & item
        Next
        TextBox2.Text = TextBox2.Text & vbCrLf
    End Sub
  
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Grouping()
    End Sub

    Private Sub ListBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox2.SelectedIndexChanged
        Translate(ListBox2.SelectedIndex)
    End Sub

    Private Sub ListBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox3.SelectedIndexChanged
        PerformOperation(0, ListBox2.SelectedIndex)
    End Sub
    Sub Grouping()
        ListBox2.Items.Clear()

        'For j As Integer = 0 To Operators.Length - 1
        For i As Integer = 0 To ListBox1.Items.Count - 1
            Operators = GetOperator(ListBox1.Items(i))

            If ListBox1.Items(i) = Operators Then
                ListBox2.Items.Add(ListBox1.Items(i - 1) & ListBox1.Items(i) & ListBox1.Items(i + 1))

            End If

        Next
        '  Next
    End Sub
    Sub SmartSplit()
        expression = TextBox1.Text
        TextBox2.Text = Nothing
        ReDim ExpressionArry(expression.Length - 1)
        For i As Integer = 0 To expression.Length - 1

            If expression.ToArray(i) = "'" Then
                ' ListBox1.Items.RemoveAt(ListBox1.Items.Count - 1)
                ExpressionArry(i - 1) = Nothing
                input = expression.ToArray(i - 1) & expression.ToArray(i)
            Else
                input = expression.ToArray(i)
            End If
            ' ListBox1.Items.Add(input)
            ExpressionArry(i) = input
        Next
        ListBox1.Items.Clear()
        For Each item In ExpressionArry
            LoadLB(item)
        Next
    End Sub
    Sub LoadLB(item As String)
        If Not item = Nothing Then
            ListBox1.Items.Add(item)
            'MsgBox("Hold")
        End If
    End Sub
    Sub Translate(index As Integer)
        Dim Bla() As String

        ListBox3.Items.Clear()
        If Not ListBox2.Items.Count = 0 Then
            'For i As Integer = 0 To Operators.Length - 1 'ListBox2.Items.Count - 1
            Operators = GetOperator(ListBox2.Items(index).ToString)
            Bla = ListBox2.Items(index).ToString.Split(Operators)
                If Bla(0) = Bla(1) Then
                    Variable = Bla(0)
                ListBox3.Items.Add("A" & Operators & "A")
                ElseIf Bla(0).Contains(Bla(1)) Or Bla(1).Contains(Bla(0)) Then
                    Variable = Bla(0)
                ListBox3.Items.Add("A" & Operators & "A'")
            ElseIf IsNumeric(Bla(0)) Or IsNumeric(Bla(1)) Then


                If IsNumeric(Bla(0)) Then

                    Select Case CInt(Bla(0))
                        Case 0
                            ListBox3.Items.Add("A" & Operators & 0)
                            Variable = Bla(1)
                        Case 1
                            ListBox3.Items.Add("A" & Operators & 1)
                            Variable = Bla(0)
                    End Select
                ElseIf IsNumeric(Bla(1)) Then


                    Select Case CInt(Bla(1))
                        Case 0
                            ListBox3.Items.Add("A" & Operators & 0)
                            Variable = Bla(1)
                        Case 1
                            ListBox3.Items.Add("A" & Operators & 1)
                            Variable = Bla(0)
                    End Select

                End If



            Else
                ListBox3.Items.Add("A" & Operators & "B")
            End If
            End If
            'Next


    End Sub
    Function GetOperator(text As String)
        Dim op As String = Nothing
        If text.Contains("+") Then
            op = "+"
        ElseIf text.Contains("*") Then
            op = "*"
        End If
        Return op
    End Function
    Sub PerformOperation(index As Integer, index2 As Integer)

        If ORLaws.ContainsKey(ListBox3.Items(index)) Then
            HasValue = True
            'MsgBox("boo")
            'MsgBox(ORLaws(ListBox3.Items(index)) & IsNumeric(ORLaws(ListBox3.Items(index))))
            If IsNumeric(ORLaws(ListBox3.Items(index))) Then
                'MsgBox()
                ListBox4.Items.Add(ORLaws(ListBox3.Items(index)))

            Else
                MsgBox(Variable)
                ListBox4.Items.Add(Variable)

            End If
            AddRest(index2)
            For Each item In ListBox1.Items
                TextBox2.Text = TextBox2.Text & item
            Next
            TextBox2.Text = TextBox2.Text & vbCrLf
            'Reset()
        Else
            HasValue = False
            'MsgBox("No way")
            For i As Integer = index2 * 2 To (index2 * 2) + 1
                ListBox4.Items.Add(ListBox1.Items(i))
            Next

        End If

    End Sub
    Sub AddRest(index As Integer)
        'MsgBox(index * 2)
        If (index * 2) + 2 <> ListBox1.Items.Count - 1 Then
            For i As Integer = (index * 2) + 3 To ListBox1.Items.Count - 1
                ListBox4.Items.Add(ListBox1.Items(i))
            Next

        End If
        Reset()
        '  

    End Sub
    Sub Reset()
        'Try
       
        ListBox1.Items.Clear()
        For Each item In ListBox4.Items
            LoadLB(item)
        Next
        ListBox4.Items.Clear()
        'Grouping()
        'Translate(0)
        'ListBox4.Items.Clear()
        'For i As Integer = 0 To ListBox3.Items.Count - 1
        '    PerformOperation(i)
        'Next
        'Catch ex As Exception
        'MsgBox(ex.ToString)
        'End Try

    End Sub
    Function IsAlpha(character As String)
        Dim result As Boolean = False
        For i As Integer = 65 To 90
            If Asc(character) = i Then
                result = True
            End If
        Next
        Return result
    End Function

   
   
   
    
   
End Class
