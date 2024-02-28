using System;
using System.IO;
using System.Linq;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using SPU_7.Models.Scripts.Operations.Results;
using SPU_7.Models.Services.StandSetting;

namespace SPU_7.Models.Scripts.Operations.Protocols;

public class ValidationProtocolCreator
{
    public int CreateProtocol(ValidationOperationResult validationOperationResult, 
        int deviceNumber, 
        IStandSettingsService standSettingsService,
        float? temperature,
        float? pressure,
        float? humidity)
    {
        try
        {
            var protocolNumber = GetProtocolNumber();

            var protocolNumberString = protocolNumber.ToString();
            while (protocolNumberString.Length < 6) protocolNumberString = protocolNumberString.Insert(0, "0");

            var protocolsDirectory = "Protocols";
            var path = $"{Directory.GetCurrentDirectory()}/{protocolsDirectory}/Протокол поверки №{protocolNumberString}.pdf";
            var dir = Path.GetDirectoryName(path);
            if (dir != null && !Directory.Exists(dir)) Directory.CreateDirectory(dir);

            var pdfDoc = new PdfDocument(new PdfWriter($"{Directory.GetCurrentDirectory()}/Protocols/Протокол поверки №{protocolNumberString}.pdf"));

            var doc = new Document(pdfDoc);
            doc.SetFont(PdfFontFactory.CreateFont($"{Directory.GetCurrentDirectory()}/Assets/Fonts/times.ttf"));

            var headerTable = new Table(UnitValue.CreatePercentArray(1))
            .UseAllAvailableWidth();

        var headerStr1 = new Paragraph(standSettingsService.StandSettingsModel.ValidationVendorType)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetBold()
            .SetMultipliedLeading(1f)
            .SetFontSize(12);

        var headerStr2 =
            new Paragraph(standSettingsService.StandSettingsModel.ValidationVendorName)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetBold()
                .SetMultipliedLeading(1f)
                .SetFontSize(12);

        var headerStr3 = new Paragraph(standSettingsService.StandSettingsModel.ValidationVendorShortName)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetBold()
            .SetMultipliedLeading(1f)
            .SetFontSize(12);

        var headerStr4 = new Paragraph(standSettingsService.StandSettingsModel.ValidationVendorUniqueNumber)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetBold()
            .SetMultipliedLeading(1f)
            .SetFontSize(12);

        var headerStr5 = new Paragraph(standSettingsService.StandSettingsModel.ValidationVendorAddress)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetMultipliedLeading(1f)
            .SetFontSize(12);

        var headerCell = new Cell()
            .SetBorder(Border.NO_BORDER)
            .Add(headerStr1)
            .Add(headerStr2)
            .Add(headerStr3)
            .Add(headerStr4)
            .Add(headerStr5);

        headerTable.AddCell(headerCell);

        doc.Add(headerTable);
        
        var protocolNumberParagraph = new Paragraph()
            .SetTextAlignment(TextAlignment.CENTER)
            .SetBold()
            .SetFontSize(12)
            .Add(new Text("ПРОТОКОЛ ПОВЕРКИ №"))
            .Add(new Text(protocolNumberString)
                .SetUnderline());
        doc.Add(protocolNumberParagraph);

        var validationInfoTable = new Table(UnitValue.CreatePercentArray(1))
            .UseAllAvailableWidth();

        var validationInfoCell = new Cell()
            .SetBorder(Border.NO_BORDER);
        
        var deviceParagraph = new Paragraph()
            .SetFontSize(12)
            .SetMultipliedLeading(1f)
            .Add(new Text("Наименование, тип, разряд СИ: ")
                .SetBold())
            .Add(new Text(standSettingsService.StandSettingsModel.DeviceInfo)
                .SetUnderline());
        validationInfoCell.Add(deviceParagraph);
        
        var vendorNumberParagraph = new Paragraph()
            .SetFontSize(12)
            .SetMultipliedLeading(1f)
            .Add(new Text("Заводской №: ")
                .SetBold())
            .Add(new Text(validationOperationResult.ValidationPointResults.First().ValidationMeasureResults.First().ValidationDeviceResults[deviceNumber].VendorNumber)
                .SetUnderline());
        validationInfoCell.Add(vendorNumberParagraph);
        
        var vendorNameParagraph = new Paragraph()
            .SetFontSize(12)
            .SetMultipliedLeading(1f)
            .Add(new Text("Предприятие-изготовитель: ")
                .SetBold())
            .Add(new Text(standSettingsService.StandSettingsModel.VendorName)
                .SetUnderline());
        validationInfoCell.Add(vendorNameParagraph);
        
        var dateCreateParagraph = new Paragraph()
            .SetFontSize(12)
            .SetMultipliedLeading(1f)
            .Add(new Text("Дата производства: ")
                .SetBold())
            .Add(new Text(standSettingsService.StandSettingsModel.VendorDate)
                .SetUnderline());
        validationInfoCell.Add(dateCreateParagraph);
        
        var measureRangeParagraph = new Paragraph()
            .SetFontSize(12)
            .SetMultipliedLeading(1f)
            .Add(new Text("Диапазон измерений: ")
                .SetBold())
            .Add(new Text(standSettingsService.StandSettingsModel.DeviceRangeInfo)
                .SetUnderline());
        validationInfoCell.Add(measureRangeParagraph);
        
        var belongerNameParagraph = new Paragraph()
            .SetFontSize(12)
            .SetMultipliedLeading(1f)
            .Add(new Text("Принадлежит: ")
                .SetBold())
            .Add(new Text(standSettingsService.StandSettingsModel.OwnerName)
                .SetUnderline());
        validationInfoCell.Add(belongerNameParagraph);
        
        var previousValidationInfoParagraph = new Paragraph()
            .SetFontSize(12)
            .SetMultipliedLeading(1f)
            .Add(new Text("Серия и номер предыдущей поверки: ")
                .SetBold())
            .Add(new Text(standSettingsService.StandSettingsModel.LastValidationInfo)
                .SetUnderline());
        validationInfoCell.Add(previousValidationInfoParagraph);
        
        var deviceValidationInfoParagraph = new Paragraph()
            .SetFontSize(12)
            .SetMultipliedLeading(1f)
            .Add(new Text("Для поверки использовались следующие средства измерений: ")
                .SetBold())
            .Add(new Text(standSettingsService.StandSettingsModel.ValidationDeviceInfo)
                .SetUnderline());
        validationInfoCell.Add(deviceValidationInfoParagraph);
        
        var validationMethodInfoParagraph = new Paragraph()
            .SetFontSize(12)
            .SetMultipliedLeading(1f)
            .Add(new Text("Методика поверки: ")
                .SetBold())
            .Add(new Text(standSettingsService.StandSettingsModel.ValidationMethod)
                .SetUnderline());
        validationInfoCell.Add(validationMethodInfoParagraph);
        
        var tightnessInfoParagraph = new Paragraph()
            .SetFontSize(12)
            .SetMultipliedLeading(1f)
            .Add(new Text("Проверка на герметичность: ")
                .SetBold())
            .Add(new Text(standSettingsService.StandSettingsModel.TightnessInfo)
                .SetUnderline());
        validationInfoCell.Add(tightnessInfoParagraph);

        validationInfoCell.Add(new Paragraph("Условия поверки:")
            .SetBold()
            .SetFontSize(12)
            .SetMultipliedLeading(1f));
        
        var temperatureParagraph = new Paragraph()
            .SetFontSize(12)
            .SetMultipliedLeading(1f)
            .Add(new Text("Температура окружающей среды, \u00b0С ")
                .SetBold())
            .Add(new Text(temperature.ToString())
                .SetUnderline());
        validationInfoCell.Add(temperatureParagraph);


        var humidityParagraph = new Paragraph()
            .SetFontSize(12)
            .SetMultipliedLeading(1f)
            .Add(new Text("Относительная влажность воздуха, % ")
                .SetBold())
            .Add(new Text(humidity.ToString())
                .SetUnderline());
        validationInfoCell.Add(humidityParagraph);

        var pressureParagraph = new Paragraph()
            .SetFontSize(12)
            .SetMultipliedLeading(1f)
            .Add(new Text("Атмосферное давление, кПа ")
                .SetBold())
            .Add(new Text(pressure.ToString())
                .SetUnderline());
        validationInfoCell.Add(pressureParagraph);

        validationInfoTable.AddCell(validationInfoCell);

        doc.Add(validationInfoTable);

        var validationTable = new Table(UnitValue.CreatePercentArray(1))
            .UseAllAvailableWidth();

        var validationCell = new Cell()
            .SetBorder(Border.NO_BORDER);

        var validationResultHeader = "Операции поверки";
        var validationResultHeaderParagraph = new Paragraph(validationResultHeader)
            .SetFontSize(12)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetMultipliedLeading(1f)
            .SetBold();
        validationCell.Add(validationResultHeaderParagraph);
        
        var outsideViewInfoParagraph = new Paragraph()
            .SetFontSize(12)
            .SetMultipliedLeading(1f)
            .Add(new Text("1. Внешний осмотр: ")
                .SetBold())
            .Add(new Text(standSettingsService.StandSettingsModel.OutsideCheckInfo)
                .SetUnderline());
        validationCell.Add(outsideViewInfoParagraph);
        
        var useTestInfoParagraph = new Paragraph()
            .SetFontSize(12)
            .SetMultipliedLeading(1f)
            .Add(new Text("2. Опробование: ")
                .SetBold())
            .Add(new Text(standSettingsService.StandSettingsModel.DeviceTestInfo)
                .SetUnderline());
        validationCell.Add(useTestInfoParagraph);

        var validationResultTableNameParagraph = new Paragraph()
            .SetFontSize(12)
            .SetMultipliedLeading(1f)
            .Add(new Text("3. Определение метрологических характеристик: ")
                .SetBold());
        validationCell.Add(validationResultTableNameParagraph);

        var validationResultTable = new Table(UnitValue.CreatePercentArray(8))
            .UseAllAvailableWidth();
        validationResultTable
            .AddHeaderCell(new Cell(2, 1).Add(new Paragraph("Номер точки")).SetTextAlignment(TextAlignment.CENTER))
            .AddHeaderCell(new Cell(2, 1).Add(new Paragraph("Расход, м3/ч")).SetTextAlignment(TextAlignment.CENTER))
            .AddHeaderCell(new Cell(1, 2).Add(new Paragraph("Кол-во воздуха по показателям, л:")).SetTextAlignment(TextAlignment.CENTER))
            .AddHeaderCell(new Cell(2, 1).Add(new Paragraph("Падение давления на ГСБ, кПа")).SetTextAlignment(TextAlignment.CENTER))
            .AddHeaderCell(new Cell(2, 1).Add(new Paragraph("Погрешность ГСБ, %")).SetTextAlignment(TextAlignment.CENTER))
            .AddHeaderCell(new Cell(2, 1).Add(new Paragraph("Допускаемая погрешность, %")).SetTextAlignment(TextAlignment.CENTER))
            .AddHeaderCell(new Cell(2, 1).Add(new Paragraph("Результат")).SetTextAlignment(TextAlignment.CENTER))
            .AddHeaderCell(new Cell().Add(new Paragraph("СПУ-5")).SetTextAlignment(TextAlignment.CENTER))
            .AddHeaderCell(new Cell().Add(new Paragraph("СГ")).SetTextAlignment(TextAlignment.CENTER));
        
        
        var result = true;
        
        foreach (var validationPointResult in validationOperationResult.ValidationPointResults)
        {
            foreach (var validationMeasureResult in validationPointResult.ValidationMeasureResults)
            {
                var devicePoint = validationMeasureResult.ValidationDeviceResults[deviceNumber];

                if (devicePoint.VolumeDifference >= devicePoint.TargetVolumeDifference || devicePoint.VolumeDifference <= -devicePoint.TargetVolumeDifference) result = false;
                
                validationResultTable
                    .AddCell(new Cell().Add(new Paragraph(devicePoint.PointNumber.ToString()).SetTextAlignment(TextAlignment.CENTER)))
                    .AddCell(new Cell().Add(new Paragraph(devicePoint.TargetFlow.ToString()).SetTextAlignment(TextAlignment.CENTER)))
                    .AddCell(new Cell().Add(new Paragraph(devicePoint.TargetVolume.ToString()).SetTextAlignment(TextAlignment.CENTER)))
                    .AddCell(new Cell().Add(new Paragraph((devicePoint.EndVolumeValue - devicePoint.StartVolumeValue).ToString()).SetTextAlignment(TextAlignment.CENTER)))
                    .AddCell(new Cell().Add(new Paragraph(devicePoint.PressureDifference.ToString()).SetTextAlignment(TextAlignment.CENTER)))
                    .AddCell(new Cell().Add(new Paragraph(devicePoint.VolumeDifference.ToString()).SetTextAlignment(TextAlignment.CENTER)))
                    .AddCell(new Cell().Add(new Paragraph("\u00b1" + devicePoint.TargetVolumeDifference.ToString()).SetTextAlignment(TextAlignment.CENTER)))
                    .AddCell(new Cell().Add(new Paragraph(devicePoint.VolumeDifference >= devicePoint.TargetVolumeDifference || devicePoint.VolumeDifference <= -devicePoint.TargetVolumeDifference ? "НЕ ГОДЕН" : "ГОДЕН").SetTextAlignment(TextAlignment.CENTER)));
            }
        }
        
        validationCell.Add(validationResultTable);
        validationCell.Add(new Paragraph("Заключение: на основании результатов первичной (периодической) поверки устройство признано " +
                                         (result ? "пригодным к применению" : "не пригодным к применению") + " в соответствии с описанием типа")
            .SetBold()
            .SetFontSize(12)
            .SetMultipliedLeading(1));

        validationTable.AddCell(validationCell);
        doc.Add(validationTable);

        var employeeInfoParagraph = new Paragraph()
            .SetFontSize(12)
            .SetMultipliedLeading(1)
            .Add("Поверку проводил:");
        doc.Add(employeeInfoParagraph);

        var employeeParagraph = new Paragraph()
            .SetFontSize(12)
            .SetMultipliedLeading(1)
            .Add(new Text(standSettingsService.StandSettingsModel.PostInfo)
                .SetTextRise(-10));
        doc.Add(employeeParagraph);
        
        var employeeExtensionParagraph = new Paragraph()
            .SetFontSize(12)
            .SetMultipliedLeading(1)
            .Add(new Paragraph()
                .Add(new Text("\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0должность               ")
                    .SetTextRise(-10)
                    .SetFontSize(10)
                    .SetUnderline()
                    .SetTextAlignment(TextAlignment.CENTER))
                .Add("  ")
                .Add(new Text("          подпись          ")
                    .SetTextRise(-10)
                    .SetFontSize(10)
                    .SetUnderline()
                    .SetTextAlignment(TextAlignment.CENTER))
                .Add("  ")
                .Add(new Text("                     фамилия, имя и отчетсво (при наличии)\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0")
                    .SetTextRise(-10)
                    .SetFontSize(10)
                    .SetUnderline()
                    .SetTextAlignment(TextAlignment.CENTER)));
        doc.Add(employeeExtensionParagraph);

        var validationDateParagraph = new Paragraph()
            .SetFontSize(12)
            .SetMultipliedLeading(1)
            .Add(new Paragraph()
                .Add("Дата поверки ")
                .Add(new Text(DateTime.Now.ToString("g"))
                    .SetUnderline()));
        doc.Add(validationDateParagraph);

        doc.Close();
            
            return protocolNumber;
        }
        catch (Exception e)
        {
            throw;
        }
    }
    
    private int GetProtocolNumber()
    {
        var protocolsDirectory = "Protocols";
        var path = $"{Directory.GetCurrentDirectory()}/{protocolsDirectory}";
        if (path != null && !Directory.Exists(path)) Directory.CreateDirectory(path);
        var fileList = Directory.GetFiles(path);

        if (fileList.Length == 0) return 1;

        var numberList = (fileList.Select(filePath => filePath.Split("№")).Select(a => a[1].Split(".")).Select(b => b[0]).Select(number => int.Parse(number))).ToList();

        return numberList.Max() + 1;
    }
    

    public string CreateTempProtocol(ValidationOperationResult validationOperationResult,
        int deviceNumber,
        IStandSettingsService standSettingsService,
        float? temperature,
        float? pressure,
        float? humidity)
    {
        try
        {
            var point = validationOperationResult.ValidationPointResults.First();
            var measure = point.ValidationMeasureResults.First();
            var device = measure.ValidationDeviceResults[deviceNumber];
            var protocolNumber = device.ProtocolNumber;

            var protocolNumberString = protocolNumber.ToString();
            while (protocolNumberString.Length < 6) protocolNumberString = protocolNumberString.Insert(0, "0");
            
            var protocolsDirectory = "Protocols/Temp";
            var path = $"{Directory.GetCurrentDirectory()}/{protocolsDirectory}/_temp.pdf";
            var dir = Path.GetDirectoryName(path);
            if (dir != null && !Directory.Exists(dir)) Directory.CreateDirectory(dir);

            var pdfDoc = new PdfDocument(new PdfWriter($"{Directory.GetCurrentDirectory()}/Protocols/Temp/_temp.pdf"));

            var doc = new Document(pdfDoc);
            doc.SetFont(PdfFontFactory.CreateFont($"{Directory.GetCurrentDirectory()}/Assets/Fonts/times.ttf"));

            var headerTable = new Table(UnitValue.CreatePercentArray(1))
            .UseAllAvailableWidth();

        var headerStr1 = new Paragraph(standSettingsService.StandSettingsModel.ValidationVendorType)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetBold()
            .SetMultipliedLeading(1f)
            .SetFontSize(12);

        var headerStr2 =
            new Paragraph(standSettingsService.StandSettingsModel.ValidationVendorName)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetBold()
                .SetMultipliedLeading(1f)
                .SetFontSize(12);

        var headerStr3 = new Paragraph(standSettingsService.StandSettingsModel.ValidationVendorShortName)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetBold()
            .SetMultipliedLeading(1f)
            .SetFontSize(12);

        var headerStr4 = new Paragraph(standSettingsService.StandSettingsModel.ValidationVendorUniqueNumber)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetBold()
            .SetMultipliedLeading(1f)
            .SetFontSize(12);

        var headerStr5 = new Paragraph(standSettingsService.StandSettingsModel.ValidationVendorAddress)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetMultipliedLeading(1f)
            .SetFontSize(12);

        var headerCell = new Cell()
            .SetBorder(Border.NO_BORDER)
            .Add(headerStr1)
            .Add(headerStr2)
            .Add(headerStr3)
            .Add(headerStr4)
            .Add(headerStr5);

        headerTable.AddCell(headerCell);

        doc.Add(headerTable);
        
        var protocolNumberParagraph = new Paragraph()
            .SetTextAlignment(TextAlignment.CENTER)
            .SetBold()
            .SetFontSize(12)
            .Add(new Text("ПРОТОКОЛ ПОВЕРКИ №"))
            .Add(new Text(protocolNumberString)
                .SetUnderline());
        doc.Add(protocolNumberParagraph);

        var validationInfoTable = new Table(UnitValue.CreatePercentArray(1))
            .UseAllAvailableWidth();

        var validationInfoCell = new Cell()
            .SetBorder(Border.NO_BORDER);
        
        var deviceParagraph = new Paragraph()
            .SetFontSize(12)
            .SetMultipliedLeading(1f)
            .Add(new Text("Наименование, тип, разряд СИ: ")
                .SetBold())
            .Add(new Text(standSettingsService.StandSettingsModel.DeviceInfo)
                .SetUnderline());
        validationInfoCell.Add(deviceParagraph);
        
        var vendorNumberParagraph = new Paragraph()
            .SetFontSize(12)
            .SetMultipliedLeading(1f)
            .Add(new Text("Заводской №: ")
                .SetBold())
            .Add(new Text(validationOperationResult.ValidationPointResults.First().ValidationMeasureResults.First().ValidationDeviceResults[deviceNumber].VendorNumber)
                .SetUnderline());
        validationInfoCell.Add(vendorNumberParagraph);
        
        var vendorNameParagraph = new Paragraph()
            .SetFontSize(12)
            .SetMultipliedLeading(1f)
            .Add(new Text("Предприятие-изготовитель: ")
                .SetBold())
            .Add(new Text(standSettingsService.StandSettingsModel.VendorName)
                .SetUnderline());
        validationInfoCell.Add(vendorNameParagraph);
        
        var dateCreateParagraph = new Paragraph()
            .SetFontSize(12)
            .SetMultipliedLeading(1f)
            .Add(new Text("Дата производства: ")
                .SetBold())
            .Add(new Text(standSettingsService.StandSettingsModel.VendorDate)
                .SetUnderline());
        validationInfoCell.Add(dateCreateParagraph);
        
        var measureRangeParagraph = new Paragraph()
            .SetFontSize(12)
            .SetMultipliedLeading(1f)
            .Add(new Text("Диапазон измерений: ")
                .SetBold())
            .Add(new Text(standSettingsService.StandSettingsModel.DeviceRangeInfo)
                .SetUnderline());
        validationInfoCell.Add(measureRangeParagraph);
        
        var belongerNameParagraph = new Paragraph()
            .SetFontSize(12)
            .SetMultipliedLeading(1f)
            .Add(new Text("Принадлежит: ")
                .SetBold())
            .Add(new Text(standSettingsService.StandSettingsModel.OwnerName)
                .SetUnderline());
        validationInfoCell.Add(belongerNameParagraph);
        
        var previousValidationInfoParagraph = new Paragraph()
            .SetFontSize(12)
            .SetMultipliedLeading(1f)
            .Add(new Text("Серия и номер предыдущей поверки: ")
                .SetBold())
            .Add(new Text(standSettingsService.StandSettingsModel.LastValidationInfo)
                .SetUnderline());
        validationInfoCell.Add(previousValidationInfoParagraph);
        
        var deviceValidationInfoParagraph = new Paragraph()
            .SetFontSize(12)
            .SetMultipliedLeading(1f)
            .Add(new Text("Для поверки использовались следующие средства измерений: ")
                .SetBold())
            .Add(new Text(standSettingsService.StandSettingsModel.ValidationDeviceInfo)
                .SetUnderline());
        validationInfoCell.Add(deviceValidationInfoParagraph);
        
        var validationMethodInfoParagraph = new Paragraph()
            .SetFontSize(12)
            .SetMultipliedLeading(1f)
            .Add(new Text("Методика поверки: ")
                .SetBold())
            .Add(new Text(standSettingsService.StandSettingsModel.ValidationMethod)
                .SetUnderline());
        validationInfoCell.Add(validationMethodInfoParagraph);
        
        var tightnessInfoParagraph = new Paragraph()
            .SetFontSize(12)
            .SetMultipliedLeading(1f)
            .Add(new Text("Проверка на герметичность: ")
                .SetBold())
            .Add(new Text(standSettingsService.StandSettingsModel.TightnessInfo)
                .SetUnderline());
        validationInfoCell.Add(tightnessInfoParagraph);

        validationInfoCell.Add(new Paragraph("Условия поверки:")
            .SetBold()
            .SetFontSize(12)
            .SetMultipliedLeading(1f));
        
        var temperatureParagraph = new Paragraph()
            .SetFontSize(12)
            .SetMultipliedLeading(1f)
            .Add(new Text("Температура окружающей среды, \u00b0С ")
                .SetBold())
            .Add(new Text(temperature.ToString())
                .SetUnderline());
        validationInfoCell.Add(temperatureParagraph);


        var humidityParagraph = new Paragraph()
            .SetFontSize(12)
            .SetMultipliedLeading(1f)
            .Add(new Text("Относительная влажность воздуха, % ")
                .SetBold())
            .Add(new Text(humidity.ToString())
                .SetUnderline());
        validationInfoCell.Add(humidityParagraph);

        var pressureParagraph = new Paragraph()
            .SetFontSize(12)
            .SetMultipliedLeading(1f)
            .Add(new Text("Атмосферное давление, кПа ")
                .SetBold())
            .Add(new Text(pressure.ToString())
                .SetUnderline());
        validationInfoCell.Add(pressureParagraph);

        validationInfoTable.AddCell(validationInfoCell);

        doc.Add(validationInfoTable);

        var validationTable = new Table(UnitValue.CreatePercentArray(1))
            .UseAllAvailableWidth();

        var validationCell = new Cell()
            .SetBorder(Border.NO_BORDER);

        var validationResultHeader = "Операции поверки";
        var validationResultHeaderParagraph = new Paragraph(validationResultHeader)
            .SetFontSize(12)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetMultipliedLeading(1f)
            .SetBold();
        validationCell.Add(validationResultHeaderParagraph);
        
        var outsideViewInfoParagraph = new Paragraph()
            .SetFontSize(12)
            .SetMultipliedLeading(1f)
            .Add(new Text("1. Внешний осмотр: ")
                .SetBold())
            .Add(new Text(standSettingsService.StandSettingsModel.OutsideCheckInfo)
                .SetUnderline());
        validationCell.Add(outsideViewInfoParagraph);
        
        var useTestInfoParagraph = new Paragraph()
            .SetFontSize(12)
            .SetMultipliedLeading(1f)
            .Add(new Text("2. Опробование: ")
                .SetBold())
            .Add(new Text(standSettingsService.StandSettingsModel.DeviceTestInfo)
                .SetUnderline());
        validationCell.Add(useTestInfoParagraph);

        var validationResultTableNameParagraph = new Paragraph()
            .SetFontSize(12)
            .SetMultipliedLeading(1f)
            .Add(new Text("3. Определение метрологических характеристик: ")
                .SetBold());
        validationCell.Add(validationResultTableNameParagraph);

        var validationResultTable = new Table(UnitValue.CreatePercentArray(8))
            .UseAllAvailableWidth();
        validationResultTable
            .AddHeaderCell(new Cell(2, 1).Add(new Paragraph("Номер точки")).SetTextAlignment(TextAlignment.CENTER))
            .AddHeaderCell(new Cell(2, 1).Add(new Paragraph("Расход, м3/ч")).SetTextAlignment(TextAlignment.CENTER))
            .AddHeaderCell(new Cell(1, 2).Add(new Paragraph("Кол-во воздуха по показателям, л:")).SetTextAlignment(TextAlignment.CENTER))
            .AddHeaderCell(new Cell(2, 1).Add(new Paragraph("Падение давления на ГСБ, Па")).SetTextAlignment(TextAlignment.CENTER))
            .AddHeaderCell(new Cell(2, 1).Add(new Paragraph("Погрешность ГСБ, %")).SetTextAlignment(TextAlignment.CENTER))
            .AddHeaderCell(new Cell(2, 1).Add(new Paragraph("Допускаемая погрешность, %")).SetTextAlignment(TextAlignment.CENTER))
            .AddHeaderCell(new Cell(2, 1).Add(new Paragraph("Результат")).SetTextAlignment(TextAlignment.CENTER))
            .AddHeaderCell(new Cell().Add(new Paragraph("СПУ-5")).SetTextAlignment(TextAlignment.CENTER))
            .AddHeaderCell(new Cell().Add(new Paragraph("СГ")).SetTextAlignment(TextAlignment.CENTER));
        
        var result = true;
        
        foreach (var validationPointResult in validationOperationResult.ValidationPointResults)
        {
            foreach (var validationMeasureResult in validationPointResult.ValidationMeasureResults)
            {
                var devicePoint = validationMeasureResult.ValidationDeviceResults[deviceNumber];

                if (devicePoint.VolumeDifference >= devicePoint.TargetVolumeDifference || devicePoint.VolumeDifference <= -devicePoint.TargetVolumeDifference) result = false;
                
                validationResultTable
                    .AddCell(new Cell().Add(new Paragraph(devicePoint.PointNumber.ToString()).SetTextAlignment(TextAlignment.CENTER)))
                    .AddCell(new Cell().Add(new Paragraph(devicePoint.TargetFlow.ToString()).SetTextAlignment(TextAlignment.CENTER)))
                    .AddCell(new Cell().Add(new Paragraph(devicePoint.TargetVolume.ToString()).SetTextAlignment(TextAlignment.CENTER)))
                    .AddCell(new Cell().Add(new Paragraph((devicePoint.EndVolumeValue - devicePoint.StartVolumeValue).ToString()).SetTextAlignment(TextAlignment.CENTER)))
                    .AddCell(new Cell().Add(new Paragraph(devicePoint.PressureDifference.ToString()).SetTextAlignment(TextAlignment.CENTER)))
                    .AddCell(new Cell().Add(new Paragraph(devicePoint.VolumeDifference.ToString()).SetTextAlignment(TextAlignment.CENTER)))
                    .AddCell(new Cell().Add(new Paragraph("\u00b1" + devicePoint.TargetVolumeDifference).SetTextAlignment(TextAlignment.CENTER)))
                    .AddCell(new Cell().Add(new Paragraph(devicePoint.VolumeDifference >= devicePoint.TargetVolumeDifference || devicePoint.VolumeDifference <= -devicePoint.TargetVolumeDifference ? "НЕ ГОДЕН" : "ГОДЕН").SetTextAlignment(TextAlignment.CENTER)));
            }
        }
        
        
        validationCell.Add(validationResultTable);
        validationCell.Add(new Paragraph("Заключение: на основании результатов первичной (периодической) поверки устройство признано " +
                                         (result ? "пригодным к применению" : "не пригодным к применению") + " в соответствии с описанием типа")
            .SetBold()
            .SetFontSize(12)
            .SetMultipliedLeading(1));

        validationTable.AddCell(validationCell);
        doc.Add(validationTable);

        var employeeInfoParagraph = new Paragraph()
            .SetFontSize(12)
            .SetMultipliedLeading(1)
            .Add("Поверку проводил:");
        doc.Add(employeeInfoParagraph);

        var employeeParagraph = new Paragraph()
            .SetFontSize(12)
            .SetMultipliedLeading(1)
            .Add(new Text(standSettingsService.StandSettingsModel.PostInfo)
                .SetTextRise(-10));
        doc.Add(employeeParagraph);
        
        var employeeExtensionParagraph = new Paragraph()
            .SetFontSize(12)
            .SetMultipliedLeading(1)
            .Add(new Paragraph()
                .Add(new Text("\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0должность               ")
                    .SetTextRise(-10)
                    .SetFontSize(10)
                    .SetUnderline()
                    .SetTextAlignment(TextAlignment.CENTER))
                .Add("  ")
                .Add(new Text("          подпись          ")
                    .SetTextRise(-10)
                    .SetFontSize(10)
                    .SetUnderline()
                    .SetTextAlignment(TextAlignment.CENTER))
                .Add("  ")
                .Add(new Text("                     фамилия, имя и отчетсво (при наличии)\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0\u00A0")
                    .SetTextRise(-10)
                    .SetFontSize(10)
                    .SetUnderline()
                    .SetTextAlignment(TextAlignment.CENTER)));
        doc.Add(employeeExtensionParagraph);

        var validationDateParagraph = new Paragraph()
            .SetFontSize(12)
            .SetMultipliedLeading(1)
            .Add(new Paragraph()
                .Add("Дата поверки ")
                .Add(new Text(DateTime.Now.ToString("g"))
                    .SetUnderline()));
        doc.Add(validationDateParagraph);

        doc.Close();
            
        return $"{Directory.GetCurrentDirectory()}/Protocols/Temp/_temp.pdf";
        }
        catch (Exception e)
        {
            throw;
        }
    }
}