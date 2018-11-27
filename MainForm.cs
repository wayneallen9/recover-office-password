using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ninject.Parameters;
using System.Runtime.InteropServices;

namespace RecoverExcelPassword
{
    public partial class MainForm : Form, IInvoker
    {
        #region winapi
        #endregion

        #region variables
        private readonly Services.ILogServices _logServices;
        private readonly Services.IWindowServices _windowServices;
        #endregion

        public MainForm(
            Services.ILogServices logServices,
            Services.IWindowServices windowServices)
        {
            // save the dependency injections
            _logServices = logServices;
            _windowServices = windowServices;

            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _logServices.TraceEnter();
            try
            {
                // creating view model
                _logServices.Trace("Creating view model...");
                var invokerParameter = new ConstructorArgument("invoker", this);
                var viewModel = Services.Kernel.Get<MainFormViewModel>(invokerParameter);

                _logServices.Trace("Assigning it to the data source...");
                bindingSourceForm.DataSource = viewModel;
            }
            catch (Exception ex)
            {
                _logServices.Error(ex);

                MessageBox.Show(Properties.Resources.ERROR_MESSAGE_TEXT, Properties.Resources.ERROR_MESSAGE_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _logServices.TraceExit();
            }
        }

        private void DataGridViewWindows_SelectionChanged(object sender, EventArgs e)
        {
            _logServices.TraceEnter();
            try
            {
                var viewModel = bindingSourceForm.DataSource as MainFormViewModel;

                _logServices.Trace("Checking if there is a selected window...");
                if (dataGridViewWindows.SelectedRows.Count == 0)
                {
                    _logServices.Trace("There is no selected window.");

                    viewModel.SelectedWindow = null;
                }
                else
                {
                    _logServices.Trace($"Getting selected window...");
                    var selectedWindow = dataGridViewWindows.SelectedRows[0].DataBoundItem as Models.Window;

                    _logServices.Trace($"Saving \"{selectedWindow} as the selected window...");
                    viewModel.SelectedWindow = selectedWindow;
                }
            }
            catch (Exception ex)
            {
                _logServices.Error(ex);

                MessageBox.Show(Properties.Resources.ERROR_MESSAGE_TEXT, Properties.Resources.ERROR_MESSAGE_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _logServices.TraceExit();
            }
        }

        private void ButtonStart_Click(object sender, EventArgs e)
        {
            _logServices.TraceEnter();
            try
            {
                var viewModel = bindingSourceForm.DataSource as MainFormViewModel;

                _logServices.Trace("Showing progress form...");
                var windowHandleParameter = new ConstructorArgument("windowHandle", viewModel.SelectedWindow.Handle);
                var progressForm = Services.Kernel.Get<ProgressForm>(windowHandleParameter);
                progressForm.ShowDialog();
            }
            catch (Exception ex)
            {
                _logServices.Error(ex);

                MessageBox.Show(Properties.Resources.ERROR_MESSAGE_TEXT, Properties.Resources.ERROR_MESSAGE_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _logServices.TraceExit();
            }
        }
    }
}