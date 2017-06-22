using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CDPReporting.Business.Models
{
    public class SidePanelQuestions
    {
        public Guid QuestionGuid { get; set; }
        public string QuestionId { get; set; }
        public string QuestionGroupId { get; set; }
        public string QuestionSubGroupId { get; set; }

        public string QuestionText { get; set;}
        public Guid TableId { get; set; }
        public int QuestionOrder { get; set; }
    }
}