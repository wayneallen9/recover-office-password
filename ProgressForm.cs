using Ninject.Parameters;
using System;
using System.Windows.Forms;

namespace RecoverExcelPassword
{
    public partial class ProgressForm : Form, IInvoker
    {
        #region variables
        private readonly Services.ILogServices _logServices;
        private readonly IntPtr _windowHandle;
        #endregion

        public ProgressForm(
            IntPtr windowHandle,
            Services.ILogServices logServices)
        {
            // save dependency injections
            _logServices = logServices;
            _windowHandle = windowHandle;

            InitializeComponent();
        }

        private void ProgressForm_Load(object sender, EventArgs e)
        {
            _logServices.TraceEnter();
            try
            {
                _logServices.Trace("Creating view model...");
                var invokerParameter = new ConstructorArgument("invoker", this);
                var viewModel = Services.Kernel.Get<ProgressFormViewModel>(invokerParameter);

                _logServices.Trace("Binding to form...");
                bindingSourceForm.DataSource = viewModel;

                _logServices.Trace("Starting password testing...");
                viewModel.Start(_windowHandle);
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

        private void BindingSourceForm_CurrentItemChanged(object sender, EventArgs e)
        {
            _logServices.TraceEnter();
            try
            {
                var viewModel = bindingSourceForm.DataSource as ProgressFormViewModel;

                _logServices.Trace("Updating unbound view model properties...");
                toolStripStatusLabelTime.Text = viewModel.Elapsed.ToString(@"dd\ hh\:mm\:ss");

                _logServices.Trace("Checking if password has been cracked...");
                if (viewModel.Success)
                {
                    MessageBox.Show($"The password is {viewModel.Password}.", "Password Found", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Close();
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

        private void ProgressForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _logServices.TraceEnter();
            try
            {
                var viewModel = bindingSourceForm.DataSource as ProgressFormViewModel;

                _logServices.Trace("Stopping password testing...");
                viewModel.Stop();
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