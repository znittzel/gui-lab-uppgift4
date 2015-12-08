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
using System.Windows.Media.Animation;

namespace Uppgift4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Shape _shapeSelected = null;
        private Point _posOfMouseOnHit;
        private Point _posOfShapeOnHit;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Shape_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point pt = e.GetPosition(MainCanvas);
            HitTestResult hr = VisualTreeHelper.HitTest(MainCanvas, pt);
            Object obj = hr.VisualHit;

            if (obj is Shape)
            {
                _shapeSelected = (Shape)obj;

                MainCanvas.Children.Remove(_shapeSelected);
                MainCanvas.Children.Add(_shapeSelected);

                _posOfMouseOnHit = pt;
                _posOfShapeOnHit.X = Canvas.GetLeft(_shapeSelected);
                _posOfShapeOnHit.Y = Canvas.GetTop(_shapeSelected);
            }
        }

        private void Shape_MouseMove(object sender, MouseEventArgs e)
        {
            if (_shapeSelected != null)
            {
                Point pt = e.GetPosition(MainCanvas);
                Canvas.SetLeft(_shapeSelected, (pt.X - _posOfMouseOnHit.X) + _posOfShapeOnHit.X);
                Canvas.SetTop(_shapeSelected, (pt.Y - _posOfMouseOnHit.Y) + _posOfShapeOnHit.Y);
            }
        }

        private void Shape_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_shapeSelected == null)
                return;

            double oldY = Canvas.GetTop(_shapeSelected); //nuvarande position
            if (_shapeSelected == balloon)
            {
                DoubleAnimation anim = new DoubleAnimation(oldY, 0, new Duration(TimeSpan.FromSeconds(3 * oldY/MainCanvas.Height)), FillBehavior.Stop);
                _shapeSelected.BeginAnimation(Canvas.TopProperty, anim);
                Canvas.SetTop(_shapeSelected, 0);
            }

            if (_shapeSelected == stone)
            {
                double fallHeight = MainCanvas.Height - _shapeSelected.Height - oldY;
                double t = Math.Sqrt(fallHeight) / 20;
                Duration duration = new Duration(TimeSpan.FromSeconds(t));
                DoubleAnimation anim = new DoubleAnimation(oldY, MainCanvas.Height - _shapeSelected.Height, duration, FillBehavior.Stop);
                anim.AccelerationRatio = 1;
                _shapeSelected.BeginAnimation(Canvas.TopProperty, anim);
                Canvas.SetTop(_shapeSelected, MainCanvas.Height - _shapeSelected.Height);
            }

            _shapeSelected = null;
        }
    }
}
