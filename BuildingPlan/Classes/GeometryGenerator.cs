using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BuildingPlan.Classes
{
    /// <summary> Генератор этажа </summary>
    public class GeometryGenerator
    {
        #region fields

        private readonly FloorClass _floor;
        private Construct[] _unions;
        private Construct[] _excludes;
        private CombinedGeometry _unionGeometry;
        private CombinedGeometry _excludeGeometry;
        private CombinedGeometry _floorGeometry;
        private GeometryGroup _unionGroup;
        private GeometryGroup _excludeGroup;
        private GeometryGroup _floorGeometryGroup;
        
        #endregion

        #region properties

        private RectangleGeometry Base => new RectangleGeometry(new Rect(_floor.StartPoint.X, _floor.StartPoint.Y, _floor.Width, _floor.Height));

        #endregion
        
        #region Constructor

        public GeometryGenerator(FloorClass floor)
        {
            _floor = floor;
            _unions = new Construct[_floor.UnionCount];
            _excludes = new Construct[_floor.ExcludeCount];
            _unionGeometry = new CombinedGeometry();
            _excludeGeometry = new CombinedGeometry();
            _floorGeometry = new CombinedGeometry();
            _unionGroup = new GeometryGroup();
            _excludeGroup = new GeometryGroup();
            _floorGeometryGroup = new GeometryGroup();

            _floorGeometry.Geometry1 = Base;
        }

        #endregion

        #region public methods

        /// <summary> Получить паттерн отрисовки </summary>
        /// <returns></returns>
        public Path GetDrawPattern()
        {
            Path path = AssambleDrawPath();
            return path;
        }

        /// <summary> Сеттер длинны этажа </summary>
        /// <param name="height"></param>
        /// <returns></returns>
        public bool SetHeight(int height) => _floor != null && _floor.SetHeight(height);

        /// <summary> Сеттер ширины этажа </summary>
        /// <param name="width"></param>
        /// <returns></returns>
        public bool SetWidth(int width) => _floor != null && _floor.SetWidth(width);

        #endregion

        #region private methods

        /// <summary> Сборка отрисовки </summary>
        /// <returns></returns>
        private Path AssambleDrawPath()
        {
            AssembleFloor();

            return new Path
            {
                Data = _floorGeometryGroup,
                Fill = Brushes.Gray
            };
        }

        /// <summary> Собрать конструкты в одну форму </summary>
        /// <returns></returns>
        private void AssembleFloor()
        {
            SortConstructs();

            _excludeGeometry = CombineGeometries(_excludes, _excludeGeometry);
            _unionGeometry = CombineGeometries(_unions, _unionGeometry);
            
            CombineGeometryFloor(_floorGeometry, _excludeGeometry, GeometryCombineMode.Exclude);
            CombineGeometryFloor(_floorGeometry, _unionGeometry, GeometryCombineMode.Union);

            _floorGeometryGroup.Children.Add(_floorGeometry);
        }

        /// <summary> Сортировка конструктов </summary>
        private void SortConstructs()
        {
            foreach (Construct construct in _floor)
            {
                if (construct.CombineMode == GeometryCombineMode.Union)
                {
                    _unions[_unions.Count(e => e != null)] = construct;
                }
                else
                {
                    _excludes[_excludes.Count(e => e != null)] = construct;
                }
            }
        }

        /// <summary> Сборка схожих конструктов в одну форму </summary>
        /// <param name="constructArray"></param>
        /// <param name="combined"></param>
        /// <returns></returns>
        private CombinedGeometry CombineGeometries(Construct[] constructArray, CombinedGeometry combined)
        {
            if (constructArray.Length > 0)
            {
                foreach (Construct construct in constructArray)
                {
                    combined = new CombinedGeometry
                    {
                        Geometry1 = combined,
                        Geometry2 = GetGeometry(construct),
                        GeometryCombineMode = GeometryCombineMode.Union
                    };
                }
            }

            return combined;
        }

        /// <summary> Привязка формы к базовой площадке </summary>
        /// <param name="geometry1"></param>
        /// <param name="geometry2"></param>
        /// <param name="combineMode"></param>
        private void CombineGeometryFloor(CombinedGeometry geometry1, CombinedGeometry geometry2, GeometryCombineMode combineMode)
        {
            _floorGeometry = new CombinedGeometry
            {
                Geometry1 = geometry1,
                Geometry2 = geometry2,
                GeometryCombineMode = combineMode
            };
        }

        #endregion
        
        #region Helpers

        private Geometry GetGeometry(Construct construct) => 
            construct.ConstructType == 0 ? (Geometry) CreateEllipse(construct) : CreateRectangle(construct);

        /// <summary> Генерация геометрии эллипса </summary>
        /// <param name="construct"></param>
        /// <returns></returns>
        private EllipseGeometry CreateEllipse(Construct construct)
        {
            return new EllipseGeometry
            {
                Center = construct.Coordinates,
                RadiusX = construct.Height,
                RadiusY = construct.Width
            };
        }

        /// <summary> Генерация геометрии прямоугольника </summary>
        /// <param name="construct"></param>
        /// <returns></returns>
        private RectangleGeometry CreateRectangle(Construct construct)
        {
            return new RectangleGeometry { Rect = new Rect(construct.Coordinates, construct.Size) };
        }

        #endregion
    }
}
