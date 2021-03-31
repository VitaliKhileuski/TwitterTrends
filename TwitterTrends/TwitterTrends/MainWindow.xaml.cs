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
using TwitterTrends.Data;
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
            Database.GetInstance().SetPathTweetFile(@"..\..\..\Data\Tweets\football_tweets2014.txt");
            
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

            foreach (var state in Database.GetInstance().Country.States)
            {
                foreach (var polygon in state.Polygons)
                {
                    List<PointLatLng> pointlatlang = new List<PointLatLng>();
                    GMapPolygon pol = new GMapPolygon(pointlatlang);
                    foreach (var point in polygon.Points)
                    {
                        pointlatlang.Add(new PointLatLng(point.X, point.Y));
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

        }
        private void listViewItemClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
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
