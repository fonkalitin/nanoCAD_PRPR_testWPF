
using System;
using System.Diagnostics;
using System.IO;
using AppServ = HostMgd.ApplicationServices;
using Application = HostMgd.ApplicationServices.Application;
using Db = Teigha.DatabaseServices;
using Ed = HostMgd.EditorInput;
using Rtm = Teigha.Runtime;
using Proc = System.Diagnostics.Process; // для запуска explorer чтобы открыть папку
using nanoCAD_PRPR_updcheck;
using System.Windows;
using PRPR_UpdateManager;
using UpdateManager = PRPR_UpdateManager.UpdateManager;
using System.Reflection;

[assembly: Rtm.CommandClass(typeof(updateCheck.CadCommand))]

namespace updateCheck
{
    /// <summary> 
    /// Комманды
    /// </summary>
    class CadCommand : Rtm.IExtensionApplication
    {
        public void Initialize()
        {

            AppServ.DocumentCollection dm = AppServ.Application.DocumentManager;
            Ed.Editor ed = dm.MdiActiveDocument.Editor;

            string sCom = "PRPR_updcheck - проверка доступного обновления версии PROMPROEKTOR®";
            ed.WriteMessage(sCom);

            PRPR_updcheck();
        }

        public void Terminate()
        {
            // Пусто
        }


        /// <summary>
        /// Основная команда для вызова из командной строки
        /// </summary>
        [Rtm.CommandMethod("PRPR_updcheck")]

        /// <summary>
        /// Это основной метод 
        /// </summary>
        public static async void PRPR_updcheck()
        {
            //nanoCAD_PRPR_updcheck.MainWindow mainWin = new nanoCAD_PRPR_updcheck.MainWindow();
            //mainWin.ShowDialog();

            // Вызываем асинхронный метод и обрабатываем результат
            string checkUpdateResult = await UpdateManager.CheckUpdateNowAsync();

            AppServ.DocumentCollection dm = AppServ.Application.DocumentManager;
            Ed.Editor ed = dm.MdiActiveDocument.Editor;

            string sCom = "PRPR_updcheck - проверка доступного обновления версии PROMPROEKTOR®";
            ed.WriteMessage(checkUpdateResult);


            if (checkUpdateResult.Contains("Доступна новая версия")) 
            {
                bool userChoice = ShowUpdateMessageBox(checkUpdateResult);

                if (userChoice)
                {
                    // Логика, если пользователь выбрал "Да"
                    startUpdateManager();
                    Application.DocumentManager.MdiActiveDocument.CloseAndDiscard();
                    //await Task.Delay(500);
                    //Application.Quit();
                }
                else
                {
                    // Логика, если пользователь выбрал "Нет"
                    ed.WriteMessage("PRPR_updcheck: отказ пользователя от обновления");
                }
            }

        }

        public static bool ShowUpdateMessageBox(string version)
        {
            string message = "Автообновление PROMPROEKTOR®:\n" +
                             $"{version}.\n" +
                             "Перед началом установки будет автоматически закрыт nanoCAD.\n" +
                             "\n" +
                             "Выполнить закрытие и установку прямо сейчас?";

            MessageBoxResult result = MessageBox.Show(
                message,
                "Автообновление PROMPROEKTOR®",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            // Возвращаем true, если пользователь нажал "Да", иначе возвращаем false
            return result == MessageBoxResult.Yes;
        }

        public static void startUpdateManager()
        {
            // Получаем путь к текущей сборке
            string currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            // Формируем полный путь к исполняемому файлу
            string exePath = Path.Combine(currentDirectory, "PRPR_UpdateManager.exe");

            // Проверяем, существует ли файл
            if (!File.Exists(exePath))
            {
                throw new FileNotFoundException("Не удалось найти модуль обновления PRPR_UpdateManager", exePath);
            }

            // Запускаем процесс
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = exePath,
                Arguments = "-update"
                //UseShellExecute = false, // Запуск без использования оболочки
                //CreateNoWindow = true,  // Не создавать окно
                //RedirectStandardOutput = true, // Перенаправить стандартный вывод (если нужно)
                //RedirectStandardError = true   // Перенаправить стандартные ошибки (если нужно)
            };

            using (Process process = Process.Start(startInfo))
            {
                if (process != null)
                {
                    //process.WaitForExit(); // дождаться завершения процесса, если это необходимо
                }
                else
                {
                    throw new Exception("Не удалось запустить процесс");
                }
            }
        }



    }
}


