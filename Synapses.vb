Public Class Synapses
    Public Weight As Double
    Public imp As Double

    Public Sub New()
        Randomize()
        Me.Weight = Rnd()
        'Me.Weight = 
    End Sub

    Public Sub New(value As Double)
        Me.Weight = value
    End Sub
End Class
