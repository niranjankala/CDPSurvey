using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CDPReporting.Business.Models
{
    public class QuestionModel
    {
        #region notrequired
        //    public Question(QuestionType questionType)
        //    {
        //        type = questionType;
        //        InitializeQuestion();
        //    }
        //    private List<string> options;       

        //    public List<string> Options
        //    {
        //        get { return options; }
        //        set { options = value; }
        //    }

        //    public string QuestionId { get; set; }
        //    public string QuestionGroupId { get; set; }
        //    private QuestionType type;

        //    public QuestionType Type
        //    {
        //        get { return type; }
        //        private set { type = value; }
        //    }

        //    public string QuestionText { get; set; }
        //    void InitializeQuestion()
        //    {
        //        switch (type)
        //        {
        //            case QuestionType.Simple:

        //            case QuestionType.Date:
        //            case QuestionType.DateRange:
        //                break;
        //            case QuestionType.Boolean:
        //            case QuestionType.Option:
        //            case QuestionType.OptionList:
        //            case QuestionType.DropDown:
        //            case QuestionType.DropDownList:
        //                options = new List<string>();
        //                break;
        //            case QuestionType.CDPGrid:
        //                break;
        //        }
        //    }
        //}

        //public class Options
        //{
        //    //public Options(QuestionType questionType, )
        //}
        #endregion
        public Guid Id { get; set; }
        public string QuestionId{ get; set; }
        public string SubGoupQuestionId { get; set; }
        public string QuestionText { get; set; }
        public string TableType { get; set; }
        public string GroupText { get;set;}
        public string SubGroupText { get; set; }
        public QuestionType QuestionType { get; set; }

    }
}