Public Class Neurons
    Public Dendrites() As Synapses
    Public Bias As Synapses
    'Public type As Integer
    Public value As Double
    Public Delta As Double
    Public Sum As Double

    Public Sub New(ByVal NumberOfImputs As Integer)
        For i = 0 To NumberOfImputs - 1
            ReDim Preserve Dendrites(i)
            Dendrites(i) = New Synapses
        Next
        Me.Bias = New Synapses
        Me.Bias.imp = 1
        'Me.type = ty
    End Sub

    Public Sub New(ByVal NumberOfImputs As Integer, ByVal Weights() As Double)
        For i = 0 To NumberOfImputs - 1
            ReDim Preserve Dendrites(i)
            Dendrites(i) = New Synapses(Weights(i))
        Next
        Me.Bias = New Synapses(Weights(Weights.Length - 1))
        Me.Bias.imp = 1
    End Sub


    Public Function Calculate(ByVal Imputs() As Double)
        Dim ret As Double
        If Imputs.Length <> Me.Dendrites.Length Then
            MsgBox("ERROR")
            Return 404   '404 = Codi de error
        End If

        Dim sum As Double = 0
        For i = 0 To Imputs.Length - 1
            sum += Me.Dendrites(i).Weight * Imputs(i)
            Me.Dendrites(i).imp = Imputs(i)
        Next

        sum += Bias.Weight

        Me.Sum = sum
        'ret = sum / (Math.Sqrt(1 + sum ^ 2))
        ret = 1 / (1 + Math.Exp(-sum))
        Me.value = ret
        Return ret
    End Function

End Class
