using Newtonsoft.Json;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CustomerConsumer
{
    class Program
    {
        static HttpClient client = new HttpClient();
        static Elephants elp = new Elephants();
        static string  uri = "https://localhost:44367/Elephant";
        static void Main(string[] args)
        {
            try
            {
                RunAsync();
                Console.ReadKey(); 
            }
            catch(Exception ex)
            {


            }
        }
        static async void RunAsync()
        {
            client.BaseAddress =  new Uri(uri);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Get all data from resources 
           IList<Elephants> getAllList = await  GetAllElephantsAsync();
           ShowObjectData(getAllList);
           Console.WriteLine("Get By Id ...............................");
            var  getAllListById = await GetAllElephantsById("1");
             ShowSingleObjectData(getAllListById);
             Console.WriteLine("Add New Resource ...............................");
             Elephants newElephant = new Elephants() { Id = 33, Name = "poly", Species = "South-African", Weight = 10000};
             var newResource = await AddNewResource(newElephant);
             ShowSingleObjectData(newResource);

        }
        static async Task<IList<Elephants>> GetAllElephantsAsync()
        {
            string jsonContent = await client.GetStringAsync(uri);
            IList<Elephants> elps = JsonConvert.DeserializeObject<IList<Elephants>>(jsonContent);
            return elps;
        }

        static async Task<Elephants> GetAllElephantsById(string id)
        {
            string uriId = uri + "/"+ id;
            HttpResponseMessage response = await client.GetAsync(uriId);
            if(response.StatusCode != HttpStatusCode.NotFound)
            {
                response.EnsureSuccessStatusCode();
                string jsonContent = await response.Content.ReadAsStringAsync();
                var singleObj = JsonConvert.DeserializeObject<Elephants>(jsonContent);
                return singleObj;
            }
            else
            {
                throw new Exception("Customer Id is not found");
            }
        }
        static async Task<Elephants> AddNewResource(Elephants newElps)
        {
            var jsonContent = JsonConvert.SerializeObject(newElps);
            StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(uri, content);
            if(response.StatusCode != HttpStatusCode.Conflict)
            {
                response.EnsureSuccessStatusCode();
                string jsonString = await response.Content.ReadAsStringAsync();
                var newlyCreatedResource = JsonConvert.DeserializeObject<Elephants>(jsonString);
                return newlyCreatedResource;
            }
            else
            {
                throw new Exception("Customer already exist, try anther Id");
            }
        }
        static async Task<Elephants> UpdateResource(Elephants elps , string id)
        {
            var jsonContent = JsonConvert.SerializeObject(elps);
            StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(uri+"/"+id, content);
            if (response.StatusCode != HttpStatusCode.Conflict)
            {
                response.EnsureSuccessStatusCode();
                string jsonString = await response.Content.ReadAsStringAsync();
                var newlyCreatedResource = JsonConvert.DeserializeObject<Elephants>(jsonString);
                return newlyCreatedResource;
            }
            else
            {
                throw new Exception("Customer already exist, try anther Id");
            }
        }
        static async void DeleteToDoItemAsync(string id)
        {
            string uriId = uri + "/" + id;
            HttpResponseMessage response = await client.DeleteAsync(uriId);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new Exception("Customer not found with that Id. Try another id");
            }
            response.EnsureSuccessStatusCode();
        }

        static void ShowObjectData(IList<Elephants> list)
        {
            foreach(var i in list)
            {
                Console.WriteLine(i.Id + ":" + i.Name + ":" + i.Species);
            }
        }
        static void ShowSingleObjectData(Elephants elps)
        {
            Console.WriteLine(elps.Id + ":" + elps.Name + ":" + elps.Species);            
        }

    }
}
