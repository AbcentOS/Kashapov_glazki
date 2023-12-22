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

namespace Kashapov_glazki
{
    /// <summary>
    /// Логика взаимодействия для ServicePage.xaml
    /// </summary>
    public partial class ServicePage : Page
    {
        int CountRecords;
        int CountPage;
        int CurrentPage = 0;
        List<Agent> CurrentPageList = new List<Agent>();
        List<Agent> TableList;
        List<Agent> PriorList;
        public ServicePage()
        {
            InitializeComponent();
            var currentService = Kashapov_glazkiEntities.GetContext().Agent.ToList();
            ServiceListView.ItemsSource = currentService;
            
            ComboType.SelectedIndex = 0;
            UpdateServices();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage(null));
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateServices();
        }

        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateServices();
        }
        private void UpdateServices()
        {
            var currentServices = Kashapov_glazkiEntities.GetContext().Agent.ToList();
            
            if (ComboType2.SelectedIndex == 0)
            {
                currentServices = currentServices.ToList();
            }
            if (ComboType2.SelectedIndex == 1)
            {
                currentServices = currentServices.Where(p => p.AgentTypeString == "ЗАО").ToList();
            }
            if (ComboType2.SelectedIndex == 2)
            {
                currentServices = currentServices.Where(p => p.AgentTypeString == "МКК").ToList();

            }
            if (ComboType2.SelectedIndex == 3)
            {
                currentServices = currentServices.Where(p => p.AgentTypeString == "МФО").ToList();

            }
            if (ComboType2.SelectedIndex == 4)
            {
                currentServices = currentServices.Where(p => p.AgentTypeString == "ОАО").ToList();

            }
            if (ComboType2.SelectedIndex == 5)
            {
                currentServices = currentServices.Where(p => p.AgentTypeString == "ООО").ToList();

            }
            if (ComboType2.SelectedIndex == 6)
            {
                currentServices = currentServices.Where(p => p.AgentTypeString == "ПАО").ToList();

            }
            if (ComboType.SelectedIndex == 0)
            {
                currentServices = currentServices.ToList();
            }
            if (ComboType.SelectedIndex == 1)
            {
                currentServices = currentServices.OrderBy(p => p.Title).ToList();
            }

            if (ComboType.SelectedIndex == 4)
            {
                currentServices = currentServices.OrderByDescending(p => p.Title).ToList();
               
            }
            if (ComboType.SelectedIndex == 5)
            {
                currentServices = currentServices.OrderBy(p => p.Priority).ToList();
                
            }
            if (ComboType.SelectedIndex == 6)
            {
                currentServices = currentServices.OrderByDescending(p => p.Priority).ToList();
            }
            string searchTEXT = TBoxSearch.Text.Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "");
            currentServices = currentServices.Where(p => p.Title.ToLower().Contains(TBoxSearch.Text.ToLower()) || 
            p.Email.ToLower().Contains(TBoxSearch.Text.ToLower()) || 
            p.Phone.ToLower().Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "").Contains(TBoxSearch.Text.ToLower())).ToList();
            ServiceListView.ItemsSource = currentServices;
            TableList = currentServices;
            //var currentService_ProductSale = Kashapov_glazkiEntities.GetContext().ProductSale.ToList();
            //foreach (var item in currentServices)
            //{
            //    currentService_ProductSale = currentService_ProductSale.Where(p => p.AgentID == item.ID).ToList();
            //}
            
            ChangePage(0, 0);

        }

        private void ComboType2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateServices();

        }

        private void LeftDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(1, null);
        }

        private void RightDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(2, null);
        }
        private void ChangePage(int direction, int? selectedPage)
        {
            CurrentPageList.Clear();
            CountRecords = TableList.Count;
            if (CountRecords % 10 > 0)
            {
                CountPage = CountRecords / 10 + 1;
            }
            else
            {
                CountPage = CountRecords / 10;
            }
            Boolean Ifupdate = true;
            int min;
            if (selectedPage.HasValue)
            {
                if (selectedPage >= 0 && selectedPage <= CountPage)
                {
                    CurrentPage = (int)selectedPage;
                    min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                    for (int i = CurrentPage * 10; i < min; i++)
                    {
                        CurrentPageList.Add(TableList[i]);
                    }
                }

            }
            else
            {
                switch (direction)
                {
                    case 1:
                        if (CurrentPage > 0)
                        {
                            CurrentPage--;
                            min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                            for (int i = CurrentPage * 10; i < min; i++)
                            {
                                CurrentPageList.Add(TableList[i]);
                            }
                        }
                        else
                        {
                            Ifupdate = false;
                        }
                        break;
                    case 2:
                        if (CurrentPage < CountPage - 1)
                        {
                            CurrentPage++;
                            min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                            for (int i = CurrentPage * 10; i < min; i++)
                            {
                                CurrentPageList.Add(TableList[i]);
                            }
                        }
                        else
                        {
                            Ifupdate = false;
                        }
                        break;
                }

            }
            if (Ifupdate)
            {
                PageListBox.Items.Clear();
                for (int i = 1; i <= CountPage; i++)
                {
                    PageListBox.Items.Add(i);
                }
                PageListBox.SelectedIndex = CurrentPage;

                min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                TBCount.Text = min.ToString();
                TBAllRecords.Text = " из " + CountRecords.ToString();

                ServiceListView.ItemsSource = CurrentPageList;
                ServiceListView.Items.Refresh();
            }
        }

        private void PageListBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ChangePage(0, Convert.ToInt32(PageListBox.SelectedItem.ToString()) - 1);
        }

        private void RightDirButton_Click_1(object sender, RoutedEventArgs e)
        {
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage(null));
            UpdateServices();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage((sender as Button).DataContext as Agent));
            UpdateServices();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Kashapov_glazkiEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                ServiceListView.ItemsSource = Kashapov_glazkiEntities.GetContext().Agent.ToList();
            }
            UpdateServices();
        }
        private Agent _currentService = new Agent();
        private void Edit_prior_Click(object sender, RoutedEventArgs e)
        {
            List<Agent> currentAgent = ServiceListView.SelectedItems.Cast<Agent>().ToList();
            ChangePrior window = new ChangePrior(currentAgent);
            window.Show();

            //Manager.MainFrame.Navigate(new ChangePrior().DataContext as Agent);

            
        }

        private void ServiceListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Edit_prior.Visibility = Visibility.Visible;
            UpdateServices();
        }

       
    }
}
