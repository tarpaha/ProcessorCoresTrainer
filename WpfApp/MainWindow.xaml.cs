using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Collections.Generic;

namespace WpfApp
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            var (columnCount, rowCount) = Utils.Dimensions.Calculate(Environment.ProcessorCount);

            Width = 100 * columnCount;
            Height = 100 * rowCount;
            MakeCoresGrid(CoresGrid, columnCount, rowCount);
            UpdateTitleFromCoresState();
        }

        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////
        
        private void MakeCoresGrid(Grid grid, int columnCount, int rowCount)
        {
            for (var row = 0; row < rowCount; row++)
                grid.RowDefinitions.Add(new RowDefinition {Height = new GridLength(1, GridUnitType.Star)});
            for (var column = 0; column < columnCount; column++)
                grid.ColumnDefinitions.Add(new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)});

            var core = 1;
            for (var row = 0; row < rowCount; row++)
            {
                for (var column = 0; column < columnCount; column++)
                {
                    var button = new ToggleButton {Content = $"Core {core++}"};
                    button.Click += OnButtonClick;
                    Grid.SetRow(button, row);
                    Grid.SetColumn(button, column);
                    grid.Children.Add(button);
                    _coreButtons.Add(button);
                }
            }
        }

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            UpdateTitleFromCoresState();
        }

        private void UpdateTitleFromCoresState()
        {
            Title = ConstructCoresState();
        }

        private string ConstructCoresState()
        {
            return string.Join("", _coreButtons.Select(b => b?.IsChecked ?? false ? "1" : "0"));
        }

        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////
        
        private readonly List<ToggleButton> _coreButtons = new List<ToggleButton>();
    }
}