using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Raktárkészlet_nyilvántartó_rendszer
{
    public partial class MainWindow : Window
    {
        private ServerConnection con;

        public MainWindow()
        {
            InitializeComponent();
            con = new ServerConnection();
        }

        private async void ClickAdd(object sender, EventArgs e)
        {
            bool success = await con.CreateProduct(
                nameInput.Text,
                typeInput.Text,
                Convert.ToInt32(priceInput.Text)
            );

            if (success)
            {
                MessageBox.Show("Termék sikeresen hozzáadva");
                ClearInputs();
            }
            else
            {
                MessageBox.Show("Hiba történt a termék hozzáadásakor");
            }
        }

        private async void ClickShowAll(object sender, EventArgs e)
        {
            ClearDisplay();

            List<JsonData> products = await con.GetAllProducts();
            if (products.Count == 0)
            {
                MessageBox.Show("Nincsenek termékek.");
                return;
            }

            foreach (var p in products)
            {
                var tb = new TextBlock
                {
                    Text = $"ID: {p.id} — Név: {p.name} — Típus: {p.type} — Ár: {p.price}",
                    Margin = new Thickness(5)
                };
                Everything.Children.Add(tb);
            }
        }

        private async void ClickShowById(object sender, EventArgs e)
        {
            ClearDisplay();

            if (!int.TryParse(IdInput.Text, out int id))
            {
                MessageBox.Show("Érvénytelen azonosító.");
                return;
            }

            JsonData product = await con.GetProductById(id.ToString());
            if (product != null)
            {
                var tb = new TextBlock
                {
                    Text = $"ID: {product.id} — Név: {product.name} — Típus: {product.type} — Ár: {product.price}",
                    Margin = new Thickness(5)
                };
                Everything.Children.Add(tb);
            }
            else
            {
                MessageBox.Show("Nem található ilyen azonosítójú termék.");
            }
        }

        private void ClearDisplay()
        {
            Everything.Children.Clear();
        }

        private void ClearInputs()
        {
            nameInput.Text = string.Empty;
            typeInput.Text = string.Empty;
            priceInput.Text = string.Empty;
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null && tb.Text == (string)tb.Tag)
            {
                tb.Text = "";
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null && string.IsNullOrWhiteSpace(tb.Text))
            {
                tb.Text = (string)tb.Tag;
            }
        }
    }
}
