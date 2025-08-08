using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;

namespace Clients
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //1 method grem vochte esqan shat??????

            //Task[] tasks = new Task[5];
            Stopwatch stopwatch = new();

            /*for (int i = 0; i < tasks.Length; i++)
            {
                Console.WriteLine($"--> Task {i} starts");
                tasks[i] = AsyncMethod();
            }*/
            stopwatch.Start();

            /*Console.WriteLine($"--> Task 0 starts");
            tasks[0] = AsyncMethod();
            Console.WriteLine($"--> Task 1 starts");
            tasks[1] = AsyncMethod1();
            Console.WriteLine($"--> Task 2 starts");
            tasks[2] = AsyncMethod2();
            Console.WriteLine($"--> Task 3 starts");
            tasks[3] = AsyncMethod3();
            Console.WriteLine($"--> Task 4 starts");
            tasks[4] = AsyncMethod4();

            Task.WaitAll(tasks);*/

            Thread[] threads = new Thread[20];

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(new ParameterizedThreadStart(ThreadMethod));
            }
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i].Start(i + 1);
            }

            stopwatch.Stop();

            Thread.Sleep(150000);

            Console.WriteLine("\nElapsed Time is {0} ms", stopwatch.ElapsedMilliseconds);

        }
        static async void ThreadMethod(object? number)
        {
            HttpClient client = new();
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"Thread: {number} -- request {i}");
                var result = await client.GetStringAsync("http://localhost:32596/api/s/customers/Name/Tigran");
                //Console.WriteLine(result);
            }
            Console.WriteLine("ThreadMethooo00000oooodddddd--- end.");
        }

        async static Task AsyncMethod()
        {
            await Task.Run(async () =>
            {
                HttpClient client = new HttpClient();
                for (int i = 0; i < 100; i++)
                {
                    var result = await client.GetAsync("http://localhost:5128/api/s/customers/Name/sdfgs");
                }
                Console.WriteLine("AsyncMethooooood-------- end.");
            });
        }

        async static Task AsyncMethod1()
        {
            await Task.Run(async () =>
            {
                HttpClient client = new HttpClient();
                for (int i = 0; i < 100; i++)
                {
                    var result = await client.GetAsync("http://localhost:5128/api/s/customers/Name/sdfgs");
                }
                Console.WriteLine("AsyncMethooooood----1 end.");
            });
        }
        async static Task AsyncMethod2()
        {
            await Task.Run(async () =>
            {
                HttpClient client = new HttpClient();
                for (int i = 0; i < 100; i++)
                {
                    var result = await client.GetAsync("http://localhost:5128/api/s/customers/Name/ewfhvu");
                }
                Console.WriteLine("AsyncMethooooooooooooooooooooooood----2 end.");
            });
        }
        async static Task AsyncMethod3()
        {
            await Task.Run(async () =>
            {
                HttpClient client = new HttpClient();
                for (int i = 0; i < 100; i++)
                {
                    var result = await client.GetAsync("http://localhost:5128/api/s/customers/Name/sdfgs");
                }
                Console.WriteLine("AsyncMethooooood----3 end.");
            });
        }
        async static Task AsyncMethod4()
        {
            await Task.Run(async () =>
            {
                HttpClient client = new HttpClient();
                for (int i = 0; i < 100; i++)
                {
                    var result = await client.GetAsync("http://localhost:5128/api/s/customers/Name/sdfgs");
                }
                Console.WriteLine("AsyncMethooooood----4 end.");
            });
        }
    }
}