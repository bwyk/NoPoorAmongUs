var dataTable;

$(document).ready(function () {
    loadDataTable();
});


function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Student/StudentDocs/GetAll"
        },
        "columns": [
            { "data": "student.id", "width": "15%" },
            { "data": "student.firstName", "width": "15%" },
            { "data": "student.lastName", "width": "15%" },
            { "data": "docType.typeName", "width": "15%" },
            { "data": "title", "width": "15%" },
            {
                "data": { id: "id", docUrl: "docUrl"},
                "render": function (data) {
                    return `
                    <div class= text-center>
                        <div class="w-75 btn-group" role="group">
                        <a href="${data.docUrl}" target="_blank"
                        class="btn btn-secondary mx-2"> <i class="bi bi-binoculars-fill"></i>View</a>
                        <a href="/Student/StudentDocs/Upsert?id=${data.id}"
                        class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i>Edit</a>
                        <a onClick=Delete('/Student/StudentDocs/Delete/${data.id}')
                        class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> Delete</a>
                        </div>
                    </div>
                    `
                },
                "width": "25%"
            }
        ]
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
                        dataTable.ajax.reload();
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