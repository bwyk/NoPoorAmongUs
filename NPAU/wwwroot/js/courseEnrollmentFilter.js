var dataTable;
var columns = []
columns[0] = "status";
columns[1] = "fullName";
columns[2] = "birthday";
columns[3] = "address";
columns[4] = "englishLevel";
columns[5] = "computerLevel";
columns[6] = "id";
var status
var ajaxCall = "/Applicant/Session/GetAllSessions?status="
var roleFilter
$(document).ready(function () {
    var url = window.location.search;

    if (url.includes("all")) {
        status = "Instructor_all"
    } else if (url.includes("private")) {
        status = "Instructor_private"
    } else if (url.includes("public")) {
        status = "Instructor_public"
    } else if (url.includes("your")) {
        status = "Instructor_your"
    } else {
        status = "Instructor_all" //TODO remove default and have it filter on role
    }
    studentDetails("Instructor")
   
});

$('#all').click(function () {
    $('#session-table-body tr').each(function () {
        var tdVal = $(this).find("td:first").text();
        if (tdVal == "Pending")// continue;
            $(this).hide();
    });
});

function addHeaders(keys) {
    var colNum = 0;
    for (var i = 0; i < keys.length; i++) {
        if (keys[i] != "id") {
            $('#tblData thead th:eq(' + colNum + ')').html(formatHeader(keys[i]));
            colNum++;
        }
    }
}

function formatHeader(colProp) {
    var split = colProp.split(/(?=[A-Z])/);
    var formatedHeader = split[0].charAt(0).toUpperCase() + split[0].slice(1);
    for (var i = 1; i < split.length; i++) {
        formatedHeader += " " + split[i];
    }
    return formatedHeader
}

function capitalizeFirstLetter(string) {
    return string.charAt(0).toUpperCase() + string.slice(1);
}

var columnHeaders
var wantedFields = []
var unwantedFields = []

function setFilters(wantedProps, status) {
    var allFields = []
    allFields[0] = "schoolType";
    allFields[1] = "courseName";
    allFields[2] = "day";
    allFields[3] = "startTime";
    allFields[4] = "endTime";
    allFields[5] = "id";
    wantedFields = allFields.filter(key => wantedProps.includes(key));
    unwantedFields = allFields.filter(key => !wantedProps.includes(key));
}

function studentDetails(filter) {
    var wantedProps = []
    roleFilter = filter
    switch (filter) {
        case "Instructor":
            wantedProps[0] = "schoolType";
            //wantedProps[1] = "course.School";
            wantedProps[1] = "courseName";
            wantedProps[2] = "day";
            wantedProps[3] = "startTime";
            wantedProps[4] = "endTime";
            setFilters(wantedProps, filter)
            break
        case "admin":
            break
        default:
            break
    }
    loadDataTable(status);
}

function getButtons(data, status) {
    switch (roleFilter) {
        case "Instructor":
            var buttons = `
                            <div class="btn-group btn-group-sm mx-1" role="group">
                                <a href="/Student/Attendance/MarkAttendance?sessionId=${data}&status=${status}"
                                class="btn btn-primary"> <i class="bi bi-info-circle"></i>&nbsp; Mark Attendance</a>
                                <a href="/Student/Attendance/ViewAttendance?sessionId=${data}&status=${status}"
                                class="btn btn-primary"> <i class="bi bi-info-circle"></i>&nbsp; View Attendance</a>
                           </div>
                           <div class="btn-toolbar" role="toolbar" aria-label="Toolbar with button groups">
                           <div class="btn-group btn-group-sm mx-1" role="group">
                                <div class="btn-group btn-group-sm" role="group">
                                   <a href="/Applicant/Session/Upsert?id=${data}&status=${status}"
                                   class="btn btn-primary"> <i class="bi bi-info-circle"></i>&nbsp; Details And Enrollment</a>
                               </div>
                               </div>
                                <div class="btn-group btn-group-sm mx-1" role="group">
                                    <a onClick=Delete('/Applicant/Session/Delete/${data}')
                                    class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> Delete</a>
                               </div>                   
                           </div>`
            break;
    }
    return buttons
}
function getStatus() {
    return status;
}
function loadDataTable(status) {
    $.ajax({
        url: ajaxCall + status,
        dataType: 'json',
        dataSrc: 'data',
        success: function (json) {
            var columnList = [];
            var propertyList = [];
            if (json.data.length == 0) {
                var keys = Object.keys(json.data).filter(key => wantedFields.includes(key));
            } else {
                var keys = Object.keys(json.data[0]).filter(key => wantedFields.includes(key));

            }

            keys.forEach((key) => {
                if (key != "id")
                    columnList.push({ 'data': key })
            })
            columnList.push({
                'data': 'id', "render": function (data) {
                    return getButtons(data, getStatus())
                },
                "width": "30%"
            })

            for (var i = 0; i < json.data.length; i++) {
                //unwantedFields.forEach(f => delete json.data[i][f])
                propertyList[i] = json.data[i]
            }
            dataTable = $('#tblData').DataTable({
                language: {
                    "emptyTable": "No Sessions Found"
                },
                data: propertyList,
                columns: columnList
            });
            addHeaders(keys);
            $('tbody').attr("id", "session-table-body")
        }
    });
}

function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        //dataTable.ajax.reload();
                        dataTable.clear().destroy()
                        loadDataTable(getStatus());
                        toastr.success(data.message);
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}
