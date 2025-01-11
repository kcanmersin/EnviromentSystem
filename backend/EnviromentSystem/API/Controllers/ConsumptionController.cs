using Core.Service.Extract;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ConsumptionController : ControllerBase
{
    private readonly IConsumptionService _consumptionService;

    public ConsumptionController(IConsumptionService consumptionService)
    {
        _consumptionService = consumptionService;
    }

    [HttpGet("export")]
    public async Task<IActionResult> ExportConsumptionData(
        [FromQuery] string consumptionType,
        [FromQuery] bool includeGraphs = false)
    {
        var consumptionData = await _consumptionService.GetConsumptionDataAsync(consumptionType);

        if (consumptionData == null || !consumptionData.Any())
        {
            return NotFound("No consumption data found.");
        }

        var excelBytes = GenerateExcel(consumptionData, consumptionType, includeGraphs);

        var fileName = $"{consumptionType}_ConsumptionData_{DateTime.Now:yyyyMMddHHmmss}.xlsx";

        return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
    }

    private byte[] GenerateExcel(IEnumerable<ConsumptionDataDto> data, string consumptionType, bool includeGraphs)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using (var package = new ExcelPackage())
        {
            if (consumptionType.Equals("Water", StringComparison.OrdinalIgnoreCase))
            {
                var allSheet = package.Workbook.Worksheets.Add("All");

                allSheet.Cells[1, 1].Value = "ID";
                allSheet.Cells[1, 2].Value = "Date";
                allSheet.Cells[1, 3].Value = "Initial Meter Value";
                allSheet.Cells[1, 4].Value = "Final Meter Value";
                allSheet.Cells[1, 5].Value = "Usage";

                var sortedData = data.OrderBy(d => d.Date).ToList();

                int row = 2;
                foreach (var item in sortedData)
                {
                    allSheet.Cells[row, 1].Value = item.Id;
                    allSheet.Cells[row, 2].Value = item.Date.ToString("yyyy-MM-dd");
                    allSheet.Cells[row, 3].Value = item.InitialMeterValue;
                    allSheet.Cells[row, 4].Value = item.FinalMeterValue;
                    allSheet.Cells[row, 5].Value = item.Usage;
                    row++;
                }

                if (includeGraphs && row > 2)
                {
                    string xRange = $"B2:B{row - 1}";
                    string yRange = $"E2:E{row - 1}";
                    AddChart(allSheet, xRange, yRange, "Usage Over Time");
                }

                return package.GetAsByteArray();
            }
            else if (consumptionType.Equals("Paper", StringComparison.OrdinalIgnoreCase))
            {
                var allSheet = package.Workbook.Worksheets.Add("Yearly");

                allSheet.Cells[1, 1].Value = "Year";
                allSheet.Cells[1, 2].Value = "Total Usage";

                var groupedData = data
                    .GroupBy(d => d.Date.Year)
                    .Select(g => new
                    {
                        Year = g.Key,
                        TotalUsage = g.Sum(d => d.Usage)
                    })
                    .OrderBy(g => g.Year)
                    .ToList();

                int row = 2;
                foreach (var item in groupedData)
                {
                    allSheet.Cells[row, 1].Value = item.Year;
                    allSheet.Cells[row, 2].Value = item.TotalUsage;
                    row++;
                }

                if (includeGraphs && row > 2)
                {
                    string xRange = $"A2:A{row - 1}";
                    string yRange = $"B2:B{row - 1}";
                    AddChart(allSheet, xRange, yRange, "Total Usage Per Year", isYear: true);
                }

                return package.GetAsByteArray();
            }

            var allSheetStandard = package.Workbook.Worksheets.Add("All");
            var groupedDataStandard = data
                .GroupBy(d => d.Date.ToString("yyyy-MM"))
                .Select(g => new
                {
                    Period = g.Key,
                    TotalUsage = g.Sum(d => d.Usage),
                    TotalKWH = consumptionType.Equals("Electric", StringComparison.OrdinalIgnoreCase) ? g.Sum(d => d.KWHValue) : 0,
                    TotalSM3 = consumptionType.Equals("NaturalGas", StringComparison.OrdinalIgnoreCase) ? g.Sum(d => d.SM3Value) : 0
                })
                .OrderBy(g => g.Period)
                .ToList();

            allSheetStandard.Cells[1, 1].Value = "Period";
            allSheetStandard.Cells[1, 2].Value = "Total Usage";
            if (consumptionType.Equals("Electric", StringComparison.OrdinalIgnoreCase))
                allSheetStandard.Cells[1, 3].Value = "Total KWH";
            if (consumptionType.Equals("NaturalGas", StringComparison.OrdinalIgnoreCase))
                allSheetStandard.Cells[1, 3].Value = "Total SM3";

            int rowStandard = 2;
            foreach (var item in groupedDataStandard)
            {
                allSheetStandard.Cells[rowStandard, 1].Value = item.Period;
                allSheetStandard.Cells[rowStandard, 2].Value = item.TotalUsage;
                if (consumptionType.Equals("Electric", StringComparison.OrdinalIgnoreCase))
                    allSheetStandard.Cells[rowStandard, 3].Value = item.TotalKWH;
                if (consumptionType.Equals("NaturalGas", StringComparison.OrdinalIgnoreCase))
                    allSheetStandard.Cells[rowStandard, 3].Value = item.TotalSM3;
                rowStandard++;
            }

            if (includeGraphs && rowStandard > 2)
            {
                string xRange = $"A2:A{rowStandard - 1}";
                string yRange = $"B2:B{rowStandard - 1}";
                AddChart(allSheetStandard, xRange, yRange, "Total Usage Over Period");
            }

            var buildingGroups = data.GroupBy(d => d.BuildingName);

            foreach (var group in buildingGroups)
            {
                var sanitizedSheetName = CleanSheetName(group.Key);
                var sheet = package.Workbook.Worksheets.Add(sanitizedSheetName);

                sheet.Cells[1, 1].Value = "ID";
                sheet.Cells[1, 2].Value = "Date";
                sheet.Cells[1, 3].Value = "Initial Meter Value";
                sheet.Cells[1, 4].Value = "Final Meter Value";
                sheet.Cells[1, 5].Value = "Usage";
                if (consumptionType.Equals("Electric", StringComparison.OrdinalIgnoreCase)) sheet.Cells[1, 6].Value = "KWH Value";
                if (consumptionType.Equals("NaturalGas", StringComparison.OrdinalIgnoreCase)) sheet.Cells[1, 6].Value = "SM3 Value";

                var sortedGroupData = group.OrderBy(d => d.Date).ToList();

                int rowBuilding = 2;
                foreach (var item in sortedGroupData)
                {
                    sheet.Cells[rowBuilding, 1].Value = item.Id;
                    sheet.Cells[rowBuilding, 2].Value = item.Date.ToString("yyyy-MM-dd");
                    sheet.Cells[rowBuilding, 3].Value = item.InitialMeterValue;
                    sheet.Cells[rowBuilding, 4].Value = item.FinalMeterValue;
                    sheet.Cells[rowBuilding, 5].Value = item.Usage;
                    if (consumptionType.Equals("Electric", StringComparison.OrdinalIgnoreCase)) sheet.Cells[rowBuilding, 6].Value = item.KWHValue;
                    if (consumptionType.Equals("NaturalGas", StringComparison.OrdinalIgnoreCase)) sheet.Cells[rowBuilding, 6].Value = item.SM3Value;
                    rowBuilding++;
                }

                if (includeGraphs && rowBuilding > 2)
                {
                    string xRange = $"B2:B{rowBuilding - 1}";
                    string yRange = $"E2:E{rowBuilding - 1}";
                    string chartTitle = $"Usage Over Time - {group.Key}";
                    AddChart(sheet, xRange, yRange, chartTitle);
                }
            }

            return package.GetAsByteArray();
        }
    }

    private void AddChart(ExcelWorksheet sheet, string xRange, string yRange, string chartTitle, bool isYear = false)
    {
        var cells = sheet.Cells[yRange];
        int totalRows = cells.End.Row;
        int totalCols = cells.End.Column;

        var chart = sheet.Drawings.AddChart("UsageChart_" + chartTitle.Replace(" ", ""), eChartType.Line) as ExcelLineChart;
        chart.Title.Text = chartTitle;
        chart.SetPosition(totalRows + 2, 0, 0, 0);
        chart.SetSize(800, 400);

        chart.Series.Add(sheet.Cells[yRange], sheet.Cells[xRange]);
        chart.XAxis.Title.Text = isYear ? "Year" : "Date";
        chart.YAxis.Title.Text = "Usage";

        if (isYear)
        {
            chart.XAxis.Format = "0"; 
        }
        else
        {
            var xAxis = chart.XAxis as ExcelChartAxisStandard;
            xAxis.Format = "yyyy-MM-dd";
            xAxis.MajorTickMark = eAxisTickMark.None;
            xAxis.MinorTickMark = eAxisTickMark.None;
        }

        chart.Legend.Position = eLegendPosition.Bottom;
    }

    private string CleanSheetName(string sheetName)
    {
        if (string.IsNullOrWhiteSpace(sheetName))
            return "Unnamed";

        var invalidChars = new[] { '\\', '/', '*', '[', ']', ':', '?' };
        foreach (var c in invalidChars)
        {
            sheetName = sheetName.Replace(c, '_');
        }

        sheetName = sheetName.Trim('\'');

        return sheetName.Length > 31 ? sheetName.Substring(0, 31) : sheetName;
    }
}
