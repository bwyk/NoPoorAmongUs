var dataTableCurrent;
var dataTablePotential;
var courseEnrollmentsJSON;
var isEditable;
var courseSessionId;
var row;
$(document).ready(function () {
    document.getElementById('course-session-form').onsubmit = function () {     // Process the enrollment table after form is submitted
        courseEnrollmentsJSON = document.getElementById('course-session-students-json');     // Grab the input element
        var data = dataTableCurrent.rows().data();                              // Grab all of the data in the table
        var courseSessionData = [];                                             // Put it in an array for JSON to process
        for (var i = 0; i < data.length; i++) {
            courseSessionData.push(data[i]);
        }
        courseEnrollmentsJSON.setAttribute('value', JSON.stringify(courseSessionData));   // Assign it to the field as a JSON string
    };
    courseSessionId = document.getElementById("course-session-id").value;             // Grab CourseSession.Id
    isEditable = (courseSessionId == 0);
    setup();
});

function setup() {

    reloadTables();
    toggleEditable();
}

function cancelEdit() {
    toggleEditable();
    $("#course-session-form")[0].reset();
    reloadTables();
}


function reloadTables() {
    $('#tblDataCurrentEnrollments').DataTable().destroy();
    $('#tblDataPotentialEnrollments').DataTable().destroy();
    loadDataTableCurrent();
    loadDataTablePotential();
}

function redrawTables() {
    dataTableCurrent.draw();
    dataTablePotential.draw();
}

function toggleEditable() {
    $(".course-session-edit-input").each(function () {
        $(this).prop('readonly', !isEditable);
    })
    $(".course-session-edit-enrollments").each(function () {
        $(this).prop('hidden', !isEditable);
    })
    $(".course-session-edit-hide").each(function () {
        $(this).prop('hidden', isEditable);
    })
    $(".course-session-edit-show").each(function () {
        $(this).prop('hidden', !isEditable);
    })
    
    isEditable = !isEditable;
}

function resetForm(id) {
    $('#' + id).val(function () {
        return this.defaultValue;
    });
}

function loadDataTableCurrent() {
    dataTableCurrent = $('#tblDataCurrentEnrollments').DataTable({
        "bAutoWidth": false,
        "language": {
            "emptyTable": "No Students Found"
        },
        "ajax": {
            "url": "/Applicant/Session/GetCurrentEnrollments?courseSessionId=" + courseSessionId
        },
        "columns": [
            { "data": "firstName", "width": "5%" },
            { "data": "lastName", "width": "5%" },
            {
                "data": "id",
                "render": function (data, type, row, meta) {
                    return `
                        <div class="w-75 btn-group" role="group">
                            <button class="btn btn-danger remove-student course-session-edit-show" id="btn_remove_${data}" ">Remove</button>
                        </div>
                        `
                },
                "width": "2%"
            }
        ]
    });

    $('#tblDataCurrentEnrollments').on('click', 'button.remove-student', function () {
        var $rowCurrent = $(this).closest('tr');                // Gets the row element of the clicked button
        var selectedRow = dataTableCurrent.row($rowCurrent);    // Converts it into a datatable row
        dataTablePotential.row.add(selectedRow.data());         // Adds it to the potential Enrollments table
        selectedRow.remove();                                   // Removes the row from its current table       
        redrawTables();                                         // Redraw the tables
    });
}

function loadDataTablePotential() {
    dataTablePotential = $('#tblDataPotentialEnrollments').DataTable({
        "bAutoWidth": false,
        "language": {
            "emptyTable": "No Enrollments Found"
        },
        "ajax": {
            "url": "/Applicant/Session/GetPotentialEnrollments?courseSessionId=" + courseSessionId
        },
        "columns": [
            { "data": "firstName", "width": "10%" },
            { "data": "lastName", "width": "10%" },
            {
                "data": "id",
                "render": function (data, type, row, meta) {
                    return `
                        <div class="w-75 btn-group" role="group">
                            <button class="btn btn-success add-student" id="btn_add_${data}" ">Add</button>
                        </div>                      
                        `
                },
                "width": "5%"
            }
        ]
    });
    $('#tblDataPotentialEnrollments').on('click', 'button.add-student', function () {
        var $rowCurrent = $(this).closest('tr');                // Gets the row element of the clicked button
        var selectedRow = dataTablePotential.row($rowCurrent);    // Converts it into a datatable row
        dataTableCurrent.row.add(selectedRow.data());         // Adds it to the potential Enrollments table
        selectedRow.remove();                                   // Removes the row from its current table       
        redrawTables();                                         // Redraw the tables
    });

}
