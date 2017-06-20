
$(document).ready(function () {
    // Hide all Question Response
    var selectedDivId;        
    $("#questionListDiv>div>div>div>div").click(function () {
        debugger;
        selectedDivId = (this.id).replace(".", "_");
        $('#questionResponseDiv').load(questionViewURL + "?questionViewId=" + selectedDivId);

        //$('div[id^="CC"]').hide();
      //  $("#"+selectedDivId.replace(".", "\\.")).show();

        // Multiselect
    //    $('#countryId').multiselect();
    //    $('#Question_CC12a_Col3_List').multiselect();
    //    $('input[name="daterange"]').daterangepicker();

    //    // Dropdown menu
    //    $("#CC0_4DropDown .dropdown-menu li a").click(function () {

    //        $("#CC0_4DropDown .btn:first-child").text($(this).text());
    //        $("#CC0_4DropDown .btn:first-child").val($(this).text());

    //    });

    //    $("#Question_CC12a_Col1 .dropdown-menu li a").click(function () {

    //        $("#Question_CC12a_Col1 .btn:first-child").text($(this).text());
    //        $("#Question_CC12a_Col1 .btn:first-child").val($(this).text());

    //    });
    //    $("#Question_CC12a_Col2 .dropdown-menu li a").click(function () {

    //        $("#Question_CC12a_Col2 .btn:first-child").text($(this).text());
    //        $("#Question_CC12a_Col2 .btn:first-child").val($(this).text());

    //    });

    //    // Add More for Table Type Question Response     

    //    $('#addMore').on('click', function () {
    //        var col1Text = $("#Question_CC12a_Col1_List option:selected ").text();
    //        var col1Val = $("#Question_CC12a_Col1_List option:selected ").val();
    //        var col2Text = $("#Question_CC12a_Col2_List option:selected ").text();
    //        var col2Val = $("#Question_CC12a_Col2_List option:selected ").val();
    //        var comment = $("#Comment_CC12a").val();
    //        var col3Text = [];
    //        var col3Val = [];
    //        $('#Question_CC12a_Col3_List :selected').each(function (i, sel) {
                
    //            col3Text.push($(sel).text());
    //            col3Val.push($(sel).val());

    //        });
    //        $('#GovernanceAnswertable').append('<tr><td>' + col1Text + '</td><td>' + col2Text + '</td><td>' + col3Text + '</td><td>' + comment + '</td></tr>');
            
    //        $('#GovernanceAnswertableIds').append('<tr><td>' + col1Val + '</td><td>' + col2Val + '</td><td>' + col3Val + '</td><td>' + comment + '</td></tr>');

           
    //    });

    //    $('#Question_CC1_2a_Save').click(function () {
    //        debugger;
    //        //var $row = $("#GovernanceAnswertableIds").closest('tr');
    //        //var $columns = $row.find('td');

    //        //var modelArray = [];
    //        //var values;

    //        //jQuery.each($columns, function (i, item) {
    //        //    values = values + 'GridCol' + (i + 1) + ':' + item.innerHTML ;

    //        //    modelArray.push(values);       
    //        //var data = [];
    //        //var data = $("#GovernanceAnswertableIds tr.data").map(function (index, elem) {
    //        //    var ret = [];
    //        //    $('.inputValue', this).each(function () {
    //        //        var d = $(this).val() || $(this).text();
    //        //        ret.push(d);
    //        //    });
    //        //    return ret;
    //        //});
    //        //alert(data);
    //        //alert(data[0]);

    //        var qcTeam = {
    //            QuestionId: selectedDivId,
    //            GridCol1: "kshk",
    //            GridCol2: "dfkashd",
    //            GridCol3: "aksdhk",
    //            GridCol4: "äsdfs",
    //            GridCol5: "asdfsd"
    //        };
    //        var ansResponse = [];
    //        ansResponse.push(qcTeam);
    //        $.post('@Url.Content("~///")', {  }, function (data) {               
    //        });
    //});     

    });


});
