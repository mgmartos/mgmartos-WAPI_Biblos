using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
//using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;




namespace WAPI_Biblos1
{
    public class Program
    {
        public static void Main(string[] args)
        {

            
            //WAPI_Biblos1.Utils.Importaciones import = new Utils.Importaciones();
           // import.TraspasoTemas();
            
            
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });




        // public static void Traspaso_Autores()
        //    {

        //    OldAuthor autor = new OldAuthor()
        //    {
        //        id = "9999",
        //        idAutor = 3999,
        //        NombreAutor = "Autor Ansurez"
        //    };

        //    string fileName = "D:\\Pruebas\\WAPI_Biblos1\\autores.json";
        //    string jsonString = File.ReadAllText(fileName);

        //    jsonString = jsonString.Replace('[', ' ');
        //    jsonString = jsonString.Replace(']', ' ');
        //    jsonString = jsonString.Replace("\r\n", "");
        //    jsonString = jsonString.Replace("},{", "};{");


        //    List<string> jsonAutores = jsonString.Trim().Split(';').ToList();
        //    List<OldAuthor> ListaAutores = new List<OldAuthor>();
        //    foreach(string item in jsonAutores) 
        //        {
        //        OldAuthor itemAutor = Newtonsoft.Json.JsonConvert.DeserializeObject<OldAuthor>(item);
        //        System.Diagnostics.Debug.WriteLine(itemAutor.NombreAutor);
        //        ListaAutores.Add(itemAutor);
          
        //    }
        //    //System.Diagnostics.Debug.WriteLine(ListaAutores.ToString());




        //    //string jsonString2 = "{\"id\": \"1\",\"idAutor\": \"3602\",\"NombreAutor\": \"A. THORKENT\"}";

        //    //    //"[{\"id\": \"1\",\"idAutor\": \"3602\",\"NombreAutor\": \"A. THORKENT \"},{\"id\": \"2\",\"idAutor\": \"3603\",\"NombreAutor\": \"AGATHA CHRISTIE \"}]";

        //    ////var autores = JsonSerializer.Deserialize<OldAuthor>(jsonString2);

        //    //var autores = Newtonsoft.Json.JsonConvert.DeserializeObject<OldAuthor>(jsonString2);

        //    //Console.WriteLine(autores.ToString());

        //}   
    }





    /*
     * jsonString = File.ReadAllText(fileName);
    weatherForecast = JsonSerializer.Deserialize<WeatherForecast>(jsonString);
    Para deserializar a partir de un archivo mediante código asincrónico, llame al método DeserializeAsync:
    C#

    Copiar
    using FileStream openStream = File.OpenRead(fileName);
    weatherForecast = await JsonSerializer.DeserializeAsync<WeatherForecast>(openStream);
     */

    //public class OldAuthor
    //{
    //    public string id{ get; set; }
    //    public int idAutor{ get; set; }
    //    public string NombreAutor { get; set; }
    //}

}
