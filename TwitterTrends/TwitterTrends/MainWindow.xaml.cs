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
using Point = TwitterTrends.Models.Point;
using Polygon = TwitterTrends.Models.Polygon;

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
                    (pol.Shape as Path).Fill = Brushes.Blue;
                    (pol.Shape as Path).Stroke = Brushes.Blue;
                    (pol.Shape as Path).StrokeThickness = 1.5;
                    (pol.Shape as Path).Effect = null;
                    gmap.Markers.Add(pol);
                }

            }
            DrawMarkers("snow_tweets2014.txt");

         
            #region Test
            //   List<Tweet> tweets = TweetParser.Parse(@"..\..\..\Data\Tweets\cali_tweets2014.txt");

            //Country country = StatesParser.Parse(@"..\..\..\Data\States\states.json");
            //   foreach(var state in country.States)
            //   {
            //       foreach(var pol in state.Polygons)
            //       {
            //           foreach(var point in pol.Points)
            //           {
            //               double bufer;
            //               bufer = point.X;
            //               point.X = point.Y;
            //               point.Y = bufer;
            //           }
            //       }
            //   }
            //   Polygon polygon = country.States[0].Polygons[0];
            //   int count = 0;
            //   foreach(var tweet in tweets)
            //   {
            //       if (StatesParser.IsInside(polygon, tweet)) { count++; } 
            //   }
            #endregion


        }





        private void DrawMarkers(string fileName)
        {
            List<Tweet> tweets = TweetParser.Parse(@"..\..\..\Data\Tweets\"+fileName);
            foreach (var tweet in tweets)
            {
                GMapMarker marker = new GMapMarker(new PointLatLng(tweet.PointOnMap.X, tweet.PointOnMap.Y));
                marker.Shape = new Ellipse
                {

                    Fill = Brushes.Red,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1.5,
                    Height = 5,
                    Width = 5
                };
             
                gmap.Markers.Add(marker);
            }
        }
        
    }
}
