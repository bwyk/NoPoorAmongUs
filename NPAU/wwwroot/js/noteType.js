var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/NoteType/GetAll"
        },
        "columns": [
            { "data": "type", "width": "20%" },
            { "data": "role.name", "width": "20%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                    <div class="d-flex justify-content-center">
                        <div class="w-50 btn-group" role="group">
                            <a href="/Admin/NoteType/Upsert?id=${data}"
                            class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>
                            <a onClick=Delete('/Admin/NoteType/Delete/${data}')
                            class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> Delete</a>
                        </div>
                    </div>
                        `
                },
                "width": "60%"
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