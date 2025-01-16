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

namespace nanoCAD_PRPR_WPF
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обработчик события "Нажатие" кнопки
        /// </summary>
        private void myActionButton_Click(object sender, RoutedEventArgs e)
        {
            // Запись нового значения поля. Данные получены с выхода обработчика мультикад.
            // В качестве аргумента функции обработчику передано значение из первого поля формы WPF
            getValBox.Text = Tools.CadCommand.PRPR_dataProc(setValBox.Text);
        }
    }
}
