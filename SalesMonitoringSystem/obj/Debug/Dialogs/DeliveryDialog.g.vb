﻿#ExternalChecksum("..\..\..\Dialogs\DeliveryDialog.xaml","{8829d00f-11b8-4213-878b-770e8597ac16}","1A219178A82F1EE913203A39D5F2AE99FA192EB4E69B9D684121FE1349E035C9")
'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.42000
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On

Imports HandyControl.Controls
Imports HandyControl.Data
Imports HandyControl.Expression.Media
Imports HandyControl.Expression.Shapes
Imports HandyControl.Interactivity
Imports HandyControl.Media.Animation
Imports HandyControl.Media.Effects
Imports HandyControl.Properties.Langs
Imports HandyControl.Themes
Imports HandyControl.Tools
Imports HandyControl.Tools.Converter
Imports HandyControl.Tools.Extension
Imports SalesMonitoringSystem
Imports System
Imports System.Diagnostics
Imports System.Windows
Imports System.Windows.Automation
Imports System.Windows.Controls
Imports System.Windows.Controls.Primitives
Imports System.Windows.Data
Imports System.Windows.Documents
Imports System.Windows.Ink
Imports System.Windows.Input
Imports System.Windows.Markup
Imports System.Windows.Media
Imports System.Windows.Media.Animation
Imports System.Windows.Media.Effects
Imports System.Windows.Media.Imaging
Imports System.Windows.Media.Media3D
Imports System.Windows.Media.TextFormatting
Imports System.Windows.Navigation
Imports System.Windows.Shapes
Imports System.Windows.Shell


'''<summary>
'''DeliveryDialog
'''</summary>
<Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>  _
Partial Public Class DeliveryDialog
    Inherits System.Windows.Controls.UserControl
    Implements System.Windows.Markup.IComponentConnector
    
    
    #ExternalSource("..\..\..\Dialogs\DeliveryDialog.xaml",23)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents Closebtn As System.Windows.Controls.Button
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\..\Dialogs\DeliveryDialog.xaml",44)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents ProductNameComboBox As HandyControl.Controls.ComboBox
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\..\Dialogs\DeliveryDialog.xaml",56)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents SellingPriceTextBox As System.Windows.Controls.TextBox
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\..\Dialogs\DeliveryDialog.xaml",68)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents CostPriceTextBox As System.Windows.Controls.TextBox
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\..\Dialogs\DeliveryDialog.xaml",80)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents QuantityTextBox As System.Windows.Controls.TextBox
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\..\Dialogs\DeliveryDialog.xaml",89)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents DeleteButton As System.Windows.Controls.Button
    
    #End ExternalSource
    
    
    #ExternalSource("..\..\..\Dialogs\DeliveryDialog.xaml",99)
    <System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")>  _
    Friend WithEvents SaveButton As System.Windows.Controls.Button
    
    #End ExternalSource
    
    Private _contentLoaded As Boolean
    
    '''<summary>
    '''InitializeComponent
    '''</summary>
    <System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")>  _
    Public Sub InitializeComponent() Implements System.Windows.Markup.IComponentConnector.InitializeComponent
        If _contentLoaded Then
            Return
        End If
        _contentLoaded = true
        Dim resourceLocater As System.Uri = New System.Uri("/SalesMonitoringSystem;component/dialogs/deliverydialog.xaml", System.UriKind.Relative)
        
        #ExternalSource("..\..\..\Dialogs\DeliveryDialog.xaml",1)
        System.Windows.Application.LoadComponent(Me, resourceLocater)
        
        #End ExternalSource
    End Sub
    
    <System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0"),  _
     System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never),  _
     System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes"),  _
     System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity"),  _
     System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")>  _
    Sub System_Windows_Markup_IComponentConnector_Connect(ByVal connectionId As Integer, ByVal target As Object) Implements System.Windows.Markup.IComponentConnector.Connect
        If (connectionId = 1) Then
            Me.Closebtn = CType(target,System.Windows.Controls.Button)
            Return
        End If
        If (connectionId = 2) Then
            Me.ProductNameComboBox = CType(target,HandyControl.Controls.ComboBox)
            Return
        End If
        If (connectionId = 3) Then
            Me.SellingPriceTextBox = CType(target,System.Windows.Controls.TextBox)
            Return
        End If
        If (connectionId = 4) Then
            Me.CostPriceTextBox = CType(target,System.Windows.Controls.TextBox)
            Return
        End If
        If (connectionId = 5) Then
            Me.QuantityTextBox = CType(target,System.Windows.Controls.TextBox)
            Return
        End If
        If (connectionId = 6) Then
            Me.DeleteButton = CType(target,System.Windows.Controls.Button)
            Return
        End If
        If (connectionId = 7) Then
            Me.SaveButton = CType(target,System.Windows.Controls.Button)
            Return
        End If
        Me._contentLoaded = true
    End Sub
End Class

