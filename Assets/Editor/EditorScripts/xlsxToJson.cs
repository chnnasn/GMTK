using UnityEngine;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEditor;

public class xlsxToJson : MonoBehaviour
{
    string jsonName;
    private void Start()
    {
    }



    private static void StatJson() // 将方法改为静态
    {
        string excelPath1 = Path.Combine(Application.dataPath, "Editor", "ArtRes", "ExcelAndJson", "levels.xlsx");
        string jsonPath1 = Path.Combine(Application.streamingAssetsPath, "levels.json");

        if (File.Exists(excelPath1))
        {
            List<TableData> tableData = ReadExcelFile(excelPath1);
            SaveToJson(tableData, jsonPath1);
        }
    }

    private static List<TableData> ReadExcelFile(string filePath)
    {
        List<TableData> tableDataList = new List<TableData>();

        FileInfo file = new FileInfo(filePath);
        using (ExcelPackage package = new ExcelPackage(file))
        {
            foreach (var worksheet in package.Workbook.Worksheets)
            {
                List<Sample> samples = new List<Sample>();

                //int rowCount = worksheet.Dimension.End.Row;
                int colCount = worksheet.Dimension.End.Column;
                List<int> start1 = new List<int> {ConvertToInt32(worksheet.Cells[1, colCount - 1].Value),ConvertToInt32(worksheet.Cells[2, colCount - 1].Value), ConvertToInt32(worksheet.Cells[1, colCount].Value), ConvertToInt32(worksheet.Cells[2, colCount].Value) };

                for (int row = 1; row < colCount-1; row++) // Iterate through all rows
                {
                    Sample sample = new Sample
                    {
                        po = new List<int>(),
                        value = new List<int>()
                    };

                    // Read the first three columns
                    for (int col = 1; col <= 3; col++)
                    {
                        sample.po.Add(ConvertToInt32(worksheet.Cells[col, row].Value));
                    }

                    // Read the last four columns
                    for (int col = 4; col <= 7; col++)
                    {
                        sample.value.Add(ConvertToInt32(worksheet.Cells[col, row].Value));
                    }
                    samples.Add(sample);
                }

                TableData tableData = new TableData
                {
                    work = worksheet.Name,
                    sampleList = new SampleList { samples = samples },
                    startAndEnd = start1,
                };

                tableDataList.Add(tableData);
            }
            //ExcelWorksheet worksheet = package.Workbook.Worksheets[1]; // Assuming data is in the first sheet
;

        }

        return tableDataList;
    }

    private static void SaveToJson(List<TableData> tableData, string filePath) // 将方法改为静态
    {
        TableDataList tableDataListWrapper = new TableDataList { tables = tableData };

        string json = JsonUtility.ToJson(tableDataListWrapper, true);

        File.WriteAllText(filePath, json);
        Debug.Log("JSON file saved to: " + filePath);
    }

    private static int ConvertToInt32(object value)
    {
        if (value == null)
        {
            return 0; // Return a default value for null
        }

        if (value is int)
        {
            return (int)value;
        }
        else if (value is double)
        {
            return (int)(double)value;
        }
        else if (value is string)
        {
            if (int.TryParse((string)value, out int result))
            {
                return result;
            }
            else
            {
                throw new InvalidCastException($"String value '{value}' cannot be converted to int.");
            }
        }
        else
        {
            throw new InvalidCastException($"Value of type '{value.GetType()}' cannot be converted to int.");
        }
    }

    public class CreatSampleExcelAsset : Editor
    {
        // 在Editor界面添加一个MenuItem来执行上面的代码
        [MenuItem("BuildJson/Build ExcelJson")]
        public static void BuildExcelSampleAsset()
        {
            StatJson(); // 调用静态方法
            // 刷新Assets
            AssetDatabase.Refresh();
        }
    }
}
