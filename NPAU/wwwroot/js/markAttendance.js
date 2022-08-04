var allPresent = false;
var allAbsent = false;
var allTardy = false;
var allChecked = false;

function uncheckAll() {
    $('.Absent').each(function () {
        $(this).attr('checked', true).click()// if it is checked click it and uncheck it
    });
    $('.Tardy').each(function () {
        $(this).attr('checked', true).click()
    });
    $('.Present').each(function () {
        $(this).attr('checked', true).click()
    });
    allAbsent = false;
    allPresent = false;
    allTardy = false;
}

function markAllPresent() {
    if (!allPresent) {
        uncheckAll();
        $('.Present').each(function () {
            $(this).attr('checked', false).click()
        });
        allPresent = true;
    } 
}
function markAllTardy() {
    if (!allTardy) {
        uncheckAll();
        $('.Tardy').each(function () {
            $(this).attr('checked', false).click()
        });
        allTardy = true;
    }    
}
function markAllAbsent() {
    if (!allAbsent) {
        uncheckAll();
        $('.Absent').each(function () {
            $(this).attr('checked', false).click()
        });
        allAbsent = true;
    } 
}

function markAllExcused() {
    if (allChecked) {
        $('.excused-checkbox').each(function () {
            $(this).prop('checked', true).click();

        });
        allChecked = false
    } else {
        $('.excused-checkbox').each(function () {
            $(this).prop('checked', false).click();

        });
        allChecked = true
    }
}
