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
using System.Windows.Shapes;

namespace Kashapov_glazki
{
    /// <summary>
    /// Логика взаимодействия для ChangePrior.xaml
    /// </summary>
    public partial class ChangePrior : Window
    {

        private List<Agent> _currentService;
        private int id = 0;
        List<AgentType> AgentTypesDBList = Kashapov_glazkiEntities.GetContext().AgentType.ToList();
        public ChangePrior(List<Agent> SelectedAgent)
        {
            InitializeComponent();
            if (SelectedAgent != null)
            {
                _currentService = SelectedAgent;


            }
            int prior = 0;
            foreach (var item in _currentService)
            {
                if (item.Priority > prior)
                {
                    prior = item.Priority;
                    id = item.ID;
                }

            }
            PriorityBox.Text = prior.ToString();
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            foreach (var item in _currentService)
            {
                
                item.Priority = Convert.ToInt32(PriorityBox.Text);
                if (item.Priority <= 0)
                {
                    errors.AppendLine("Укажите положительный приоритет агента");

                }
                if (errors.Length > 0)
                {
                    MessageBox.Show(errors.ToString());
                    return;
                }
                try
                {
                    MessageBox.Show(PriorityBox.Text);
                    Kashapov_glazkiEntities.GetContext().SaveChanges();
                    MessageBox.Show("Информация сохранена");
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            var currentServices = Kashapov_glazkiEntities.GetContext().Agent.ToList();
            currentServices = currentServices.ToList();
        }

        private void History_Click(object sender, RoutedEventArgs e)
        {
           
        }
    }
}
