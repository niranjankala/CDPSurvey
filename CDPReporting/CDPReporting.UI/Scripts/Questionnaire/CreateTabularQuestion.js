
$(document).ready(function () {
    debugger;
    var questionId = document.getElementById("TabularDiv").getAttribute("questionId");
    var questiontype = document.getElementById("TabularDiv").getAttribute("questiontype");

    $.ajax({
        type: 'Get',
        url: tableStructureURL,
        dataType: "json",
        data: { questionId: questionId },
        success: function (data) {
            var result = JSON.parse(data.data);
            debugger;

            $('#tabularQuestion').append(CreateHtmlTable(result));
           
            $('#multiselectdiv').multiselect();
            if (questiontype == 'CDPGrid') {
                debugger;
                SetInitialValuesForColumn();
            }
            
        },

        error: function (data) {
            debugger;
            alert("An error has occured!!!");
        }

    });

    



   

});






function CreateHtmlTable(data) {
    debugger;
    
    var html = "<div class=\"table-responsive\">" + "<table id=\"tabularQuestionTable\" class=\"table table-bordered\">"+ "<thead>"+"<tr>"
     
    $.each(data[0].Headers, function (i, value) {
        html += "<th>" + value+"</th>";
    });

    html+="</tr></thead>"
    
    html += "<tbody>"
    html+="<tr>"
       $.each(data, function (i, value) {
           if (value.hasOwnProperty('ColType')) {
               html += ("<td>" + SelectTypeCreate(value) + "</td>");
           }
           
       });
       html += "</tr>";
       html += "</tbody> </table> </div>"
       return html;
}


function SelectTypeCreate(data)
{
    debugger;
   

        if (data.hasOwnProperty('ColType')) {
            if (data.ColType == "SingleSelect") {
               return CreateSimpleDropDown(data);
            }
            else if (data.ColType == "MultiSelect") {
                return CreateMultiSelectDropDown(data);
            }
            else if (data.ColType == "Textfield") {
                return CreateTextBox();
            }

            else if (data.ColType == "DateType") {
                return createInputDateType();
            }

            else if (data.ColType == "InputType") {
                return createInputNumberType();
            }

            else if (data.ColType == "Text") {
                return CreateSimpleText(data);
            }
            else if (data.ColType == "Numerical") {
                return createInputNumberType();
            }

            else if (data.ColType == "DateType") {
                return createInputDateType();
            }

            else if (data.ColType == "AttachmentType") {
                return UploadFile(data.SelectText);
            }

        }
    
    
}

function CreateSimpleDropDown(data)
{
    debugger;
    //count = 0;
    //var DropDownId = data
    var html = "<div class=\"btn-group\" id=\"SimpleDropDownDiv\">";
   
    html += "<button class=\"btn dropdown-toggle\" name=\"recordinput\" data-toggle=\"dropdown\">"+"Select From"+"<span class=\"caret\">"+"</span>"+"</button>"+"<ul class=\"dropdown-menu\" style=\"max-height:250px; overflow-y:scroll\">";
    for (i = 0; i < data.Elements.length; i++) {
        //html = "<option value='" + datea[i] + "'>" + datea[i] + "</option>";
        html += "<li><a>" + data.Elements[i] + "</a>" + "</li>";
        
    }
    html += "</ul>";
    html += "</div>";
    return html;
}


function CreateMultiSelectDropDown(data)
{
    debugger;
    //var newSelect = document.createElement('select');
    //newDiv.setAttribute("id", "MuliselectId");
    //newDiv.setAttribute("multiple", "multiple");
    //newDiv.addClass('class_two');
    var html="<select id=\"multiselectdiv\" name=\"selectmultiple\" multiple=\"multiple\">"+"Multiple select from"
    for (i = 0; i < data.Elements.length; i++) {
        html += "<option>" + data.Elements[i] + "</option>";
    }
    html+="</select>"
    
    return html;
}

function CreateTextBox()
{
    debugger;
    var html = "<textarea class=\"form-control\" rows=\"5\" id=\"comment\">" + "</textarea>"
    return html;
     }

function createInputNumberType()
{

    var InputType="<input type=\"number\"  id=\"numField\" name=\"NumberEnter\" >";
    return InputType;

}

function createInputDateType()
{
    var DateType = "<input type=\"date\"  id=\"dateField\" name=\"DateEnter\"  max=\"2016-12-31\">";
    return DateType;
}

function CreateSimpleText(data)
{
    return data;
}

function UploadFile(selectText)
{
    var FileUpload = "<label id=\"myFile\">" + selectText + "</label>";
    
    return FileUpload;
}
