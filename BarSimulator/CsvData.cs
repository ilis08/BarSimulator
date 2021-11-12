using LumenWorks.Framework.IO.Csv;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarSimulator
{
    class CsvData
    {
        public List<string> data = new();

        public CsvData()
        {

        }

        public List<string> LoadString(string fileName)
        {
            var csvTable = new DataTable();

            try
            {
                using (var csvReader = new CsvReader(new StreamReader(File.OpenRead(@$"C:\IzbiraemaParallelProgrammingC#\Hometasks\FourthHomeTask\BarSimulator\Files\{fileName}.csv")), true))
                {
                    csvTable.Load(csvReader);
                }

                for (int i = 0; i < csvTable.Rows.Count; i++)
                {
                    data.Add(csvTable.Rows[i][0].ToString());
                }

                return data;
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                return default;
            }
        }
    }
}
