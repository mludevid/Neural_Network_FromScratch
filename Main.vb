Public Class Form1
    Public NS As NeuralSystem
    Private erranterior As Double = 99
    Public TrainingS As New TrainingSet

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Dim Dime() As Integer = {2, 2, 1}
        NS = New NeuralSystem(Dime, 2)
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        TrainingS.Add({0, 0}, {0})
        TrainingS.Add({1, 0}, {1})
        TrainingS.Add({0, 1}, {1})
        TrainingS.Add({1, 1}, {0})

        Dim Execution_Start As New Stopwatch
        Execution_Start.Start()
        Dim dou As Double
        dou = NS.Train(TrainingS, 5000000)
        Label2.Text = dou
        MsgBox("Hours: " & Execution_Start.Elapsed.Hours & " Minutes: " & Execution_Start.Elapsed.Minutes & " Seconds: " & Execution_Start.Elapsed.Seconds & " Miliseconds: " & Execution_Start.Elapsed.Milliseconds)
    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        Dim imp(1) As Double
        imp(0) = TextBox1.Text
        imp(1) = TextBox2.Text
        Dim out() As Double = NS.Calculate(imp)
        Label3.Text = out(0)
    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        Dim bol As Boolean = NS.Save("C:\Users\marc\Documents\Temporal\Borrar.txt")
        Dim NSNew As New NeuralSystem("C:\Users\marc\Documents\Temporal\Borrar.txt")

        Dim imp(1) As Double
        imp(0) = TextBox1.Text
        imp(1) = TextBox2.Text
        Dim out() As Double = NSNew.Calculate(imp)
        Label4.Text = out(0)
    End Sub
End Class
