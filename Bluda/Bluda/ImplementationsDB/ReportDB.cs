using ClassLibrary.BindingModel;
using ClassLibrary.Interface;
using ClassLibrary.Models;
using ClassLibrary.ViewModel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace ClassLibrary.ImplementationsDB
{
    public class ReportDB : IReportService
    {
        private DBContext context;

        public ReportDB(DBContext context)
        {
            this.context = context;
        }

        public async Task SaveToPdf(ReportBindingModel model)
        {
            FileStream fs = new FileStream(model.FileName, FileMode.OpenOrCreate, FileAccess.Write);
            //создаем документ, задаем границы, связываем документ и поток
            iTextSharp.text.Document doc = new iTextSharp.text.Document();
            doc.SetMargins(0.5f, 0.5f, 0.5f, 0.5f);
            PdfWriter writer = PdfWriter.GetInstance(doc, fs);

            doc.Open();
            BaseFont baseFont = BaseFont.CreateFont();

            //вставляем заголовок
            var phraseTitle = new Phrase("Products",
                new iTextSharp.text.Font(baseFont, 16, iTextSharp.text.Font.BOLD));
            iTextSharp.text.Paragraph paragraph = new iTextSharp.text.Paragraph(phraseTitle)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 12
            };
            doc.Add(paragraph);
            //вставляем таблицу, задаем количество столбцов, и ширину колонок
            PdfPTable table = new PdfPTable(5);
            //вставляем шапку
            PdfPCell cell = new PdfPCell();
            var fontForCellBold = new iTextSharp.text.Font(baseFont, 10, iTextSharp.text.Font.BOLD);
            table.AddCell(new PdfPCell(new Phrase("BludaName", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            table.AddCell(new PdfPCell(new Phrase("DateCreate", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            table.AddCell(new PdfPCell(new Phrase("ProductName", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            table.AddCell(new PdfPCell(new Phrase("PlaceCreate", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            table.AddCell(new PdfPCell(new Phrase("DataPostavki", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            //заполняем таблицу
            var list = await GetList(model);
            var fontForCells = new iTextSharp.text.Font(baseFont, 10);
            for (int i = 0; i < list.Count; i++)
            {
                cell = new PdfPCell(new Phrase(list[i].BludaName, fontForCells));
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(list[i].DateCreate, fontForCells));
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(list[i].ProductName, fontForCells));
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(list[i].PlaceCreate, fontForCells));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(list[i].DataPostavki, fontForCells));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell);
            }
            //вставляем таблицу
            doc.Add(table);

            doc.Close();
        }

        private async Task<List<ReportViewModel>> GetList(ReportBindingModel model)
        {
            return await context.Bludas.Where(rec => rec.DateCreate >= model.DateFrom && rec.DateCreate <= model.DateTo).Include(rec => rec.Products).SelectMany(rec => rec.Products).Include(rec => rec.Bluda)
                .Select(rec => new ReportViewModel
                {
                    BludaName = rec.Bluda.NameBluda,
                    DataPostavki = SqlFunctions.DateName("dd", rec.DatePostavki) + " " +
                                   SqlFunctions.DateName("mm", rec.DatePostavki) + " " +
                                   SqlFunctions.DateName("yyyy", rec.DatePostavki),
                    DateCreate = SqlFunctions.DateName("dd", rec.Bluda.DateCreate) + " " +
                                   SqlFunctions.DateName("mm", rec.Bluda.DateCreate) + " " +
                                   SqlFunctions.DateName("yyyy", rec.Bluda.DateCreate),
                    PlaceCreate = rec.PlaceProizvod,
                    ProductName = rec.ProductName
                }).ToListAsync();
        }

        public async Task SaveToJson(ReportBindingModel model)
        {
            DataContractJsonSerializer bludaJS = new DataContractJsonSerializer(typeof(List<Bluda>));
            MemoryStream msBluda = new MemoryStream();
            bludaJS.WriteObject(msBluda, await context.Bludas.ToListAsync());
            msBluda.Position = 0;
            StreamReader srBluda = new StreamReader(msBluda);
            string bludasJSON = srBluda.ReadToEnd();
            srBluda.Close();
            msBluda.Close();

            DataContractJsonSerializer productJS = new DataContractJsonSerializer(typeof(List<Produckt>));
            MemoryStream msproduct = new MemoryStream();
            productJS.WriteObject(msproduct, await context.Products.ToListAsync());
            msproduct.Position = 0;
            StreamReader srproduct = new StreamReader(msproduct);
            string productsJSON = srproduct.ReadToEnd();
            srproduct.Close();
            msproduct.Close();

            File.WriteAllText(model.FileName, "{\n" + "    \"Bluda\": " + bludasJSON + ",\n" + "    \"Product\": " + productsJSON + ",\n" + "}");
        }
    }
}
