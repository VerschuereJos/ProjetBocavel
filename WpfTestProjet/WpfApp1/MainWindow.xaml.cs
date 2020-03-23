using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const double step = 10;
        double xmin;
        double ymin;
        double xmax;
        double ymax;
        public MainWindow()
        {
            InitializeComponent();
            xmin = 1;
            ymin = 1;
            xmax = canGraph.Width - xmin;
            ymax = canGraph.Height / 2 - ymin;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RecuperationDonneesPDF recuperation = new RecuperationDonneesPDF();
            const double margin = 20;
            
            

            // Make the X axis.
            int a = 1;
            GeometryGroup xaxis_geom = new GeometryGroup();
            xaxis_geom.Children.Add(new LineGeometry(new Point(0, ymax), new Point(canGraph.Width, ymax)));
            for (double x = xmin + step; x <= canGraph.Width - step; x += step)
            {
                
                xaxis_geom.Children.Add(new LineGeometry(
                    new Point(x, ymax - margin / 2),
                    new Point(x, ymax + margin / 2)));
                
                if (a == 3)
                {
                    Label label = new Label();
                    label.FontSize = 11;
                    label.Margin = new Thickness(x - 10, ymax + 10 + margin / 2 - 14, 0, 4);
                    label.Content = (x - 1);
                    canGraph.Children.Add(label);
                    a = 0;
                }
                a++;
            }

            Path xaxis_path = new Path();
            xaxis_path.StrokeThickness = 1;
            xaxis_path.Stroke = Brushes.Black;
            xaxis_path.Data = xaxis_geom;

            canGraph.Children.Add(xaxis_path);

            // Make the Y ayis.
            GeometryGroup yaxis_geom = new GeometryGroup();
            yaxis_geom.Children.Add(new LineGeometry(
                new Point(xmin, 0), new Point(xmin, canGraph.Height)));
            for (double y = step; y <= canGraph.Height - step; y += step)
            {
                yaxis_geom.Children.Add(new LineGeometry(
                    new Point(xmin - margin / 2, y),
                    new Point(xmin + margin / 2, y)));

                Label label = new Label();
                label.Margin = new Thickness(xmin - 19 - margin / 2 , y-14, 0, 4);
                label.Content = (ymax - y + 1) / step;
                canGraph.Children.Add(label);
            }
            
            Path yaxis_path = new Path();
            yaxis_path.StrokeThickness = 1;
            yaxis_path.Stroke = Brushes.Black;
            yaxis_path.Data = yaxis_geom;

            canGraph.Children.Add(yaxis_path);

            
            
        }
        

        private void activeSearch(object sender, RoutedEventArgs e)
        {
            
            
            Brush[] brushes = { Brushes.Red, Brushes.Blue };
            RecuperationDonneesPDF recuperation = new RecuperationDonneesPDF();
            ArrayList myAL = new ArrayList();
            ArrayList arrayTemp = new ArrayList();
            ArrayList arrayTemperature = new ArrayList();
            myAL = recuperation.Processus();

            Information leInfo;
            Information lePremierInfo = (Information)myAL[0];
            int lePremierTemp = ((int.Parse(lePremierInfo.GetHeure().Substring(0, 2)) * 60) + (int.Parse(lePremierInfo.GetHeure().Substring(3, 2))));
            int changementDate = 0;
            for (int i = 0; i < myAL.Count; i++)
            {

                leInfo = (Information)myAL[i];
                string stringTemperature = leInfo.getTemperature().Replace('.', ',');
                arrayTemperature.Add(Double.Parse(stringTemperature));
                if (i > 1)
                {
                    Information leInfoAvant = (Information)myAL[i - 1];
                    if (leInfo.GetDate() != leInfoAvant.GetDate())
                    {
                        changementDate = changementDate + 1;
                    }
                }

                string stringMinute = leInfo.GetHeure().Substring(3, 2);
                string stringHeure = leInfo.GetHeure().Substring(0, 2);
                int minute = int.Parse(stringMinute);
                int heure = int.Parse(stringHeure);
                int uniteDeTemp = minute + (heure * 60) + (changementDate * 24 * 60) - lePremierTemp;
                arrayTemp.Add(uniteDeTemp);
            }
            PointCollection points = new PointCollection();
            for (int i = 0; i < arrayTemp.Count; i++)
            {
                // a cause du fait qu'on ait un origine à haut et droit en utilisé le -1 pour obtenir une orientation correcte de la courbe et on ajoute Ymax pour le mettre au bon position
                if (int.Parse(arrayTemp[i].ToString()) < xmax)
                {
                    points.Add(new Point((int.Parse(arrayTemp[i].ToString())), (-step * (double)arrayTemperature[i]) + ymax));
                }
                else
                {
                    labelAlerte.Content = "alerte : les valeurs depassent la graphe";
                }

            }



            Polyline polyline = new Polyline();
            polyline.StrokeThickness = 1;
            polyline.Stroke = brushes[0];
            polyline.Points = points;

            canGraph.Children.Add(polyline);
        }
    }
    


}
