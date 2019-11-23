using System.Windows;
using System.Windows.Controls;

namespace ClassicAssist.UI.Misc
{
    public static class BindableSelectedItemHelper
    {
        #region Properties

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.RegisterAttached(
            "SelectedItem", typeof( object ), typeof( BindableSelectedItemHelper ),
            new FrameworkPropertyMetadata( null, OnSelectedItemPropertyChanged ) );

        public static readonly DependencyProperty AttachProperty = DependencyProperty.RegisterAttached( "Attach",
            typeof( bool ), typeof( BindableSelectedItemHelper ), new PropertyMetadata( false, Attach ) );

        private static readonly DependencyProperty IsUpdatingProperty =
            DependencyProperty.RegisterAttached( "IsUpdating", typeof( bool ), typeof( BindableSelectedItemHelper ) );

        #endregion

        #region Implementation

        public static void SetAttach( DependencyObject dp, bool value )
        {
            dp.SetValue( AttachProperty, value );
        }

        public static bool GetAttach( DependencyObject dp )
        {
            return (bool) dp.GetValue( AttachProperty );
        }

        public static string GetSelectedItem( DependencyObject dp )
        {
            return (string) dp.GetValue( SelectedItemProperty );
        }

        public static void SetSelectedItem( DependencyObject dp, object value )
        {
            dp.SetValue( SelectedItemProperty, value );
        }

        private static bool GetIsUpdating( DependencyObject dp )
        {
            return (bool) dp.GetValue( IsUpdatingProperty );
        }

        private static void SetIsUpdating( DependencyObject dp, bool value )
        {
            dp.SetValue( IsUpdatingProperty, value );
        }

        private static void Attach( DependencyObject sender, DependencyPropertyChangedEventArgs e )
        {
            if ( !( sender is TreeView treeListView ) )
            {
                return;
            }

            if ( (bool) e.OldValue )
            {
                treeListView.SelectedItemChanged -= SelectedItemChanged;
            }

            if ( (bool) e.NewValue )
            {
                treeListView.SelectedItemChanged += SelectedItemChanged;
            }
        }

        private static void OnSelectedItemPropertyChanged( DependencyObject sender,
            DependencyPropertyChangedEventArgs e )
        {
            if ( !( sender is TreeView treeListView ) )
            {
                return;
            }

            treeListView.SelectedItemChanged -= SelectedItemChanged;

            if ( !GetIsUpdating( treeListView ) )
            {
                foreach ( TreeViewItem item in treeListView.Items )
                {
                    if ( Equals( item, e.NewValue ) )
                    {
                        item.IsSelected = true;
                        break;
                    }

                    item.IsSelected = false;
                }
            }

            treeListView.SelectedItemChanged += SelectedItemChanged;
        }

        private static void SelectedItemChanged( object sender, RoutedEventArgs e )
        {
            if ( !( sender is TreeView treeListView ) )
            {
                return;
            }

            SetIsUpdating( treeListView, true );
            SetSelectedItem( treeListView, treeListView.SelectedItem );
            SetIsUpdating( treeListView, false );
        }

        #endregion
    }
}