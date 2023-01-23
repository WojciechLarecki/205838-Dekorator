using System;

namespace DekoratorUI
{
    class Program
    {
        static void Main(string[] args)
        {
            IDistanceCalculator calculator = new DistanceCalculator();

            do
            {
                PrintMenu(calculator);
                Console.WriteLine();
            } while (true);
        }

        public static void PrintMenu(IDistanceCalculator calculator)
        {
            Console.Write("Podaj ilość km w trasie: ");
            var distance = double.Parse(Console.ReadLine());
            Console.Write("Podaj rodzaj subskrypcji (F/S/E): ");
            var subscriptionType = Console.ReadLine();

            if (subscriptionType == "F")
            {
                calculator = new FreeSubscriptionDecorator(calculator);
            }
            else if (subscriptionType == "S")
            {
                calculator = new SMCompanyDecorator(calculator);
            }
            else if (subscriptionType == "E")
            {
                calculator = new EnterpriseSubscriptionDecorator(new SMCompanyDecorator(calculator));
            }
            calculator.ComputeCostOfDistance(distance);
        }
    }

    public interface IDistanceCalculator
    {
        double ComputeCostOfDistance(double distance);
    }

    public class DistanceCalculator : IDistanceCalculator
    {
        double costRate;
        public DistanceCalculator()
        {
            var randomInt = new Random().Next(1, 4);
            var random = new Random().NextDouble() + 0.5;
            costRate = random + randomInt;
        }
        public double ComputeCostOfDistance(double distance)
        {
            var distanceCost = Math.Round(costRate * distance, 2);
            Console.WriteLine($"Cena trasy wyniesie {distanceCost}PLN");
            return distanceCost;
        }
    }


    public class FreeSubscriptionDecorator : IDistanceCalculator
    {
        IDistanceCalculator calculator;

        public FreeSubscriptionDecorator(IDistanceCalculator calculator)
        {
            this.calculator = calculator;
        }

        public double ComputeCostOfDistance(double distance)
        {
            var distanceCost = calculator.ComputeCostOfDistance(distance);
            Console.WriteLine("Spróbuj najlepsze pierogi w Pierogarni na Mokotowie!.");
            return distanceCost;
        }
    }

    public class SMCompanyDecorator : IDistanceCalculator
    {
        IDistanceCalculator calculator;
        double marketAverage = new Random().NextDouble() / 2;

        public SMCompanyDecorator(IDistanceCalculator calculator)
        {
            this.calculator = calculator;
        }

        public double ComputeCostOfDistance(double distance)
        {
            var distanceCost = calculator.ComputeCostOfDistance(distance);
            var marketDistanceCost = marketAverage * distance;
            Console.WriteLine($"Podana cena różni się od ceny rynkowej o {Math.Round(distanceCost - marketDistanceCost, 2)}PLN.");
            return distanceCost;
        }
    }

    public class EnterpriseSubscriptionDecorator : IDistanceCalculator
    {
        IDistanceCalculator calculator;

        public EnterpriseSubscriptionDecorator(IDistanceCalculator calculator)
        {
            this.calculator = calculator;
        }

        public double ComputeCostOfDistance(double distance)
        {
            var distanceCost = calculator.ComputeCostOfDistance(distance);
            var optimalCost = Math.Round(0.8 * distanceCost, 2);
            Console.WriteLine($"Cena za zoptymalizowaną trasę wynosi {optimalCost}PLN.");
            return distanceCost;
        }
    }
}
