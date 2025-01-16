
 using App = HostMgd.ApplicationServices;
 using Db = Teigha.DatabaseServices;
 using Ed = HostMgd.EditorInput;
 using Rtm = Teigha.Runtime;
 using Proc = System.Diagnostics.Process; // для запуска explorer чтобы открыть папку
using System;
using nanoCAD_PRPR_WPF;
using System.Windows;


[assembly: Rtm.CommandClass(typeof(Tools.CadCommand))]

namespace Tools
{
    /// <summary> 
    /// Комманды
    /// </summary>
    class CadCommand : Rtm.IExtensionApplication
    {
        public void Initialize()
        {

            App.DocumentCollection dm = App.Application.DocumentManager;
            Ed.Editor ed = dm.MdiActiveDocument.Editor;

            string sCom = "prprtestwpf - test WPF";
            ed.WriteMessage(sCom);
        }

        public void Terminate()
        {
            // Пусто
        }

        /// <summary>
        /// Основная команда для вызова из командной строки
        /// </summary>
        [Rtm.CommandMethod("PRPR_testWPF")]

        /// <summary>
        /// Это основной метод 
        /// </summary>
        public static void prprtestwpf()
        {
            Window1 mainWin = new Window1();
            mainWin.ShowDialog();
        }

        /// <summary>
        /// Это один из обработчиков мультикада. 
        /// Для примера он выводит значение из формы в консоль и в сообщение 
        /// а так же возвращает принятое на вход знаяение + дополнительный текст.
        /// </summary>
        /// /// <param name="value">Описание параметра</param>
        /// <returns>возвращаемое значение - это строка с результатом обработки</returns>
        /// 
        public static string PRPR_dataProc(string value)
        {
            Db.Database db = Db.HostApplicationServices.WorkingDatabase;
            App.Document doc = App.Application.DocumentManager.MdiActiveDocument;
            Ed.Editor ed = doc.Editor;

            ed.WriteMessage(value);
            MessageBox.Show($"Сообщение вызвано обработчиком мультикад. Из поля формы WPF получено значение: {value}", "Окно сообщения");

            // иммитация работы обработчика (добавяляет к входному значению текст)
            string retVal = value + " + данные из обработчика";

            return retVal; // Обработчик вернет результат свой работы
        }
    }
}


