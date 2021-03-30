using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;

namespace DynamicCurrenciesConverter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            currencyList();
        }

        public void currencyList()
        {
            APIRequester currencyListRequest = new APIRequester("https://free.currconv.com/api/v7/currencies?apiKey=7d15744edeb3d3d38667");
            CurrencyList currencyList = CurrencyList.Deserialize(currencyListRequest.SendAndGetResponse());

            CurrencyData[] datas = currencyList.ToArray();
            foreach (CurrencyData currency in datas)
            {
                comboBox1.Items.Add(currency.id);
                comboBox2.Items.Add(currency.id);
            }
        }

        public static double Exchange(string from, string to, string date)
        {
            string url;
            url = "https://free.currencyconverterapi.com/api/v6/" + "convert?q=" + from + "_" + to + "&compact=y&date=" + date + "&apiKey=6f8ea18f7f98fa33e29a";
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            string jsonString;
            using (var response = (HttpWebResponse)request.GetResponse())
            using (var stream = response.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                jsonString = reader.ReadToEnd();
            }

            return JObject.Parse(jsonString).First.First["val"].First.ToObject<double>();
        }
        

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                textBox2.Text = "Masukkan Nilai Asal";
            }
            else
            {
                if (string.IsNullOrEmpty(comboBox1.Text) || string.IsNullOrEmpty(comboBox2.Text))
                {
                    textBox2.Text = "Pilih Mata Uang Asal dan Tujuan";
                }
                else
                {
                    double amount = Convert.ToDouble(textBox1.Text);
                    double rate = Exchange(comboBox1.Text, comboBox2.Text, dateTimePicker1.Value.Date.ToString("yyyy-MM-dd"));
                    amount = amount * rate;

                    textBox2.Text = Convert.ToString(amount);
                }
            }
        }
    }
}
