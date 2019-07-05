using System.Configuration;
using System.Windows;
using System.Windows.Input;
using BuildingPlan.Classes;

namespace BuildingPlan
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region fields

        private FloorClass _floor;
        private int _width;
        private int _height;
        private int _x;
        private int _y;
        private int _complexity;
        
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            SetDefaultValues();
            ReadConfig();
            GenerateFloor();
        }

        /// <summary> Установить значения по умолчанию </summary>
        private void SetDefaultValues()
        {
            _complexity = 0;
            _height = 300;
            _width = 300;
            _x = 50;
            _y = 50;
        }

        /// <summary> Прочитать конфиг </summary>
        private void ReadConfig()
        {
            int.TryParse(ConfigurationManager.AppSettings.Get("complexity"), out int complexity);
            if (complexity != 0) _complexity = complexity;

            int.TryParse(ConfigurationManager.AppSettings.Get("height"), out int height);
            if (height != 0) _height = height;

            int.TryParse(ConfigurationManager.AppSettings.Get("width"), out int width);
            if (width != 0) _width = width;

            int.TryParse(ConfigurationManager.AppSettings.Get("X"), out int x);
            if (x != 0) _x = x;

            int.TryParse(ConfigurationManager.AppSettings.Get("Y"), out int y);
            if (y != 0) _y = y;
        }

        /// <summary> Сгенерировать и отрисовать пол </summary>
        private void GenerateFloor()
        {
            InitPhisics();
            DrawFloor();
            SetWindowSize();
        }

        /// <summary> Инициализация пола </summary>
        private void InitPhisics() => _floor = CreateFloor(new Point(_x, _x), _height, _width, _complexity);

        /// <summary> Сгенерировать пол </summary>
        /// <param name="startPoint"> Стартовые координаты отрисовки </param>
        /// <param name="height"> Длинна этажа </param>
        /// <param name="width"> Ширина этажа </param>
        /// <param name="complexity"> Сложность </param>
        /// <returns></returns>
        private FloorClass CreateFloor(Point startPoint, int height, int width, int complexity) =>
            new FloorClass(startPoint, height, width, complexity);

        /// <summary> Нарисовать пол </summary>
        private void DrawFloor()
        {
            var gen = new GeometryGenerator(_floor);
            DrawCanvas.Children.Add(gen.GetDrawPattern());
        }

        /// <summary> Поменять размер окна </summary>
        private void SetWindowSize()
        {
            if (Application.Current.MainWindow.Height < _x * 2 + _height) Application.Current.MainWindow.Height = _x * 2 + _height;
            if (Application.Current.MainWindow.Width < _y * 2 + _width) Application.Current.MainWindow.Width = _y * 2 + _width;
        }

        private void DrawCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            DrawCanvas.Children.Clear();
            GenerateFloor();
        }
    }
}
