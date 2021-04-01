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
using WpfAnimatedGif;
using Brushes = System.Windows.Media.Brushes;
using Point = TwitterTrends.Models.Point;

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
            //MainWindowViewModel viewModel = new MainWindowViewModel();
            //this.DataContext = viewModel;


        }
        Country country;
        private void Loaded_gmap(object sender, RoutedEventArgs e)
        {
            gmap.Bearing = 0;
            gmap.CanDragMap = true;
            gmap.DragButton = MouseButton.Left;


            gmap.MaxZoom =5;
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

            //Country country;
            //country = StatesParser.Parse(@"..\..\..\Data\States\states.json");

            //foreach (var state in Database.GetInstance().Country.States)
            //{
            //    foreach (var polygon in state.Polygons)
            //    {
            //        List<PointLatLng> pointlatlang = new List<PointLatLng>();
            //        GMapPolygon pol = new GMapPolygon(pointlatlang);
            //        foreach (var point in polygon.Points)
            //        {
            //            pointlatlang.Add(new PointLatLng(point.X, point.Y));
            //        }
            //        pol.Points = pointlatlang;
            //        gmap.RegenerateShape(pol);
            //        (pol.Shape as Path).Fill = GetColorByMood(state);
            //        (pol.Shape as Path).Stroke = Brushes.Blue;
            //        (pol.Shape as Path).StrokeThickness = 1.5;
            //        (pol.Shape as Path).Effect = null;
            //        gmap.Markers.Add(pol);
            //    }

            //}

        }
        private void DrawStates()
        {
            gmap.Markers.Clear();
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

                DrawMarkers(state);

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
        private SolidColorBrush GetColorByTweet(Tweet tweet)
        {
            double temp = tweet.MoodWeight;

            if (temp == 0) { return Brushes.White; }
            else if (temp > 0)
            {
                if (temp <= 0.5) { return Brushes.LightBlue; }
                else if (temp <= 0.75) { return Brushes.Blue; }
                else { return Brushes.DarkBlue; }
            }
            else
            {
                if (temp >= -0.5) { return Brushes.Yellow; }
                else if (temp >= -0.75) { return Brushes.Orange; }
                else { return Brushes.Red; }
            }
            
          
        }
        private void DrawMarkers(State state)
        {
            //   List<Tweet> tweets = TweetParser.Parse(path);
            List<Tweet> tweets = state.Tweets;
            foreach (var tweet in tweets)
            {
                GMapMarker marker = new GMapMarker(new PointLatLng(tweet.PointOnMap.X,tweet.PointOnMap.Y));
                marker.Shape = new Ellipse
                {
                    Width = 5,
                    Height = 5,
                    Fill = GetColorByTweet(tweet),
                };
                gmap.Markers.Add(marker);
            }
        }

        private async void ComboBox_Selected(object sender, SelectionChangedEventArgs e)

        {
            string path = null;
            ComboBoxItem selectedItem = (ComboBoxItem)ComboBoxChooseCountry.SelectedItem;
            string response= selectedItem.Content.ToString();
            switch (response) 
            {

                case "Cali":
                    {
                        path = @"..\..\..\Data\Tweets\cali_tweets2014.txt";
                        break;
                    }
                case "Family":
                    {
                        path = @"..\..\..\Data\Tweets\family_tweets2014.txt";
                        break;
                    }
                case "Football":
                    {
                        path = @"..\..\..\Data\Tweets\cali_tweets2014.txt";
                        break;
                    }
                case "School":
                    {
                        path = @"..\..\..\Data\Tweets\high_school_tweets2014.txt";
                        break;
                    }
                case "Movie":
                    {
                        path = @"..\..\..\Data\Tweets\movie_tweets2014.txt";
                        break;
                    }
                case "Shopping":
                    {
                        path = @"..\..\..\Data\Tweets\shopping_tweets2014.txt";
                        break;
                    }
                case "Snow":
                    {
                        path = @"..\..\..\Data\Tweets\snow_tweets2014.txt";
                        break;
                    }
                case "Texas":
                    {
                        path = @"..\..\..\Data\Tweets\texas_tweets2014.txt";
                        break;
                    }
                case "Weekend":
                    {
                        path = @"..\..\..\Data\Tweets\weekend_tweets2014.txt";
                        break;
                    }
            }

            var controller = ImageBehavior.GetAnimationController(RightLogoImage);
            controller.Play();

            Database.GetInstance().SetPathTweetFile(path);
            await Task.Run(Database.GetInstance().StartNewState);
            country = Database.GetInstance().Country;
            DrawStates();

            controller.Pause();
            controller.GotoFrame(0);

        }

        private void MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
           
            PointLatLng clickpoint = new PointLatLng();
            clickpoint = gmap.FromLocalToLatLng((int)Mouse.GetPosition(this).X,(int)Mouse.GetPosition(this).Y);
            Point point = new Point(clickpoint.Lat,clickpoint.Lng);
            Tweet tweet = new Tweet();
            tweet.PointOnMap = point;


             foreach (var state in country.States)
             {
                foreach (var polygon in state.Polygons)
                {
                    if (StatesParser.IsInside(polygon, tweet))
                    {
                        if (state.Tweets.Count == 0)
                        {
                            mostNegativeBlock.Text = "NaN";
                            mostPositiveBlock.Text = "NaN";
                            indexBlock.Text = state.Name.ToString();

                        }
                        else
                        {
                            indexBlock.Text = state.Name.ToString();
                            mostNegativeBlock.Text = state.Tweets.Min(u => u.MoodWeight).ToString();
                            mostPositiveBlock.Text = state.Tweets.Max(u => u.MoodWeight).ToString();
                        }

                    }
                }

             }
            
        }
    }
}
