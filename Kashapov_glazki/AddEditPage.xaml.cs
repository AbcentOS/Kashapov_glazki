using Microsoft.Win32;
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
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private Agent _currentService = new Agent();
        List<AgentType> AgentTypesDBList = Kashapov_glazkiEntities.GetContext().AgentType.ToList();
        public AddEditPage(Agent SelectedAgent)
        {
            InitializeComponent();
            if (SelectedAgent != null)
            {
                _currentService = SelectedAgent;
                ComboType.SelectedIndex = _currentService.AgentTypeID - 1;
                

            }
            DataContext = _currentService; var currentService = Kashapov_glazkiEntities.GetContext().AgentType.ToList();

        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_currentService.Title))
            {
                errors.AppendLine("Укажите наименование агента");
            }
            if (string.IsNullOrWhiteSpace(_currentService.Address))
            {
                errors.AppendLine("Укажите адрес агента");
            }
            if (string.IsNullOrWhiteSpace(_currentService.DirectorName))
            {
                errors.AppendLine("Укажите ФИО директора");
            }
            if (ComboType.SelectedItem == null)
            {
                errors.AppendLine("Укажите тип агента");
            }
            if (_currentService.Priority <= 0)
            {
                errors.AppendLine("Укажите положительный приоритет агента");
            }
            if (string.IsNullOrWhiteSpace(_currentService.INN))
            {
                errors.AppendLine("Укажите ИНН агента");
            }
            if (string.IsNullOrWhiteSpace(_currentService.KPP))
            {
                errors.AppendLine("Укажите КПП агента");
            }
            if (string.IsNullOrWhiteSpace(_currentService.Phone))
            {
                errors.AppendLine("Укажите телефон агента");
            }
            else
            {
                string ph = _currentService.Phone.Replace("(", "").Replace("-", "").Replace("+", "");
                if (((ph[1] == '9' || ph[1] == '4' || ph[1] == '8') && ph.Length != 11)
                    || (ph[1] == '3' && ph.Length != 12))
                {
                    errors.AppendLine("Укажите правильно телефон агента");
                }
            }
            if (string.IsNullOrWhiteSpace(_currentService.Email))
            {
                errors.AppendLine("Укажите почту агента");
            }
            var currentType = (TextBlock)ComboType.SelectedItem;
            string currentTypeContent = currentType.Text;
            foreach (AgentType type in AgentTypesDBList)
            {
                if (type.Title.ToString() == currentTypeContent)
                {
                    _currentService.AgentType = type;
                    _currentService.AgentTypeID = type.ID;
                    break;
                }
            }
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            MessageBox.Show(_currentService.ID.ToString());
            if (_currentService.ID == 0)
            {
                Kashapov_glazkiEntities.GetContext().Agent.Add(_currentService);
            
            }
            try
            {

                Kashapov_glazkiEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }


        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var currentAgent = (sender as Button).DataContext as Agent;
            var curruntProductSale = Kashapov_glazkiEntities.GetContext().ProductSale.ToList();
            curruntProductSale = curruntProductSale.Where(p => p.AgentID == currentAgent.ID).ToList();
            if (curruntProductSale.Count != 0)
            {
                MessageBox.Show("Невозможно выполнить удаление,так как существует реализация продукции");
            }
            else
            {
                if (MessageBox.Show("Вы точно хотите выполнить удаление?","Внимание!",MessageBoxButton.YesNo,MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        Kashapov_glazkiEntities.GetContext().Agent.Remove(currentAgent);
                        Kashapov_glazkiEntities.GetContext().SaveChanges();
                        MessageBox.Show("Запись удалена");
                    }catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            }

        }

        private void ChangePictureBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog myOpenFileDialog = new OpenFileDialog();
            if (myOpenFileDialog.ShowDialog() == true)
            {
                _currentService.Logo = myOpenFileDialog.FileName;
                LogoImage.Source = new BitmapImage(new Uri(myOpenFileDialog.FileName));
            }
        }

        private void HistBtn_Click(object sender, RoutedEventArgs e)
        {
            Add_sale window = new Add_sale(_currentService);
            window.Show();
        }
    }
}
