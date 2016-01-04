using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestionnairePrototype.Infrastructure
{
    public class ExcelWriter
    {
        private System.IO.StringWriter writer;

        public ExcelWriter()
        {
            writer = new System.IO.StringWriter();
            writer.WriteLine("<?xml version=\"1.0\"?>");
            writer.WriteLine("<?mso-application progid=\"Excel.Sheet\"?>");
            writer.WriteLine("<Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\"");
            writer.WriteLine(" xmlns:o=\"urn:schemas-microsoft-com:office:office\"");
            writer.WriteLine(" xmlns:x=\"urn:schemas-microsoft-com:office:excel\"");
            writer.WriteLine(" xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\"");
            writer.WriteLine(" xmlns:html=\"http://www.w3.org/TR/REC-html40/\">");
            writer.WriteLine(" <DocumentProperties xmlns=\"urn:schemas-microsoft-com:office:office\">;");
            writer.WriteLine("  <Author>AFFI Report Generator</Author>");
            writer.WriteLine(string.Format("  <Created>{0}T{1}Z</Created>", DateTime.Now.ToString("yyyy-mm-dd"), DateTime.Now.ToString("HH:MM:SS")));
            writer.WriteLine("  <Company>Seneca</Company>");
            writer.WriteLine("  <Version>11.6408</Version>");
            writer.WriteLine(" </DocumentProperties>");
            writer.WriteLine(" <ExcelWorkbook xmlns=\"urn:schemas-microsoft-com:office:excel\">");
            writer.WriteLine("  <WindowHeight>8955</WindowHeight>");
            writer.WriteLine("  <WindowWidth>11355</WindowWidth>");
            writer.WriteLine("  <WindowTopX>480</WindowTopX>");
            writer.WriteLine("  <WindowTopY>15</WindowTopY>");
            writer.WriteLine("  <ProtectStructure>False</ProtectStructure>");
            writer.WriteLine("  <ProtectWindows>False</ProtectWindows>");
            writer.WriteLine(" </ExcelWorkbook>");
            writer.WriteLine(" <Styles>");
            writer.WriteLine("  <Style ss:ID=\"Default\" ss:Name=\"Normal\">");
            writer.WriteLine("   <Alignment ss:Vertical=\"Bottom\"/>");
            writer.WriteLine("   <Borders/>");
            writer.WriteLine("   <Font/>");
            writer.WriteLine("   <Interior/>");
            writer.WriteLine("   <Protection/>");
            writer.WriteLine("  </Style>");
            writer.WriteLine("  <Style ss:ID=\"s21\">");
            writer.WriteLine("   <Alignment ss:Vertical=\"Bottom\" ss:WrapText=\"1\"/>");
            writer.WriteLine("  </Style>");
            writer.WriteLine(" </Styles>");
        }

        public void startWorksheet(String name)
        {
            writer.WriteLine("<Worksheet ss:Name=\"" + name + "\">");
            writer.WriteLine("<Table>");
        }

        public void endWorksheet()
        {
            writer.WriteLine("  </Table>");
            writer.WriteLine("  <WorksheetOptions xmlns=\"urn:schemas-microsoft-com:office:excel\">");
            writer.WriteLine("   <ProtectObjects>False</ProtectObjects>");
            writer.WriteLine("   <ProtectScenarios>False</ProtectScenarios>");
            writer.WriteLine("  </WorksheetOptions>");
            writer.WriteLine(" </Worksheet>");
        }

        public void startRow()
        {
            writer.WriteLine("<Row>");
        }

        public void endRow()
        {
            writer.WriteLine("</Row>");
        }

        public void addCell(String text)
        {
            writer.WriteLine("<Cell ss:StyleID=\"s21\"><Data ss:Type=\"String\">" + System.Security.SecurityElement.Escape(text) + "</Data></Cell>");
        }

        public void addCell(int number)
        {
            writer.WriteLine("<Cell ss:StyleID=\"s21\"><Data ss:Type=\"Number\">" + number + "</Data></Cell>");
        }

        public void addCell(double number)
        {
            writer.WriteLine("<Cell ss:StyleID=\"s21\"><Data ss:Type=\"Number\">" + number + "</Data></Cell>");
        }

        public void addCell(DateTime date)
        {
            writer.WriteLine("<Cell ss:StyleID=\"s21\"><Data ss:Type=\"String\">" + date.ToString("G") + "</Data></Cell>");
        }

        public void addEmptyCell()
        {
            addCell("");
        }

        public void endWorkbook()
        {
            writer.WriteLine("</Workbook>");
            writer.Close();
        }

        public String getString()
        {
            return writer.ToString();
        }
    }
}
