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
        private int _length = 50;
        private int _shift = 3;
        private GeometryGenerator _gg;
        private readonly bool _horizontal = true;
        private readonly bool _vertical = false;
        private RectangleGeometry _base;
        private FloorClass _floor;

        public MainWindow()
        {
            InitializeComponent();
            DrawFloor();
           
        }

        private FloorClass CreateFloor(Point startPoint, int height, int width, int complexity) =>
            new FloorClass(startPoint, width, height, complexity);

        private void DrawFloor()
        {
            InitPhisics();
            GeometryGroup gg = new GeometryGroup();
            
            foreach (Construct construct in _floor)
            {
                CombinedGeometry cg = new CombinedGeometry();
                cg.Geometry1 = _base;

                if (construct.ConstructType == 0)
                {
                    var ellipse = new EllipseGeometry
                    {
                        Center = construct.Coordinates,
                        RadiusX = construct.Height,
                        RadiusY = construct.Width
                    };
                    
                    cg.Geometry2 = ellipse;
                }
                else
                {
                    var rectangle = new RectangleGeometry
                    {
                        Rect = new Rect(construct.Coordinates, new Size(construct.Width, construct.Height))
                    };
                    
                    cg.Geometry2 = rectangle;
                }

                cg.GeometryCombineMode = construct.CombineMode;

                gg.Children.Add(cg);
            }

            Path path = new Path
            {
                Data = gg,
                Stroke = Brushes.Gray,
                StrokeThickness = 1,
                Fill = Brushes.Gray
            };


            DrawCanvas.Children.Add(path);
            //DrawPerimeter(floor);

        }

        private void InitPhisics()
        {
            var width = 100;
            var height = 100;
            var center = 50;
            var complexity = 3;

            _floor = CreateFloor(new Point(center,center), height, width, complexity);
            _base = new RectangleGeometry(new Rect(center, center, width, height));
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
