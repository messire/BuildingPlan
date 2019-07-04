using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using BuildingPlan.Classes;

namespace BuildingPlan
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FloorClass _floor;

        public MainWindow()
        {
            InitializeComponent();
            DrawFloor();
           
        }

        private FloorClass CreateFloor(Point startPoint, int height, int width, int complexity) =>
            new FloorClass(startPoint, height, width, complexity);

        private void DrawFloor()
        {
            var gen = new GeometryGenerator(_floor, startPoint, height, width);

            DrawCanvas.Children.Add(gen.GetDrawPattern());
            //DrawPerimeter(floor);
        }

        private void InitPhisics()
        {
            var width = 100;
            var height = 100;
            var center = 50;
            var complexity = 3;

            _floor = CreateFloor(new Point(center,center), height, width, complexity);
        }

        private void DrawPerimeter(Block[,] floor)
        {
            var columns = floor.GetLength(0);
            var rows = floor.GetLength(1);

            var r = new Rectangle
            {
                Width = _shift + columns * (_length + _shift),
                Height = _shift + rows * (_length + _shift),
                Stroke = Brushes.Red,
                StrokeThickness = 2
            };

            Canvas.SetLeft(r, 100 - _shift);
            Canvas.SetTop(r, 50 - _shift);

            DrawCanvas.Children.Add(r);
        }

        private void DrawLine(Point p1, Point p2, Brush color, double thickness) => DrawCanvas.Children.Add(new Line
        {
            X1 = p1.X,
            Y1 = p1.Y,
            X2 = p2.X,
            Y2 = p2.Y,
            Stroke = color,
            StrokeThickness = thickness
        });
        
        private void DrawCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            DrawCanvas.Children.Clear();
            DrawFloor();
        }
    }
}
