namespace ClientTest
{
    public partial class CreClients1 : Form
    {
        public CreClients1()
        {
            InitializeComponent();

        }
        private void button1_Click(object sender, EventArgs e)
        {
            Int32.TryParse(Clientquantity_Box.Text, out int a);
            Int32.TryParse(ReqQuanBox.Text, out int b);
            var form = new Form2(a, b);
            form.Show();
        }
    }
}