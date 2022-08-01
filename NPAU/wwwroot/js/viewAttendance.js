var attendanceId
var loadEditTable
var courseEnrId

$(document).ready(function () {
    attendanceId = document.getElementById("attendanceId").vaule;
    courseEnrId = document.getElementById("courseEnrId").vaule;
});

function toggleEdit() {
    loadTblChangeAttendance();
}

function loadTblChangeAttendance() {
    loadEditTable = $('#tblChangeAttendance').DataTable({
        "bAutoWidth": false,
        "language": {
            "emptyTable": "No Attendance To Change"
        },
        "ajax": {
            "url": "/Student/Attendance/GetStudent?id=" + attendanceId
        },
        "columns": [
            { "data": "fullName", "width": "25%" },
            { "data": "markedAttendance", "width": "35%" },
            {
                "data": "excused",
                "render": function () {
                    return '< input type="checkbox" asp-for="@Model.AttendanceList[i].Excused" >'
                },
                "width": "20%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                            <button class="btn btn-success add-applicant" id="btn_add_${data}" ">Update</button>
                        </div>                      
                        `
                },
                "width": "20%" 
            }

        ]
    });
}