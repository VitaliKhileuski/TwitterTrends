using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
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
using TwitterTrends.Models;
using TwitterTrends.Models.Parsers;
using TwitterTrends.Services.Parsers;
using TwitterTrends.ViewModels;
using Brushes = System.Windows.Media.Brushes;

namespace TwitterTrends
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {

            //data
            //загрузка вьюмодел для кнопок меню
            MainWindowViewModel viewModel = new MainWindowViewModel();
            this.DataContext = viewModel;

            
        }

        private void Loaded_gmap(object sender, RoutedEventArgs e)
        {
            gmap.Bearing = 0;
            gmap.CanDragMap = true;
            gmap.DragButton = MouseButton.Left;


            gmap.MaxZoom = 18;
            gmap.MinZoom = 3;

            gmap.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionWithoutCenter;

            gmap.ShowTileGridLines = false;
            gmap.Zoom = 2;
            gmap.ShowCenter = false;

            gmap.MapProvider = GMapProviders.BingMap;
            GMaps.Instance.Mode = AccessMode.ServerOnly;
            gmap.Position = new PointLatLng(51.39920565355378, -108.63281250000001);


            GMapProvider.WebProxy = WebRequest.GetSystemWebProxy();
            GMapProvider.WebProxy.Credentials = CredentialCache.DefaultCredentials;

            Country country;
            country = StatesParser.Parse(@"..\..\..\Data\States\states.json");

            foreach (var state in country.States)
            {
                foreach (var polygon in state.Polygons)
                {
                    List<PointLatLng> pointlatlang = new List<PointLatLng>();
                    GMapPolygon pol = new GMapPolygon(pointlatlang);
                    foreach (var point in polygon.Points)
                    {
                        pointlatlang.Add(new PointLatLng(point.Y, point.X));
                    }
                    pol.Points = pointlatlang;
                    gmap.RegenerateShape(pol);
                    (pol.Shape as Path).Fill = GetColorByMood(state);
                    (pol.Shape as Path).Stroke = Brushes.Blue;
                    (pol.Shape as Path).StrokeThickness = 1.5;
                    (pol.Shape as Path).Effect = null;
                    gmap.Markers.Add(pol);
                }

            }



            //          List<PointLatLng> pointlatlang= new List<PointLatLng>()
            //          {

            //new PointLatLng( 39.804456,-75.414089),
            //          new PointLatLng(39.683964,-75.507197),
            //           new PointLatLng(39.61824,-75.611259),
            //            new PointLatLng(39.459409,-75.589352),
            //             new PointLatLng(39.311532,-75.441474),
            //               new PointLatLng(39.065069,-75.403136),
            //          new PointLatLng(38.807653,-75.189535),
            //           new PointLatLng(38.796699,-75.09095),
            //            new PointLatLng(38.451652,-75.047134),
            //             new PointLatLng(38.462606,-75.693413),
            //               new PointLatLng(39.722302,-75.786521),
            //          new PointLatLng(39.831841,-75.616736),
            //             new PointLatLng(39.804456,-75.414089)
            //      };

            //          GMapPolygon polygon = new GMapPolygon(pointlatlang);
            //          gmap.RegenerateShape(polygon);
            //          (polygon.Shape as Path).Fill = Brushes.Red;
            //         (polygon.Shape as Path).Stroke =Brushes.Blue;
            //          (polygon.Shape as Path).StrokeThickness = 1.5;
            //          (polygon.Shape as Path).Effect = null;
            //          gmap.Markers.Add(polygon);

           

        }

        private SolidColorBrush GetColorByMood(State currentState)
        {
            double temp = currentState.TotalWeight;
            if (currentState.isMoodDefined)
            {
                if (temp == 0) return Brushes.White;
                else if(temp > 0)
                {
                    if (temp <= 0.5) return Brushes.LightBlue;
                    else if (temp <= 0.75) return Brushes.Blue;
                    else return Brushes.DarkBlue;
                }
                else
                {
                    if (temp >= -0.5) return Brushes.Yellow;
                    else if (temp >= -0.75) return Brushes.Orange;
                    else return Brushes.Red;
                }
            }
            return Brushes.Gray;
        }
    }
}
