using ClassLibrary.BindingModel;
using ClassLibrary.Interface;
using ClassLibrary.ViewModel;
using Forms;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Unity;
using Unity.Attributes;

namespace ClassLibrary
{
    public partial class AddForm1 : Form
    {
        [Dependency]
        public new IUnityContainer container { set; get; }
        public int ID { set { id = value; } }
        private int? id;
        private readonly IBluda service;
        private List<ProductViewModel> productElems;

        public AddForm1()
        {
            InitializeComponent();
        }

        public AddForm1(IBluda service)
        {
            this.service = service;
            InitializeComponent();
        }

        private void AddForm1_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    BludaViewModel view = service.GetElement(id.Value);
                    if (view != null)
                    {
                        Name.Text = view.NameBluda;
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                productElems = new List<ProductViewModel>();
            }
        }

        private void LoadData()
        {
            try
            {
                if (productElems != null)
                {
                    //dataGridView1.DataSource = null;
                    //dataGridView1.DataSource = productElems;
                    //dataGridView1.Columns[0].Visible = false;
                    //dataGridView1.Columns[1].Visible = false;
                    //dataGridView1.Columns[2].Visible = false;
                    //dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(NameTextBox.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(TypeTextBox.Text))
            {
                MessageBox.Show("Заполните цену", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                if (id.HasValue)
                {
                    service.Update(new BludaBindingModel
                    {
                        Id = id.Value,
                        NameBluda = NameTextBox.Text,
                        TypeBluda = TypeTextBox.Text
                    });
                }
                else
                {
                    service.Add(new BludaBindingModel
                    {
                        NameBluda = NameTextBox.Text,
                        TypeBluda = TypeTextBox.Text,
                        ListProduct = new List<ProductBindingModel>()
                    });
                }
                MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
