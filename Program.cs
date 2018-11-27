using System;
using System.Windows.Forms;

namespace RecoverExcelPassword
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // configure AutoMapper
            AutoMapper.Mapper.Initialize(c => c.AddProfile<AutoMapperProfile>());

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // create the form
            var mainForm = Services.Kernel.Get<MainForm>();

            // now show the form
            Application.Run(mainForm);
        }
    }
}