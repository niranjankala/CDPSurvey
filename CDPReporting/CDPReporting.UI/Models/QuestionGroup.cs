using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CDPReporting.UI.Models
{
    public class QuestionGroup
    {
        public string QuestionGroupId { get; set; }
        public string QuestionGroupName { get; set; }
        public List<Question> Questions { get; set; }
        public QuestionGroup()
        {
            Questions = new List<Question>();
        }
    }
    
}