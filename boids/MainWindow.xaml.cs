using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace boids
{
    public partial class MainWindow : Window
    {
        private List<Boid> boids = new List<Boid>();
        private DispatcherTimer timer;
        private bool isRunning = false;

        private int frameCount = 0;
        private DateTime lastFpsUpdate = DateTime.Now;

        public MainWindow()
        {
            InitializeComponent();
            // timer pro simulaci Update()
            timer = new DispatcherTimer();

            timer.Interval = TimeSpan.FromMilliseconds(12);
            timer.Tick += Timer_Tick;
        }

        // Aktualizuje boidy a vykresluje scénu
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
                boid.MaxSpeed = MaxSpeedSlider.Value;
                boid.Update(boids.ToArray(), separationStrength, alignmentStrength, cohesionStrength, canvasWidth, canvasHeight);
            }

            // render boids
            RenderBoids();
            CountFPS();   
        }

        private void CountFPS()
        {
            frameCount++;
            var currentTime = DateTime.Now;
            var elapsed = (currentTime - lastFpsUpdate).TotalSeconds;
            if (elapsed >= 1.0)
            {
                double fps = frameCount / elapsed;
                FpsDisplay.Text = $"{fps:F0}";
                frameCount = 0;
                lastFpsUpdate = currentTime;
            }
            Canvas.SetZIndex(FpsDisplay, 1000);
        }

        // Vykresluje boidy jako trojúhelníky na Canvas
        private void RenderBoids()
        {
            SimulationCanvas.Children.Clear();
            Color neonGreen = Color.FromRgb(0, 255, 127);
            foreach (var boid in boids)
            {
                // tvar boidu
                Polygon triangle = new Polygon
                {
                    Fill = new SolidColorBrush(neonGreen),
                    Stroke = new SolidColorBrush(neonGreen),
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
                if (TrailCheckBox.IsChecked == true)
                {
                    int index = 0;
                    foreach (var point in boid.Trail)
                    {
                        double opacity = (double)index / boid.Trail.Count;
                        Color trailColor = Color.FromArgb((byte)(opacity * 255), neonGreen.R, neonGreen.G, neonGreen.B);

                        Ellipse dot = new Ellipse
                        {
                            Width = 3,
                            Height = 3,
                            Fill = new SolidColorBrush(trailColor),
                            StrokeThickness = 0
                        };
                        Canvas.SetLeft(dot, point.X - 1.5);
                        Canvas.SetTop(dot, point.Y - 1.5);
                        SimulationCanvas.Children.Add(dot);
                        index++;
                    }
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
            else
            {
                timer.Stop();
                isRunning = false;
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            isRunning = false;
            boids.Clear();
            SimulationCanvas.Children.Clear();

            StartButton_Click(StartButton, new RoutedEventArgs());
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*",
                Title = "Import"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                var state = ImportExport.Import(openFileDialog.FileName);
                if (state != null)
                {
                    SeparationSlider.Value = state.SeparationStrength;
                    AlignmentSlider.Value = state.AlignmentStrength;
                    CohesionSlider.Value = state.CohesionStrength;
                    MaxSpeedSlider.Value = state.MaxSpeed;
                    boids.Clear();
                    foreach (var bd in state.Boids)
                    {
                        var b = new Boid(bd.X, bd.Y);
                        b.Velocity = new Vector(bd.VelocityX, bd.VelocityY);
                        b.MaxSpeed = state.MaxSpeed;
                        boids.Add(b);
                    }
                    RenderBoids();
                }
            }
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*",
                Title = "Export",
                DefaultExt = "json",
                AddExtension = true
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                SimulationState state = new SimulationState
                {
                    SeparationStrength = SeparationSlider.Value,
                    AlignmentStrength = AlignmentSlider.Value,
                    CohesionStrength = CohesionSlider.Value,
                    MaxSpeed = boids.Count > 0 ? boids[0].MaxSpeed : 5.0,
                    Boids = boids.Select(b => new BoidData
                    {
                        X = b.Position.X,
                        Y = b.Position.Y,
                        VelocityX = b.Velocity.X,
                        VelocityY = b.Velocity.Y
                    }).ToList()
                };
                ImportExport.Export(saveFileDialog.FileName, state);
            }
        }
    }
}