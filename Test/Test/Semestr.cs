using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class Semestr
    {
        int number_;
        public List<Subject> LS_;

        public int number
        {
            get { return number_; }
            set
            {
                if (number < 0) number_ = 0; else number_ = number;

            }
        }

        public Semestr(int in_number, List<Subject> in_LS)
        {
            LS_ = new List<Subject>();
            LS_.AddRange(in_LS);
            number = in_number;
        }
    }
}
