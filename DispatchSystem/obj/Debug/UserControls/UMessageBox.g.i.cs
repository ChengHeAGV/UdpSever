﻿#pragma checksum "..\..\..\UserControls\UMessageBox.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "EDE2343BD9C3ADD3AFA73914032FB61A"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace WpfAppTest {
    
    
    /// <summary>
    /// UMessageBox
    /// </summary>
    public partial class UMessageBox : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 5 "..\..\..\UserControls\UMessageBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal WpfAppTest.UMessageBox main;
        
        #line default
        #line hidden
        
        
        #line 10 "..\..\..\UserControls\UMessageBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Media.Animation.Storyboard sbOpShow;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\..\UserControls\UMessageBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock lblTitle;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\..\UserControls\UMessageBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock lblMsg;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\..\UserControls\UMessageBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border border1;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\..\UserControls\UMessageBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border border2;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/AGV调度系统;component/usercontrols/umessagebox.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\UserControls\UMessageBox.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.main = ((WpfAppTest.UMessageBox)(target));
            return;
            case 2:
            this.sbOpShow = ((System.Windows.Media.Animation.Storyboard)(target));
            return;
            case 3:
            this.lblTitle = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            this.lblMsg = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 5:
            this.border1 = ((System.Windows.Controls.Border)(target));
            
            #line 36 "..\..\..\UserControls\UMessageBox.xaml"
            this.border1.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Yes_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 6:
            this.border2 = ((System.Windows.Controls.Border)(target));
            
            #line 41 "..\..\..\UserControls\UMessageBox.xaml"
            this.border2.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.No_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
