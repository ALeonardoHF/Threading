using System;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        int duration = int.Parse(args[0]);
        int gpuConsumption = int.Parse(args[1]);

        Console.WriteLine($"Proceso iniciado. Duración: {duration / 1000} segundos. Consumo de GPU: {gpuConsumption}%.");

        DateTime endTime = DateTime.Now + TimeSpan.FromMilliseconds(duration);

        while (DateTime.Now < endTime)
        {
            // Simula el consumo de la GPU
            SimulateGPUUsage(gpuConsumption);
        }

        Console.WriteLine("Proceso finalizado.");
    }

    static void SimulateGPUUsage(int gpuConsumption)
    {
        int sleepTime = 100 - gpuConsumption;
        if (sleepTime < 0) sleepTime = 0;

        Thread.Sleep(sleepTime);
    }
}
