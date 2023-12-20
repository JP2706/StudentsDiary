using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentsDiary
{
    public partial class AddEditStudent : Form
    {

        private FileHelper<List<Student>> fileHelper = new FileHelper<List<Student>>(Path.Combine(Environment.CurrentDirectory, "Students.txt"));
        private int studentId;
        private Student student;
        private List<Group> groups = GroupHelper.GetGroups("Brak");
        
        public AddEditStudent(int id = 0)
        {
            InitializeComponent();
            InitGroupCombobox();
            studentId = id;
            GetStudentData();
            
        }

        private void InitGroupCombobox()
        {
            cbGroupAES.DataSource = groups;
            cbGroupAES.DisplayMember = "Name";
            cbGroupAES.ValueMember = "Id";
        }

        private void GetStudentData()
        {
            if (studentId != 0)
            {
                Text = "Edytowanie danych studenta";
                var students = fileHelper.DeserializeFromFile();
                student = students.FirstOrDefault(x => x.Id == studentId);
                if (student == null)
                    throw new Exception("Brak użytkownika o podanym id");
                FillTextBox();
                
            }
        }

        private void FillTextBox()
        {
            tbId.Text = student.Id.ToString();
            tbName.Text = student.FirstName;
            tbSurname.Text = student.SurName;
            tbMath.Text = student.Math;
            tbTech.Text = student.Technology;
            tbLangPol.Text = student.LangPol;
            tbLangOder.Text = student.LangOther;
            rtComments.Text = student.Comments;
            cbExAc.Checked = student.ExtraActivites;
            cbGroupAES.SelectedItem = groups.FirstOrDefault(x => x.Id == student.GroupId);
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {

            var students = fileHelper.DeserializeFromFile();

            if(studentId != 0)
            {
                students.RemoveAll(x => x.Id == studentId);
            }
            else
            {
                AssignIdToNewStudent(students);
            }

            AddNewUserToList(students);

            fileHelper.SerializeToFile(students);

            Close();

           
        }

        private void AssignIdToNewStudent(List<Student> students)
        {
            var studentsWithHighestId = students.OrderByDescending(x => x.Id).FirstOrDefault();

            if (studentsWithHighestId == null)
            {
                studentId = 1;
            }
            else
            {
                studentId = studentsWithHighestId.Id + 1;
            }
        }

        private void AddNewUserToList(List<Student> students)
        {
            Student student = new Student()
            {
                Id = studentId,
                FirstName = tbName.Text,
                SurName = tbSurname.Text,
                Math = tbMath.Text,
                Technology = tbTech.Text,
                LangPol = tbLangPol.Text,
                LangOther = tbLangOder.Text,
                Comments = rtComments.Text,
                ExtraActivites = cbExAc.Checked,
                GroupId = (cbGroupAES.SelectedItem as Group).Id,
                

            };

            students.Add(student);
        }

        
    }
}
