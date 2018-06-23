using Microsoft.Reporting.WinForms;
using System;
using ClassLibrary.Interface;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unity;
using Unity.Attributes;
using ClassLibrary.BindingModel;
using System.IO;

namespace Forms
{
    public partial class PDFForm : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly IBluda service;

        public PDFForm()
        {
            InitializeComponent();
        }

        public PDFForm(IBluda service)
        {
            this.service = service;
            InitializeComponent();
        }

        private void PDFForm_Load(object sender, EventArgs e)
        {
            this.reportViewer1.RefreshReport();
        }

        private void Form_Click(object sender, EventArgs e)
        {
            if (dateTimePicker1.Value.Date >= dateTimePicker2.Value.Date)
            {
                MessageBox.Show("Дата начала должна быть меньше даты окончания", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                ReportParameter parameter = new ReportParameter("ReportParameterPeriod",
                                            "c " + dateTimePicker1.Value.ToShortDateString() +
                                            " по " + dateTimePicker2.Value.ToShortDateString());
                reportViewer1.LocalReport.SetParameters(parameter);

                var dataSource = service.GetListOrder(new ReportBindingModel
                {
                    DateFrom = dateTimePicker1.Value,
                    DateTo = dateTimePicker2.Value
                });
                bindingSource.DataSource = dataSource;
                ReportDataSource source = new ReportDataSource("DataSet1", dataSource);
                reportViewer1.LocalReport.DataSources.Add(source);

                reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void Save_Click(object sender, EventArgs e)
        {
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string filenameExtension;
            byte[] b = reportViewer1.LocalReport.Render(
                "PDF", null, out mimeType, out encoding, out filenameExtension,
                out streamids, out warnings);
            await Write(b);
        }

        public async Task Write(byte[] bytes) 
        {
            using (FileStream fs = new FileStream("D:\\output.pdf", FileMode.Create))
            {
                await fs.WriteAsync(bytes, 0, bytes.Length);
            }
        }

        private void Back_Click(object sender, EventArgs e)
        {

        }
    }
}
