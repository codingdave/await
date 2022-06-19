using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/
namespace AsyncBreakfast
{
    public class Breakfast
    {
        public void HaveBreakfast()
        {
            _ = PourCoffee();
            Console.WriteLine("coffee is ready");

            FryEggs(2);
            Console.WriteLine("Eggs are ready");

            FryBacon(3);
            Console.WriteLine("Bacon is ready");

            MakeToastWithButterAndJam(2);
            Console.WriteLine("Toast is ready");

            _ = PourOrangeJuice();
            Console.WriteLine("orange juice is ready");
            Console.WriteLine("Breakfast is ready!");
        }

        public async Task HaveBreakfastAsync()
        {
            _ = PourCoffee();
            Console.WriteLine("coffee is ready");

            var eggsTask = FryEggsAsync(2);
            var baconTask = FryBaconAsync(3);
            var toastTask = MakeToastWithButterAndJamAsync(2);

            _ = PourOrangeJuice();
            Console.WriteLine("orange juice is ready");
            Console.WriteLine("Breakfast is ready!");

            var breakfastTasks = new List<Task> { eggsTask, baconTask, toastTask };
            while (breakfastTasks.Count > 0)
            {
                Task finishedTask = await Task.WhenAny(breakfastTasks).ConfigureAwait(false);
                if (finishedTask == eggsTask)
                {
                    Console.WriteLine("Eggs are ready");
                }
                else if (finishedTask == baconTask)
                {
                    Console.WriteLine("Bacon is ready");
                }
                else if (finishedTask == toastTask)
                {
                    Console.WriteLine("Toast is ready");
                }
                _ = breakfastTasks.Remove(finishedTask);
            }
        }

        static async Task<Toast> MakeToastWithButterAndJamAsync(int number)
        {
            var toast = await ToastBreadAsync(number).ConfigureAwait(false);
            ApplyButter(toast);
            ApplyJam(toast);

            return toast;
        }

        static Toast MakeToastWithButterAndJam(int number)
        {
            var toast = ToastBread(number);
            ApplyButter(toast);
            ApplyJam(toast);

            return toast;
        }

        private static Juice PourOrangeJuice()
        {
            Console.WriteLine("Pouring orange juice");
            return new Juice();
        }

        private static void ApplyJam(Toast toast) =>
            Console.WriteLine($"Putting jam on the toast {toast}");

        private static void ApplyButter(Toast toast) =>
            Console.WriteLine($"Putting butter on the toast {toast}");

        private static async Task<Toast> ToastBreadAsync(int slices)
        {
            for (int slice = 0; slice < slices; slice++)
            {
                Console.WriteLine("Putting a slice of bread in the toaster");
            }
            Console.WriteLine("Start toasting...");
            await Task.Delay(3000).ConfigureAwait(false);
            Console.WriteLine("Remove toast from toaster");

            return new Toast();
        }

        private static async Task<Bacon> FryBaconAsync(int slices)
        {
            Console.WriteLine($"putting {slices} slices of bacon in the pan");
            Console.WriteLine("cooking first side of bacon...");
            await Task.Delay(3000).ConfigureAwait(false);
            for (int slice = 0; slice < slices; slice++)
            {
                Console.WriteLine("flipping a slice of bacon");
            }
            Console.WriteLine("cooking the second side of bacon...");
            await Task.Delay(3000).ConfigureAwait(false);
            Console.WriteLine("Put bacon on plate");

            return new Bacon();
        }

        private static async Task<Egg> FryEggsAsync(int howMany)
        {
            Console.WriteLine("Warming the egg pan...");
            await Task.Delay(3000).ConfigureAwait(false);
            Console.WriteLine($"cracking {howMany} eggs");
            Console.WriteLine("cooking the eggs ...");
            await Task.Delay(3000).ConfigureAwait(false);
            Console.WriteLine("Put eggs on plate");

            return new Egg();
        }

        private static Toast ToastBread(int slices)
        {
            for (int slice = 0; slice < slices; slice++)
            {
                Console.WriteLine("Putting a slice of bread in the toaster");
            }
            Console.WriteLine("Start toasting...");
            Task.Delay(3000).Wait();
            Console.WriteLine("Remove toast from toaster");

            return new Toast();
        }

        private static Bacon FryBacon(int slices)
        {
            Console.WriteLine($"putting {slices} slices of bacon in the pan");
            Console.WriteLine("cooking first side of bacon...");
            Task.Delay(3000).Wait();
            for (int slice = 0; slice < slices; slice++)
            {
                Console.WriteLine("flipping a slice of bacon");
            }
            Console.WriteLine("cooking the second side of bacon...");
            Task.Delay(3000).Wait();
            Console.WriteLine("Put bacon on plate");

            return new Bacon();
        }

        private static Egg FryEggs(int howMany)
        {
            Console.WriteLine("Warming the egg pan...");
            Task.Delay(3000).Wait();
            Console.WriteLine($"cracking {howMany} eggs");
            Console.WriteLine("cooking the eggs ...");
            Task.Delay(3000).Wait();
            Console.WriteLine("Put eggs on plate");

            return new Egg();
        }

        private static Coffee PourCoffee()
        {
            Console.WriteLine("Pouring coffee");
            return new Coffee();
        }
    }
}
