using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class Subject
    {
        string profile_, number_, cikl_, name_, forma_;
        int time_;
        bool check_;

        public bool check
        {
            get { return check_; }
            set {check_=value;}
        }
        public string profile
        {
            get { return profile_; }
            set { profile_ = value; }
        }

        public string number
        {
            get { return number_; }
            set { number_ = value; }

        }

        public string cikl
        {
            get { return cikl_; }
            set { cikl_ = value; }
        }
        public string name
        {
            get { return name_; }
            set { name_ = value; }
        }

        public string forma
        {
            get { return forma_; }
            set { forma_ = value; }
        }
        public int time
        {
            get { return time_; }
            set
            {
                if (time < 0) time_ = 0; else time_ = value;

            }
        }
        public Subject(string in_profile, string in_number, string in_cikl, string in_name, string in_forma, int in_time)
        {
            profile = in_profile;
            number = in_number;
            cikl = in_cikl;
            name = in_name;
            forma = in_forma;
            time = in_time;
            check = false;
        }
    }
}
