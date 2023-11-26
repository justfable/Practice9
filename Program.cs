using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice9
{

    abstract class Storage
    {
        private string name; 
        private string model;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Model
        {
            get { return model; }
            set { model = value; }
        }

        public abstract double GetMemoryCapacity();
        public abstract void CopyData(double dataAmount);
        public abstract double GetFreeMemory();
        public abstract void GetDeviceInfo();
    }

    class Flash : Storage
    {
        public double Speed { get; set; }
        public double MemoryCapacity { get; set; }
        private double usedMemory = 0;

        public override double GetMemoryCapacity()
        {
            return MemoryCapacity;
        }

        public override void CopyData(double dataAmount)
        {
            Console.WriteLine("Копирование данных на Flash-память ");
            // Проверка на возможность копирования
            if (usedMemory + dataAmount <= MemoryCapacity)
            {
                usedMemory += dataAmount;
                Console.WriteLine($"На Flash-память скопировано {dataAmount} Гб");
            }
            else
            {
                Console.WriteLine("Недостаточно свободного места на Flash-памяти ");
            }
        }

        public override double GetFreeMemory()
        {
            return MemoryCapacity - usedMemory;
        }

        public override void GetDeviceInfo()
        {
            Console.WriteLine($"Flash-память: {Name}, модель: {Model}, скорость: {Speed}, объем: {MemoryCapacity}");
        }
    }


    class DVD : Storage
    {
        public double Speed { get; set; }
        public double MemoryCapacity { get; set; } 
        public bool DoubleSide { get; set; }
        private double usedMemory = 0;

        public override double GetMemoryCapacity()
        {
            return DoubleSide ? MemoryCapacity * 2 : MemoryCapacity;
        }

        public override void CopyData(double dataAmount)
        {
            Console.WriteLine("Копирование данных на DVD...");
            // Проверка на возможность копирования
            if (usedMemory + dataAmount <= GetMemoryCapacity())
            {
                usedMemory += dataAmount;
                Console.WriteLine($"На DVD скопировано {dataAmount} Гб");
            }
            else
            {
                Console.WriteLine("Недостаточно свободного места на DVD ");
            }
        }

        public override double GetFreeMemory()
        {
            return GetMemoryCapacity() - usedMemory;
        }

        public override void GetDeviceInfo()
        {
            string sideInfo = DoubleSide ? "двусторонний" : "односторонний";
            Console.WriteLine($"DVD: {Name}, модель: {Model}, скорость: {Speed}, тип: {sideInfo}, объем: {GetMemoryCapacity()} Гб");
        }
    }

    class HDD : Storage
    {
        public double Speed { get; set; }
        public int PartitionCount { get; set; }
        public double PartitionSize { get; set; }
        private double usedMemory = 0;
        public override double GetMemoryCapacity()
        {
            return PartitionCount * PartitionSize;
        }

        public override void CopyData(double dataAmount)
        {
            Console.WriteLine("Копирование данных на HDD...");
            // Проверка на возможность копирования
            if (usedMemory + dataAmount <= GetMemoryCapacity())
            {
                usedMemory += dataAmount;
                Console.WriteLine($"На HDD скопировано {dataAmount} Гб ");
            }
            else
            {
                Console.WriteLine("Недостаточно свободного места на HDD ");
            }
        }

        public override double GetFreeMemory()
        {
            return GetMemoryCapacity() - usedMemory;
        }

        public override void GetDeviceInfo()
        {
            Console.WriteLine($"HDD: {Name}, модель: {Model}, скорость: {Speed}, количество разделов: {PartitionCount}, размер раздела: {PartitionSize} Гб");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Создание объектов для носителей информации
            Storage flashDrive = new Flash { Name = "SuperFlash", Model = "FlashModel1", Speed = 100, MemoryCapacity = 32 }; // Объем в Гб
            Storage dvd = new DVD { Name = "SuperDVD", Model = "DVDModel1", Speed = 10, MemoryCapacity = 4.7, DoubleSide = false }; // Объем в Гб
            Storage hdd = new HDD { Name = "SuperHDD", Model = "HDDModel1", Speed = 60, PartitionCount = 4, PartitionSize = 250 }; // Размер раздела в Гб

            // Работа с объектами через массив
            Storage[] storages = new Storage[] { flashDrive, dvd, hdd };

            // Расчет общего количества памяти всех устройств
            double totalMemory = 0;
            foreach (var storage in storages)
            {
                storage.GetDeviceInfo();
                totalMemory += storage.GetMemoryCapacity();
            }
            Console.WriteLine($"Общее количество памяти всех устройств: {totalMemory} Гб");

            // Входные данные
            double totalData = 565; // Объем данных для переноса в Гб
            double fileData = 0.780; // Размер файла в Гб

            // Расчет необходимого количества Flash-памяти
            int flashDriveCount = (int)Math.Ceiling(totalData / flashDrive.GetMemoryCapacity());
            Console.WriteLine($"Необходимое количество Flash-памяти: {flashDriveCount} штук");

            // Расчет времени копирования для Flash-памяти
            double timeToCopyFlash = (fileData * 1024) / ((Flash)flashDrive).Speed / 60 * flashDriveCount; // Время в минутах
            Console.WriteLine($"Время копирования на все Flash-памяти: {timeToCopyFlash} минут");

            // Копирование данных и вычисление свободной памяти для Flash-памяти
            flashDrive.CopyData(fileData);
            Console.WriteLine($"Свободная память на Flash-памяти: {flashDrive.GetFreeMemory()} Гб");

            // Расчет необходимого количества DVD
            int dvdCountSingle = (int)Math.Ceiling(totalData / ((DVD)dvd).GetMemoryCapacity());
            Console.WriteLine($"Необходимое количество односторонних DVD: {dvdCountSingle} штук");

            // Расчет времени копирования для DVD
            double timeToCopyDVD = (fileData * 1024) / ((DVD)dvd).Speed / 60 * dvdCountSingle; // Время в минутах
            Console.WriteLine($"Время копирования на все DVD: {timeToCopyDVD} минут");

            // Копирование данных и вычисление свободной памяти для DVD
            dvd.CopyData(fileData);
            Console.WriteLine($"Свободная память на DVD: {dvd.GetFreeMemory()} Гб");

            // Расчет необходимого количества HDD
            int hddCount = (int)Math.Ceiling(totalData / hdd.GetMemoryCapacity());
            Console.WriteLine($"Необходимое количество HDD: {hddCount} штук");

            // Расчет времени копирования для HDD
            double timeToCopyHDD = (fileData * 1024) / ((HDD)hdd).Speed / 60 * hddCount; // Время в минутах
            Console.WriteLine($"Время копирования на все HDD: {timeToCopyHDD} минут");

            // Копирование данных и вычисление свободной памяти для HDD
            hdd.CopyData(fileData);
            Console.WriteLine($"Свободная память на HDD: {hdd.GetFreeMemory()} Гб");
        }

    }

}
