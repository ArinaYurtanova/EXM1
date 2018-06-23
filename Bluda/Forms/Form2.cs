using ClassLibrary.Interface;
using ClassLibrary.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Unity;
using Unity.Attributes;

namespace ClassLibrary
{
    public partial class Form2 : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly IBluda serviceB;

        private readonly IProduct serviceP;

        public int idBluda { set { id = value; } }

        private int? id;

        public Form2()
        {
            InitializeComponent();
        }

        public Form2(IBluda serviceB, IProduct serviceP)
        {
            this.serviceP = serviceP;
            this.serviceB = serviceB;
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                List<ProductViewModel> list = serviceB.GetElement(id.Value).Products;
                if (list != null)
                {
                    dataGridView1.DataSource = list;
                    dataGridView1.Columns[0].Visible = false;
                    dataGridView1.Columns[1].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<AddForm2>();
            form.ID = id.Value;
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                var form = Container.Resolve<AddForm2>();
                form.IDsuka = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
                form.ID = id.Value;
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadData();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                if (MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
                    try
                    {
                        serviceP.Delete(id);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    LoadData();
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
