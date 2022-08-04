var dataTableCurrent;
var dataTablePotential;
var guardianJSON;
var isEditable;
var ratingIsEditable;
var studentId;
var row;
var status;
var col1;
var col2;
var col3;
var col4;
var col5;
$(document).ready(function () {
    var url = window.location.search;

    if (url.includes("all")) {
        status = "Instructor_all"
    } else if (url.includes("private")) {
        status = "Instructor_private"
    } else if (url.includes("public")) {
        status = "Instructor_public"
    } else if (url.includes("your")) {
        status = "Instructor_your"
    } else {
        status = "Instructor_all" //TODO remove default and have it filter on role
    }

    $("#ratingSubmit").click(function (e) {
        ratingToggleEditable();
        var s  =getStatus()
        //Serialize the form datas.   
        var valdata = $("#form-rating").serialize();
        $.ajax({
            url: "/Applicant/Manage/SaveRatings?studentId=" + studentId + "&status=" + s,
            type: "POST",
            dataType: 'json',
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            data: valdata,
           
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

function getStatus() {
    return status;
}

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

function resetForm(id) {
    $('#' + id).val(function () {
        return this.defaultValue;
    });
}

//TODO validation only works the first time
function addGuardian() {
    
    var firstName = document.getElementById("guardian-first-name").value;
    var lastName = document.getElementById("guardian-last-name").value;
    var phoneNumber = document.getElementById("guardian-phone").value;
    var relationship = $("#guardian-relationship").val();

    // Manually show the guardian form errors
    if (!firstName) {
        $("#first-guardian-error").show()
    }
    if (!lastName) {
        $("#last-guardian-error").show()
    }
    if (!phoneNumber) {
        $("#phone-guardian-error").show()
    }
    if (!relationship) {
        $("#rel-guardian-error").show()

    }

    if (firstName && lastName && relationship) {
        $("#form-guardian")[0].reset();
        $("#form-guardian").prop("isvalid", true) // Reset Form validation
        //var validator = $("#form-guardian").validate();
        //validator.resetForm();
        //$("#form-guardian").validate().css("display", "none");
        $(".guardian-error").each(function () {
            //$(this).prop('hidden', true);
            $(this).hide();
        })
        //ValidatorUpdateDisplay(validator);
        dataTableCurrent.row.add({
            "id": -1,
            "firstName": firstName,
            "lastName": lastName,
            "phoneNumber": phoneNumber,
            "relationship": relationship
        });
        dataTableCurrent.draw();
    } else {

    }
}
var test = "firstName"
var ajaxCall = "/Applicant/Manage/GetCurrentGuardians?studentId="
function loadDataTableCurrent() {
    dataTableCurrent = $('#tblDataCurrentGuardians').DataTable({
        "bAutoWidth": false,
        "language": {
            "emptyTable": "No Guardians Found"
        },
        "ajax": {
            "url": ajaxCall + studentId
        },
        "columns": [
            { "data": test, "width": "20%" },
            { "data": "lastName", "width": "20%" },
            { "data": "relationship", "width": "10%" },
            //{ "data": "phoneNumber", "width": "5%" },
            {
                "data": "phoneNumber",
                "render": function (data) {
                    if (!data) {
                        return "No Phone Number Found"
                    }
                    else {
                        return data
                    }
                },
                "width": "25%"
            },
            {
                "data": "id",
                "render": function (data, type, row, meta) {
                    return `
                        <div class="w-75 btn-group" role="group">
                            <button class="btn btn-danger remove-applicant student-edit-show" id="btn_remove_${data}" ">Remove</button>
                        </div>
                        `
                },
                "width": "10%"
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


//$('#distance-slider').mdbRange({
//    single: {
//        active: true,
//        counting: true,
//        countingTarget: '#distance'
//    }
//});