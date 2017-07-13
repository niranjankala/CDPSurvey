using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDPReporting.Business.Models
{
    public class QuestionResponseTableTypeModel
    {
        public Guid Id { get; set; }
        public int year { get; set; }
        public Guid QuestionId { get; set; }
        public string Answer { get; set; }
       
    }
}
