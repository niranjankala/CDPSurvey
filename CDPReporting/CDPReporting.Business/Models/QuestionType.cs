﻿namespace CDPReporting.Business.Models
{
    public enum QuestionType
    {
        Simple,
        List,
        DropDown,
        DropDownList,
        Option,
        OptionList,
        DateRange,
        Date,
        Boolean,
        CDPGrid,
        MultipleSelectList,
        CDPGridResultList,
        NumericField
    }

    public enum ValidationType
    {
        None,
        Length,
        DefaultValue
    }
}