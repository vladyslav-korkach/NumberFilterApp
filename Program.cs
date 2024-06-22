using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "/app/input.xlsx";
            string outputPath = "/app/output/output.xlsx";
            
            List<int> numbers = LoadNumbersFromExcel(inputPath);
            List<int> filteredNumbers = FilterNumbers(numbers);
            SaveNumbersToExcel(filteredNumbers, outputPath);

            Console.WriteLine("Processing complete. Check the output file.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
        }
    }

    static List<int> LoadNumbersFromExcel(string filePath)
    {
        FileInfo fileInfo = new FileInfo(filePath);

        using (ExcelPackage package = new ExcelPackage(fileInfo))
        {
            // Check if the workbook has any worksheets
            if (package.Workbook.Worksheets.Count == 0)
            {
                throw new InvalidOperationException("The workbook does not contain any worksheets.");
            }

            // Access the first worksheet
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

            List<int> numbers = new List<int>();

            // Assuming numbers are in the first column
            int row = 1;
            while (worksheet.Cells[row, 1].Value != null)
            {
                if (int.TryParse(worksheet.Cells[row, 1].Value.ToString(), out int number))
                {
                    numbers.Add(number);
                }
                row++;
            }

            return numbers;
        }
    }

    static List<int> FilterNumbers(List<int> numbers)
    {
        List<int> filteredNumbers = new List<int>();
        if (numbers.Count == 0) return filteredNumbers;

        int previousNumber = numbers[0];
        filteredNumbers.Add(previousNumber);

        for (int i = 1; i < numbers.Count; i++)
        {
            if (numbers[i] - previousNumber >= 100000)
            {
                previousNumber = numbers[i];
                filteredNumbers.Add(previousNumber);
            }
        }

        return filteredNumbers;
    }

    static void SaveNumbersToExcel(List<int> numbers, string filePath)
    {
        FileInfo fileInfo = new FileInfo(filePath);

        using (ExcelPackage package = new ExcelPackage())
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("FilteredNumbers");

            for (int i = 0; i < numbers.Count; i++)
            {
                worksheet.Cells[i + 1, 1].Value = numbers[i];
            }

            package.SaveAs(fileInfo);
        }
    }
}