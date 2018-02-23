Public Class NeuralSystem
    Public Lay() As Layer
    'Public TrainingS As New TrainingSet

    Public Sub New(ByVal DimensionOfEachLayer() As Integer, ByVal NumberOfImputsAtTheBeginning As Integer)

        For i = 0 To DimensionOfEachLayer.Length - 1
            ReDim Preserve Lay(i)
            Dim imputs As Integer

            If i = 0 Then
                imputs = NumberOfImputsAtTheBeginning
            Else
                imputs = DimensionOfEachLayer(i - 1)
            End If

            Lay(i) = New Layer(DimensionOfEachLayer(i), imputs)
        Next


    End Sub

    Public Function Train(ByRef TrainingS As TrainingSet, ByVal Repetition As Integer) As Double
        If TrainingS.Length = 0 Then Return 999
        Dim ret As Double = 0
        For Integ As Integer = 0 To Repetition
            For i = 0 To TrainingS.Length - 1
                'ret = Me.RecalculateW(speed, trainingS(i).TargetOutput, Me.Calculate(trainingS(i).Imputs))
                ret = Me.RecalculateW(TrainingS.Velocity, TrainingS.Out(i), Me.Calculate(TrainingS.Imp(i)))
            Next
        Next
        Return ret
    End Function

    Public Function Calculate(ByVal Imputs() As Double) As Double()
        Dim ret() As Double = Nothing
        Dim imp() As Double
        imp = Imputs.Clone

        For i = 0 To Lay.Length - 1
            Dim out() As Double = Lay(i).Calculate(imp)
            imp = out.Clone
        Next

        ret = imp
        Return ret
    End Function

    Public Function RecalculateW(ByVal SpeedOfCorrection As Double, ByVal Target() As Double, ByRef output() As Double) As Double
        Dim ret As Double = 0

        'Calculate Delta
        For Layers As Integer = Me.Lay.Length - 1 To 0 Step -1

            For Neur As Integer = Me.Lay(Layers).Neu.Length - 1 To 0 Step -1

                If Layers = Me.Lay.Length - 1 Then
                    'Output Layer
                    Dim ErrorFactor As Double = Target(Neur) - output(Neur)
                    ret += ErrorFactor
                    Me.Lay(Layers).Neu(Neur).Delta = Me.Lay(Layers).Neu(Neur).value * (1 - Me.Lay(Layers).Neu(Neur).value) * ErrorFactor
                Else
                    Dim ErrorFactor As Double = 0
                    For i = 0 To Me.Lay(Layers + 1).Neu.Length - 1
                        ErrorFactor += Me.Lay(Layers + 1).Neu(i).Delta * Me.Lay(Layers + 1).Neu(i).Dendrites(Neur).Weight
                    Next
                    Me.Lay(Layers).Neu(Neur).Delta = Me.Lay(Layers).Neu(Neur).value * (1 - Me.Lay(Layers).Neu(Neur).value) * ErrorFactor
                End If

            Next

        Next



        'Recalculate Weights
        For Layers As Integer = Me.Lay.Length - 1 To 0 Step -1

            For Neur As Integer = Me.Lay(Layers).Neu.Length - 1 To 0 Step -1

                For Syn As Integer = Me.Lay(Layers).Neu(Neur).Dendrites.Length - 1 To 0 Step -1

                    Me.Lay(Layers).Neu(Neur).Dendrites(Syn).Weight += SpeedOfCorrection * Me.Lay(Layers).Neu(Neur).Dendrites(Syn).imp * Me.Lay(Layers).Neu(Neur).Delta

                Next Syn

                Me.Lay(Layers).Neu(Neur).Bias.Weight += SpeedOfCorrection * Me.Lay(Layers).Neu(Neur).Bias.imp * Me.Lay(Layers).Neu(Neur).Delta

            Next Neur

        Next Layers

        Return ret
    End Function

    Public Function Save(ByVal URL As String) As Boolean
        Dim ret As Boolean = False

        ' Dimensions (each layer and begining), Weights (Synaps / Bias)

        Dim Dimensions As String = Nothing
        For i = 0 To Me.Lay.Length - 1
            If i = 0 Then
                Dimensions = Me.Lay(i).Neu(0).Dendrites.Length ' & vbCrLf
                Dimensions = Dimensions & ":" & Me.Lay(i).Neu.Length
            Else
                Dimensions = Dimensions & ";" & Me.Lay(i).Neu.Length
            End If
        Next

        Dim Weights As String = ""

        For Layers As Integer = 0 To Me.Lay.Length - 1

            For Neur As Integer = 0 To Me.Lay(Layers).Neu.Length - 1

                For Syn As Integer = 0 To Me.Lay(Layers).Neu(Neur).Dendrites.Length - 1

                    If Syn = 0 Then
                        Weights = Weights & Me.Lay(Layers).Neu(Neur).Dendrites(Syn).Weight
                    Else
                        Weights = Weights & ";" & Me.Lay(Layers).Neu(Neur).Dendrites(Syn).Weight
                    End If

                Next Syn

                Weights = Weights & ";" & Me.Lay(Layers).Neu(Neur).Bias.Weight & "//"
                'Me.Lay(Layers).Neu(Neur).Bias.Weight

            Next Neur

        Next Layers

        Dim Memory As String = Dimensions & vbCrLf & Weights


        Dim FILE_NAME As String = URL
        Dim objWriter As New System.IO.StreamWriter(FILE_NAME)
        objWriter.Write(Memory)
        objWriter.Close()
        'MessageBox.Show("Text written to file")

        Return ret
    End Function

    Public Sub New(ByVal URL As String)

        Dim FILE_NAME As String = URL
        Dim objWriter As New System.IO.StreamReader(FILE_NAME)
        Dim LoadedFile As String = objWriter.ReadToEnd()
        objWriter.Close()

        Dim Dimension_Weights() As String = LoadedFile.Split(vbCrLf)
        Dimension_Weights(1) = Dimension_Weights(1).Substring(1) 'Sacar El espacio del principio
        Dim NumberOfImputsAtTheBeginning_DimenionsOfEachLayer() As String = Dimension_Weights(0).Split(":") 'Separar Imputs at the beguinning
        Dim DimensionOfEachLayerS() As String = NumberOfImputsAtTheBeginning_DimenionsOfEachLayer(1).Split(";")

        Dim NumberOfImputsAtTheBeginning As Integer = NumberOfImputsAtTheBeginning_DimenionsOfEachLayer(0)
        Dim DimensionOfEachLayer() As Integer = Nothing
        For i = 0 To DimensionOfEachLayerS.Length - 1
            ReDim Preserve DimensionOfEachLayer(i)
            DimensionOfEachLayer(i) = DimensionOfEachLayerS(i)
        Next

        Dim ListOfWeights As New List(Of List(Of Double()))

        Dim count As Integer = 0
        Dim WeightsOfEachNeuron() As String = Dimension_Weights(1).Split("//")

        For i = 0 To DimensionOfEachLayer.Length - 1

            Dim temp As New List(Of Double())

            Dim bool As Boolean = True
            'For i2 = 0 To DimensionOfEachLayer(i) + 2 'Original : +1
            While bool

                Dim S() As String = WeightsOfEachNeuron(count).Split(";")
                Dim SCorrected() As String = Nothing
                Dim counter As Integer = 0
                For Integ As Integer = 0 To S.Length - 1
                    If S(Integ) <> "" Then
                        ReDim Preserve SCorrected(counter)
                        SCorrected(counter) = S(Integ)
                        counter += 1
                    End If
                Next
                Dim D() As Double = Nothing
                If SCorrected IsNot Nothing Then
                    For i3 = 0 To SCorrected.Length - 1
                        ReDim Preserve D(i3)
                        D(i3) = SCorrected(i3)
                    Next
                End If
                If D IsNot Nothing Then
                    temp.Add(D)
                    If temp.Count = DimensionOfEachLayer(i) Then
                        bool = False
                    End If

                End If
                count += 1

            End While
            'Next
            ListOfWeights.Add(temp)
        Next



        For i = 0 To DimensionOfEachLayer.Length - 1
            ReDim Preserve Me.Lay(i)

            Dim imputs As Integer

            If i = 0 Then
                imputs = NumberOfImputsAtTheBeginning
            Else
                imputs = DimensionOfEachLayer(i - 1)
            End If

            Me.Lay(i) = New Layer(DimensionOfEachLayer(i), imputs, ListOfWeights(i))
        Next



    End Sub

End Class
