using System;
using System.Collections.Generic;
using System.Windows;

namespace boids
{
    public class Boid
    {
        public Vector Position { get; set; }
        public Vector Velocity { get; set; }
        public double MaxSpeed { get; set; } = 2.0;
        public double PerceptionRadius { get; set; } = 50.0;

        // trail
        public Queue<Vector> Trail { get; } = new Queue<Vector>();
        public int MaxTrailLength { get; set; } = 15;

        public Boid(double x, double y)
        {
            Position = new Vector(x, y);
            Random rand = new Random();

            Velocity = new Vector(rand.NextDouble() * 2 - 1, rand.NextDouble() * 2 - 1) * MaxSpeed;
        }

        public void Update(Boid[] boids, double separationStrength, double alignmentStrength, double cohesionStrength, double canvasWidth, double canvasHeight)
        {
            Vector separation = CalculateSeparation(boids);
            Vector alignment = CalculateAlignment(boids);
            Vector cohesion = CalculateCohesion(boids);

            Vector acceleration = separation * separationStrength + alignment * alignmentStrength + cohesion * cohesionStrength;

            Velocity += acceleration;
            if (Velocity.Length > MaxSpeed)
            {
                Velocity = Velocity / Velocity.Length * MaxSpeed;
            }

            Position += Velocity;

            if (Position.X < 0) Position = new Vector(Position.X + canvasWidth, Position.Y);
            if (Position.X > canvasWidth) Position = new Vector(Position.X - canvasWidth, Position.Y);
            if (Position.Y < 0) Position = new Vector(Position.X, Position.Y + canvasHeight);
            if (Position.Y > canvasHeight) Position = new Vector(Position.X, Position.Y - canvasHeight);

            // trail
            Trail.Enqueue(Position);
            if (Trail.Count > MaxTrailLength)
                Trail.Dequeue();
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