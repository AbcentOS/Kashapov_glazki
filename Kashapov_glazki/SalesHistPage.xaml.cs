using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Zelentsov_eyes
{
    /// <summary>
    /// Логика взаимодействия для SalesHistPage.xaml
    /// </summary>
    public partial class SalesHistPage : Page
    {
        public Agent CurrentAgent;
        public SalesHistPage(Agent agent)
        {
            CurrentAgent = agent;
            InitializeComponent();
            //load Sales
            SalesLB.ItemsSource = this.CurrentAgent.ProductSale.ToList();
            //load products
            ProductCB.ItemsSource = Zelentsov_eyesEntities.GetContext().Product.ToList().Select(p => p.Title);
        }


        private void AddSaleBtn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrEmpty(DatePick.Text))
                errors.AppendLine("Выберите дату продажи");
            if (string.IsNullOrEmpty(ProductCB.Text))
                errors.AppendLine("Выберите продукт");
            if (string.IsNullOrEmpty(CountTB.Text))
                errors.AppendLine("Введите количество");
            if(errors.Length != 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            var TemplateSale = new ProductSale();
            TemplateSale.ProductID = Zelentsov_eyesEntities.GetContext().Product.ToList().Where(p => p.Title == ProductCB.Text).ToList().First().ID;
            TemplateSale.SaleDate = DatePick.DisplayDate;
            TemplateSale.AgentID = CurrentAgent.ID;
            TemplateSale.ProductCount = Convert.ToInt32(CountTB.Text);
            try
            {
                Zelentsov_eyesEntities.GetContext().ProductSale.Add(TemplateSale);
                Zelentsov_eyesEntities.GetContext().SaveChanges();
                SalesLB.ItemsSource = this.CurrentAgent.ProductSale.ToList();
                SalesLB.Items.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void DelSaleBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SalesLB.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите продажу которую необходимо удалить");
                return;
            }
            if (MessageBox.Show("Вы точно хотите выполнить удаление?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Zelentsov_eyesEntities.GetContext().ProductSale.Remove(SalesLB.Items[SalesLB.SelectedIndex] as ProductSale);
                    Zelentsov_eyesEntities.GetContext().SaveChanges();
                    SalesLB.ItemsSource = this.CurrentAgent.ProductSale.ToList();
                    SalesLB.Items.Refresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void SearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            ProductCB.ItemsSource = Zelentsov_eyesEntities.GetContext().Product.ToList().Select(p => p.Title).Where(p => p.ToLower().Contains(SearchTB.Text.ToLower()));
        }
    }
}
