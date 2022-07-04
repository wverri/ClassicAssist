﻿using System.Linq;
using System.Windows;
using System.Windows.Input;
using ClassicAssist.Data.Hotkeys;
using static ClassicAssist.Misc.SDLKeys;

namespace ClassicAssist.UI.Controls
{
    /// <inheritdoc cref="TextBox" />
    /// <summary>
    ///     Interaction logic for TestBoxKey.xaml
    /// </summary>
    public partial class TextBoxKey
    {
        public static readonly DependencyProperty ShortcutProperty = DependencyProperty.Register( nameof( Shortcut ),
            typeof( ShortcutKeys ), typeof( TextBoxKey ),
            new FrameworkPropertyMetadata( default( ShortcutKeys ),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault ) );

        private readonly Key[] _modifiers =
        {
            Key.LeftCtrl, Key.RightCtrl, Key.LeftShift, Key.RightShift, Key.LeftAlt, Key.RightAlt
        };

        public TextBoxKey()
        {
            InitializeComponent();
        }

        public Key Key { get; private set; }
        public ModKey Modifier { get; set; }

        public object Shortcut
        {
            get => GetValue( ShortcutProperty );
            set => SetValue( ShortcutProperty, value );
        }

        private ModKey CheckModifiers()
        {
            return KeymodFromKeyList( _modifiers.Where( Keyboard.IsKeyDown ) );
        }

        private bool IsModifier( Key key )
        {
            return _modifiers.Any( k => key == k );
        }

        private void TextBox_PreviewKeyDown( object sender, KeyEventArgs e )
        {
            e.Handled = true;
            Key key = e.Key;

            if ( key == Key.System )
            {
                key = e.SystemKey;
            }

            if ( IsModifier( key ) )
            {
                e.Handled = true;

                return;
            }

            Modifier = CheckModifiers();

            // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
            switch ( key )
            {
                case Key.DeadCharProcessed:
                    key = e.DeadCharProcessedKey;
                    break;
                case Key.ImeProcessed:
                    key = e.ImeProcessedKey;
                    break;
                case Key.System:
                    key = e.SystemKey;
                    break;
            }

            Key = key;

            Shortcut = new ShortcutKeys { Modifier = Modifier, Key = Key };

            //ShortcutChanged?.Execute( new ShortcutKeys { Modifier = Modifier, Key = Key } );
        }

        private void UIElement_OnPreviewMouseDown( object sender, MouseButtonEventArgs e )
        {
            if ( ( e.ChangedButton == MouseButton.Left ) | ( e.ChangedButton == MouseButton.Right ) )
            {
                return;
            }

            e.Handled = true;

            Modifier = CheckModifiers();

            Shortcut = new ShortcutKeys { Modifier = Modifier, Mouse = (MouseOptions) e.ChangedButton };
        }

        private void UIElement_OnMouseWheel( object sender, MouseWheelEventArgs e )
        {
            e.Handled = true;

            Modifier = CheckModifiers();

            Shortcut = e.Delta < 0
                ? new ShortcutKeys { Mouse = MouseOptions.MouseWheelDown, Modifier = Modifier }
                : new ShortcutKeys { Mouse = MouseOptions.MouseWheelUp, Modifier = Modifier };
        }
    }
}