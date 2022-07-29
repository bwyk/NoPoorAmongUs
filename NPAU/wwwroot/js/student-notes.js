var dataTable;

/*$(document).ready(function () {
    loadDataTable();
});*/

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
                    "ajax": {
                        "url": "/Student/StudentNotes/GetAllForUser"
                    },
                    "columns": [
                        { title: 'First Name', "data": "student.firstName", "width": "20%" },
                        { title: 'Last Name', "data": "student.lastName", "width": "20%" },
                        {
                            title: 'Actions',
                            "data": "studentId",
                            "render": function (data) {
                                return `
                    <div class= text-center>
                        <div class="w-100 btn-group" role="group">
                        <a onClick=viewStudentNoteList('/Student/StudentNotes/GetNotesByStudent/${data}')
                        class="btn btn-secondary mx-2"> <i class="bi bi-binoculars-fill"></i>View All Notes</a>
                        </div>
                    </div>
                    `
                            },
                            "width": "15%"
                        }
                    ]
                });
            break;
    }
}

/*
 https://localhost:7140/Student/StudentNotes/GetNotesByStudent/5
 */
function viewStudentNoteList(url)
{
    var tr = $(this).closest('tr');
    var row = dataTable.row(tr);

    if (row.child.isShown()) {
        // This row is already open - close it
        row.child.hide();
        tr.removeClass('shown');
    } else {
        // Open this row

        $.getJSON(url, function (allData)
        {
            $.each(allData.data, function (key, val) {
                console.log(val.id);
            });
        });

        console.log("This would open the notes for: " );
/*        row.child(format(row.data())).show();
        tr.addClass('shown');*/
    }


    /*This grabs the data and is working.*/
/*    var id;
    console.log("url: " + url);
    $.getJSON(url, function (data)
    {
        console.log("data: " + data);
        $.each(data, function (key, val)
        {
            console.log(data)
        });
        console.log("id value: " + id);
    });*/
}

function format(d) {
    // `d` is the original data object for the row
    return (
        '<table cellpadding="5" cellspacing="0" border="0" style="padding-left:50px;">' +
        '<tr>' +
        '<td>Note ID:</td>' +
        '<td>' +
        d.id +
        '</td>' +
        '</tr>' +
        '<tr>' +
        '<td>Student Id:</td>' +
        '<td>' +
        d.studentId +
        '</td>' +
        '</tr>' +
        '</table>'
    );
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