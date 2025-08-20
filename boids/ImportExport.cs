using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace boids
{
    public class SimulationState
    {
        public List<BoidData> Boids { get; set; }
        public double SeparationStrength { get; set; }
        public double AlignmentStrength { get; set; }
        public double CohesionStrength { get; set; }
        public double MaxSpeed { get; set; }
    }

    public class BoidData
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double VelocityX { get; set; }
        public double VelocityY { get; set; }
    }

    public static class ImportExport
    {
        // Serializuje SimulationState do JSON
        public static void Export(string path, SimulationState state)
        {
            try
            {
                string json = JsonSerializer.Serialize(state, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(path, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Export failed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Deserializuje JSON do SimulationState
        public static SimulationState Import(string path)
        {
            try
            {
                string json = File.ReadAllText(path);
                return JsonSerializer.Deserialize<SimulationState>(json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Import failed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }
    }
}