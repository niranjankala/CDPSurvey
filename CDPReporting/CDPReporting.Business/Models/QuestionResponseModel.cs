using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDPReporting.Business.Models
{
    public class QuestionResponseModel
    {
        public Guid AnswerId { get; set; }
        public Guid QuestionId { get; set; }
        public string Caption { get; set; }
        public string QuestionText { get; set; }
        public int Year { get; set; }
        public Object Value { get; set; }
        public QuestionType QuestionType { get; set; }
        public Options OptionList { get; set; }

    }

    public class DateRange
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class Options
    {
        public Guid OptionId { get; set; }
        public Guid QuestionId { get; set; }
        public List<string> OptionCSVText { get; set; }
        public string OtherOptions { get; set; }
    }
    
}
