﻿#pragma checksum "C:\Users\kiss_\source\repos\Практика 3 - Моторка\Motorka\StorekeeperPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "E6F24852FC6C73E8C573607959D0D1AA"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Motorka
{
    partial class StorekeeperPage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.18362.1")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1: // StorekeeperPage.xaml line 1
                {
                    global::Windows.UI.Xaml.Controls.Page element1 = (global::Windows.UI.Xaml.Controls.Page)(target);
                    ((global::Windows.UI.Xaml.Controls.Page)element1).Loaded += this.Page_Loaded;
                }
                break;
            case 2: // StorekeeperPage.xaml line 65
                {
                    this.dataGrid = (global::Microsoft.Toolkit.Uwp.UI.Controls.DataGrid)(target);
                    ((global::Microsoft.Toolkit.Uwp.UI.Controls.DataGrid)this.dataGrid).SelectionChanged += this.dataGrid_SelectionChanged;
                }
                break;
            case 3: // StorekeeperPage.xaml line 53
                {
                    this.TovarName = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 4: // StorekeeperPage.xaml line 54
                {
                    this.Cost = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 5: // StorekeeperPage.xaml line 55
                {
                    this.Amount = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 6: // StorekeeperPage.xaml line 56
                {
                    this.SupplierComboBox = (global::Windows.UI.Xaml.Controls.ComboBox)(target);
                }
                break;
            case 7: // StorekeeperPage.xaml line 58
                {
                    global::Windows.UI.Xaml.Controls.Button element7 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)element7).Click += this.Insert_Click;
                }
                break;
            case 8: // StorekeeperPage.xaml line 59
                {
                    global::Windows.UI.Xaml.Controls.Button element8 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)element8).Click += this.Update_Click;
                }
                break;
            case 9: // StorekeeperPage.xaml line 60
                {
                    global::Windows.UI.Xaml.Controls.Button element9 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)element9).Click += this.Delete_Click;
                }
                break;
            case 10: // StorekeeperPage.xaml line 26
                {
                    global::Windows.UI.Xaml.Controls.Button element10 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)element10).Click += this.Button_Click;
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.18362.1")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

