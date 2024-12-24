using System;
using System.Collections.Generic;
using System.Threading;

namespace MeteringSystem
{
   
    public interface IMeter
    {
        void Authorize(string address, string password);
        DateTime ReadDateTime();
        void WriteDateTime(DateTime dateTime);
        void SetTariffProgram(int tariff, TimeSpan startTime);
    }

    
    public abstract class MeterBase : IMeter
    {
        protected const string ConstantAddress = "16";
        protected const string ConstantPassword = "1234567812345678";
        protected List<(int tariff, TimeSpan startTime)> tariffProgram = new List<(int, TimeSpan)>();

        public void Authorize(string address, string password)
        {
            Console.WriteLine("Авторизация...");
            Thread.Sleep(2000); 

            if (address != ConstantAddress || password != ConstantPassword)
            {
                throw new UnauthorizedAccessException("Неверный адрес или пароль.");
            }

            Console.WriteLine("Авторизация успешна.");
        }

        public DateTime ReadDateTime()
        {
            DateTime currentDateTime = DateTime.Now;
            Console.WriteLine($"Считанное время с ПК: {currentDateTime:dd.MM.yyyy HH:mm:ss}");
            return currentDateTime;
        }

        public void WriteDateTime(DateTime dateTime)
        {
            Console.WriteLine($"Дата и время для записи: {dateTime:yyyy.MM.dd HH.mm.ss}");
        }

        public abstract void SetTariffProgram(int tariff, TimeSpan startTime);
    }

    
    public class CE207 : MeterBase
    {
        public override void SetTariffProgram(int tariff, TimeSpan startTime)
        {
            if (tariffProgram.Count >= 4)
            {
                Console.WriteLine("Превышено количество тарифов (максимум 4).");
                return;
            }

            tariffProgram.Add((tariff, startTime));
            Console.WriteLine($"Тариф {tariff} установлен на время {startTime.Hours:D2}:{startTime.Minutes:D2}.");
            DisplayTariffProgram();
        }

        private void DisplayTariffProgram()
        {
            Console.WriteLine("Тарифная программа:");
            foreach (var (tariff, startTime) in tariffProgram)
            {
                Console.WriteLine($"Тариф {tariff} - Время начала: {startTime.Hours:D2}:{startTime.Minutes:D2}");
            }
        }
    }

   
    public class CE208 : MeterBase
    {
        public override void SetTariffProgram(int tarif, TimeSpan startTime)
        {
            if (tariffProgram.Count >= 8)
            {
                Console.WriteLine("Превышено количество тарифов (максимум 8).");
                return;
            }

            tariffProgram.Add((tarif, startTime));
            Console.WriteLine($"Тариф {tarif} установлен на время {startTime.Hours:D2}:{startTime.Minutes:D2}.");
            DisplayTariffProgram();
        }

        private void DisplayTariffProgram()
        {
            Console.WriteLine("Тарифная программа:");
            foreach (var (tarif, startTime) in tariffProgram)
            {
                Console.WriteLine($"Тариф {tarif} - Время начала: {startTime.Hours:D2}:{startTime.Minutes:D2}");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            IMeter meter207 = new CE207();
            IMeter meter208 = new CE208();

            try
            {
                
                

                meter207.Authorize("16", "1234567812345678");
                meter208.Authorize("16", "1234567812345678");

               
                DateTime now = meter207.ReadDateTime();
                meter207.WriteDateTime(now);
                


                
                meter207.SetTariffProgram(1, new TimeSpan(6, 0, 0)); 
                meter207.SetTariffProgram(2, new TimeSpan(18, 0, 0)); 

                
                meter208.SetTariffProgram(1, new TimeSpan(6, 0, 0)); 
                meter208.SetTariffProgram(2, new TimeSpan(12, 0, 0)); 
                meter208.SetTariffProgram(3, new TimeSpan(18, 0, 0)); 
                meter208.SetTariffProgram(4, new TimeSpan(22, 0, 0)); 

                
        

            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}