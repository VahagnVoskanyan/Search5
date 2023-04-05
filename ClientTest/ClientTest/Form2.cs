using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace ClientTest
{
    public partial class Form2 : Form
    {
        private readonly int _clientsQuantity;
        private readonly int _requestQuantity;

        public Form2(int clientsQuantity, int requestQuantity)
        {
            InitializeComponent();
            _clientsQuantity = clientsQuantity;
            _requestQuantity = requestQuantity;
            Start1();
        }

        private void Start1()
        {
            Thread[] threads = new Thread[_clientsQuantity];

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(new ParameterizedThreadStart(ThreadMethod));
            }
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i].Start(i);
            }
        }

        private async void ThreadMethod(object? number)
        {
            HttpClient client = new HttpClient();

            int a = (int)number;

            for (int i = 0; i <= _requestQuantity; i++)
            {
                //Thread.Sleep(100);
                var result = await client.GetStringAsync("http://localhost:31577/api/s/customers/Name/Tigran");
                //Console.WriteLine(result);

                textBoxes[a].Text = i.ToString();
            }
        }

    }
}
