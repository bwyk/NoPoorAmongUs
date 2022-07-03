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
            "url": "/Student/Student/GetAll?status=" + status
        },
        "columns": [
            { "data": "status", "width": "%10" },
            { "data": "fullName", "width": "10%" },
            {
                "data": "birthday", "width": "10%",
                "render": DataTable.render.date()
            },
            { "data": "address", "width": "10%" },
            { "data": "village", "width": "10%" },
            { "data": "englishLevel", "width": "10%" },
            { "data": "computerLevel", "width": "10%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                        <a href="/Applicant/Manage/Edit?id=${data}"
                        class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i>Edit</a>
                        </div>
                        `
                },
                "width": "10%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                        <a href="/Student/Student/Delete?id=${data}"
                        class="btn btn-danger mx-2"> <i class="bi bi-pencil-square"></i>Delete</a>
                        </div>
                        `
                },
                "width": "10%"
            }
        ]
    });
}
