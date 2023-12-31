﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace AlgorithmsAnalysis
{
    class Program
    {
        //Создаем список алгоритмов для сортировки.
        static void Main(string[] args)
        {
            List<IResercheable> algorythmList = new List<IResercheable>()
            {
                new TreeSort(2000,"Tree Sort"),
                new MergeSort(2000, "Merge Sort"),
                new BubbleSort(2000, "BubbleSort"),
                new TimSort(2000, "TimSort"),
                new Linal(2000, "Linal"),
                new Summ(2000, "Summ"),
                new Gorner(2000, "Gorner"),
                new GornerDirect(2000,"Direct"),
                new Multiplication(2000, "Multiplication"),
                new CycleSort(2000, "CycleSort"),
                new QuickSort(2000, "QuickSort")
            };

            foreach (var algol in algorythmList)
            {
                Tools.ConductResearch(algol);
            }
        }
    }

    class Tools
    {
        //Создающем массив заданного размера со случайными значениями.
        public static int[] GenerateArray(int size)
        {
            int[] array = new int[size];
            Random random = new Random();
            for (int i = 0; i < array.Length; i++)
                array[i] = random.Next(500000);
            return array;
        }

        //Измеряем время выполнения алгоритма.
        public static long MeasureTime(int[] array, IResercheable algorithm)
        {
            Random random = new Random();
            Stopwatch watch = new Stopwatch();
            algorithm.Run(array, array.Length);
            watch.Start();
            algorithm.Run(array, array.Length);
            watch.Stop();
            return watch.ElapsedTicks / 100;
        }

        //Метод для тестирования
        public static void ConductResearch(IResercheable algorithm, bool sorted = false)
        {
            List<(int, long)> results = new List<(int, long)>();
            results.Add((-1, sorted ? 1 : 0));

            //Тестируем алгоритм 5 раз
            foreach (int dimension in algorithm.TestArray)
            {
                for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine($"Проверка размера {algorithm.Name}: {dimension}");
                    results.Add((dimension,
                        MeasureTime(GenerateArray(dimension), algorithm)));
                }
            }

            Console.WriteLine("Выполнено!");
            //Усредняем время на итерации алгоритма.
            List<(int, long)> res = new List<(int, long)>();
            for (int i = 1; i <= algorithm.TestArray.Length; i++)
            {
                var average = results.Where(x => x.Item1 == i).Average(x => x.Item2);
                res.Add((i, (long)average));
            }
            ExportAsCsv(res, algorithm);
        }

        //Метод, который записывает наши данные в файл
        private static void ExportAsCsv(List<(int, long)> researches, IResercheable algorithm)
        {
            string path =
                $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\{algorithm.Name}_{DateTime.Now.ToString("yyyy-M-dd")}.csv";


            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Tested algorithm:;{algorithm.GetType().Name}({algorithm.Name})");
            sb.AppendLine($"Dimension (elements);Spent time (µs)");
            foreach (var pos in researches)
                sb.AppendLine($"{pos.Item1};{pos.Item2}");
            File.WriteAllText(path, sb.ToString());
            Console.WriteLine($"File saved at: {path}");
        }
    }

    //Абстрактный класс, для алгоритмов, которые будут тестироваться.
    public abstract class IResercheable
    {
        //Принимаем размер и имя в конструктор.
        protected IResercheable(int size, string name)
        {
            TestArray = GenerateArray(size);
            Name = name;
        }
        //Создаем массив по заданным размерам.
        private int[] GenerateArray(int size)
        {
            int[] dimensions = new int[size];
            for (int i = 0; i < dimensions.Length; i++)
            {
                dimensions[i] = i + 1;
            }

            return dimensions;
        }
        //Абстрактный метод, который реализуется в классах наследках.
        public abstract void Run(int[] array, int value = 0);

        public int[] TestArray { get; }
        //Имя, которое отображается в экспортном файле.
        public string Name { get; }
    }
}