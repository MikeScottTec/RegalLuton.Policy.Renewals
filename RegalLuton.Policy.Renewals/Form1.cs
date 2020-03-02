using System;
using System.Configuration;
using System.Windows.Forms;
using RegalLuton.Policy.Renewals.FileService.Interfaces;
using RegalLuton.Policy.Renewals.FileService.Implementation;
using RegalLuton.Policy.Renewals.Processor.Implementation;
using RegalLuton.Policy.Renewals.Processor.Calculators;
using RegalLuton.Policy.Renewals.Processor.Interfaces;

namespace RegalLuton.Policy.Renewals
{
    public partial class Form1 : Form
    {
        private string sourceFile;
        private string targetFolder;
        public Form1()
        {
            InitializeComponent();

            sourceFile = ConfigurationManager.AppSettings["CustomerInputSourceFile"];
            targetFolder = ConfigurationManager.AppSettings["CustomerTargetFolder"];
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            IFileHandler fileHandler = new FileHandler();
            ICalculator paymentCalculator = new PaymentCalculator();

            CustomerProcessor customerProcessor = new CustomerProcessor(fileHandler, paymentCalculator);

            CustomerFacade cf = new CustomerFacade(customerProcessor);
            cf.Process(sourceFile, targetFolder);

            MessageBox.Show("Processing completed.", "Policy Renewals", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
