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

            SetupWindow(this, columnCount, rowCount, 100);
            SetupGrid(CoresGrid, columnCount, rowCount);
            
            _buttons = MakeCoreButtons(CoresGrid, columnCount, rowCount);
            foreach (var button in _buttons)
                button.Click += (sender, args) => UpdateTitleFromCoresState();

            UpdateTitleFromCoresState();
        }

        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////
        
        private static void SetupWindow(MainWindow window, int columnCount, int rowCount, int cellSize)
        {
            window.Width = cellSize * columnCount;
            window.Height = cellSize * rowCount;
        }

        private static void SetupGrid(Grid grid, int columnCount, int rowCount)
        {
            for (var row = 0; row < rowCount; row++)
                grid.RowDefinitions.Add(new RowDefinition {Height = new GridLength(1, GridUnitType.Star)});
            for (var column = 0; column < columnCount; column++)
                grid.ColumnDefinitions.Add(new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)});
        }

        private static IEnumerable<ToggleButton> MakeCoreButtons(Grid grid, int columnCount, int rowCount)
        {
            var core = 1;
            var buttons = new List<ToggleButton>();
            for (var row = 0; row < rowCount; row++)
            {
                for (var column = 0; column < columnCount; column++)
                {
                    var button = new ToggleButton {Content = $"Core {core++}"};
                    Grid.SetRow(button, row);
                    Grid.SetColumn(button, column);
                    grid.Children.Add(button);
                    buttons.Add(button);
                }
            }
            return buttons;
        }

        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////

        private void UpdateTitleFromCoresState()
        {
            Title = ConstructCoresState();
        }

        private string ConstructCoresState()
        {
            return string.Join("", _buttons.Select(b => b?.IsChecked ?? false ? "1" : "0"));
        }

        //////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////
        
        private readonly IEnumerable<ToggleButton> _buttons;
    }
}