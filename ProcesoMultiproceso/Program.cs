using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;

class Program
{
    static Random random = new Random();
    static ConcurrentDictionary<int, (int gpu, int core)> processGpuMapping = new ConcurrentDictionary<int, (int, int)>();
    static int gpuCount = 4;
    static int coresPerGpu = 2;

    static void Main(string[] args)
    {
        Console.WriteLine("Presiona cualquier tecla para generar un nuevo proceso...");
        Console.WriteLine("Presiona 'q' para salir.");

        while (true)
        {
            var key = Console.ReadKey(true);

            if (key.KeyChar == 'q')
                break;

            if (key.KeyChar != '\u0000') // Se presionó una tecla
            {
                Thread thread = new Thread(GenerateProcess);
                thread.Start();
            }
        }
    }

    static void GenerateProcess()
    {
        string dummyProcessPath = @"C:\Github\Threading\DummyProcess\bin\Debug\net8.0\DummyProcess.exe"; // Reemplaza netX.X con la versión de .NET
        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.FileName = dummyProcessPath;
        startInfo.WindowStyle = ProcessWindowStyle.Hidden;

        int duration = random.Next(20000, 30001); // Duración del proceso en milisegundos (entre 20 y 30 segundos)
        int gpuConsumption = random.Next(1, 101); // Consumo de la GPU en porcentaje

        startInfo.Arguments = $"{duration} {gpuConsumption}";

        Process process = Process.Start(startInfo);

        var assignedGpuCore = AssignGpuCore();
        processGpuMapping[process.Id] = assignedGpuCore;

        Console.WriteLine($"Proceso {process.Id} generado con duración de {duration / 1000} segundos, consumo de GPU del {gpuConsumption}%, asignado a GPU {assignedGpuCore.gpu} núcleo {assignedGpuCore.core}.");

        // Esperar a que el proceso termine y mostrar su ID
        process.WaitForExit();
        Console.WriteLine($"Proceso {process.Id} ha finalizado. Estaba asignado a GPU {assignedGpuCore.gpu} núcleo {assignedGpuCore.core}.");

        // Remover el proceso de la asignación
        processGpuMapping.TryRemove(process.Id, out _);
    }

    static (int gpu, int core) AssignGpuCore()
    {
        int assignedGpu = random.Next(0, gpuCount);
        int assignedCore = random.Next(0, coresPerGpu);

        return (assignedGpu, assignedCore);
    }
}
