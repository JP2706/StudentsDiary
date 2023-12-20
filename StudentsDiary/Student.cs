using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsDiary
{
    public class Student
    {

        public int Id { get; set; }
        public string FirstName { get; set; }   
        public string SurName { get; set; }
        public string Math { get; set; }
        public string Technology { get; set; }
        public string LangPol { get; set; }
        public string LangOther { get; set; }
        public string Comments { get; set; }
        public bool ExtraActivites {  get; set; }
        public int GroupId { get; set; }

    }
}
