using SelectPdf;
using System.Data;
using System.IO;
using System.Text;

namespace MyWorkApplication.Classes
{
    internal class ReportToPDF
    {
        public DataTable dt;

        //build report parts
        public string BuildReport()
        {
            var sb = new StringBuilder();
            var header = ReadHtmlFile("header.html");
            sb.AppendLine(header);

            //load table
            var reportTable = LoadReportTable();
            sb.AppendLine(reportTable);

            var footer = ReadHtmlFile("footer.html");
            sb.AppendLine(footer);

            return sb.ToString();
        }

        //read from external file
        public string ReadHtmlFile(string FileName)
        {
            var text = File.ReadAllText(FileName);
            return text;
        }

        public string LoadReportTable()
        {
            var sb = new StringBuilder();
            //design table header
            sb.Append("<thead> <tr>");
            for (var i = 0; i < dt.Columns.Count; i++)
            {
                var TH = ReadHtmlFile("tableHeader.html");
                sb.Append(TH.Replace("[[NA]]", dt.Columns[i].ToString()));
            }

            sb.Append("</tr></thead>");
            //design table row
            sb.Append("<tr>");
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                for (var j = 0; j < dt.Columns.Count; j++)
                {
                    var TH = ReadHtmlFile("tableBody.html");
                    sb.Append(TH.Replace("[[NA]]", dt.Rows[i][j].ToString()));
                }

                sb.Append("</tr>");
            }

            return sb.ToString();
        }

        public void ExportHtmlToPDF(string htmlString, string loc)
        {
            var converter = new HtmlToPdf();
            //set converter options
            converter.Options.PdfPageSize = PdfPageSize.A4;
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
            //create new document
            var doc = converter.ConvertHtmlString(htmlString);
            //save pdf document
            doc.Save(loc);
            //close pdf document
            doc.Close();
        }
    }
}