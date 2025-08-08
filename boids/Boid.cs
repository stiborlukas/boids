using System;
using System.Windows;

namespace boids
{
    public class Boid
    {
        public Vector Position { get; set; }
        public Vector Velocity { get; set; }
        public double MaxSpeed { get; set; } = 2.0;
        public double PerceptionRadius { get; set; } = 50.0; 

        public Boid(double x, double y)
        {
            Position = new Vector(x, y);
            Random rand = new Random();
        }

        public void Update(Boid[] boids, double separationStrength, double alignmentStrength, double cohesionStrength, double canvasWidth, double canvasHeight)
        {
            Vector separation = CalculateSeparation(boids);
            Vector alignment = CalculateAlignment(boids);
            Vector cohesion = CalculateCohesion(boids);
        }


        private Vector CalculateSeparation(Boid[] boids)
        {
            
        }

        private Vector CalculateAlignment(Boid[] boids)
        {
            
        }

        private Vector CalculateCohesion(Boid[] boids)
        {
            
        }
    }
}