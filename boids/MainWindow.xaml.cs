using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace boids
{
    public partial class MainWindow : Window
    {
        private List<Boid> boids = new List<Boid>();
        private DispatcherTimer timer;
        private bool isRunning = false;

        public MainWindow()
        {
            InitializeComponent();
            // timer pro simulaci Update()
            timer = new DispatcherTimer();
            
            // ~60 FPS
            timer.Interval = TimeSpan.FromMilliseconds(16);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // update boids
            double separationStrength = SeparationSlider.Value;
            double alignmentStrength = AlignmentSlider.Value;
            double cohesionStrength = CohesionSlider.Value;
            double canvasWidth = SimulationCanvas.ActualWidth;
            double canvasHeight = SimulationCanvas.ActualHeight;

            foreach (var boid in boids)
            {
                boid.Update(boids.ToArray(), separationStrength, alignmentStrength, cohesionStrength, canvasWidth, canvasHeight);
            }

            // render boids
            RenderBoids();
        }

        private void RenderBoids()
        {
            SimulationCanvas.Children.Clear();
            foreach (var boid in boids)
            {
                // tvar boidu
                Polygon triangle = new Polygon
                {
                    Fill = Brushes.Blue,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1,
                    Points = new PointCollection
                    {
                        new Point(0, 0),
                        new Point(-5, 2.5),
                        new Point(-5, -2.5)
                    }
                };

                double angle = Math.Atan2(boid.Velocity.Y, boid.Velocity.X) * 180 / Math.PI;

                triangle.RenderTransform = new TransformGroup
                {
                    Children = new TransformCollection
                    {
                        new RotateTransform(angle),
                        new TranslateTransform(boid.Position.X, boid.Position.Y)
                    }
                };

                SimulationCanvas.Children.Add(triangle);

                // vykresli trail
                int index = 0;
                foreach (var point in boid.Trail)
                {
                    double opacity = (double)index / boid.Trail.Count;
                    Ellipse dot = new Ellipse
                    {
                        Width = 3,
                        Height = 3,
                        Fill = new SolidColorBrush(Color.FromArgb((byte)(opacity * 255), 0, 0, 255)),
                        StrokeThickness = 0
                    };
                    Canvas.SetLeft(dot, point.X - 1.5);
                    Canvas.SetTop(dot, point.Y - 1.5);
                    SimulationCanvas.Children.Add(dot);
                    index++;
                }

            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isRunning)
            {
                if (boids.Count == 0)
                {
                    Random rand = new Random();
                    int boidCount = (int)BoidCountSlider.Value;
                    for (int i = 0; i < boidCount; i++)
                    {
                        double x = rand.NextDouble() * SimulationCanvas.ActualWidth;
                        double y = rand.NextDouble() * SimulationCanvas.ActualHeight;
                        boids.Add(new Boid(x, y));
                    }
                }
                timer.Start();
                isRunning = true;
            }
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            isRunning = false;
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            isRunning = false;
            boids.Clear();
            SimulationCanvas.Children.Clear();
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}