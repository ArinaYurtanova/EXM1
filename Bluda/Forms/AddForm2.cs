using ClassLibrary.BindingModel;
using ClassLibrary.Interface;
using ClassLibrary.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Unity;
using Unity.Attributes;

namespace ClassLibrary
{
    public partial class AddForm2 : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int ID { set { Id = value; } }
        
        private readonly IProduct _service;
        
        private int? Id;

        public int IDsuka { set { id = value; } }
        private int? id;
        
        public AddForm2()
        {
            InitializeComponent();
        }

        public AddForm2(IProduct service)
        {
            InitializeComponent();
            _service = service;
        }

        private void AddForm2_Load(object sender, EventArgs e)
        {
            if (Id.HasValue)
            {
                try
                {
                    ProductViewModel element = _service.GetElement(id.Value);
                    NameTextBox.Text = element.ProductName;
                    placeTextBox.Text = element.PlaceProizvod;
                    CountTextBox.Text = element.Count.ToString();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(NameTextBox.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                if (id.HasValue)
                {
                    _service.Update(new ProductBindingModel
                    {
                        Id = Id.Value,
                        ProductName = NameTextBox.Text,
                        PlaceProizvod = placeTextBox.Text,
                        Count = Int32.Parse(CountTextBox.Text),
                        IdBluda = Id.Value
                    });
                }
                else
                {
                    _service.Add(new ProductBindingModel
                    {
                        ProductName = NameTextBox.Text,
                        PlaceProizvod = placeTextBox.Text,
                        Count = Int32.Parse(CountTextBox.Text),
                        IdBluda = Id.Value
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

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
