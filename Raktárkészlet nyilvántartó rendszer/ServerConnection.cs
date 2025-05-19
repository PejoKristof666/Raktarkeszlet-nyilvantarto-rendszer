using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;

namespace Raktárkészlet_nyilvántartó_rendszer
{
    internal class ServerConnection
    {
        public string BaseUrl = "http://localhost:3000";
        HttpClient client = new HttpClient();
        public ServerConnection() 
        {

        }
        public async Task<bool> CreateProduct(string name, string type, int price)
        {
            string url = BaseUrl + "/productcreate";

            try
            {
                var jsonInfo = new
                {
                    createProductName = name,
                    createProductType = type,
                    createProductPrice = price
                };

                string jsonString = JsonConvert.SerializeObject(jsonInfo);
                HttpContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();

                string result = await response.Content.ReadAsStringAsync();
                JsonData responseData = JsonConvert.DeserializeObject<JsonData>(result);

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Hiba a termék létrehozásakor: " + e.Message);
                return false;
            }
        }

        public async Task<List<JsonData>> GetAllProducts()
        {
            string url = BaseUrl + "/products";
            List<JsonData> products = new List<JsonData>();

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string result = await response.Content.ReadAsStringAsync();
                products = JsonConvert.DeserializeObject<List<JsonData>>(result);
            }
            catch (Exception e)
            {
                Console.WriteLine("Hiba a termékek lekérdezésekor: " + e.Message);
            }

            return products;
        }

        public async Task<JsonData> GetProductById(string id)
        {
            string url = BaseUrl + "/productsid";

            try
            {
                var jsonInfo = new { id = id };
                string jsonString = JsonConvert.SerializeObject(jsonInfo);
                HttpContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();

                string result = await response.Content.ReadAsStringAsync();
                JsonData product = JsonConvert.DeserializeObject<JsonData>(result);

                return product;
            }
            catch (Exception e)
            {
                Console.WriteLine("Hiba a termék lekérdezésekor: " + e.Message);
                return null;
            }
        }
    }
}

