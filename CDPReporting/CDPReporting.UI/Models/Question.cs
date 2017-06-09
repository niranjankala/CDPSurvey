using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CDPReporting.UI.Models
{
    public class Question
    {
        public string QuestionId { get; set; }
        public string QuestionGroupId { get; set; }
        public QuestionType Type { get; set; }
        public string QuestionText { get; set; }
    }

    public enum QuestionType
    {
        Simple = 0,
        Option = 1,
        DateRange = 2
    }
}