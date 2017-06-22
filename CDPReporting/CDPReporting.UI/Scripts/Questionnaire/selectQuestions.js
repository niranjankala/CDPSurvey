
$(document).ready(function () {
    // Hide all Question Response
    var selectedDivId;
    $("#questionListDiv>div>div>div>div>div>div").click(function () {
        debugger;
        selectedDivId = (this.id).replace(".", "_");
        $('#questionResponseDiv').load(questionViewURL + "?questionViewId=" + selectedDivId);
    });
});
