using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BuildingPlan.Classes
{
    public class GeometryGenerator
    {
        private FloorClass _floor;

        private Point _startPoint;
        private int _height;
        private int _width;

        private RectangleGeometry Base => new RectangleGeometry(new Rect(_startPoint.X, _startPoint.Y, _width, _height));

        public GeometryGenerator(FloorClass floor, Point startPoint, int height, int width)
        {
            _floor = floor;
            _startPoint = startPoint;
            _height = height;
            _width = width;
        }

        public Path GetDrawPattern()
        {
            Path path = AssambleDrawPath();
            return path;
        }

        public bool SetHeight(int height)
        {
            if (height < 0) return false;

            _height = height;
            return true;
        }

        public bool SetWidth(int width)
        {
            if (width < 0) return false;

            _height = width;
            return true;
        }

        private EllipseGeometry CreateEllipse(Construct construct)
        {
            return new EllipseGeometry
            {
                Center = construct.Coordinates,
                RadiusX = construct.Height,
                RadiusY = construct.Width
            };
        }

        private RectangleGeometry CreateRectangle(Construct construct)
        {
            return new RectangleGeometry {Rect = new Rect(construct.Coordinates, new Size(construct.Width, construct.Height))};
        }

        private GeometryGroup AssembleFloor()
        {
            GeometryGroup floorGeometryGroup = new GeometryGroup();

            foreach (Construct construct in _floor)
            {
                CombinedGeometry cg = new CombinedGeometry
                {
                    Geometry1 = Base,
                    Geometry2 = construct.ConstructType == 0 ? (Geometry)CreateEllipse(construct) : CreateRectangle(construct),
                    GeometryCombineMode = construct.CombineMode
                };
                floorGeometryGroup.Children.Add(cg);
            }

            return floorGeometryGroup;
        }

        private Path AssambleDrawPath()
        {
            return new Path
            {
                Data = AssembleFloor(),
                Fill = Brushes.Gray
            };
        }
    }
}
