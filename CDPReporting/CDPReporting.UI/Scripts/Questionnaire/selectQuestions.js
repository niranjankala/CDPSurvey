
$(document).ready(function () {
    // Hide all Question Response
    var selectedDivId;
    $('div[id^="CC"]').hide();
    //$('#GovernanceAnswertable').DataTable({ 'bSort': false});
    $("#questionListDiv>div>div>div>div").click(function () {
        selectedDivId = (this.id).replace("Question_", "");
        $('div[id^="CC"]').hide();
        $("#" + selectedDivId.replace(".", "\\.")).show();

        // Multiselect
        $('#countryId').multiselect();
        $('#Question_CC12a_Col3_List').multiselect();
        $('input[name="daterange"]').daterangepicker();

        // Dropdown menu
        $("#CC0_4DropDown .dropdown-menu li a").click(function () {

            $("#CC0_4DropDown .btn:first-child").text($(this).text());
            $("#CC0_4DropDown .btn:first-child").val($(this).text());

        });

        $("#Question_CC12a_Col1 .dropdown-menu li a").click(function () {

            $("#Question_CC12a_Col1 .btn:first-child").text($(this).text());
            $("#Question_CC12a_Col1 .btn:first-child").val($(this).text());

        });
        $("#Question_CC12a_Col2 .dropdown-menu li a").click(function () {

            $("#Question_CC12a_Col2 .btn:first-child").text($(this).text());
            $("#Question_CC12a_Col2 .btn:first-child").val($(this).text());

        });
    });


});
