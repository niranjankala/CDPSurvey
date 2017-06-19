using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CDPReporting.Business.Models
{
    public class GroupQuestionModel
    {
        public string QuestionGroupId { get; set; }
        public string QuestionGroupName { get; set; }
        public List<SubGroupQuestionModel> SubGroupQuestion { get; set; }

        public GroupQuestionModel()
        {
            SubGroupQuestion = new List<SubGroupQuestionModel>();
        }
    }
    
}