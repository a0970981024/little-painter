﻿using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Win32;

namespace 小畫家
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        String shapeType = "Line";
        String actionType = "Draw";
        Color strokeColor = Colors.Red;
        Color fillColor = Colors.Yellow;
        int strokeThickness = 1;
        Point start, dest;
        public MainWindow()
        {
            InitializeComponent();
            colorpicker1.SelectedColor = strokeColor;
            fillcolorpicker1.SelectedColor = fillColor;
        }

        private void ShapeButton_Click(object sender, RoutedEventArgs e)
        {
            var targetRadioButton = sender as RadioButton;
            shapeType = targetRadioButton.Tag.ToString();
            actionType = "Draw";
            //MessageBox.Show(shapeType);
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
            switch (actionType) 
            {
                case "Draw":
                    start = e.GetPosition(canvas1);
                    canvas1.Cursor = Cursors.Cross;
                    Brush stroke = Brushes.Gray;
                    Brush fill = Brushes.LightGray;
                    //MessageBox.Show($"({start.X},{start.Y})");
                    displayStatus();

                    switch (shapeType)
                    {
                        case "Line":
                            Line line = new Line
                            {
                                Stroke = stroke,
                                StrokeThickness = 1,
                                X1 = start.X,
                                Y1 = start.Y,
                                X2 = dest.X,
                                Y2 = dest.Y
                            };
                            canvas1.Children.Add(line);
                            break;
                        case "Rectangle":
                            var rect = new Rectangle
                            {
                                Stroke = stroke,
                                Fill = fill,
                                StrokeThickness = 1,
                                Width = 50,
                                Height = 100
                            };
                            rect.SetValue(Canvas.LeftProperty, start.X);
                            rect.SetValue(Canvas.TopProperty, start.Y);
                            canvas1.Children.Add(rect);
                            break;
                        case "Ellipse":
                            var ellipse = new Ellipse
                            {
                                Stroke = stroke,
                                Fill = fill,
                                StrokeThickness = 1,
                                Width = 50,
                                Height = 100
                            };
                            ellipse.SetValue(Canvas.LeftProperty, start.X);
                            ellipse.SetValue(Canvas.TopProperty, start.Y);
                            canvas1.Children.Add(ellipse);
                            break;
                        case "Polyline":
                            var polyline = new Polyline
                            {
                                Stroke = stroke,
                                Fill = fill,
                                StrokeThickness = 1,
                            };
                            canvas1.Children.Add(polyline);
                            break;
                        default:
                            break;

                    }
                    break;
                case "Erase":

                    break;

            }
        }

        private void canvas1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            dest = e.GetPosition(canvas1);
            displayStatus();
            switch (actionType)
            {
                case "Draw":
                    canvas1.Cursor = Cursors.Arrow;
                    Brush stroke = new SolidColorBrush(strokeColor);
                    Brush fill = new SolidColorBrush(fillColor);
                    switch (shapeType)
                    {
                        case "Line":
                            var line = canvas1.Children.OfType<Line>().LastOrDefault();

                            line.Stroke = stroke;
                            line.StrokeThickness = strokeThickness;
                            break;
                        case "Rectangle":
                            var rect = canvas1.Children.OfType<Rectangle>().LastOrDefault();
                            rect.Stroke = stroke;
                            rect.Fill = fill;
                            rect.StrokeThickness = strokeThickness;
                            break;
                        case "Ellipse":
                            var ellipse = canvas1.Children.OfType<Ellipse>().LastOrDefault();
                            ellipse.Stroke = stroke;
                            ellipse.Fill = fill;
                            ellipse.StrokeThickness = strokeThickness;
                            break;
                        case "Polyline":
                            var polyline = canvas1.Children.OfType<Polyline>().LastOrDefault();
                            polyline.Stroke = stroke;
                            polyline.Fill = fill;
                            polyline.StrokeThickness = strokeThickness;
                            break;
                        default:
                            break;

                    }
                    break;
                case "Erase":
                    break;
            }
        }

        private void canvas1_MouseMove(object sender, MouseEventArgs e)
        {
            switch (actionType)
            {
                case "Draw":
                    if (e.LeftButton == MouseButtonState.Pressed)
                    {
                        dest = e.GetPosition(canvas1);
                        displayStatus();
                        Brush stroke = new SolidColorBrush(Colors.Gray);
                        Brush fill = new SolidColorBrush(Colors.LightGray);
                        Point origin = new Point
                        {
                            X = Math.Min(start.X, dest.X),
                            Y = Math.Min(start.Y, dest.Y)
                        };
                        double width = Math.Abs(dest.X - start.X);
                        double height = Math.Abs(dest.Y - start.Y);

                        switch (shapeType)
                        {
                            case "Line":
                                var line = canvas1.Children.OfType<Line>().LastOrDefault();
                                line.X2 = dest.X;
                                line.Y2 = dest.Y;
                                break;
                            case "Rectangle":
                                var rect = canvas1.Children.OfType<Rectangle>().LastOrDefault();
                                rect.Width = width;
                                rect.Height = height;
                                rect.SetValue(Canvas.LeftProperty, origin.X);
                                rect.SetValue(Canvas.TopProperty, origin.Y);
                                break;
                            case "Ellipse":
                                var ellipse = canvas1.Children.OfType<Ellipse>().LastOrDefault();
                                ellipse.Width = width;
                                ellipse.Height = height;
                                ellipse.SetValue(Canvas.LeftProperty, origin.X);
                                ellipse.SetValue(Canvas.TopProperty, origin.Y);
                                break;
                            case "Polyline":
                                var polyline = canvas1.Children.OfType<Polyline>().LastOrDefault();
                                polyline.Points.Add(dest);
                                break;
                            default:
                                break;

                        }
                    }
                    break;
                case "Erase":
                    var shape = e.OriginalSource as Shape;
                    if (shape != null)
                    {
                        canvas1.Children.Remove(shape);
                        displayStatus();
                    }
                     
                    break;

            }
         
            
        }

        private void fillcolorpicker1_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            fillColor = (Color)fillcolorpicker1.SelectedColor;
        }

        private void clearMenuItem_Click(object sender, RoutedEventArgs e)
        {
            canvas1.Children.Clear();
            displayStatus();
        }

        private void eraseButton_Click(object sender, RoutedEventArgs e)
        {
            if(canvas1.Children.Count != 0)
            {
                canvas1.Cursor = Cursors.Hand;
                actionType = "Erase";
                displayStatus();

            }
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            canvas1.Children.Clear();
            displayStatus();

        }

        private void SaveCanvas_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PNG Files(*.png)|*.pbg";
            saveFileDialog.DefaultExt = ".png";
            saveFileDialog.AddExtension = true;

            if (saveFileDialog.ShowDialog() == true)
            {
                Uri path = new Uri(saveFileDialog.FileName);

                Transform transform = canvas1.LayoutTransform;
                canvas1.LayoutTransform = null;

                Size size = new Size(900, 600);
                canvas1.Measure(size);
                canvas1.Arrange(new Rect(size));

                RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)size.Width, (int)size.Height, 96d, 96d, PixelFormats.Pbgra32);
                renderTargetBitmap.Render(canvas1);

                using(FileStream outstream = new FileStream(path.LocalPath, FileMode.Create))
                {
                    PngBitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
                    encoder.Save(outstream);
                }
                canvas1.LayoutTransform = transform;

            }

        }

       

        private void displayStatus()
        {
            int lineCount = canvas1.Children.OfType<Line>().Count();
            int rectCount = canvas1.Children.OfType<Rectangle>().Count();
            int ellipseCount = canvas1.Children.OfType<Ellipse>().Count();
            int polylineCount= canvas1.Children.OfType<Polyline>().Count();
            coordinateLabel.Content = $"{actionType}模式 | 座標點:({Math.Round(start.X)},{Math.Round(start.Y)}) - ({Math.Round(dest.X)},{Math.Round(dest.Y)})";
            shapeLabe.Content = $"Line: {lineCount},Rectangle:{rectCount},Ellipse:{ellipseCount},Polyline:{polylineCount}";
        }
    }
}
