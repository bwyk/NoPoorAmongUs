var dataTable;

$(document).ready(function () {

    var url = window.location.search;
    if (url.includes("student")) {
        loadDataTable("student");
    }
    else {
        if (url.includes("pending")) {
            loadDataTable("pending");
        }
        else {
            if (url.includes("rejected")) {
                loadDataTable("rejected");
            }
            else {
                loadDataTable("all");

            }
        }
    }
});

function loadDataTable(status) {
    dataTable = $('#tblData').DataTable({
        "language": {
            "emptyTable": "No Students Found"
        },
        "ajax": {
            "url": "/Applicant/Manage/GetAll?status=" + status
        },
        "columns": [
            { "data": "status", "width": "%10" },
            { "data": "fullName", "width": "20%" },
            {
                "data": "birthday", "width": "14%",
                "render": DataTable.render.date()
            },
            { "data": "address", "width": "14%" },
            { "data": "englishLevel", "width": "10%" },
            { "data": "computerLevel", "width": "10%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="btn-group mb-1" role="group">
                            <div class="btn-group" role="group">
                                <a href="/Applicant/PublicSchoolSchedule/Index?id=${data}"
                                class="btn btn-primary"> <i class="bi bi-plus-circle"></i>&nbsp; Add Public School Schedule</a>
                            </div>
                        </div>
                        <div class="btn-group mb-1" role="group">
                            <div class="btn-group" role="group">
                                <a href="/Applicant/PublicSchoolSchedule/Schedule?id=${data}"
                                class="btn btn-primary"> <i class="bi bi-plus-circle"></i>&nbsp; View Public School Schedule</a>
                            </div>
                        </div>
                        <hr/>
                        <div class="btn-group" role="group">
                            <div class="btn-group mx-1" role="group">
                            <a href="/Applicant/Manage/Details?id=${data}"
                            class="btn btn-primary"> <i class="bi bi-info-circle"></i>Details</a>
                            </div>
                            <div class="btn-group mx-1" role="group">
                            <a href="/Applicant/Manage/Edit?id=${data}"
                            class="btn btn-primary"> <i class="bi  bi-pencil-square"></i>Edit</a>
                            </div>
                            <div class="btn-group mx-1" role="group">
                            <a href="/Applicant/Manage/Delete?id=${data}"
                            class="btn btn-danger"> <i class="bi bi-trash-fill"></i>Delete</a>
                            </div>                       
                        </div>
                        `
                },
                "width": "22%"
            }
        ]
    });
}
