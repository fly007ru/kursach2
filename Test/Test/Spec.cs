using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class Spec
    {
        string name_, form_, URL_;
        public List<Semestr> LSem_;
        bool state_;
        public bool state
        {
            get { return state_; }
            set { state_ = value; }
        }
        public string name
        {
            get { return name_; }
            set { name_ = value; }
        }
        public string form
        {
            get { return form_; }
            set { form_ = value; }
        }
        public string URL
        {
            get { return URL_; }
            set { URL_ = value; }
        }

        public Spec(string in_name, string in_form, string in_URL, List<Semestr> in_LSem)
        {
            LSem_ = new List<Semestr>();
            if (in_LSem.Count!=0)
            LSem_.AddRange(in_LSem);
            name = in_name;
            form = in_form;
            URL = in_URL;
        }
    }
}
