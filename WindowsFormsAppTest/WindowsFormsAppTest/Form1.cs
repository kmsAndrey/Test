using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Data.Entity;

namespace WindowsFormsAppTest
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Part logics application
        /// </summary>
        App app;

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Download tree
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            app = new App();
            treeView1.Nodes.Add(app.LoadDataTreeDepartment());
        }

        /// <summary>
        /// Download list empoyee
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            empoyeeBindingSource.DataSource = app.SelectedNodeDepartment(e.Node);
        }

        /// <summary>
        /// Update or Save data empoyee
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            decimal employeeId;
            try
            {
                employeeId = Convert.ToDecimal(textBoxID.Text);
            }
            catch
            {
                MessageBox.Show("Не выбран отдел");
                return;
            }
            Empoyee employee = new Empoyee() {
                    ID = Convert.ToDecimal(employeeId),
                    DepartmentID = app.number,
                    FirstName = textBoxFirstName.Text.Trim(),
                    SurName = textBoxSurName.Text.Trim(),
                    DateOfBirth = dateTimePickerDateOfBirthday.Value,
                    Position = textBoxPosition.Text.Trim(),
                    Patronymic = textBoxPatronymic.Text.Trim(),
                    DocNumber = maskedTextBoxDocNumber.Text,
                    DocSeries = maskedTextBoxDocSeries.Text

            };
            ClearTextBox();
            empoyeeBindingSource.DataSource = app.UpdateAndSaveEmpoyee(employee, employeeId);

        }

        /// <summary>
        /// Download Empoyee
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            if(dataGridViewEmpoyees.CurrentRow.Index!=-1)
            {
                decimal numberId = Convert.ToDecimal(dataGridViewEmpoyees.CurrentRow.Cells["ID"].Value);
                Empoyee empoyee = app.LoadSelectedEmpoyee(numberId);

                textBoxID.Text = empoyee.ID.ToString();
                textBoxDepartmentID.Text = empoyee.DepartmentID.ToString();
                textBoxFirstName.Text = empoyee.FirstName;
                textBoxSurName.Text = empoyee.SurName;
                dateTimePickerDateOfBirthday.Value = empoyee.DateOfBirth;
                textBoxPosition.Text = empoyee.Position;
                textBoxPatronymic.Text = empoyee.Patronymic;
                maskedTextBoxDocNumber.Text = empoyee.DocNumber;
                maskedTextBoxDocSeries.Text = empoyee.DocSeries;
            }
        }

        /// <summary>
        /// Event clear
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            ClearTextBox();
        }

        /// <summary>
        /// Calculate age empoyee
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dateTimePickerDateOfBirthday_ValueChanged(object sender, EventArgs e)
        {
            DateTime dateNow = DateTime.Now;
            int year = dateNow.Year - dateTimePickerDateOfBirthday.Value.Year;
            if (dateNow.Month < dateTimePickerDateOfBirthday.Value.Month ||
                (dateNow.Month == dateTimePickerDateOfBirthday.Value.Month && dateNow.Day < dateTimePickerDateOfBirthday.Value.Day)) year--;
            labelAge.Text = year.ToString();
        }

        /// <summary>
        /// Clear input data
        /// </summary>
        private void ClearTextBox()
        {
            textBoxID.Text = "0";
            textBoxFirstName.Clear();
            textBoxSurName.Clear();
            dateTimePickerDateOfBirthday.Value = DateTime.Now;
            textBoxPosition.Clear();
            textBoxPatronymic.Clear();
            maskedTextBoxDocNumber.Clear();
            maskedTextBoxDocSeries.Clear();
        }
    }
}
