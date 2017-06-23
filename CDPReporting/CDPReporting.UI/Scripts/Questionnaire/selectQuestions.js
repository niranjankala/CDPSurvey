
$(document).ready(function () {
    // Hide all Question Response
    var selectedDivId;
    $(".question").click(function () {
        debugger;
        selectedDivId = (this.id).replace(".", "_");
        $('#questionResponseDiv').load(questionViewURL + "?questionViewId=" + selectedDivId);
    });
});
