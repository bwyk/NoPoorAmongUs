var dataTable;
var tableRowContainer;
var jsonData;
var columns = []
columns[0] = "status";
columns[1] = "fullName";
columns[2] = "birthday";
columns[3] = "address";
columns[4] = "englishLevel";
columns[5] = "computerLevel";
columns[6] = "id";
var status = "all"
var ajaxCall = "/Applicant/Manage/GetAll?status="
var roleFilter
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
                if (url.includes("rating_incomplete")) {
                    loadDataTable("rating_incomplete");
                } else {
                    if (url.includes("rating_complete")) {
                        loadDataTable("rating_complete");
                    } else {
                        if (url.includes("social")) {
                            studentDetails("social")
                        } else {
                            if (url.includes("instructor")) {
                                studentDetails("instructor")
                            } else {
                                if (url.includes("admin")) {
                                    studentDetails("admin")
                                } else {
                                    loadDataTable2("all");
                                }
                            }
                        }
                    }
                }
            }      
        }
    }
    //addHeaders(7)    
});
var apiData
var dataObj
function PopulateAllTokens() {
    $.ajax({

        url: ajaxCall + status,
        dataType: "json",
        success: function (result) {
            apiData = result;
            console.log(apiData);
            dataObj = eval(apiData)
            loadDataTable2()
            
        }
    });
}

function addHeaders(keys) {
    for (var i = 0; i < keys.length; i++) {
        $('#tblData thead th:eq(' + i + ')').html(formatHeader(keys[i]))

    }
}

function formatHeader(colProp) {
    var split = colProp.split(/(?=[A-Z])/);
    var formatedHeader = split[0].charAt(0).toUpperCase() + split[0].slice(1);
    for (var i = 1; i < split.length; i++) {
        formatedHeader += " " + split[i];
    }
    return formatedHeader
}

function capitalizeFirstLetter(string) {
    return string.charAt(0).toUpperCase() + string.slice(1);
}

var columnHeaders
var wantedFields = []
var unwantedFields = []

function setFilters(wantedProps, status) {
    var allFields = []
    allFields[0] = "status";
    allFields[1] = "firstName";
    allFields[2] = "middleName";
    allFields[3] = "lastName";
    allFields[4] = "fullName";
    allFields[5] = "birthday";
    allFields[6] = "address";
    allFields[7] = "englishLevel";
    allFields[8] = "computerLevel";
    allFields[9] = "id";
    allFields[10] = "village";
    wantedFields = allFields.filter(key => wantedProps.includes(key));
    unwantedFields = allFields.filter(key => !wantedProps.includes(key));
}


function studentDetails(filter) {
    var wantedProps = []
    roleFilter = filter
    switch (filter) {
        case "social":
            wantedProps[0] = "status";
            wantedProps[1] = "fullName";
            wantedProps[2] = "birthday";
            wantedProps[3] = "address";
            wantedProps[4] = "englishLevel";
            wantedProps[5] = "computerLevel";
            wantedProps[6] = "id";
            wantedProps[7] = "village";
            setFilters(wantedProps, filter)
            break
        case "instructor":
            wantedProps[1] = "fullName";
            wantedProps[2] = "englishLevel";
            wantedProps[3] = "computerLevel";
            wantedProps[4] = "id";
            setFilters(wantedProps, filter)
            break
        case "admin":
            break
        default:
            break
    }
    loadDataTable2(filter);
}

function getButtons(data) {
    switch (roleFilter) {
        case "social":
            var buttons = `<div class="btn-group mb-1" role="group">
                        <div class="btn-group" role="group">
                            <a href="/Applicant/PublicSchoolSchedule/Index?id=${data}"
                            class="btn btn-primary"> <i class="bi bi-plus-circle"></i>&nbsp; Add Public School Schedule</a>
                        </div>
                    </div>
                    <div class="btn-group mb-1" role="group">
                        <div class="btn-group" role="group">
                            <a href="/Applicant/PublicSchoolSchedule/Schedule?id=${data}"
                            class="btn btn-primary"> <i class="bi bi-plus-circle"></i>&nbsp; View Public School Schedule</a>
                        </div>
                    </div>
                    <hr/>
                    <div class="btn-group" role="group">
                        <div class="btn-group mx-1" role="group">
                            <a href="/Applicant/Manage/Upsert?id=${data}"
                            class="btn btn-primary"> <i class="bi bi-info-circle"></i>Details</a>
                        </div>
                        <div class="btn-group mx-1" role="group">
                            <a href="/Applicant/Manage/Delete?id=${data}"
                            class="btn btn-danger"> <i class="bi bi-trash-fill"></i>Delete</a>
                        </div>                   
                    </div>`
            break;

        case "instructor":
            var buttons = `<div class="btn-group mb-1" role="group">
                            <div class="btn-group" role="group">
                                <a href="/Applicant/PublicSchoolSchedule/Index?id=${data}"
                                class="btn btn-primary"> <i class="bi bi-plus-circle"></i>&nbsp; Add Public School Schedule</a>
                            </div>
                        </div>
                        <div class="btn-group mb-1" role="group">
                            <div class="btn-group" role="group">
                                <a href="/Applicant/PublicSchoolSchedule/Schedule?id=${data}"
                                class="btn btn-primary"> <i class="bi bi-plus-circle"></i>&nbsp; View Public School Schedule</a>
                            </div>
                         </div>
                        <hr/>
                        <div class="btn-group" role="group">
                            <div class="btn-group mx-1" role="group">
                            <a href="/Applicant/Manage/Upsert?id=${data}"
                            class="btn btn-primary"> <i class="bi bi-info-circle"></i>Details</a>
                        </div>`
            break;
    }
    return buttons
}

function loadDataTable2(status) {
    $.ajax({
        url: ajaxCall + status,
        dataType: 'json',
        dataSrc: 'data',
        success: function (json) {
            var keys = Object.keys(json.data[0]).filter(key => wantedFields.includes(key));
            var columnList = [];
            var propertyList = [];

            keys.forEach((key) => {
                columnList.push({ 'data': key })   
            })
            columnList.push({
                'data': 'id', "render": function (data) {
                    return getButtons(data)
                },
                "width": "22%" }) 


      
            for (var i = 0; i < json.data.length; i++) {
                unwantedFields.forEach(f => delete json.data[i][f])
                propertyList[i] = json.data[i]
            }        

            dataTable = $('#tblData').DataTable({
                language: {
                    "emptyTable": "No Data Found"
                },
                data: propertyList,
                columns: columnList
                
            });
            addHeaders(keys);
        }
    });
}

var testing = '{ "data": columns[0], "width": "%10" },'
function loadDataTable(dataObject) {
    dataTable = $('#tblData').DataTable({
        "language": {
            "emptyTable": "No Students Found"
        },
        "ajax": {
            "url": ajaxCall + status
        },
        "columns": [
            { "data": columns[0], "width": "%10" },
            { "data": columns[1], "width": "20%" },
            {
                "data": columns[2], "width": "14%",
                "render": DataTable.render.date()
            },
            { "data": columns[3], "width": "14%" },
            { "data": columns[4], "width": "10%" },
            { "data": columns[5], "width": "10%" },
            {
                "data": columns[6],
                "render": function (data) {
                    return getButtons(data)
                        
                        
                },
                "width": "22%"
            }
        ]
    });
}
