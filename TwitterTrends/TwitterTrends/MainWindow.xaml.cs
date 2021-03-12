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
            gmap.Zoom = 4;
            gmap.ShowCenter = false;

            gmap.MapProvider = GMapProviders.BingMap;
            GMaps.Instance.Mode = AccessMode.ServerOnly;
            gmap.Position = new PointLatLng(38.8951100, -77.0363700);


            GMapProvider.WebProxy = WebRequest.GetSystemWebProxy();
            GMapProvider.WebProxy.Credentials = CredentialCache.DefaultCredentials;

            Country country;
            country = StatesParser.Parse("E:\\states.json");
            List<GMapPolygon> polygons;

            foreach (var state in country.States)
            {
                List<PointLatLng> pointlatlang1= new List<PointLatLng>();
                foreach (var polygon1 in state.Polygons)
                {
                    polygons = new List<GMapPolygon>();

                    foreach (var point in polygon1.Points)
                    {
                        pointlatlang1.Add(new PointLatLng(point.Y, point.X));
                    }
                    GMapPolygon pol = new GMapPolygon(pointlatlang1);
                    gmap.RegenerateShape(pol);
                    (pol.Shape as Path).Fill = Brushes.Red;
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
    }
}
