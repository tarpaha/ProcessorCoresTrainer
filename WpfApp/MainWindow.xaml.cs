using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            var (columnCount, rowCount) = Utils.Dimensions.Calculate(Environment.ProcessorCount);

            Width = 100 * columnCount;
            Height = 100 * rowCount;
            MakeCoresGrid(CoresGrid, columnCount, rowCount);
        }

        private static void MakeCoresGrid(Grid grid, int columnCount, int rowCount)
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
                    Grid.SetRow(button, row);
                    Grid.SetColumn(button, column);
                    grid.Children.Add(button);
                }
            }
        }
    }
}