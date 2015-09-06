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

        public string name
        {
            get { return name_; }
            set { name_ = name; }
        }
        public string form
        {
            get { return form_; }
            set { form_ = form; }
        }
        public string URL
        {
            get { return URL_; }
            set { URL_ = URL; }
        }

        public Spec(string in_name, string in_form, string in_URL, List<Semestr> in_LSem)
        {
            LSem_ = new List<Semestr>();
            LSem_.AddRange(in_LSem);
            name = in_name;
            form = in_form;
            URL = in_URL;
        }
    }
}
