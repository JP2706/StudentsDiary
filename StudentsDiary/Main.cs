using StudentsDiary.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace StudentsDiary
{
    public partial class Main : Form
    {
      
        private FileHelper<List<Student>> fileHelper = new FileHelper<List<Student>>(Path.Combine(Environment.CurrentDirectory, "Students.txt"));
        private List<Group> groups = GroupHelper.GetGroups("Wszyscy");

        public bool IsMaximize
        {
            get
            {
                return Settings.Default.IsMaximize;
            }
            set
            {
                Settings.Default.IsMaximize = value;
            }
        }

        public Main()
        {
            InitializeComponent();
            InitGroupCombobox();
            var students = fileHelper.DeserializeFromFile();
            dgvDiary.DataSource = students;
            SetColumnHeader();
            HideColumns();
            if(IsMaximize)
                WindowState = FormWindowState.Maximized;

        }
        private void InitGroupCombobox()
        {
            cbGroupM.DataSource = groups;
            cbGroupM.DisplayMember = "Name";
            cbGroupM.ValueMember = "Id";
        }

        private void SetColumnHeader()
        {
            dgvDiary.Columns[0].HeaderText = "Id";
            dgvDiary.Columns[1].HeaderText = "Imię";
            dgvDiary.Columns[2].HeaderText = "Nazwisko";
            dgvDiary.Columns[3].HeaderText = "Matematyka";
            dgvDiary.Columns[4].HeaderText = "Technologia";
            dgvDiary.Columns[5].HeaderText = "Język Polski";
            dgvDiary.Columns[6].HeaderText = "Język Obcy";
            dgvDiary.Columns[7].HeaderText = "Uwagi";
            dgvDiary.Columns[8].HeaderText = "Zajęcia Dod";
        }



        private void btnAdd_Click(object sender, EventArgs e)
        {
            var addEditStudent = new AddEditStudent();
            addEditStudent.ShowDialog();

            var students = fileHelper.DeserializeFromFile();
            dgvDiary.DataSource = students;

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvDiary.SelectedRows.Count == 0)
                MessageBox.Show("Zaznacz ucznia którego chcesz edytować!");

            var addEditStudent2 = new AddEditStudent(Convert.ToInt32(dgvDiary.SelectedRows[0].Cells[0].Value));
            addEditStudent2.ShowDialog();

            var studentsRefuse = fileHelper.DeserializeFromFile();
            dgvDiary.DataSource = studentsRefuse;
            

        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if(dgvDiary.SelectedRows.Count == 0)
            {
                MessageBox.Show("Proszę o zaznacznie ucznia którego chcesz usunąć!");
                return;
            }

            var selectedStudent = dgvDiary.SelectedRows[0];

            var confirmDelete = MessageBox.Show($"Czy napewno chcesz usunąć ucznia {selectedStudent.Cells[1].Value.ToString() + " " + selectedStudent.Cells[2].Value.ToString().Trim()}", "Usuwanie", MessageBoxButtons.YesNo);

            if(confirmDelete == DialogResult.Yes)
            {
                var students = fileHelper.DeserializeFromFile();
                students.RemoveAll(x => x.Id == Convert.ToInt32(selectedStudent.Cells[0].Value));
                fileHelper.SerializeToFile(students);
                dgvDiary.DataSource = students;
            }

            var studentsRefuse = fileHelper.DeserializeFromFile();
            dgvDiary.DataSource = studentsRefuse;
        }

        private void btnRef_Click(object sender, EventArgs e)
        {

            FiltrStudentClass();

        }
            
        private void FiltrStudentClass()
        {
            var students = fileHelper.DeserializeFromFile();
            var studentsListFiltr = new List<Student>();
            if ((cbGroupM.SelectedItem as Group).Id != 0)
            {
                foreach (var student in students)
                {
                    if (student.GroupId == (cbGroupM.SelectedItem as Group).Id)
                        studentsListFiltr.Add(student);

                }
                dgvDiary.DataSource = studentsListFiltr;
            }
            else
                dgvDiary.DataSource = students;
            
        }

        private void HideColumns()
        {
            dgvDiary.Columns[nameof(Student.GroupId)].Visible = false;
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(WindowState == FormWindowState.Maximized)
                IsMaximize = true;
            else
                IsMaximize = false;

            Settings.Default.Save();
        }
    }
}
