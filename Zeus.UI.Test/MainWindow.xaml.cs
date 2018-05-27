﻿using Zeus.UI.Controls;
using Zeus.UI.Themes.Enums;

namespace Zeus.UI.Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ZeusWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ZeusButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ZeusDialog dialog = new ZeusDialog();
            dialog.ShowCloseButton = true;
            dialog.Color = ZeusColorStyles.Green;
            dialog.Show();
        }
    }
}
