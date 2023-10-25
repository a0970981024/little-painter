using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace 小畫家
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        String shapeType = "Line";
        Color strokeColor = Colors.Red;
        int strokeThickness = 1;
        Point start, dest;
        public MainWindow()
        {
            
            InitializeComponent();
            colorpicker1.SelectedColor = strokeColor;
        }

        private void ShapeButton_Click(object sender, RoutedEventArgs e)
        {
            var targetRadioButton = sender as RadioButton;
            shapeType = targetRadioButton.Tag.ToString();
            MessageBox.Show(shapeType);
        }

        private void thickenessSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            strokeThickness = Convert.ToInt32(thickenessSlider.Value);
        }

        private void colorpicker1_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            strokeColor = (Color)colorpicker1.SelectedColor;
            //MessageBox.Show(strokeColor.ToString());
        }

        private void canvas1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            start = e.GetPosition(canvas1);
            canvas1.Cursor = Cursors.Cross;
            //MessageBox.Show($"({start.X},{start.Y})");
            displayStatus();
        }

        private void canvas1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            dest = e.GetPosition(canvas1);
            canvas1.Cursor = Cursors.Arrow;
            displayStatus();
        }

        private void canvas1_MouseMove(object sender, MouseEventArgs e)
        {
            //if(MouseButtonState.Pressed == )
            dest = e.GetPosition(canvas1);
            displayStatus();
        }

        private void displayStatus()
        {
            coordinateLabel.Content = $"座標點:({Math.Round(start.X)},{Math.Round(start.Y)}) - ({Math.Round(dest.X)},{Math.Round(dest.Y)})";
        }
    }
}
