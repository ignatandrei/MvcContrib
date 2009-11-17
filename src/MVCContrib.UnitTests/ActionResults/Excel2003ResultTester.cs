using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using System.Web.Mvc;
using System.IO;
using System.Web.Routing;
using System.Diagnostics;
using MVCContrib.Export.ST_Renderer;
using MvcContrib.ActionResults;
using MVCContrib.Export.Renderer;
using MVCContrib.Export;
namespace MvcContrib.UnitTests.ActionResults
{
    [TestFixture]
    public class Excel2003ResultTester
    {
        List<PersonMoney> DataSourcePersons;
        string verificationData;
        string defaultHeader;

        private MockRepository _mocks;
        private ControllerContext _controllerContext;

        [SetUp]
        public void InitializeMock()
        {
            _mocks = new MockRepository();
            _controllerContext = new ControllerContext(_mocks.DynamicHttpContextBase(), new RouteData(), _mocks.DynamicMock<ControllerBase>());
            _mocks.ReplayAll();

        }
        [TestFixtureSetUp]
        public void Initialize()
        {
            DateTime dt = DateTime.Now;
            DataSourcePersons = new List<PersonMoney>()
            {
                new PersonMoney(0),
                new PersonMoney(1),
                new PersonMoney(2)
            };
            verificationData =
                string.Format(@"<?xml version='1.0'?>
<?mso-application progid='Excel.Sheet'?>
<Workbook xmlns='urn:schemas-microsoft-com:office:spreadsheet'
xmlns:o='urn:schemas-microsoft-com:office:office'
xmlns:x='urn:schemas-microsoft-com:office:excel'
xmlns:ss='urn:schemas-microsoft-com:office:spreadsheet'
xmlns:html='http://www.w3.org/TR/REC-html40'>
<DocumentProperties xmlns='urn:schemas-microsoft-com:office:office'>
<Author>Andrei Ignat</Author>
<LastAuthor>Andrei Ignat</LastAuthor>
<Version>11.9999</Version>
</DocumentProperties>
<ExcelWorkbook xmlns='urn:schemas-microsoft-com:office:excel'>
<WindowHeight>11760</WindowHeight>
<WindowWidth>15195</WindowWidth>
<WindowTopX>480</WindowTopX>
<WindowTopY>90</WindowTopY>
<ProtectStructure>False</ProtectStructure>
<ProtectWindows>False</ProtectWindows>
</ExcelWorkbook>
<Styles>
<Style ss:ID='Default' ss:Name='Normal'>
<Alignment ss:Vertical='Bottom'/>
<Borders/>
<Font/>
<Interior/>
<NumberFormat/>
<Protection/>
</Style>
<Style ss:ID='s21'>
<Font x:Family='Swiss' ss:Bold='1'/>
</Style>
</Styles>
<Worksheet ss:Name='Sheet1'>
<Table  x:FullColumns='1'
x:FullRows='1'>
<Row>   
<Cell ss:StyleID='s21'><Data ss:Type='String'>Money</Data></Cell><Cell ss:StyleID='s21'><Data ss:Type='String'>dt</Data></Cell>        
</Row>
<Row><Cell><Data ss:Type='String'>0</Data></Cell><Cell><Data ss:Type='String'>{0}</Data></Cell></Row><Row><Cell><Data ss:Type='String'>100</Data></Cell><Cell><Data ss:Type='String'>{1}</Data></Cell></Row><Row><Cell><Data ss:Type='String'>200</Data></Cell><Cell><Data ss:Type='String'>{2}</Data></Cell></Row>

</Table>
<WorksheetOptions xmlns='urn:schemas-microsoft-com:office:excel'>
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
</Workbook>",
                dt.ToString("yyyyMMdd"), dt.AddDays(1).ToString("yyyyMMdd"), dt.AddDays(2).ToString("yyyyMMdd"));
            verificationData = verificationData.Replace("'", "\"");
            //File.WriteAllText("a.xls", verificationData);
            //Process.Start("a.xls");
            defaultHeader = "<Cell ss:StyleID=\"s21\"><Data ss:Type=\"String\">DepositDate</Data></Cell><Cell ss:StyleID=\"s21\"><Data ss:Type=\"String\">Name</Data></Cell><Cell ss:StyleID=\"s21\"><Data ss:Type=\"String\">Number</Data></Cell>";
        }

        [Test]
        [Category("Base")]
        public void TestExportSTExcel()
        {

            STExcel2003Renderer<PersonMoney> render = new STExcel2003Renderer<PersonMoney>("Templates");

            var s = DataSourcePersons.Render(render);
            string res = s.ResultAsString();
            render.Columns.Clear();
            string res2 = DataSourcePersons.Render(new STExcel2003Renderer<PersonMoney>("Templates").ApplyCodeFormat(true))
                .AddColumns(x =>
                {
                    x.For("Number").named("Money");
                    x.For(val => val.DepositDate).named("dt").Format("{0:yyyyMMdd}");
                    x.For(val => val.Name).Visible(false);
                }
            ).ResultAsString();


            //Console.WriteLine(res2);
            //File.WriteAllText("a.xls", res2);
            //Process.Start("a.xls");

            Assert.IsTrue(res.Contains(defaultHeader));
            Assert.Greater(res.Length , res2.Length);
            Assert.AreEqual(res2, verificationData);
            
        }
        [Test]
        [Category("Advanced customization")]
        public void TestExportSTExcel_CustomFile()
        {
            var strender = new STExcel2003Renderer<PersonMoney>("Templates").NameTemplate("PersonMoneyExcel2003");
            string res = DataSourcePersons.Render(strender).ResultAsString();
            //Console.WriteLine(res);
            Assert.AreEqual(verificationData, res);

        }
        [Test]
        [Category("web")]
        public void TestActionResult_FormatFromCode()
        {
            ExcelResult<PersonMoney> result = new ExcelResult<PersonMoney>(
                new ExportModel<PersonMoney>()
                {
                    Renderer = new STExcel2003Renderer<PersonMoney>("Templates")
                    {
                        dataSource = DataSourcePersons
                    }.ApplyCodeFormat(true)
                }
                .AddColumns(x =>
                {
                    x.For("Number").named("Money");
                    x.For(val => val.DepositDate).named("dt").Format("{0:yyyyMMdd}");
                    x.For(val => val.Name).Visible(false);
                }
            ));

            result.ExecuteResult(_controllerContext);
            MemoryStream ms = _controllerContext.HttpContext.Response.OutputStream as MemoryStream;
            ms.Flush();
            ms.Position = 0;
            StreamReader sr = new StreamReader(ms);
            Assert.AreEqual(verificationData, sr.ReadToEnd());


        }
        [Test]
        [Category("web")]
        public void TestActionResult_FormatFromFile()
        {
            ExcelResult<PersonMoney> result = new ExcelResult<PersonMoney>(
                new ExportModel<PersonMoney>()
                {
                    Renderer = new STExcel2003Renderer<PersonMoney>("Templates")
                    {
                        dataSource = DataSourcePersons,
                        StringTemplateFileName = "PersonMoneyExcel2003"
                    }.ApplyCodeFormat(false)
                }

            );

            result.ExecuteResult(_controllerContext);
            MemoryStream ms = _controllerContext.HttpContext.Response.OutputStream as MemoryStream;
            ms.Flush();
            ms.Position = 0;
            StreamReader sr = new StreamReader(ms);
            Assert.AreEqual(verificationData, sr.ReadToEnd());

        }  
    }

   
    public class PersonMoney
    {

        public PersonMoney(int TestDataNr)
        {
            Number = TestDataNr * 100;
            DepositDate = DateTime.Now.AddDays(TestDataNr);
            Name = "My Name : " + TestDataNr;
        }
        public DateTime DepositDate { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
    }
}
