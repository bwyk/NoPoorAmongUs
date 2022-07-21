var dataTableCurrent;
var dataTablePotential;
var guardianJSON;
var isEditable;
var ratingIsEditable;
var studentId;
var row;
$(document).ready(function () {
    $("#ratingSubmit").click(function (e) {
        ratingToggleEditable();

        //Serialize the form datas.   
        var valdata = $("#form-rating").serialize();
        $.ajax({
            url: "/Applicant/Manage/SaveRatings",
            type: "POST",
            dataType: 'json',
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            data: valdata
        });
    });  


    document.getElementById('applicant-form').onsubmit = function () {      // Process the guardians table after form is submitted
        guardianJSON = document.getElementById('guardianJSON');             // Grab the input element
        var data = dataTableCurrent.rows().data();                          // Grab all of the data in the table
        var guardianData = [];                                              // Put it in an array for JSON to process
        for (var i = 0; i < data.length; i++) {
            guardianData.push(data[i]);
        }
        guardianJSON.setAttribute('value', JSON.stringify(guardianData));   // Assign it to the field as a JSON string
    };
    studentId = document.getElementById("studentId").value;             // Grab Student.Id
    isEditable = (studentId == 0);
    ratingIsEditable = false;
    setup();
});

function setup() {

    reloadTables();
    toggleEditable();
    ratingToggleEditable()
}

function cancelEdit() {
    toggleEditable();
    $("#applicant-form")[0].reset();
    reloadTables();
}

function ratingCancelEdit() {
    ratingToggleEditable();
    $("#form-rating")[0].reset();
}

function reloadTables() {
    $('#tblDataCurrentGuardians').DataTable().destroy();
    $('#tblDataPotentialGuardians').DataTable().destroy();
    loadDataTableCurrent();
    loadDataTablePotential();
}

function redrawTables() {
    dataTableCurrent.draw();
    dataTablePotential.draw();
}

function toggleEditable() {
    $(".student-edit-input").each(function () {
        $(this).prop('readonly', !isEditable);
    })
    $(".student-edit-guardian").each(function () {
        $(this).prop('hidden', !isEditable);
    })
    $(".student-edit-hide").each(function () {
        $(this).prop('hidden', isEditable);
    })
    $(".student-edit-show").each(function () {
        $(this).prop('hidden', !isEditable);
    })
    isEditable = !isEditable;
}

function ratingToggleEditable() {
    $(".rating-edit-input").each(function () {
        $(this).prop('readonly', !ratingIsEditable);
    })
    $(".rating-edit-hide").each(function () {
        $(this).prop('hidden', ratingIsEditable);
    })
    $(".rating-edit-show").each(function () {
        $(this).prop('hidden', !ratingIsEditable);
    })
    ratingIsEditable = !ratingIsEditable;
}


//function saveRating() {
//    var age = document.getElementById("rating-age").value;
//    var academics = document.getElementById("rating-academics").value;
//    var income = document.getElementById("rating-income").value;
//    var family = document.getElementById("rating-family").value;
//    var distance = document.getElementById("rating-distance").value;

//    $("#form-rating")[0].reset();
//    if (age && academics && income && fami) {
//        document.getElementById("guardian-first-name").value = "";
//        document.getElementById("guardian-last-name").value = "";
//        var relationship = document.getElementById("guardian-relationship").value;
//        document.getElementById("guardian-relationship").selectedIndex = 0;
//        dataTableCurrent.row.add({
//            "id": -1,
//            "firstName": firstName,
//            "lastName": lastName,
//            "relationship": relationship
//        });
//        dataTableCurrent.draw();
//    }
//}

function addGuardian() {
    var firstName = document.getElementById("guardian-first-name").value;
    var lastName = document.getElementById("guardian-last-name").value;
    if (firstName && lastName) {
        document.getElementById("guardian-first-name").value = "";
        document.getElementById("guardian-last-name").value = "";
        var relationship = document.getElementById("guardian-relationship").value;
        document.getElementById("guardian-relationship").selectedIndex = 0;
        dataTableCurrent.row.add({
            "id": -1,
            "firstName": firstName,
            "lastName": lastName,
            "relationship": relationship
        });
        dataTableCurrent.draw();
    }
}

function loadDataTableCurrent() {
    dataTableCurrent = $('#tblDataCurrentGuardians').DataTable({
        "bAutoWidth": false,
        "language": {
            "emptyTable": "No Guardians Found"
        },
        "ajax": {
            "url": "/Applicant/Manage/GetCurrentGuardians?studentId=" + studentId
        },
        "columns": [
            { "data": "firstName", "width": "5%" },
            { "data": "lastName", "width": "5%" },
            { "data": "relationship", "width": "5%" },
            {
                "data": "id",
                "render": function (data, type, row, meta) {
                    return `
                        <div class="w-75 btn-group" role="group">
                            <button class="btn btn-danger remove-applicant student-edit-show" id="btn_remove_${data}" ">Remove</button>
                        </div>
                        `
                },
                "width": "2%"
            }
        ]
    });

    $('#tblDataCurrentGuardians').on('click', 'button.remove-applicant', function () {
        var $rowCurrent = $(this).closest('tr');                // Gets the row element of the clicked button
        var selectedRow = dataTableCurrent.row($rowCurrent);    // Converts it into a datatable row
        dataTablePotential.row.add(selectedRow.data());         // Adds it to the potential guardians table
        selectedRow.remove();                                   // Removes the row from its current table       
        redrawTables();                                         // Redraw the tables
    });
}

function loadDataTablePotential() {
    dataTablePotential = $('#tblDataPotentialGuardians').DataTable({
        "bAutoWidth": false,
        "language": {
            "emptyTable": "No Guardians Found"
        },
        "ajax": {
            "url": "/Applicant/Manage/GetPotentialGuardians?studentId=" + studentId
        },
        "columns": [
            { "data": "firstName", "width": "10%" },
            { "data": "lastName", "width": "10%" },
            {
                "data": "id",
                "render": function (data, type, row, meta) {
                    return `
                        <div class="w-75 btn-group" role="group">
                            <button class="btn btn-success add-applicant" id="btn_add_${data}" ">Add</button>
                        </div>                      
                        `
                },
                "width": "5%"
            }
        ]
    });


    $('#tblDataPotentialGuardians').on('click', 'button.add-applicant', function () {
        row = $(this).closest('tr');                                // Gets the row element of the clicked button
        $("#relationshipModal").modal("show");                      // Shows the popup for relationship type
    });
}

$('#close').on('click', function () {                       // If closed hide the window
    $("#relationshipModal").modal("hide");                  // Hides the popup for relationship type
});

$('#save').click(function () {                              // If saved process the row
    $("#relationshipModal").modal("hide");                  // Hides the popup for relationship type
    var selectedRow = dataTablePotential.row(row);          // Converts the row html element into a datatable row
    var rel = $("#guardian-relationship").val();            // Set the relationship
    selectedRow.data()["relationship"] = rel;

    dataTableCurrent.row.add(selectedRow.data());           // Adds it to the current guardians table
    selectedRow.remove();                                   // Removes the row from its current table
    redrawTables();                                         // Redraw the tables
});
