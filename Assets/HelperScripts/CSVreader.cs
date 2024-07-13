using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CSVReader 
{


    public List<Dictionary<string, string>> LoadCSV(string _fileName)
    {
        TextAsset csvFile = Resources.Load<TextAsset>(_fileName);
        if (csvFile == null)
        {
            Debug.LogError("CSV file not found!");
            return null;
        }

        List<Dictionary<string, string>> data = new List<Dictionary<string, string>>();
        StringReader reader = new StringReader(csvFile.text);

        string[] headers = null;
        bool isFirstLine = true;

        while (true)
        {
            string line = reader.ReadLine();
            if (line == null) break;

            string[] values = line.Split(',');

            if (isFirstLine)
            {
                headers = values;
                isFirstLine = false;
            }
            else
            {
                Dictionary<string, string> row = new Dictionary<string, string>();
                for (int i = 0; i < headers.Length; i++)
                {
                    row[headers[i]] = values[i];
                }
                data.Add(row);
            }
        }

        return data;
    }
}
