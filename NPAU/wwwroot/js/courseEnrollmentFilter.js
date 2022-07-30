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
    if (url.includes("Instructor")) {
        if (url.includes("allSessions")) {
            status = "Instructor_all"
        } else if(url.includes("yourSessions")) {
            status = "Instructor_your"
        } else if (url.includes("privateSessions")) {
            status = "Instructor_private"
        } else if (url.includes("publicSessions")) {
            status = "Instructor_public"
        }
        studentDetails("Instructor")
    } 
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

function getButtons(data) {
    switch (roleFilter) {
        case "Instructor":
            var buttons = `<div class="btn-group mb-1" role="group">
                               <div class="btn-group" role="group">
                                   <a href="/Applicant/Session/Upsert?id=${data}"
                                   class="btn btn-primary"> <i class="bi bi-info-circle"></i>&nbsp; Details</a>
                               </div>
                           </div>
                           <div class="btn-group mx-1" role="group">
                                   <a href="/Applicant/Session/DeleteEnrollment?id=${data}"
                                   class="btn btn-danger"> <i class="bi bi-trash-fill"></i>Delete</a>
                               </div>                   
                           </div>`
            break;
    }
    return buttons
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
                        return getButtons(data)
                    },
                    "width": "22%"
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

