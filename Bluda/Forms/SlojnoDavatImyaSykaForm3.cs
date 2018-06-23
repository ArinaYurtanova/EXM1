using ClassLibrary.Interface;
using System.Collections.Generic;
using System;
using System.Windows.Forms;
using Unity;
using Unity.Attributes;
using ClassLibrary.ViewModel;

namespace Forms
{
    public partial class SlojnoDavatImyaSykaForm3 : Form
    {
        [Dependency]
        public new IUnityContainer container { set; get; }
        private readonly IProduct service;
        private ProductViewModel model;
        public ProductViewModel Model { set { model = value; } get { return model; } }

        public SlojnoDavatImyaSykaForm3(IProduct service)
        {
            InitializeComponent();
            this.service = service;
        }

        private void SlojnoDavatImyaSykaForm3_Load(object sender, EventArgs e)
        {
            try
            {
                List<ProductViewModel> list = service.GetList();
                if (list != null)
                {
                    comboBox1.DisplayMember = "ElementName";
                    comboBox1.ValueMember = "ID";
                    comboBox1.DataSource = list;
                    comboBox1.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (model != null)
            {
                comboBox1.Enabled = false;
                comboBox1.SelectedValue = model.Id;
                Number.Text = model.Count.ToString();
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Number.Text))
            {
                MessageBox.Show("Введите количество", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBox1.SelectedValue == null)
            {
                MessageBox.Show("Выберите компонент", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                if (model == null)
                {
                    model = new ProductViewModel
                    {
                        Id = Convert.ToInt32(comboBox1.SelectedValue),
                        ProductName = comboBox1.Text,
                        Count = Convert.ToInt32(Number.Text),
                    };
                }
                else
                {
                    model.Count = Convert.ToInt32(Number.Text);
                }
                MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
