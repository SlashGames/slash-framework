Attribute VB_Name = "QuoteCommaExport"
' http://support.microsoft.com/kb/291296/en-us

Sub QuoteCommaExport()
   ' Dimension all variables.
   Dim DestFile As String
   Dim FileNum As Integer
   Dim ColumnCount As Integer
   Dim RowCount As Integer

   ' Prompt user for destination file name.
   DestFile = InputBox("Enter the destination filename" _
      & Chr(10) & "(with complete path):", "Quote-Comma Exporter")

   ' Obtain next free file handle number.
   FileNum = FreeFile()

   ' Turn error checking off.
   On Error Resume Next

   ' Attempt to open destination file for output.
   Open DestFile For Output As #FileNum

   ' If an error occurs report it and end.
   If Err <> 0 Then
      MsgBox "Cannot open filename " & DestFile
      End
   End If

   ' Turn error checking on.
   On Error GoTo 0

   ' Loop for each row in selection.
   For RowCount = 1 To Selection.Rows.Count

      ' Loop for each column in selection.
      For ColumnCount = 1 To Selection.Columns.Count

         ' Write current cell's text to file with quotation marks.
         Print #FileNum, """" & Selection.Cells(RowCount, _
            ColumnCount).Text & """";

         ' Check if cell is in last column.
         If ColumnCount = Selection.Columns.Count Then
            ' If so, then write a blank line.
            Print #FileNum,
         Else
            ' Otherwise, write a comma.
            Print #FileNum, ",";
         End If
      ' Start next iteration of ColumnCount loop.
      Next ColumnCount
   ' Start next iteration of RowCount loop.
   Next RowCount

   ' Close destination file.
   Close #FileNum
End Sub

