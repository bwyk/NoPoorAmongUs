var dataTable;

$(document).ready(function () {
    loadDataTable();
});


function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Student/StudentNotes/GetAllByRole"
        },
        "columns": [
            { "data": "createdDate", "render": DataTable.render.datetime(), "width": "20%" },
            { "data": "priority", "width": "10%" },
            { "data": "applicationUser.fullName", "width": "15%" },
            { "data": "student.firstName", "width": "15%" },
            { "data": "student.lastName", "width": "15%" },
            { "data": "noteType.type", "width": "15%" },
            { "data": "id",
                "render": function (data) {
                    return `
                    <div class= text-center>
                        <div class="w-100 btn-group" role="group">
                        <a onClick=viewNoteText('/Student/StudentNotes/GetNoteText/${data}')
                        class="btn btn-secondary mx-2"> <i class="bi bi-binoculars-fill"></i>View</a>
                        <a href="/Student/StudentNotes/Upsert?id=${data}"
                        class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i>Edit</a>
                        <a onClick=Delete('/Student/StudentNotes/Delete/${data}')
                        class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i>Delete</a>
                        </div>
                    </div>
                    `
                },
                "width": "15%"
            }
        ]
    });
}

function viewNoteText(url) {
    var text;
    var id;
    $.getJSON(url, function (data) {
        console.log(data);
        $.each(data, function (key, val) {
            text = val.text;
            id = val.id;
        });

        Swal.fire({
            title: 'Student Note',
            html: text,
            showDenyButton: true,
            confirmButtonText: 'Close',
            denyButtonText: 'Update Note',
        }).then((result) => {
            if (result.isDenied)
            {
                window.location.href = '/Student/StudentNotes/Upsert?id=' + id;
            }
        })
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