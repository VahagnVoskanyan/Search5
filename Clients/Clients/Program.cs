using System.Threading.Tasks;

namespace Clients
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //1 method grem vochte esqan shat??????

            /*Task task1 = AsyncMethod1();
            Task task2 = AsyncMethod2();
            Task task3 = AsyncMethod3();
            Task task4 = AsyncMethod4();
            task1.Wait();
            task2.Wait();
            task3.Wait();
            task4.Wait();*/

            Task task1 = AsyncMethod0();
            Console.WriteLine("--> task1 end");
            Task task2 = AsyncMethod();
            Console.WriteLine("--> task2 end");
            Task task3 = AsyncMethod();
            Console.WriteLine("--> task3 end");
            Task task4 = AsyncMethod();
            Console.WriteLine("--> task4 end");

            task1.Wait();
            task2.Wait();
            task3.Wait();
            task4.Wait();
        }
        async static Task AsyncMethod()
        {
            await Task.Run(async () =>
            {
                for (int i = 0; i < 100; i++)
                {
                }
                Console.WriteLine("AsyncMethooooood----1 end.");
            });
        }
        async static Task AsyncMethod0()
        {
            await Task.Run(async () =>
            {
                for (int i = 0; i < 100; i++)
                {
                    Thread.Sleep(100);
                }
                Console.WriteLine("AsyncMethooooood----000000000 end.");
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