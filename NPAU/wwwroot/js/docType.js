var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/DocType/GetAll"
        },
        "columns": [
            { "data": "typeName", "width": "10%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="w-100 btn-group" role="group">
                        <a href="/Admin/DocType/Upsert?id=${data}"
                        class="btn btn-primary mx-1"> <i class="bi bi-pencil-square"></i> Edit</a>
                        <a onClick=Delete('/Admin/DocType/Delete/${data}')
                        class="btn btn-danger mx-1"> <i class="bi bi-trash-fill"></i> Delete</a>
                    </div>
                        `
                },
                "width": "5%"
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