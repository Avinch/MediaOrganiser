using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaOrganiser.Model
{
    public class Category
    {
        public Category(int id, string name)
        {
            Id = id;
            Name = name;
        }

        private int _id;

        public int Id   
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
    }
}
