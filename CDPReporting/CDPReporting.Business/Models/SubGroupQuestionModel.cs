using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CDPReporting.Business.Models
{
    public class SubGroupQuestionModel
    {
        public string SubQuestionGroupId { get; set; }
        public string SubQuestionGroupName { get; set; }

         public List<QuestionModel> Question { get; set; }

         public SubGroupQuestionModel()
        {
            Question = new List<QuestionModel>();
        }
    }
}