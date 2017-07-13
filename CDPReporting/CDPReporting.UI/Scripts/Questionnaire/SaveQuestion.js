
$(document).ready(function () {
    $("#btnSave").click(function () {
            //debugger;
            //var questionId = 
            //var col1 = 
            var col2 = $("#plantList option:selected").text();
            var col3 = $("#TeamName").val();
            var col4 = ($("#IsActive").is(':checked'));
            var data = $("#listQCM").jqGrid('getRowData');
            var memberlist = [];
            for (var i = 0; i < data.length; i++) {
                var rowdata = {
                    TeamId: data[i].TeamId,
                    TeamMemberId: data[i].TeamMemberId,
                    MemberName: data[i].MemberName,
                    MemberEmail: data[i].MemberEmail,
                    MemberType: data[i].MemberType,
                    ImageDataURL: $(data[i].ImageDataURL).attr('src'),
                    EmpCode: data[i].EmpCode,
                    Gender: data[i].Gender,
                    Qualification: data[i].Qualification,
                    Designation: data[i].Designation,
                    Department: data[i].Department
                };
                memberlist.push(rowdata);

            }
            var qcTeam = {
                TeamId: teamId,
                PlantId: unitId,
                PlantName: unitName,
                TeamName: teamName,
                IsActive: active,
                TeamMembers: memberlist
            };
            //debugger;
            $.ajax({
                url: btnaddURL,
                cache: false,
                async: false,
                type: "POST",
                //    datatype: "json",
                data: JSON.parse(JSON.stringify(qcTeam)),
                success: function (id) {
                    //debugger;
                    if (isEditMode == false) {
                        var createNewPorject = NewProjectDialog();
                        if (createNewPorject == true) {
                            var groupId = '00000000-0000-0000-0000-000000000000'
                            window.location.href = (redirecttocreatenewprojectURL + "?groupId=" + groupId + "&isFromIndex=" + true + "&teamId=" + id);
                        }
                        else {
                            location.href = redirecttoviewURL;
                        }
                    }
                    else {
                        location.href = redirecttoviewURL;
                    }

                    //}
                },
                error: function (e) {
                    // debugger;
                    alert(e);
                }
            });
    });
});