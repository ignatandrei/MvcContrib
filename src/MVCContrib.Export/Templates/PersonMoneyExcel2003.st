<?xml version="1.0"?>
<?mso-application progid="Excel.Sheet"?>
<Workbook xmlns="urn:schemas-microsoft-com:office:spreadsheet"
xmlns:o="urn:schemas-microsoft-com:office:office"
xmlns:x="urn:schemas-microsoft-com:office:excel"
xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet"
xmlns:html="http://www.w3.org/TR/REC-html40">
<DocumentProperties xmlns="urn:schemas-microsoft-com:office:office">
<Author>Andrei Ignat</Author>
<LastAuthor>Andrei Ignat</LastAuthor>
<Version>11.9999</Version>
</DocumentProperties>
<ExcelWorkbook xmlns="urn:schemas-microsoft-com:office:excel">
<WindowHeight>11760</WindowHeight>
<WindowWidth>15195</WindowWidth>
<WindowTopX>480</WindowTopX>
<WindowTopY>90</WindowTopY>
<ProtectStructure>False</ProtectStructure>
<ProtectWindows>False</ProtectWindows>
</ExcelWorkbook>
<Styles>
<Style ss:ID="Default" ss:Name="Normal">
<Alignment ss:Vertical="Bottom"/>
<Borders/>
<Font/>
<Interior/>
<NumberFormat/>
<Protection/>
</Style>
<Style ss:ID="s21">
<Font x:Family="Swiss" ss:Bold="1"/>
</Style>
</Styles>
<Worksheet ss:Name="Sheet1">
<Table  x:FullColumns="1"
x:FullRows="1">
<Row>   
<Cell ss:StyleID="s21"><Data ss:Type="String">Money</Data></Cell><Cell ss:StyleID="s21"><Data ss:Type="String">dt</Data></Cell>        
</Row>
$lines.values:{k|<Row><Cell><Data ss:Type="String">$k.Number$</Data></Cell><Cell><Data ss:Type="String">$k.DepositDate;format="yyyyMMdd"$</Data></Cell></Row>}$

</Table>
<WorksheetOptions xmlns="urn:schemas-microsoft-com:office:excel">
<Print>
<ValidPrinterInfo/>
<PaperSizeIndex>9</PaperSizeIndex>
<HorizontalResolution>600</HorizontalResolution>
<VerticalResolution>600</VerticalResolution>
</Print>
<Selected/>
<Panes>
<Pane>
 <Number>3</Number>
 <RangeSelection>R1C1:R1C1</RangeSelection>
</Pane>
</Panes>
<ProtectObjects>False</ProtectObjects>
<ProtectScenarios>False</ProtectScenarios>
</WorksheetOptions>
</Worksheet>
</Workbook>
