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
            { "data": "status", "width": "%14" },
            { "data": "fullName", "width": "14%" },
            {
                "data": "birthday", "width": "14%",
                "render": DataTable.render.date()
            },
            { "data": "address", "width": "14%" },
            { "data": "englishLevel", "width": "14%" },
            { "data": "computerLevel", "width": "14%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="btn-group" role="group">
                            <div class="w-75 btn-group" role="group">
                            <a href="/Applicant/Manage/Details?id=${data}"
                            class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i>Details</a>
                            </div>
                            <div class="w-75 btn-group" role="group">
                            <a href="/Applicant/Manage/Edit?id=${data}"
                            class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i>Edit</a>
                            </div>
                            <div class="w-75 btn-group" role="group">
                            <a href="/Applicant/Manage/Delete?id=${data}"
                            class="btn btn-danger mx-2"> <i class="bi bi-pencil-square"></i>Delete</a>
                            </div>
                        </div>
                        `
                },
                "width": "14%"
            }
        ]
    });
}
