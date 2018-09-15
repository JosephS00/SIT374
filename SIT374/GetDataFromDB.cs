using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace DataApi
{
    /// <summary>
    /// this class contains the function of loading csv file into program
    /// </summary>
    public class GetDataFromDB
    {
        /// <summary>
        /// read a csv file, translate it into json data, and encode it by json
        /// </summary>
        /// <param name="csvPath">path of csv file</param>
        /// <returns>a JArray object containing all data in csv file, every JObject in JArray contains data in one csv line</returns>
        public static JArray ReadCsvFile(string csvPath)
        {
            List<string> jsonList = new List<string>();
            //read data from csv file
            var csv = new List<string[]>();
            var lines = System.IO.File.ReadAllLines(csvPath);
            var dict = new Dictionary<string, string>();
            foreach (string line in lines)
            {
                csv.Add(line.Split(','));
                //Console.WriteLine(String.Join(",", line.Split(',')));

            }
            string json = "";
            for (int j = 1; j < csv.Count(); j++)
            {
                for (int i = 0; i < csv[0].Count(); i++)
                {
                    dict.Add(csv[0][i], csv[j][i]);

                }
                string newJson = MyDictionaryToJson(dict);
                json += newJson;
                jsonList.Add(newJson);
                json += ',';
                dict.Clear();
            }

            //encode every json string into a encoded JObject
            JArray jArray = new JArray();
            foreach(string jsonString in jsonList)
            {
                JObject jObject = JObject.Parse(jsonString);
                jArray.Add(jObject);
            }

            //out put json into file for test
            //StreamWriter writer = new StreamWriter("result.txt");
            //foreach (JObject x in jArray)
            //{
            //    writer.WriteLine(x["Accelaration Pedal"]);
            //    writer.WriteLine(x["Power"]);
            //    writer.WriteLine(x["Weight"]);
            //    writer.WriteLine(json);
            //}
            //writer.WriteLine(jArray[2]["Torque"]);
            //writer.Close();

            return jArray;
        }
        
        private static string MyDictionaryToJson(Dictionary<string, string> dict)
        {
            var entries = dict.Select(d => string.Format("\"{0}\": \"{1}\"", d.Key, d.Value));
            return "{" + string.Join(",", entries) + "}";
        }
    }
}
