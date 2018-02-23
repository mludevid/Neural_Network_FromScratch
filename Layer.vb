Public Class Layer
    Public Neu() As Neurons

    Public Sub New(ByVal dimension As Integer, ByVal NumberOfImputs As Integer)
        'Dim me.Neu(dimension)
        For i As Integer = 0 To dimension - 1
            ReDim Preserve Me.Neu(i)
            Me.Neu(i) = New Neurons(NumberOfImputs)
        Next
    End Sub

    Public Sub New(ByVal dimension As Integer, ByVal NumberOfImputs As Integer, ByVal Weights As List(Of Double()))
        For i As Integer = 0 To dimension - 1
            ReDim Preserve Me.Neu(i)
            Me.Neu(i) = New Neurons(NumberOfImputs, Weights(i))
        Next
    End Sub

    Public Function Calculate(ByVal Imputs() As Double) As Double()
        Dim ret() As Double = Nothing

        For i = 0 To Me.Neu.Length - 1
            ReDim Preserve ret(i)
            ret(i) = Me.Neu(i).Calculate(Imputs)
        Next

        Return ret
    End Function
End Class
