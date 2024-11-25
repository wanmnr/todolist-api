using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace todolist_api.Data
{
    public partial class Todo
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public bool Complete { get; set; }
    }
}
