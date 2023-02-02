using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace JBC.Coletor.Helper
{
    class ApiHelper
    {
        public static string GetArquivo(string ip, string porta, string caminho)
        {
            string result = string.Empty;

            try
            {
                Task<string> t = HttpGetResponse(ip, porta, caminho);

                t.Wait();
                result = t.Result;
            }
            catch (AggregateException ex)
            {
                throw ex.InnerExceptions.First().InnerException;
            }

            return result;
        }

        static async Task<string> HttpGetResponse(string ip, string porta, string caminho)
        {
            string url = string.Format("http://{0}:{1}/arquivo?caminho={2}", ip, porta, caminho.Replace(@"\", "%5C"));

            //string url = string.Format("http://{0}:{1}/arquivo?caminho={2}", ip, porta, caminho);

            //url = HttpUtility.UrlEncode(url);

            WebRequest request = WebRequest.Create(url);

            string responseData;
            //Stream objStream = request.GetResponse().GetResponseStream();
            //StreamReader objReader = new StreamReader(objStream);
            //string sLine = "";
            //int i = 0;
            //while (sLine != null)
            //{
            //    i++;
            //    sLine = objReader.ReadLine();
            //    if (sLine != null)
            //        Console.WriteLine(sLine);
            //}

                using (var client = new HttpClient())
                {
                    using (var response = await client.GetAsync(url))
                    {
                        responseData = await response.Content.ReadAsStringAsync();
                        //Console.WriteLine(responseData);
                    }
                }
            

            return responseData;
        }
    }           
}
