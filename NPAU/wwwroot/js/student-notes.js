//credit to kthorngren's post on https://datatables.net/forums/discussion/42045/nested-tables for nested datatables. -Kevin McLennan

var dataTable;

$(document).ready(function () {
    var url = window.location.search;

    switch (true) {
        case url.includes("GetAllForUser"):
            loadDataTable("GetAllForUser");
            break;
        case (url.includes("StudentList")):
            loadDataTable("StudentList");
            break;
        default:
            loadDataTable("GetAllForUser");
    }
});

function format(rowData) {
    return '<table id="' + rowData.id + '" class="table table-bordered table-striped" style = "width:100%">' +
        '</table>';
}

function loadDataTable(table) {

    switch (table) {
        case "GetAllForUser":
            dataTable = $('#tblData').DataTable({
                "ajax": {
                    "url": "/Student/StudentNotes/GetAllForUser"
                },
                "columns": [
                    { title: 'Date Created', "data": "createdDate", "render": DataTable.render.datetime(), "width": "20%" },
                    { title: 'Priority Level', "data": "priority", "width": "10%" },
                    { title: 'Author', "data": "applicationUser.fullName", "width": "15%" },
                    { title: 'First Name', "data": "student.firstName", "width": "15%" },
                    { title: 'Last Name', "data": "student.lastName", "width": "15%" },
                    { title: 'Note Type', "data": "noteType.type", "width": "15%" },
                    {
                        title: 'Actions',
                        "data": "id",
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
            break;
        case "StudentList":
            dataTable = $('#tblData').DataTable(
                {
                    responsive: true,
                    "ajax": {
                        "url": "/Student/StudentNotes/GetStudents"
                    },
                    "columns": [
                        { title: 'First Name', "data": "student.firstName", "width": "20%" },
                        { title: 'Last Name', "data": "student.lastName", "width": "20%" },
                        {
                            title: 'Actions',
                            "data": "studentId",
                            "render": function (data, type, row, meta)
                            {
                                return `
                                    <div class= text-center>
                                        <div class="w-100 btn-group" role="group">
                                        <button data-name="' + row[0] + '" class="btn btn-secondary mx-2"> <i class="bi bi-binoculars-fill"></i>View All Notes</button>
                                        </div>
                                    </div>
                                    `
                            },
                            "width": "15%"
                        },
                    ]
                });
            $('#tblData tbody').on('click', 'button', function () {
                var tr = $(this).closest('tr');
                var row = dataTable.row(tr);
                var rowData = row.data();

                if (row.child.isShown()) {
                    // This row is already open - close it
                    row.child.hide();
                    tr.removeClass('shown');
                    // Destroy the Child Datatable
                    $('#' + rowData.id).DataTable().destroy();
                } else {
                    // Open this row
                    row.child(format(rowData)).show();
                    var id = rowData.id;

                    $('#' + id).DataTable({
                        "ajax": {
                            "url": "/Student/StudentNotes/GetNotesByStudent/" + rowData.studentId
                        },
                        dom: "t",
                        columns:[
                                { title: 'Date Created', "data": "createdDate", "render": DataTable.render.datetime(), "width": "20%" },
                                { title: 'Priority Level', "data": "priority", "width": "10%" },
                                { title: 'Author', "data": "applicationUser.fullName", "width": "15%" },
                                { title: 'Note Type', "data": "noteType.type", "width": "15%" },
                                {
                                    title: 'Actions',
                                    "data": "id",
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
                        ,
                        scrollY: '300px',
                        select: true,
                    });

                    tr.addClass('shown');
                }
            });
            break;
    }
}

function viewNoteText(url) {
    var text;
    var id;
    $.getJSON(url, function (data) {
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
            if (result.isDenied) {
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